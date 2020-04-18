using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventCatalogApi.ViewModels
{
    public class RandomItemsViewModel<T> where T : class
    {
        public int RandomItemCount { get; set; }

        public long TotalItemCount { get; set; }

        public IEnumerable<T> Data { get; set; }
    }
}
