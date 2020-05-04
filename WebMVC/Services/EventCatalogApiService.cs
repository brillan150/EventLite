using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.Models;
using WebMVC.Models.Common;
using WebMVC.Models.FilterModels;

namespace WebMVC.Services
{
    public class EventCatalogApiService : IEventCatalogApiService
    {
        private readonly IHttpClient _client;
        private readonly string _baseUrl;

        public EventCatalogApiService(IConfiguration config, IHttpClient client)
        {
            _baseUrl = $"{ config["EventCatalogApiUrl"]}/api/catalog/";
            _client = client;
        }

        public async Task<CatalogEvent> GetSingleEventAsync(int? id)
        {
            var singleEventUri = ApiPaths.Catalog.GetSingleEvent(_baseUrl, id);
            var eventstring = await _client.HttpGetStringAsync(singleEventUri);
            return JsonConvert.DeserializeObject<CatalogEvent>(eventstring);
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
        public async Task<RandomEvents> GetRandomEventsAsync(int? topicFilterApplied)
        {
            var randomEventsUri = ApiPaths.Catalog.GetRandomEventsApiPath(_baseUrl);
            var dataString = await _client.HttpGetStringAsync(randomEventsUri);
            return JsonConvert.DeserializeObject<RandomEvents>(dataString);




        }

        public Task<IEnumerable<SelectListItem>> GetTopicsAsync()
        {
            throw new NotImplementedException();
        }


        //public async Task<IEnumerable<SelectListItem>> GetTopicsAsync()
        //{
        //    var topicUrl = ApiPaths.Catalog.GetAllTopics(_baseUrl, catalogTopicId);
        //    var dataString = await _client.HttpGetStringAsync(topicUrl);
        //    var items = new List<SelectListItem>
        //    {
        //        new SelectListItem
        //        {
        //            Value=null,
        //            Text="All",
        //            Selected=true
        //        }
        //    };
        //    var topics = JArray.Parse(dataString);
        //    foreach(var topic in topics)
        //    {
        //        items.Add(
        //            new SelectListItem
        //            {
        //                Value = topic.Value<string>("id")
        //                Text = topic.Value<string>("topic")
        //            }
        //            );
        //        return items;
        //    }
        //}


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
