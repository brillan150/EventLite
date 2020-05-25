using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;
using WebMVC.Models;
using WebMVC.Models.CartModels;
using WebMVC.Models.Common;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    [Authorize]
    public class CartController : Controller
    {

        private readonly IEventCartApiService _cartService;
        private readonly IEventCatalogApiService _catalogService;
        private readonly IIdentityService<ApplicationUser> _identityService;

        public CartController(IIdentityService<ApplicationUser> identityService, IEventCartApiService cartService, IEventCatalogApiService catalogService)
        {
            _identityService = identityService;
            _cartService = cartService;
            _catalogService = catalogService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Dictionary<string, int> quantities, string action)
        {
            // Arrived here from the csthml view file for this method
            // Noting that this method's first parameter is a Dictionary<string, int> called quantities
            // 


            // Arrived here again from the cshtml view file for this method
            // while integrating the OrderApi microservice...it's a popular place to be

            // Checking to see if the action is checkout
            if (action == "[ Checkout ]") // action is the text of the button
            {
                // Redirect the user to the OrderController, specifically to the Create action...go there now
                return RedirectToAction("Create", "Order");
            }

            // If the action is not checkout
            try
            {
                // Get the current user
                var user = _identityService.Get(HttpContext.User);

                // Set the quantities
                var basket = await _cartService.SetQuantities(user, quantities);
                // Note that all SetQuantities does is get the cart from the server
                // and overwrite that cart's quantities with this method's parameter
                // quanitites (the ones that were posted back to this controller/action
                // by the page)
                // SetQuantities DOES NOT POST QUANTITIES TO THE SERVER

                // UpdateCart posts an update, rather an entire overwrite of the Cart,
                // to the server
                var vm = await _cartService.UpdateCart(basket);


                // That's it for Cart microservice and WebMVC integration
                //https://youtu.be/3NjVcWju7EE?list=PLdbymrfiqF-wmh3VsbxysBsu2O9w6Z3Ks&t=1492
                // Next up is OrderApi
            }
            catch (BrokenCircuitException)
            {
                // Catch error when CartApi is in open circuit  mode                 
                HandleBrokenCircuitException();
            }

            return View();

        }


        // Gets called when the green [ ADD TO CART ] button is clicked
        public async Task<IActionResult> AddToCart(CatalogEvent productDetails)
        // productDetails is the actual CatalogEvent that we want to add
        // the one that was represented in the _product partial when the user
        // clicked on the green [ ADD TO CART ] button
        {
            try
            {
                // Basic check for plausability of this being an actual product in the db
                if (productDetails.Id > 0)
                {
                    // Ask identity service to get the current user
                    var user = _identityService.Get(HttpContext.User);

                    // Create a new CartItem
                    var product = new CartItem()
                    {
                        // TODO:
                        // Fix up all of this CartItem stuff to fit with our
                        // Events data

                        // Auto-generating guid
                        Id = Guid.NewGuid().ToString(),
                        Quantity = 1,
                        EventTitle = productDetails.Title,
                        PictureUrl = productDetails.PictureUrl,
                        UnitPrice = 999.99M,
                        //UnitPrice = productDetails.Price,
                        EventId = productDetails.Id.ToString()
                    };

                    // Invoke the CartApi service to invoke the microservice
                    await _cartService.AddItemToCart(user, product);
                }

                // Then redirects back to home page
                // TODO: Decide where we want user to be redirected to after they add to cart
                return RedirectToAction("Index", "RandomEvents");
                // TODO: Upgrade these to use nameof()
                // Also do a fixup to remove "Controller" so that we can use nameof for both of them
            }
            // BrokenCircuitException requires a nuget package called Polly
            // Polly, like the parrot who likes crackers
            catch (BrokenCircuitException)
            {
                // QUESTION: Wat. How does Polly work, and why do we need a nuget
                // package for something that sounds kind of obvious

                // Polly can check if a service is broken
                // An API, an external service, etc.

                // If get this exception, means that the API is broken,
                // I'm not able to access it

                // Per Sathis' question, Polly doesn't have any mechanism
                // whereby you could retry, it only gives you these exceptions
                // so that you can tell the user that stuff is broken
                // they can try again later

                // You'd have to write your own retry logic if you like


                // Catch error when CartApi is in circuit-opened mode                 
                HandleBrokenCircuitException();
            }

            return RedirectToAction("Index", "Catalog");

        }

        // Hopefully this never happens
        // You want your API to be up and available all the time
        private void HandleBrokenCircuitException()
        {
            TempData["BasketInoperativeMsg"] = "cart Service is inoperative, please try later on. (Business Msg Due to Circuit-Breaker)";
        }

    }
}