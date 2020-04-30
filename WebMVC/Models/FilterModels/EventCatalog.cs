using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models.Common;

namespace WebMVC.Models.FilterModels
{
    public class EventCatalog
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long ItemCount { get; set; }

        public List<CatalogEvent> Events { get; set; }
    }
}
