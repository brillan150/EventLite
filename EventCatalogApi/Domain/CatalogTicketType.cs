using System;

namespace EventLite.Domain.EventLite
{
    // CatalogTicketType objects are specific to each CatalogEvents
    // CatalogTicketType does not need a table in the db.
    public class CatalogTicketType
    {
        public int Id { get; set; }

        // Name of ticket type
        // ex. "Souper VIP"
        public string Name { get; set; }

        // Description of the ticket type
        // ex. "Includes: Admission one hour early. Reserved parking. Commemorative spoon."
        public string Description { get; set; }

        // Price of this ticket type in USD.
        public decimal Price { get; set; }

        // Total ticket sales limit for this ticket type (across all orders).
        // Note: This ticket type may become unavailable before its limit
        // is reached if the event's TotalTicketLimitAllTypes is reached first.
        public int TicketLimitThisType { get; set; }

        // Latest date and time permitted for sales of this
        // ticket type.
        public DateTime SalesEndOn { get; set; }




        // Each CatalogEvent has one or more CatalogTicketType,
        // essentially, a collection of CatalogTicketType
        // Each CatalogTicketType is specified custom for each Event
        // That is, each CatalogEvent has many CatalogTicketType
        // Each CatalogTicketType has one CatalogEvent
        // (TicketType that happen to be identical between different CatalogEvent
        // are not associted in any way)

        // Foreign Key
        public int CatalogEventId { get; set; }

        // Navigational Property
        public virtual CatalogEvent CatalogEvent { get; set; }
    }
}
