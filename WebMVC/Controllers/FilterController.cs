using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Services;
using WebMVC.ViewModels;

namespace WebMVC.Controllers
{
    public class FilterController : Controller
    {
        private readonly IEventCatalogApiService _service;

        public FilterController(IEventCatalogApiService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            // TODO:
            // Date bucketing
            //string earliestStart,
            //string latestStart,


            // For display and query:
            // Date filter value (TODO)
            // Topic filter value
            // Format fitler value
            // Price filter value (TODO)

            // Only for display:
            // Total events
            // Number events fetched


            // Collection of events fetched


            // Only for query:
            // Page index
            // Page size


            // ---

            // High-level of FilterController.Index:
            // ? Get params from routing/razor/explicitly from another page
            // ? Any pre-processing of params


            // (DONE) * Design model class to mirror shape of viewmodel/object serialized to json by api endpoint 
            
            // Query api for data (via apiservice)



            // ?? FEATURE:
            // Instead of a single, fixed number of items to show per page,
            // allow the user to select from a drop-down box
            // (or maybe allow them to type?)
            // how many events they want to see per page
            const int ITEMS_PER_PAGE = 2;

            // Temporary until get params set up:
            int? formatFilterApplied = 0; // All
            int? topicFilterApplied = 0; // All
            int? pageNum = 0; // Zero-based first page

            var eventsCatalog = _service.GetEventsAsync(formatFilterApplied, topicFilterApplied, pageNum, ITEMS_PER_PAGE);








            // ? Throw if invalid
            // ? Any post-processing of model object (any calculations necessary?)
            // * Design viewmodel class for binding to the Filter cshtml view
            // Create viewmodel object
            // Pass viewmodel object to View(), return that ViewResult to WebMVC routing




            var viewModel = new FilterIndexViewModel
            {
                Number = 7
            };

            return View(viewModel);
        }
    }
}