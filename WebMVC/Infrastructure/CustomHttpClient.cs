using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebMVC.Infrastructure
{
    public class CustomHttpClient : IHttpClient
    {
        private readonly HttpClient _client;
        public CustomHttpClient()
        {
            _client = new HttpClient();
        }

        // async disallowed on interface method delcarations
        // requie

        public async Task<string> HttpGetStringAsync(string uri, string authToken = null, string authMethod = "Bearer")
        {
            if (authToken != null)
            {
                throw new NotImplementedException();
            }

            // Set up the request message

            // Send the request

            // Unpack the string from the response, return it

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await _client.SendAsync(requestMessage);

            return await response.Content.ReadAsStringAsync();

        }

        public async Task<HttpResponseMessage> HttpPostAsync<T>(string uri, T item, string authToken = null, string authMethod = "Bearer")
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> HttpPutAsync<T>(string uri, T item, string authToken = null, string authMethod = "Bearer")
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> HttpDeleteAsync(string uri, string authToken = null, string authMethod = "Bearer")
        {
            throw new NotImplementedException();
        }
    }
}
