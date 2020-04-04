using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Domain
{
    // 1) Define models
    // not starting with item since item immediately has a dependency on
    // brand
    // start with a class that has no dependencies

    // Tiffany, Kay, etc.
    public class CatalogBrand
    {
        public int Id { get; set; }

        public string Brand { get; set; }
    }
}
