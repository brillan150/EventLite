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


            if (false == context.CatalogFormats.Any())
            {
                context.CatalogFormats.AddRange( GetPreconfiguredCatalogFormats() );
            }

            if(!context.CatalogTopics.Any())
            {
                context.CatalogTopics.AddRange(GetPreconfiguredCatalogTopics());
            }

            context.SaveChanges();

            if (false == context.CatalogEvents.Any())
            {
                context.CatalogEvents.AddRange( GetPreconfiguredCatalogEvents() );
                
            }

            context.SaveChanges();

        }

        private static IEnumerable<CatalogEvent> GetPreconfiguredCatalogEvents()
        {
            DateTime storedStart;

            return new List<CatalogEvent>
            {

                new CatalogEvent
                {

                    Title = "FIRST: 14th Annual Soup Festival",
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


                    // TODO: Understand why this syntax works for Venue property vs.
                    // Hypothetical Venue2 field (see CatalogEvent)
                    //Venue = new Venue
                    //{
                    //    Name = "Washington State Convention Center",
                    //    AddressLine1 = "705 Pike St.",
                    //    City = "Seattle",
                    //    StateProvince = "WA",
                    //    PostalCode = "98101",
                    //    MapUrl = "http://TODOREPLACEME/"
                    //},
                    HostOrganizer = "Fancy Food Company",
                    CatalogFormatId = 1,
                    CatalogTopicId = 2,


                    // TODO: Snap to nearest future Saturday
                   
                },
                new CatalogEvent
                {

                    Title = "SECOND: 14th Annual Soup Festival",
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


                    // TODO: Understand why this syntax works for Venue property vs.
                    // Hypothetical Venue2 field (see CatalogEvent)
                    //Venue = new Venue
                    //{
                    //    Name = "Washington State Convention Center",
                    //    AddressLine1 = "705 Pike St.",
                    //    City = "Seattle",
                    //    StateProvince = "WA",
                    //    PostalCode = "98101",
                    //    MapUrl = "http://TODOREPLACEME/"
                    //},
                    HostOrganizer = "Fancy Food Company",
                    CatalogFormatId = 2,
                    CatalogTopicId = 2,


                    // TODO: Snap to nearest future Saturday
                   
                },
                new CatalogEvent
                {

                    Title = "THIRD: 14th Annual Soup Festival",
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


                    // TODO: Understand why this syntax works for Venue property vs.
                    // Hypothetical Venue2 field (see CatalogEvent)
                    //Venue = new Venue
                    //{
                    //    Name = "Washington State Convention Center",
                    //    AddressLine1 = "705 Pike St.",
                    //    City = "Seattle",
                    //    StateProvince = "WA",
                    //    PostalCode = "98101",
                    //    MapUrl = "http://TODOREPLACEME/"
                    //},
                    HostOrganizer = "Fancy Food Company",
                    CatalogFormatId = 1,
                    CatalogTopicId = 1,


                    // TODO: Snap to nearest future Saturday
                   
                },

                new CatalogEvent
                {

                    Title = "FOURTH: 14th Annual Soup Festival",
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


                    // TODO: Understand why this syntax works for Venue property vs.
                    // Hypothetical Venue2 field (see CatalogEvent)
                    //Venue = new Venue
                    //{
                    //    Name = "Washington State Convention Center",
                    //    AddressLine1 = "705 Pike St.",
                    //    City = "Seattle",
                    //    StateProvince = "WA",
                    //    PostalCode = "98101",
                    //    MapUrl = "http://TODOREPLACEME/"
                    //},
                    HostOrganizer = "Fancy Food Company",
                    CatalogFormatId = 1,
                    CatalogTopicId = 2,


                    // TODO: Snap to nearest future Saturday
                   
                },



            };
        }

        private static IEnumerable<CatalogTopic> GetPreconfiguredCatalogTopics()
        {
            return new List<CatalogTopic>
            {
                new CatalogTopic {Topic="Music"},
                new CatalogTopic {Topic="Animals and pets"},
                new CatalogTopic{Topic="Food of Drink"},
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
