using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Domain
{
    // Ring, Necklace, etc.
    public class CatalogType
    {
        public int Id { get; set; }

        public string Type { get; set; }
    }
}
