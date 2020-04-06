namespace EventLite.Domain.EventLite
{
    // Site design does not support looking up, sorting or filtering by venue,
    // so Venue does not need to be a table in the db, right?
    // Given that, does having a Venue class type cause any sort of issue with
    // EntityFramework, the db, or anything else?
    public class Venue
    {
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }

        public string MapUrl { get; set; }
        // Should MapUrl be a property on the Venue type or not?
        // it is assembled from the other properties at
        // some point. Whose job should this be?
        // My guess is that the CatalogController API method(s)
        // should handle this before passing event data
        // back to the user of the API.

        // Does it make any sense in this case (or in general)
        // for there to be two versions of a domain/model type
        // within a microservice? No state is maintained in the
        // MapUrl property, so it seems like a waste to have this field
        // stored for each CatalogEvents in the database. Thoughts?
    }
}
