using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Infrastructure
{
    public static class ApiPaths
    {
        public static class Catalog
        {
            // Brillan TODO:
            // GetEventsApiPath

            // GetFormatsApiPath

            // GetTopicsApiPath
            public static string GetAllTopics(string baseUri)
            {
                return $"{baseUri}catalogtopics";
            }

            // GetSingleEventApiPath

            // GetRandomEventsApiPath
            public static string GetRandomEventsApiPath(string baseUri)
            {
                return $"{baseUri}randomevents";
            }



            // ...more?

        }

        public static class Cart
        {
        
        }

        public static class Order
        {
        
        }
    }
}
