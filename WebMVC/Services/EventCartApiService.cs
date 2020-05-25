using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.Models;
using WebMVC.Models.CartModels;
using WebMVC.Models.Common;
using WebMVC.Models.FilterModels;
using WebMVC.Models.OrderModels;

namespace WebMVC.Services
{
    public class EventCartApiService : IEventCartApiService
    {
        private readonly IConfiguration _config;
        private IHttpClient _apiClient;
        private readonly string _remoteServiceBaseUrl;

        // IHttpContextAccessor:
        // This is where your token comes from
        // This is how you retreive a token from the id server
        // 
        private IHttpContextAccessor _httpContextAccesor;

        private readonly ILogger _logger;
        public EventCartApiService(IConfiguration config, IHttpContextAccessor httpContextAccesor,
            IHttpClient httpClient, ILoggerFactory logger)
        {
            _config = config;
            _remoteServiceBaseUrl = $"{_config["CartUrl"]}/api/cart";

            // This was the bug that Kal had in her code that she spent a lot of time
            // debugging:
            //_remoteServiceBaseUrl = $"{_config["CartUrl"]}/api/v1/cart";

            // (back up here to revisit bug after finished discussing this services
            // file for the day)
            // We will also look at Fiddler today
            // Also! How to re-execute a line of code in VS:
            // https://youtu.be/V0WWdU3klQ8?list=PLdbymrfiqF-wmh3VsbxysBsu2O9w6Z3Ks&t=3730
            // "Set as Next Statement" ... this actually goes back (somehow?)
            // QUESTION: How does Set as Next Statement work?
            // Remember yellow arrow and yellow line is next statement,
            // has not been executed yet

            // Kal got really frustrated because she had the incorrect apipath generated
            // from the WebMVC and she wasn't hitting the api breakpoint because of that
            // But when she tested in postman, it was working because she typed the correct
            // apipath into postman. Gah!

            // Kal tipped to use Output window
            // also that the Console.WriteLine about the serialized object getting posted
            


            // Enter Fiddler
            // Network traffic
            // Track network calls
            // Stop code from execution first
            // Open fiddler
            // Capture every call happen on entire machine
            // Where is the call going???
            
            // Kal noted that Fiddler should have shown a call to 6801 (the port for
            // CartApi microservice
            // 404? Not found. Clear indication that the problem is with the url that using
            // to reach the api...the apipath.


            // Many knobs use for debugging
            // VS output window
            // breakpoints, f10, f11
            // Console.WriteLine
            // exceptions in output window
            // Postman
            // Fiddler

            // Tomorrow: rest of intergration to get Cart showing in WebMVC
            // ViewModels, ViewComponents, etc.


            _httpContextAccesor = httpContextAccesor;
            _apiClient = httpClient;
            _logger = logger.CreateLogger<EventCartApiService>();
        }


        // Second method discussed
        // Adds a product, one item, to the cart
        public async Task AddItemToCart(ApplicationUser user, CartItem product)
        {
            // First thing to do is to make a call to GetCart, the first method
            // we discussed. 
            var cart = await GetCart(user);
            // Will result in either the cart fetched from the server, or
            // a brand new cart created because there was no cart already on the server

            // cart can't ever be null here since GetCart prevents it

            // Note that AddToCart is an example of a service method on the WebMVC
            // that invokes another service method to request data from the microservice
            _logger.LogDebug("User Name: " + user.Email);

            // Check to see if the cart already has one of this item that is being added
            var basketItem = cart.Items
                .Where(p => p.EventId == product.EventId)
                .FirstOrDefault();

            // Items is a List<CartItem> on the Cart object
            // The cart versions of the events currently in the user's cart

            // If there isn't already one of this item in the user's cart
            if (basketItem == null)
            {
                // Add this item to their cart
                cart.Items.Add(product);
            }
            else
            { 
                // Otherwise just increase the quantity of the item
                basketItem.Quantity += 1;
            }

            // Update...this is the next service method we'll discuss
            await UpdateCart(cart);
        }

        public async Task ClearCart(ApplicationUser user)
        {
            var token = await GetUserTokenAsync();
            var cleanBasketUri = ApiPaths.Basket.CleanBasket(_remoteServiceBaseUrl, user.Email);
            _logger.LogDebug("Clean Basket uri : " + cleanBasketUri);
            var response = await _apiClient.HttpDeleteAsync(cleanBasketUri);
            _logger.LogDebug("Basket cleaned");
        }



        // First method discussed: GetCart
        public async Task<Cart> GetCart(ApplicationUser user)
            // First you see who it is (user)
        {
            // Need to see if token exists
            // Must fire this off ourselves

            var token = await GetUserTokenAsync();
            // Now we have the token


            _logger.LogInformation(" We are in get basket and user id " + user.Email);
            _logger.LogInformation(_remoteServiceBaseUrl);

            // Now get apipaths
            // user's email is their unique id
            var getBasketUri = ApiPaths.Basket.GetBasket(_remoteServiceBaseUrl, user.Email);
            _logger.LogInformation(getBasketUri);
            // Recall that the job of apipaths is to generate the string that you use
            // to call the api (as from browser or postman)


            // This is how the token is given when an api call is made
            // (then the CustomHttpClient puts the token in the Authorization field of the Headers field of the requestMessage object that is prepared to be sent over the wire: requestMessage.Headers.Authorization)
            var dataString = await _apiClient.HttpGetStringAsync(getBasketUri, token);
            _logger.LogInformation(dataString);
            // Data string contains result of api call


            // Recall from EventCartApi that the Get method returns an object of type Cart
            // Need to deserialize the string 
            var response = JsonConvert.DeserializeObject<Cart>(dataString.ToString()) ??
                // Recall ?? operator/syntax
                // If the deserialization fails, if it couldn't make a Cart out of the string
                // that came back from the api call, then DeserializeObject returns null
                // instead of a Cart object (right?)
                // We detect that with ?? and create a new Cart since there wasn't already one
                // User has never added anything to the cart before
               new Cart()
               {
                   BuyerId = user.Email
               };
            // QUESTION:
            // Why call ToString on dataString? dataString is already a string
            // Is there some magic happening here or is it simply redundant and an overight?
            // What happens when you call ToString on a string?

            // Now send Cart response back to the WebMVC controller that requested it
            return response;
            // Now on to AddToCart
        }

        //public Order MapCartToOrder(Cart cart)
        //{
        //    var order = new Order();
        //    order.OrderTotal = 0;

        //    cart.Items.ForEach(x =>
        //    {
        //        order.OrderItems.Add(new OrderItem()
        //        {
        //            ProductId = int.Parse(x.ProductId),

        //            PictureUrl = x.PictureUrl,
        //            ProductName = x.ProductName,
        //            Units = x.Quantity,
        //            UnitPrice = x.UnitPrice
        //        });
        //        order.OrderTotal += (x.Quantity * x.UnitPrice);
        //    });

        //    return order;
        //}



        public Order MapCartToOrder(Cart cart)
        {
            var order = new Order();
            order.OrderTotal = 0;

            cart.Items.ForEach(x =>
            {
                order.OrderItems.Add(new OrderItem()
                {
                    ProductId = int.Parse(x.EventId),

                    PictureUrl = x.PictureUrl,
                    ProductName = x.EventTitle,
                    Units = x.Quantity,
                    UnitPrice = x.UnitPrice
                });
                order.OrderTotal += (x.Quantity * x.UnitPrice);
            });

            return order;
        }

        // Gets the user's cart
        // Sets the quantities on that cart and retuns the cart item
        // NOTE THAT THIS METHOD DOES NOT CHANGE QUANTITIES ON THE SERVER???
        public async Task<Cart> SetQuantities(ApplicationUser user, Dictionary<string, int> quantities)
        {
            var basket = await GetCart(user);

            basket.Items.ForEach(x =>
            {
                // Simplify this logic by using the
                // new out variable initializer.
                if (quantities.TryGetValue(x.Id, out var quantity))
                {
                    x.Quantity = quantity;
                }
            });

            return basket;
        }


        // Third service method discussed
        // 
        public async Task<Cart> UpdateCart(Cart cart)
        {
            // Now time to make a call to the api
            
            // Like we did with GetCart,
            // need to get the token first (we'll need to send the token along with our request)
            var token = await GetUserTokenAsync();

            _logger.LogDebug("Service url: " + _remoteServiceBaseUrl);

            // Then get the api path to use
            var updateBasketUri = ApiPaths.Basket.UpdateBasket(_remoteServiceBaseUrl);
            _logger.LogDebug("Update Basket url: " + updateBasketUri);

            // Make the call to the CustomHttpClient which will make the call to the api
            var response = await _apiClient.HttpPostAsync(updateBasketUri, cart, token);

            // Ensure got the 200 response
            // Else throw an exception 
            response.EnsureSuccessStatusCode();

            // Return the same cart (unmodified) that the user specified
            // (our user, the WebMVC controller)
            return cart;
        }
        // That's what we covered for this EventCartApiService today
        // Will go over the views, etc. for the cart tomorrow
        // but want to discuss debugging for the rest of today...
        // 
        // Recall Kal's _remoteServiceBaseUrl bug noted at the top of this file...




        // Helper method
        // We use this helper in every service method call in this file
        // every call requires authorization
        private async Task<string> GetUserTokenAsync()
        {

            // _httpContextAccesor represents the current browser window that one
            var context = _httpContextAccesor.HttpContext;
            // Give me the information for that browser, the context for that
            // browser window: Header, user information, all of that

            // Once we have that information, use the
            // Built-in http context method: GetTokenAsync
            return await context.GetTokenAsync("access_token");
            // Must specify what type of token you need
            // Remember back in the about page
            // Two kinds of token: "access_token" and "refresh_token"

            // access token is the primary token
            // refresh is a keep-alive
            // smaller version of token
            // not clear on when you would use refresh token instead of access token
        }
        // K, now back to GetCart
    }
}
