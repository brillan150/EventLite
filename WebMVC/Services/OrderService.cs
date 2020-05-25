using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.Models.OrderModels;

namespace WebMVC.Services
{
    public class OrderService : IOrderService
    {
        private IHttpClient _apiClient;
        private readonly string _remoteServiceBaseUrl;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly ILogger _logger;
        public OrderService(IConfiguration config,
            IHttpContextAccessor httpContextAccesor,
            IHttpClient httpClient, ILoggerFactory logger)
        {
            _remoteServiceBaseUrl = $"{config["OrderUrl"]}/api/orders";
            // in docker yaml, must provide OrderUrl
            _config = config;
            _httpContextAccesor = httpContextAccesor;
            _apiClient = httpClient;
            _logger = logger.CreateLogger<OrderService>();
        }

        async public Task<Order> GetOrder(string id)
        {
            var token = await GetUserTokenAsync();
            var getOrderUri = ApiPaths.Order.GetOrder(_remoteServiceBaseUrl, id);

            var dataString = await _apiClient.HttpGetStringAsync(getOrderUri, token);
            _logger.LogInformation("DataString: " + dataString);
            var response = JsonConvert.DeserializeObject<Order>(dataString);

            return response;
        }

        public async Task<List<Order>> GetOrders()
        {
            var token = await GetUserTokenAsync();
            var allOrdersUri = ApiPaths.Order.GetOrders(_remoteServiceBaseUrl);

            var dataString = await _apiClient.HttpGetStringAsync(allOrdersUri, token);
            var response = JsonConvert.DeserializeObject<List<Order>>(dataString);

            return response;
        }

        // First method discussed
        // This method gets called immediately when the CHECKOUT button is clicked
        // The order contained on the page becomes the parameter
        public async Task<int> CreateOrder(Order order)
        {
            // Get the token first
            var token = await GetUserTokenAsync();

            var addNewOrderUri = ApiPaths.Order.AddNewOrder(_remoteServiceBaseUrl);
            _logger.LogDebug(" OrderUri " + addNewOrderUri);

            // Call to the Order Api
            var response = await _apiClient.HttpPostAsync(addNewOrderUri, order, token); // token required, user info
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception("Error creating order, try later.");
            }

            // response.EnsureSuccessStatusCode();
            var jsonString = response.Content.ReadAsStringAsync();

            jsonString.Wait(); // Kal demonstrating another way to wait other than
            // await-ing the async call above
            _logger.LogDebug("response " + jsonString);
            dynamic data = JObject.Parse(jsonString.Result);
            // Just one data coming back, only the id
            // Alternative to Jsonconvert

            // Recall that the CreateOrder Api endpoint returns the order id
            // of the created order (if the order wqas successfully created):
            // return Ok(new { order.OrderId }); // order id generated from db

            // Json will take this key "OrderId" but make it camel case: orderId
            string value = data.orderId; // camel case because json library
            return Convert.ToInt32(value);

            // This CreateOrder method is the only method needed for the Order service
            // Now on to page/view
        }

        async Task<string> GetUserTokenAsync()
        {
            var context = _httpContextAccesor.HttpContext;

            return await context.GetTokenAsync("access_token");
        }


        //public async  Task<List<Order>> GetOrdersByUser(ApplicationUser user)
        //{
        //    var token = await GetUserTokenAsync();
        //    var allMyOrdersUri = ApiPaths.Order.GetOrdersByUser(_remoteServiceBaseUrl,user.Email);

        //    var dataString = await _apiClient.HttpGetStringAsync(allMyOrdersUri, token);
        //    var response = JsonConvert.DeserializeObject<List<Order>>(dataString);

        //    return response;
        //}


    }
}
