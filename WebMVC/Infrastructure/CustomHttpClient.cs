using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        // required here when awaiting an async method

        public async Task<string> HttpGetStringAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            // Set up the request message

            // Send the request

            // Unpack the string from the response, return it

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }

            var response = await _client.SendAsync(requestMessage);

            return await response.Content.ReadAsStringAsync();

        }

        // eventually come back and take a closer look at custom http stuff again
        //a bit prior to this time in this class video :
        //https://youtu.be/V0WWdU3klQ8?list=PLdbymrfiqF-wmh3VsbxysBsu2O9w6Z3Ks&t=1927


        // Post and Put very similar
        // When do Post, Put params come FromBody
        private async Task<HttpResponseMessage> DoPostPutAsync<T>(HttpMethod method, string uri, T item, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put.", nameof(method));
            }

            // a new StringContent must be created for each retry 
            // as it is disposed after each call

            var requestMessage = new HttpRequestMessage(method, uri);


            // This is the important different piece between Post/Put and Get:

            // This prints the serialized object to the Output so it is handy
            // then you can paste it into postman for testing
            // As done here by Kal in class:
            // https://youtu.be/V0WWdU3klQ8?list=PLdbymrfiqF-wmh3VsbxysBsu2O9w6Z3Ks&t=2424
            Console.WriteLine(JsonConvert.SerializeObject(item));

            // Wrap content of object up into string and message content
            requestMessage.Content =
                new StringContent(
                    JsonConvert.SerializeObject(item), // data serialized into a string
                    System.Text.Encoding.UTF8, // encoding type of string
                    "application/json"); // type of data sending
            // THE ESSENCE OF THE DIFFERENCE BETWEEN PUT/POST AND GET IS THIS LINE (requestMessage.Content =)

            // According to my direct-experimentation research
            // from when I was helping Richa with her issue:
            // After an object is put into a requestMessage, you can't peek at it anymore
            // that's why the Console.WriteLine of the serialized object above is so important


            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }

            var response = await _client.SendAsync(requestMessage);

            // raise exception if HttpResponseCode 500 
            // needed for circuit breaker to track fails

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException();
            }

            return response;
        }


        public async Task<HttpResponseMessage> HttpPostAsync<T>(string uri, T item, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            return await DoPostPutAsync(HttpMethod.Post, uri, item, authorizationToken, authorizationMethod);
        }

        public async Task<HttpResponseMessage> HttpPutAsync<T>(string uri, T item, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            return await DoPostPutAsync(HttpMethod.Put, uri, item, authorizationToken, authorizationMethod);
        }
        public async Task<HttpResponseMessage> HttpDeleteAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);
            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }

            return await _client.SendAsync(requestMessage);
        }
    }
}
