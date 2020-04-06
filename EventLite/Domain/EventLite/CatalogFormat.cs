using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventLite.Domain.EventLite
{   // Fundraiser, Networking Event, Conference, Concert
    // Tour, Dinner, Festival
    public class CatalogFormat
    {
        public int Id { get; set; }

        public string Format { get; set; }


        // TODO: Figure out hypothetical for why doesn't work (see: CatalogContext)
        //public virtual CatalogEvents CatalogEvents { get; set; }
    }
}
