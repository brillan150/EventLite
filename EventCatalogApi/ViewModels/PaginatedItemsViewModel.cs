using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventCatalogApi.ViewModels
{

    // TODO: Why must these be properties for it to work
    // with returning from api method?
    // Is because http client?
    // Json serialization?
    // Dotnet runtime?
    public class PaginatedItemsViewModel<T> where T : class
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long ItemCount { get; set; }

        public IEnumerable<T> Data { get; set; }
    }
}
