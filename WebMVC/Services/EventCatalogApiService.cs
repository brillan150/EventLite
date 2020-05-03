using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class EventCatalogApiService : IEventCatalogApiService
    {
        private readonly IHttpClient _client;
        private readonly string _baseUri;

        public EventCatalogApiService(IConfiguration config, IHttpClient client)
        {
            _baseUri = $"{config["EventCatalogApiUrl"]}/api/catalog/";
            _client = client;
        }

        public async Task<CatalogEvent> GetSingleEventAsync(int? id)
        {
            var singleEventUri = ApiPaths.Catalog.GetSingleEvent(_baseUri, id);
            var eventstring = await _client.HttpGetStringAsync(singleEventUri);
            return JsonConvert.DeserializeObject<CatalogEvent>(eventstring);
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
