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

            if (!context.CatalogFormats.Any())
            {
                context.CatalogFormats.AddRange( GetPreconfiguredCatalogFormats());
                context.SaveChanges();
            }

            if(!context.CatalogTopics.Any())
            {
                context.CatalogTopics.AddRange(GetPreconfiguredCatalogTopics());
                context.SaveChanges();
            }

            if (!context.CatalogEvents.Any())
            {
                context.CatalogEvents.AddRange( GetPreconfiguredCatalogEvents() );
                context.SaveChanges();
            }

        }

        private static IEnumerable<CatalogEvent> GetPreconfiguredCatalogEvents()
        {
            DateTime storedStart;

            return new List<CatalogEvent>
            {

                new CatalogEvent
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
                    CatalogFormatId = 3,
                    CatalogTopicId = 3,
                  
                },

                new CatalogEvent
                {

                    Title = "Concert 1",
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

                },

                new CatalogEvent
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
                },
                new CatalogEvent
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
                },
                new CatalogEvent
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
                },
                new CatalogEvent
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
                },

                new CatalogEvent
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
                },

            };
        }

        private static IEnumerable<CatalogTopic> GetPreconfiguredCatalogTopics()
        {
            return new List<CatalogTopic>
            {
                new CatalogTopic {Topic="Music"},
                new CatalogTopic {Topic="Animals and Pets"},
                new CatalogTopic {Topic="Food and Drink"},
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
