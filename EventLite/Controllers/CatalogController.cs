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
    public class CatalogController : ControllerBase
    {
        CatalogContext _context;
        IConfiguration _config;

        public CatalogController(CatalogContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        
        }


        // Almost all apis should be async
        // Don't block your webui
        // requirements:
        // async keyword
        // compatable return type (probably wrapped in a Task<>)
        // await the async call in the method
        // (involves storing a ref to the object that you get back)

        [HttpGet()]
        [Route("[action]")]
        public async Task<IActionResult> Events(
            [FromQuery]int pageIndex = 0,
            [FromQuery]int pageSize = 2)
        {
            var events = await _context.CatalogEvents
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();


            events.ForEach(e =>
                e.PictureUrl = e.PictureUrl.Replace(
                "http://externalcatalogbaseurltobereplaced",
                _config["ExternalCatalogUrl"]));


            var viewModel = new PaginatedItemsViewModel<CatalogEvent>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ItemCount = await _context.CatalogEvents.LongCountAsync(),
                Data = events
            };

            return Ok( viewModel );
        }




    }
}