using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebMVC.Infrastructure
{
    public interface IHttpClient
    {
        // Put, post and delete all return an HttpResponseMessage
        // Get only returns a string
        // Put and post are generic methods (on item of type T)
        // Get and Delete aren't generic since the caller doesn't pass in an item

        // Note that you don't put async on the declaration of an
        // interface, only on the definition of a method


        public Task<string> HttpGetStringAsync(
            string uri,
            string authorizationToken = null,
            string authorizationMethod = "Bearer");


        Task<HttpResponseMessage> HttpPostAsync<T>(
            string uri,
            T item,
            string authorizationToken = null,
            string authorizationMethod = "Bearer");

        Task<HttpResponseMessage> HttpPutAsync<T>(
            string uri,
            T item,
            string authorizationToken = null,
            string authorizationMethod = "Bearer");

        Task<HttpResponseMessage> HttpDeleteAsync(
            string uri,
            string authorizationToken = null,
            string authorizationMethod = "Bearer");
    }
}
