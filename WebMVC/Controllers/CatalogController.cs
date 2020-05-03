using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class CatalogController : Controller
    {

        private readonly IEventCatalogApiService _service;
        public CatalogController(IEventCatalogApiService service)
        {
            _service = service;
        }


        public async Task<IActionResult> Index(int? id)
        {
            var test = await _service.GetSingleEventAsync(id);

            return View(test);
        }
    }
}