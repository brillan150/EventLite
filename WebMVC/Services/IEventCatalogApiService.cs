using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Services
{
    public interface IEventCatalogApiService
    {
        // See EventCatalogApiService file for notes
        // Remember to put the method signatures here, too
        Task<CatalogEvent> GetSingleEventAsync(int? id);
        


    }
}
