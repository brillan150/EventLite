using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;
using WebMVC.Models.Common;
using WebMVC.Models.FilterModels;

namespace WebMVC.Services
{
    public interface IEventCatalogApiService
    {
        // See EventCatalogApiService file for notes
        // Remember to put the method signatures here, too

        public Task<EventCatalog> GetEventsAsync(
            int? catalogFormatId,
            int? catalogTopicId,
            int? pageIndex,
            int? pageSize);

        Task<RandomEvents> GetRandomEventsAsync(int? topicFilterApplied);

        Task<IEnumerable<SelectListItem>> GetTopicsAsync();

        Task<CatalogEvent> GetSingleEventAsync(int? id);

    }
}
