using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models.Common;

namespace WebMVC.Models
{
    public class RandomEvents
    {

        public int RandomItemCount { get; set; }

        // public long TotalItemCount { get; set; }

        // public int CappedItemDomainCount { get; set; }

        public List<CatalogEvent> Data { get; set; }

    }
}
