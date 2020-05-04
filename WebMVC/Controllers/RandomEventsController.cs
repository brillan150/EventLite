using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models.Common;
using WebMVC.Services;
using WebMVC.ViewModels;

namespace WebMVC.Controllers
{
    public class RandomEventsController : Controller
    {
        private readonly IEventCatalogApiService _randomEvents;
        public RandomEventsController(IEventCatalogApiService randomEvent)
        {
            _randomEvents = randomEvent;
        }
        public async Task<IActionResult> Index(int? topicFilterApplied)
        {
           
            var events = await _randomEvents.GetRandomEventsAsync(topicFilterApplied);

            var tempRandomEvents = await _randomEvents.GetRandomEventsAsync(topicFilterApplied);

            var vm = new RandomEventsPageViewModel
            {

                HeaderEvent = tempRandomEvents.Data[0],
                CatalogEvents = events.Data,
                //Topic = await _randomEvents.GetTopicsAsync(),

                TopicFilterApplied = topicFilterApplied??0


            };
           // return View(vm);
            return View(vm);
        }
    }
}