using System;

namespace EventLite.Domain.EventLite
{
    // TicketType objects are specific to each CatalogEvent
    // TicketType does not need a table in the db.
    public class TicketType
    {
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
    }
}
