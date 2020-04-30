using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models.Common
{
    public class CatalogEvent
    {
        // Primary key
        public int Id { get; set; }

        // Title of the event.
        // Ex. "14th Annual Soup Festival"
        public string Title { get; set; }

        // Plain text description of event
        // "Soup Festival is all about soup! The ultimate liquid meal..."
        public string Description { get; set; }

        // Start date and time of event (in time zone of venue's address)
        public DateTime Start { get; set; }

        // End date and time of event (in time zone of venue's address)
        public DateTime End { get; set; }
        // default is zero duration, that is, Start == End

        // Picture representing the event in .png file format
        public string PictureUrl { get; set; }

        // Street address of the event's location
        public string VenueName { get; set; }
        public string VenueAddressLine1 { get; set; }
        public string VenueAddressLine2 { get; set; }
        public string VenueAddressLine3 { get; set; }
        public string VenueCity { get; set; }
        public string VenueStateProvince { get; set; }
        public string VenuePostalCode { get; set; }

        public string VenueMapUrl { get; set; }


        // "Fancy Food Company"
        public string HostOrganizer { get; set; }

        // STILL TODO ON API END:
        // Total number of ticket sales allowed for this event
        // across all types of tickets offered.
        //public int TotalTicketLimitAllTypes { get; set; }

        // STILL TODO ON API END:
        // Price, ticket limit and sales end date for each type of ticket offered
        // for this event.
        //public List<TicketType> TicketTypes { get; set; }


        // Foreign keys
        public int CatalogFormatId { get; set; }
        public int CatalogTopicId { get; set; }

        public string CatalogFormat { get; set; }
        public string CatalogTopic { get; set; }


    }
}
