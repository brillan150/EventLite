using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Infrastructure;

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
