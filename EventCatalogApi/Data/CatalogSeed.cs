using EventLite.Domain.EventLite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventCatalogApi.Data
{
    public static class CatalogSeed
    {
        public static void Seed(CatalogContext context)
        {
            // Remember that you Migrate() a Database, not a context
            context.Database.Migrate();

            // Only seed db if all tables are empty
            if (!context.CatalogFormats.Any() &&
                !context.CatalogTopics.Any() &&
                !context.CatalogEvents.Any() &&
                !context.CatalogTicketTypes.Any())
            {
                List<CatalogEvent>      preconfigEvents;
                List<CatalogTicketType> preconfigTicketTypes;

                GetPreconfiguredTableData(
                    out preconfigEvents,
                    out preconfigTicketTypes);

                context.CatalogFormats.AddRange( GetPreconfiguredCatalogFormats() );
                context.SaveChanges();

                context.CatalogTopics.AddRange( GetPreconfiguredCatalogTopics() );
                context.SaveChanges();

                context.CatalogEvents.AddRange(preconfigEvents);
                context.SaveChanges();

                context.CatalogTicketTypes.AddRange(preconfigTicketTypes);
                context.SaveChanges();
            }

        }

        private static void GetPreconfiguredTableData(
            out List<CatalogEvent>       events,
            out List<CatalogTicketType>  tickets)
        {
            // Need because can't refer to a class property by name during
            // object initialization
            DateTime storedStart;

            events = new List<CatalogEvent>();
            tickets = new List<CatalogTicketType>();

            events.Add(new CatalogEvent
            {
                Title = "14th Annual Soup Festival",
                Description = "Soup Festival is all about soup! The ultimate liquid meal.",
                // Always starts "tomorrow" Aren't you lucky!
                Start = storedStart = DateTime.Today.AddDays(1).AddHours(12),
                End = storedStart.AddHours(9),
                PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/1",
                VenueName = "Washington State Convention Center",
                VenueAddressLine1 = "705 Pike St.",
                VenueCity = "Seattle",
                VenueStateProvince = "WA",
                VenuePostalCode = "98101",
                VenueMapUrl = "http://TODOREPLACEME/",

                // TODO: Understand why this syntax works for CatalogVenue property vs.
                // Hypothetical Venue2 field (see CatalogEvent)	
                //CatalogVenue = new CatalogVenue	
                //{	
                //    Name = "Washington State Convention Center",	
                //    AddressLine1 = "705 Pike St.",	
                //    City = "Seattle",	
                //    StateProvince = "WA",	
                //    PostalCode = "98101",	
                //    MapUrl = "http://TODOREPLACEME/"	
                //},
                HostOrganizer = "Fancy Food Company",
                CatalogFormatId = 3,
                CatalogTopicId = 3,
                TotalTicketLimitAllTypes = 2000,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "General Admission",
                Description = "All you can slurp!",
                Price = 20.00M,
                TicketLimitThisType = 2000,
                SalesEndOn = storedStart.AddDays(-1),
                // Id of CatalogEvent just added
                // ! Depends on each set of CatalogTicketType being added right after its CatalogEvent
                // is added.
                CatalogEventId = events.Count,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "Child under 13",
                Description = "With ticketed adult.",
                Price = 0.00M,
                TicketLimitThisType = 2000,
                SalesEndOn = storedStart.AddDays(-1),
                // Id of CatalogEvent just added
                // ! Depends on each set of CatalogTicketType being added right after its CatalogEvent
                // is added.
                CatalogEventId = events.Count,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "Souper Slurper VIP",
                Description = "Includes access to exclusive VIP restroom and commemorative spoon.",
                Price = 240.00M,
                TicketLimitThisType = 50,
                SalesEndOn = storedStart.AddDays(-1),
                CatalogEventId = events.Count,
            });


            events.Add(new CatalogEvent
            {
                Title = "Spirit of Music",
                Description = "A new concert experience! Discover our new artists from the comfort of your own home!",
                Start = storedStart = DateTime.Today.AddDays(2).AddHours(19),
                End = storedStart.AddHours(4),
                PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/2",
                VenueName = "Zoom",
                VenueAddressLine1 = "111 Zoom St.",
                VenueCity = "Seattle",
                VenueStateProvince = "WA",
                VenuePostalCode = "98100",
                VenueMapUrl = "http://TODOREPLACEME/",
                HostOrganizer = "Fancy Music Company",
                CatalogFormatId = 1,
                CatalogTopicId = 1,
                TotalTicketLimitAllTypes = 5000,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "General Admission",
                Description = "Pajamas permitted.",
                Price = 15.00M,
                TicketLimitThisType = 5000,
                SalesEndOn = storedStart.AddDays(-1),
                // Id of CatalogEvent just added
                // ! Depends on each set of CatalogTicketType being added right after its CatalogEvent
                // is added.
                CatalogEventId = events.Count,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "Couch VIP",
                Description = "Commemorative autograph mailed to your home.",
                Price = 100.00M,
                TicketLimitThisType = 35,
                SalesEndOn = storedStart.AddDays(-1),
                CatalogEventId = events.Count,
            });



            events.Add(new CatalogEvent
            {
                Title = "11th Annual Music Education Fundraiser",
                Description = "Come to our event to support keeping Music as part of the standard school curriculum!",
                Start = storedStart = DateTime.Today.AddDays(4).AddHours(17),
                End = storedStart.AddHours(2),
                PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/3",
                VenueName = "Washington School",
                VenueAddressLine1 = "100 Madison Ave.",
                VenueCity = "Seattle",
                VenueStateProvince = "WA",
                VenuePostalCode = "98123",
                VenueMapUrl = "http://TODOREPLACEME/",
                HostOrganizer = "Fancy School",
                CatalogFormatId = 2,
                CatalogTopicId = 1,
                TotalTicketLimitAllTypes = 75,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "Supporter",
                Description = "General seating. Our thanks!",
                Price = 25.00M,
                TicketLimitThisType = 40,
                SalesEndOn = storedStart.AddDays(-1),
                CatalogEventId = events.Count,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "Sustainer",
                Description = "Second row center seat or front row side seat. Our deep appreciation!",
                Price = 75.00M,
                TicketLimitThisType = 25,
                SalesEndOn = storedStart.AddDays(-1),
                CatalogEventId = events.Count,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "Visionary",
                Description = "Front and center seat. Our undying admiration!",
                Price = 150.00M,
                TicketLimitThisType = 10,
                SalesEndOn = storedStart.AddDays(-1),
                CatalogEventId = events.Count,
            });

            events.Add(new CatalogEvent
            {
                Title = "Indoor Lands!",
                Description = "The Premiere Indoor Music Festival, featuring at home performances from John Legend, Coldplay and Taylor Swift!",
                Start = storedStart = DateTime.Today.AddDays(7).AddHours(17),
                End = storedStart.AddHours(6),
                PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/4",
                VenueName = "Google Hangouts",
                VenueAddressLine1 = "10 Google Drive",
                VenueCity = "Seattle",
                VenueStateProvince = "WA",
                VenuePostalCode = "98144",
                VenueMapUrl = "http://TODOREPLACEME/",
                HostOrganizer = "Ja Rule",
                CatalogFormatId = 3,
                CatalogTopicId = 3,
                TotalTicketLimitAllTypes = 10000,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "General Admission",
                Description = "Your couch. Our music.",
                Price = 30.00M,
                TicketLimitThisType = 100000,
                SalesEndOn = storedStart.AddDays(-1),
                CatalogEventId = events.Count,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "With Exclusive Pre-Show Hangout: John Legend",
                Description = "Chill with John Legend on video chat before the show.",
                Price = 375.00M,
                TicketLimitThisType = 20,
                SalesEndOn = storedStart.AddDays(-1),
                CatalogEventId = events.Count,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "With Exclusive Pre-Show Hangout: Coldplay",
                Description = "Chill with Coldplay on video chat before the show.",
                Price = 375.00M,
                TicketLimitThisType = 20,
                SalesEndOn = storedStart.AddDays(-1),
                CatalogEventId = events.Count,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "With Exclusive Pre-Show Hangout: Taylor Swift",
                Description = "Chill with Taylor Swift on video chat before the show.",
                Price = 375.00M,
                TicketLimitThisType = 20,
                SalesEndOn = storedStart.AddDays(-1),
                CatalogEventId = events.Count,
            });



            events.Add(new CatalogEvent
            {
                Title = "Save the Rhinos Fundraiser",
                Description = "Join us to help protect the endangered Black Rhino from extinction!",
                Start = storedStart = DateTime.Today.AddDays(4).AddHours(17),
                End = storedStart.AddHours(2),
                PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/5",
                VenueName = "AXIS Pioneer Square",
                VenueAddressLine1 = "308 1st Street",
                VenueCity = "Seattle",
                VenueStateProvince = "WA",
                VenuePostalCode = "98104",
                VenueMapUrl = "http://TODOREPLACEME/",
                HostOrganizer = "Captain Planet",
                CatalogFormatId = 2,
                CatalogTopicId = 2,
                TotalTicketLimitAllTypes = 75,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "Supporter",
                Description = "General seating. Our thanks!",
                Price = 100.00M,
                TicketLimitThisType = 40,
                SalesEndOn = storedStart.AddDays(-1),
                CatalogEventId = events.Count,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "Sustainer",
                Description = "Second row center seat or front row side seat. Our deep appreciation!",
                Price = 200.00M,
                TicketLimitThisType = 25,
                SalesEndOn = storedStart.AddDays(-1),
                CatalogEventId = events.Count,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "Visionary",
                Description = "Front and center seat. Our undying admiration!",
                Price = 300.00M,
                TicketLimitThisType = 10,
                SalesEndOn = storedStart.AddDays(-1),
                CatalogEventId = events.Count,
            });


            events.Add(new CatalogEvent
            {
                Title = "Music is Life Concert",
                Description = "Music is life! Brought to you by Hershey's.",
                Start = storedStart = DateTime.Today.AddDays(1).AddHours(15),
                End = storedStart.AddHours(4),
                PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/6",
                VenueName = "Canvas Event Space",
                VenueAddressLine1 = "3412 4th Ave.",
                VenueCity = "Seattle",
                VenueStateProvince = "WA",
                VenuePostalCode = "98134",
                VenueMapUrl = "http://TODOREPLACEME/",
                HostOrganizer = "Hershey's Chocolates",
                CatalogFormatId = 1,
                CatalogTopicId = 3,
                TotalTicketLimitAllTypes = 2000,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "General Admission",
                Description = "Have a sweet time.",
                Price = 20.00M,
                TicketLimitThisType = 2000,
                SalesEndOn = storedStart.AddDays(-1),
                // Id of CatalogEvent just added
                // ! Depends on each set of CatalogTicketType being added right after its CatalogEvent
                // is added.
                CatalogEventId = events.Count,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "Double Dipper VIP",
                Description = "Unlimited visits to the exclusive chocolate fountain lounge.",
                Price = 240.00M,
                TicketLimitThisType = 50,
                SalesEndOn = storedStart.AddDays(-1),
                CatalogEventId = events.Count,
            });


            events.Add(new CatalogEvent
            {
                Title = "Foodie Fundraiser",
                Description = "Support local restaurants get back in action after COVID-19.",
                Start = storedStart = DateTime.Today.AddDays(15).AddHours(11),
                End = storedStart.AddHours(3),
                PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/7",
                VenueName = "Seattle Design Center Events",
                VenueAddressLine1 = "5701 6th Ave. S",
                VenueCity = "Seattle",
                VenueStateProvince = "WA",
                VenuePostalCode = "98108",
                VenueMapUrl = "http://TODOREPLACEME/",
                HostOrganizer = "Tom Douglas",
                CatalogFormatId = 2,
                CatalogTopicId = 3,
                TotalTicketLimitAllTypes = 75,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "Supporter",
                Description = "General seating. Our thanks!",
                Price = 100.00M,
                TicketLimitThisType = 40,
                SalesEndOn = storedStart.AddDays(-1),
                CatalogEventId = events.Count,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "Sustainer",
                Description = "Second row center seat or front row side seat. Our deep appreciation!",
                Price = 200.00M,
                TicketLimitThisType = 25,
                SalesEndOn = storedStart.AddDays(-1),
                CatalogEventId = events.Count,
            });
            tickets.Add(new CatalogTicketType
            {
                Name = "Visionary",
                Description = "Front and center seat. Our undying admiration!",
                Price = 300.00M,
                TicketLimitThisType = 10,
                SalesEndOn = storedStart.AddDays(-1),
                CatalogEventId = events.Count,
            });

            return;
        }

        private static IEnumerable<CatalogTopic> GetPreconfiguredCatalogTopics()
        {
            return new List<CatalogTopic>
            {
                new CatalogTopic { Topic = "Music" },
                new CatalogTopic { Topic = "Animals and Pets" },
                new CatalogTopic { Topic = "Food and Drink" },
            };
        }

        private static IEnumerable<CatalogFormat> GetPreconfiguredCatalogFormats()
        {
            return new List<CatalogFormat>
            {
                new CatalogFormat { Format = "Concert" },
                new CatalogFormat { Format = "Fundraiser" },
                new CatalogFormat { Format = "Festival" }
            };

        }
    }
}
