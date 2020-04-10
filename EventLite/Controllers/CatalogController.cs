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

        //[HttpGet()]
        //[Route("[action]")]
        //public async Task<IActionResult> Events(
        //    [FromQuery]int pageIndex = 0,
        //    [FromQuery]int pageSize = 2)
        //{
        //    // TODO!
        //    // HANDLE NOT PUTTING PAST EVENTS ON TOP

        //    var events = await _context.CatalogEvents
        //        .OrderBy(e => e.Start) // Soonest events first
        //        .Skip(pageSize * pageIndex)
        //        .Take(pageSize)
        //        .ToListAsync();


        //    events.ForEach(e =>
        //        e.PictureUrl = e.PictureUrl.Replace(
        //        "http://externalcatalogbaseurltobereplaced",
        //        _config["ExternalCatalogUrl"]));


        //    var viewModel = new PaginatedItemsViewModel<CatalogEvent>
        //    {
        //        PageIndex = pageIndex,
        //        PageSize = pageSize,
        //        ItemCount = await _context.CatalogEvents.LongCountAsync(),
        //        Data = events
        //    };

        //    return Ok( viewModel );
        //}


        [HttpGet()]
        [Route("[action]")]
        public async Task<IActionResult> Events(
            [FromQuery]int? catalogFormatId,
            [FromQuery]int? catalogTopicId,
            [FromQuery]int pageIndex = 0,
            [FromQuery]int pageSize = 2)
        {
            var query = (IQueryable<CatalogEvent>)_context.CatalogEvents;

            if (catalogFormatId.HasValue)
            {
                query = query.Where(e =>
                e.CatalogFormatId == catalogFormatId.Value);

            }

            if (catalogTopicId.HasValue)
            {
                query = query.Where(e =>
                e.CatalogTopicId == catalogTopicId.Value);

            }


            //filteredEventsQuery = _context.CatalogEvents.Where(e =>
            //    e.CatalogTopicId == catalogTopicId.Value);


            // TODO:
            // Why coparing an int with a null nullable int doesn't
            // compiler error or crash...seems like should be type mismatch
            // or null exception


            // TODO!
            // HANDLE NOT PUTTING PAST EVENTS ON TOP

            var events = await query
                .OrderBy(e => e.Start) // Soonest events first
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
                ItemCount = await query.LongCountAsync(),
                Data = events
            };


            // TODO:
            // help. how does this work?
            // database caching??
            // For the objects that the events and query collection have in common,
            // they point to the same object in memory. Specifically, you can
            // use the events reference to modify the in-memory object,
            // then if you:
            //_context.SaveChanges();
            //_context.Database.Migrate();
            // the replaced PictureUrls would get committed to the db
            // What is the lifetime of edits to the in-memory db objects?
            // If we don't save changes and update the db here (which we wouldn't)
            // are we still at risk of having these changes inadvertently
            // written to the db if these commands are executed later?



            return Ok(viewModel);

        }


    }
}