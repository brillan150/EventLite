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


            // GetSingleEventApiPath
            public static string GetSingleEvent(string baseUri, int? id)
            {
                return $"{baseUri}singleevent/{id}";
            }



            // GetRandomEventsApiPath

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
