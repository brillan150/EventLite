using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.Models.FilterModels;

namespace WebMVC.Services
{
    public class EventCatalogApiService : IEventCatalogApiService
    {
        private readonly IHttpClient _client;

        public EventCatalogApiService(IHttpClient client)
        {

            _client = client;
        }

        // Brillan TODO:
        // GetEventsAsync

        // In Kal's code, all params were optional
        public async Task<EventCatalog> GetEventsAsync(
            int? catalogFormatId,
            int? catalogTopicId,
            int? pageIndex,
            int? pageSize)
        {

            // Url
            // baseurl
            // * apipath

            
            // Http call
            // gets you json

            // deserialize



            // TODO
            return new EventCatalog();
        }



        // GetFormatsAsync

        // GetTopicsAsync



        // *** Propose for Andrea:
        // GetSingleEventAsync

        // (SingleEvent api (based on Id) doesn't exist yet...can you make it, too?)


        // *** Propose for Ainur:
        // GetRandomEventsAsync
        // Api exists and is working.


        // ...more?

    }
}
