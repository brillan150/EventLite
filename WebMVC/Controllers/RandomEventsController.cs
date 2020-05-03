using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class RandomEventsController : Controller
    {
        private readonly IEventCatalogApiService _randomEvents;
        public RandomEventsController(IEventCatalogApiService randomEvent)
        {
            _randomEvents = randomEvent;
        }
        public async Task<IActionResult> Index()
        {
            var events = await _randomEvents.GetRandomItemsAsync();
            return View(events);
        }
    }
}