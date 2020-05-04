using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;
using WebMVC.Models.Common;

namespace WebMVC.ViewModels
{
    public class RandomEventsPageViewModel
    {
        public CatalogEvent HeaderEvent { get; set; }
        public IEnumerable<SelectListItem> Topic { get; set; }
        public IEnumerable<CatalogEvent> CatalogEvents { get; set; }
        public int? TopicFilterApplied { get; set; }
    }
}
