using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventCatalogApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventCatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        CatalogContext _context;

        public CatalogController(CatalogContext context)
        {
            _context = context;
        
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
        public async Task<IActionResult> Events()
        {
            var events = await _context.CatalogEvents.ToListAsync();
  
            return Ok( events );
        }


    }
}