using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventCatalogApi.Data;
using EventCatalogApi.ViewModels;
using EventLite.Domain.EventLite;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventCatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly CatalogContext _context;
        private readonly IConfiguration _config;
        public EventsController(CatalogContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Event([FromQuery]int pageIndex = 0, [FromQuery]int pageSize = 3)
        {
            var itemsCount = await _context.CatalogEvents.LongCountAsync();
            var events = await _context.CatalogEvents
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            events = ChangePictureUrl(events);

            var model = new PaginatedEventsVM<CatalogEvent>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Count = itemsCount,
                Data = events
            };
            return Ok(model);

        }

        private List<CatalogEvent> ChangePictureUrl(List<CatalogEvent> events)
        {
            events.ForEach(e => e.PictureUrl = e.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced/api/pic/1", _config["ExternalCatalogBaseUrl"]));
            return events;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogFormat()
        {
            var events = await _context.CatalogFormats.ToListAsync();
            return Ok(events);
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogTopics()
        {
            var events = await _context.CatalogTopics.ToListAsync();
            return Ok(events);
        }
    }
}