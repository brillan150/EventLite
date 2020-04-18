using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventCatalogApi.Data;
using EventCatalogApi.ViewModels;
using EventLite.Domain.EventLite;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventCatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogContext _context;
        private readonly IConfiguration _config;

        private static readonly Random randNumGen = new Random(DateTime.Now.Millisecond);
        // Based on the docs, for dotnetcore, plain old Random() should work.
        // Explicitly seeding with time for futureproofing and because
        // may be necessary/relevant in future porting

        public CatalogController(CatalogContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogFormats()
        {
            var events = await _context.CatalogFormats.ToListAsync();
            return Ok(events);
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogTopics()
        {
            var events = await _context.CatalogTopics.ToListAsync();
            return Ok(events);
        }

        // Almost all apis should be async
        // Don't block your webui
        // requirements:
        // async keyword
        // compatable return type (probably wrapped in a Task<>)
        // await the async call in the method
        // (involves storing a ref to the object that you get back)

        //[HttpGet()]
        //[Route("[action]")]
        //public async Task<IActionResult> Events(
        //    [FromQuery]int pageIndex = 0,
        //    [FromQuery]int pageSize = 2)
        //{
        //    // TODO!
        //    // HANDLE NOT PUTTING PAST EVENTS ON TOP

        //    var events = await _context.CatalogEvents
        //        .OrderBy(e => e.Start) // Soonest events first
        //        .Skip(pageSize * pageIndex)
        //        .Take(pageSize)
        //        .ToListAsync();


        //    events.ForEach(e =>
        //        e.PictureUrl = e.PictureUrl.Replace(
        //        "http://externalcatalogbaseurltobereplaced",
        //        _config["ExternalCatalogBaseUrl"]));


        //    var viewModel = new PaginatedItemsViewModel<CatalogEvent>
        //    {
        //        PageIndex = pageIndex,
        //        PageSize = pageSize,
        //        ItemCount = await _context.CatalogEvents.LongCountAsync(),
        //        Data = events
        //    };

        //    return Ok( viewModel );
        //}



        [HttpGet()]
        [Route("[action]")]
        public async Task<IActionResult> Events(
            [FromQuery]int? catalogFormatId,
            [FromQuery]int? catalogTopicId,
            [FromQuery]string earliestStart,
            [FromQuery]string latestStart,
            // [FromQuery]bool? hasFreeTicketType, <- Have to come back to this one after get TicketType in, or abort on it and go with simple pricing
            [FromQuery]int pageIndex = 0,
            [FromQuery]int pageSize = 2)
        {
            // TODO:
            // Why comparing an int with a null nullable int doesn't
            // compiler error or crash...seems like should be type mismatch
            // or null exception
            // (No example of this remains, but I saw it happen!)

            var query = (IQueryable<CatalogEvent>)_context.CatalogEvents;

            if (catalogFormatId.HasValue)
            {
                query = query.Where(e =>
                    e.CatalogFormatId == catalogFormatId.Value);
            }

            if (catalogTopicId.HasValue)
            {
                query = query.Where(e =>
                    e.CatalogTopicId == catalogTopicId.Value);
            }

            DateTime earliestStartDateTime;
            if (DateTime.TryParse(earliestStart, out earliestStartDateTime))
            // Ignore earliestStart parameter if cannot parse to DateTime
            {
                // If date but no time is specified, Parse() defaults
                // appropriately to 12:00am

                query = query.Where(e =>
                    // Where event starts after earliest start time
                    e.Start >= earliestStartDateTime);
            }

            DateTime latestStartDateTime;
            if (DateTime.TryParse(latestStart, out latestStartDateTime))
            // Ignore latestStart parameter if cannot parse to DateTime
            {
                // If paramter specifies date but not time,
                var latestStartLower = latestStart.ToLower();
                if (false == (latestStartLower.Contains(':') ||
                                latestStartLower.Contains("am") ||
                                latestStartLower.Contains("pm")))
                {
                    // Set to 11:59:59pm on that date
                    latestStartDateTime = latestStartDateTime.AddHours(23).AddMinutes(59).AddSeconds(59);
                }

                query = query.Where(e =>
                    // Where event starts before latest start time
                    e.Start <= latestStartDateTime);
            }

            var events = await query
                .OrderBy(e => e.Start) // Soonest (or oldest past) events first
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            events.ForEach(e =>
                e.PictureUrl = e.PictureUrl.Replace(
                "http://externalcatalogbaseurltobereplaced",
                _config["ExternalCatalogBaseUrl"]));

            var viewModel = new PaginatedItemsViewModel<CatalogEvent>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ItemCount = await query.LongCountAsync(),
                Data = events
            };

            // TODO:
            // help. how does this work? (these two db commands commented out below)
            // database caching??
            // For the objects that the events and query collection have in common,
            // they point to the same object in memory. Specifically, you can
            // use the events reference to modify the in-memory object,
            // then if you:
            //_context.SaveChanges();
            //_context.Database.Migrate();
            // the replaced PictureUrls would get committed to the db
            // What is the lifetime of edits to the in-memory db objects?
            // If we don't save changes and update the db here (which we wouldn't)
            // are we still at risk of having these changes inadvertently
            // written to the db if these commands are executed later?

            return Ok(viewModel);
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> RandomEvents(
            [FromQuery]int? itemCount = 6)
            // TODO:
            // FIX BUG WHERE THE CALLER CAN REQUEST MORE RANDOM EVENTS
            // THAN TOTAL EVENTS IN THE CATALOG

            // 6 is expected based on current wireframe of our front-end
        {
            var totalEventsCount = await _context.CatalogEvents.LongCountAsync();

            var randomEvents = new List<CatalogEvent>();
            int validatedItemCount;

            if (itemCount.Value <= 0)
            {
                validatedItemCount = 0;
                // Bad or trivial request
                // Leave the list empty
            }
            else // Nontrivial request
            {
                validatedItemCount = itemCount.Value;

                // If there are very many events, restict domain of the random sample
                // to the first Int32.MaxValue events
                var cappedEventsDomainCount = totalEventsCount <= Int32.MaxValue ? (int)totalEventsCount : Int32.MaxValue;

                // Zero-based indicies (not one-based like the Ids/row nums in the db)
                var randomIndicies = ChooseUniqueRandomValuesInclusive(
                                        validatedItemCount,
                                        0,
                                        cappedEventsDomainCount - 1,
                                        randNumGen)
                                     .ToList();

                // Need indicies in ascending order so we can set up the query
                // using Skip and Take/First
                randomIndicies.Sort();

                var query = (IQueryable<CatalogEvent>)_context.CatalogEvents;


                // Set up for the query construction loop: do the initial skip-and-take
                // This is more intuitive than setting prevIndex to -1 to allow for doing
                // the first skip-and-take within the loop
                var randomIndiciesEnumerator = randomIndicies.GetEnumerator();
                // For some reason, a new enumerator doesn't point to the first object?
                randomIndiciesEnumerator.MoveNext();
                var firstIndex = randomIndiciesEnumerator.Current;
                query = query
                            .Skip(firstIndex)
                            // Can't use First() since it returns the object, not an IQueryable
                            .Take(1);
                // Note that we've validated that validatedItemCount >= 1, so this is safe

                if (randomIndicies.Count > 1)
                {
                    var prevIndex = firstIndex;
                    while (randomIndiciesEnumerator.MoveNext())
                    {
                        var currIndex = randomIndiciesEnumerator.Current;

                        query = query
                                .Skip(currIndex - prevIndex - 1)
                                .Take(1);
                        
                        // Update for next loop
                        prevIndex = currIndex;
                    }
                }
                // TODO:
                // Refactor all of this to avoid confusing use of "index" and "value" terms



                //BUG IS FIRST EVIDENT HERE...
                randomEvents = await query.ToListAsync();
                // The skips and takes looked fine
                // Why is ToListAsync giving an empty list?



                // QUESTION:
                // Is there a better way to construct this query than Skip and Take/First?
                // I wanted to use ElementAt() and then join the resulting IQueryable collections
                // but I can't seem to find a way to do that which doesn't create a new
                // collection to be the joined result
                // Is it true that, in terms of "modifying an IQueryable in-place,"
                // you can only narrow/reduce/further refine a query, but you can't
                // expand it, even if the objects that you want to expand it with
                // came from the original collection?
                // Waiiiit...ElementAt() returns an object not an IQueryable
                // This would not have worked anyway.





                // NOW A BUNCH OF EXPERIMENTING TO TRY AND FIGURE OUT HOW
                // ALL OF THIS WORKS:

                //// What happens if we try to access the events from the db
                //// one-at-a-time, each in their own query...does that even work?
                //// can you use non-async linq query methods to query an EF db?

                //try
                //{
                //    var anEvent = _context.CatalogEvents.ElementAt(0);

                //} catch (Exception e)
                //{
                //    // "Processing of the LINQ expression 'DbSet<CatalogEvent>\n    .ElementAt(__p_0)' by 'NavigationExpandingExpressionVisitor' failed. This may indicate either a bug or a limitation in EF Core. See https://go.microsoft.com/fwlink/?linkid=2101433 for more detailed information."
                //}







                //var newList = await _context.CatalogEvents.Skip(3).Take(1).ToListAsync();
                // Can take
                // Can skip, then take
                // If skip, take, then skip again, THIS DOESN'T WORK
                // Hrm...
                // Need to skip what you just took?
                // If take again after already took, has no effect.
                // If skip after take, apparently destroys the query
                // Ok, how is correct way to do this?
                // It is true that we want to get all of these in a single query to the db, right?





                //// EXPERIMENTS WITH DELETE/REMOVE...
                //// To try and determine if an event's zero-based index will always be Id-1


                //var listBeforeDelete = await _context.CatalogEvents.ToListAsync();


                //// "The most effective way if you know ID and don't have entity loaded is to create fake entity and delete it"
                //// https://stackoverflow.com/a/12831161
                ////var catEventToDeleteById = new CatalogEvent { Id = 5 }; // Rhinos
                //// But apparently we do have it loaded, as this call:
                //// _context.CatalogEvents.Remove(catEventToDeleteById);
                //// Throws InvalidOperationException:
                //// "The instance of entity type 'CatalogEvent' cannot be tracked because another instance with the same key value for {'Id'} is already being tracked. When attaching existing entities, ensure that only one entity instance with a given key value is attached. Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the conflicting key values."

                ////var catEventToDelete = _context.CatalogEvents


                ////_context.CatalogEvents.ExecuteSqlCommand();
                //// By Executing SQL Query
                //// https://www.c-sharpcorner.com/UploadFile/ff2f08/delete-an-object-without-retrieving-it-in-entity-framework-6/


                ////var catEventToDeleteById = new CatalogEvent { Id = 5 }; // Rhinos
                ////_context.Entry(catEventToDeleteById).State = EntityState.Deleted;
                //// By Changing State
                //// https://www.c-sharpcorner.com/UploadFile/ff2f08/delete-an-object-without-retrieving-it-in-entity-framework-6/
                //// But doesn't work beacuse "tracked?"


                //// Ok, fine.
                //// Hit the db to get the event that we want to delete...
                //// even though we already knew the Id of it *sigh*
                //var catEventToBeDeleted = await _context.CatalogEvents.FindAsync(5);

                //_context.CatalogEvents.Remove(catEventToBeDeleted);
                //_context.SaveChanges();
                //// Oh! Do rememebr that we are changing the actual db here.
                //// Changes to db persist on a run-to-run basis
                //// What if clean and rebuild?
                //// Do we get a clean, new db?
                //// Yes, clean and rebuild gives a clean db and re-seeds it (for better or worse)


                //var listAfterDelete = await _context.CatalogEvents.ToListAsync();

                //var nowWhatDoesFiveMean = await _context.CatalogEvents.FindAsync(5);
                // Event with Id 6 moved into the rhino spot (fifth position)
                // Event Ids do not change when an event is deleted

                // FindAsync(5) returns null when no event could be found that
                // where Id == 5 (regardless of its current position/index in the db)


                //REMAINING OPTIONS:
                // OPTION A) Rethink Skip and Take:
                // figure out how to use them with my zero - based db index approach
                //   Even if the only way to do this would be to construct a new
                //   query(and cause a separate db hit) to get each random event...
                // would be curious to observe this working, even if only for learning
                //  purposes, since all of the infrastructure is already in place.
                // IF it works, it would likely be the quickest way to adapt what
                //  I've already written to work

                // OPTION B) Re-purpose current randomly generated zero-based index collection
                // as a "parallel" collection to "the collection of" event Ids in the db
                // This is reminiscent of a common non-Fisher-Yates randomization alternative
                // Query the db for a collection of all Ids
                // Use the randomly generated zero-based index collection to choose
                // Ids from the collection of all Ids, create a new collection of those
                // chosen Ids
                // Query the db again using the collection of chosen Ids to get
                // the corresponding events

                // OPTION C) Rework ChooseUniqueRandomValuesInclusive to take in an existing
                // collection of values(rather than generating a sequence of adjacent values
                // based on a min and a max)
                // Query the db for a collection of all Ids to pass to the Choose method
                // Query the db again using the collection of chosen Ids to get
                // the corresponding events

                // BEFORE DECIDING WHICH OPTION TO DO...
                // Do a proof-of-concept demo of constructing a query for events
                // based on a collection of known Ids
                // Can we achieve this using a single async db access?
                // That is: "Get the events that match these Ids (the events corresponding
                // to the Ids in this parameter collection?)
                // Probably a lambda (maybe even nested ones?) in there somewhere?


            }



            // Construct a ViewModel object to send back over the wire
            // Remember that this will be deserialized into an object on the other end
            // Will need to develop a Model type on the WebMVC end for this purpose
            var viewModel = new RandomItemsViewModel<CatalogEvent>
            {
                RandomItemCount = validatedItemCount,
                TotalItemCount = totalEventsCount,
                Data = randomEvents
            };

            return Ok(viewModel);
        }






        // Maybe revise to allow other types for random values to be created
        // (such as long, which is the default db collection count/length/size)
        // Umm...I tried to make this generic and yikes is that more complicated
        // that you would expect:
        // https://stackoverflow.com/questions/32664/is-there-a-constraint-that-restricts-my-generic-method-to-numeric-types/4834066#4834066
        // Short-term hacky solution is to limit the indices of the source
        // collection on which to operate to be only up to Int32.MaxValue
        // About the idea of using an unsigned type:
        // I think that I remember reading something in a credible source advising
        // the use (in C#? in dotnetcore?) of signed types in places where you might
        // naively use unsigned types. *sigh* I don't recall the rationale.
        // Anyway, this solution is not ideal. Interested to explore more robust solutions.
        private static IEnumerable<int> ChooseUniqueRandomValuesInclusive
            (int desiredCount,
             int minValue,
             int maxValue,
             Random randNumGen = null)
        {
            if (randNumGen == null)
            {
                randNumGen = new Random(DateTime.Now.Millisecond);
            }

            // Construct the initial array of values specified by the caller
            // inclusive of both minValue and maxValue
            var valuesCount = maxValue - minValue + 1;
            var valueArray = new int[valuesCount];
            valueArray[0] = minValue;
            for (int i = 1; i < valuesCount; ++i)
            {
                valueArray[i] = 1 + valueArray[i - 1];
            }

            ShuffleElementsInPlace(valueArray, randNumGen);

            // Return the first n elements where n is the count
            // of values that the caller specified
            return valueArray.Take(desiredCount);
        }

        private static void ShuffleElementsInPlace<T>
            (T[] objectArray,
             Random randNumGen = null)
        {
            if (randNumGen == null)
            {
                randNumGen = new Random(DateTime.Now.Millisecond);
            }


            // Given an indexable collection of N objects (any type)

            // Randomize these objects, in-place, in O(n) (constant time).
            // Use a method that ensures every possible permutation outcome is equally likely
            // (Assume that pseudorandom number generator supplies adequately
            // random values.)


            // Random value results can range from 0 to fruitCount - 1
            // that is, [0, fruitCount)
            // This suits the needs of our zero-based indexable collection.


            // Algorithm:

            // From all objects in a collection, choose a random object
            // (represented by its index, let that index be i) to be the last object in the collection
            // (at index length - 1)
            // if you randomly chose the last object (i == length - 1), no swap and move on to next loop.
            // if you randomly chose a different object (i != length - 1), swap the object at index i
            // with the last object in the collection (object at index length - 1). (use a temp var to do the swap)

            // Now the last object in the collection (at index length - 1) has been randomly chosen
            // and is in its final randomized position.

            // Repeat the process to choose a random object in the collection (new i)
            // to be the next-to-last object in the collection (length - 2)



            // When to stop?
            // briefly consider when to "not start" the process to begin with:

            // empty collection (length 0) is by definition "already randomized"
            // similarly, collection of length 1 is already randomized (there is one object at index 0)
            // we do, however, need to randomize a collection of length 2 (and all greater lengths)
            // one object at index 0 and one object at index 1

            // So, we'll never need to explicitly randomly choose an object for position 0 (length - length)
            // the last swap (or lack-of-swap) does that automatically.
            // The final position that we need to determine the object for is
            // position 1 (length - length + 1)

            // So, last loop will be for considering (randomly) whether or not to swap
            // the first (index 0) and second (index 1) objects of the collection
            // that is, when we are determining which object should be at index 1

            // so, let's let the index of the first postition that we determine
            // the object for (the last position in the collection) be k
            // (avoiding i,j indexing as that suggests nested loops and/or 2d arrays)
            // start with k = length - 1
            // final loop iteration is when k == 1, or continue while k >= 1


            for (var k = objectArray.Length - 1; k >= 1; --k)
            // k starts at the last index in the fruits collection and walks
            // backwards towards the first index.
            // last (possible) swap is for when k == 1 (swap would be of objects at [0] and [1])
            {
                var i = randNumGen.Next(0, k + 1);
                // Recall that Random.Next generates values in the range: [min, max)
                // but we want k to be a possible result: [0, k], so we'll use [0, k+1)

                if (i == k)
                {
                    // Object at index k has randomly been determined to be (remain at) index k
                }
                else
                {
                    // Object at index i randomly determined to be at (move to) index k
                    // Object at index k takes the spot of object at index i (yet to be randomized...unless this is the last iteration)
                    var tempObject = objectArray[k];
                    objectArray[k] = objectArray[i];
                    objectArray[i] = tempObject;
                }
            }
        }
    }
}







//For front page features:
//N random(default 6) events of Format X
//N random(default 6) events of Topic X

//(Best way to specify format? topic?)
//    String?
//    Index?
//    Other?


//TicketType table(as per convo with Ainur on Mon eve)


//Add functionality to existing Events API:
//    For filter page:
//Price filter<- Will have to come back to this one after get TicketType in, or abort on it and go with simple pricing
//    hasAFreeTicketType
