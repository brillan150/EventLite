using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Services;
using WebMVC.Models;
using WebMVC.Models.OrderModels;

// For [Authorize] attribute
using Microsoft.AspNetCore.Authorization;

// For ILogger
using Microsoft.Extensions.Logging;

// For IConfiguration
using Microsoft.Extensions.Configuration;

// For BrokenCircuitException
using Polly.CircuitBreaker;



using Stripe;

// Name collision between our Order model type and Stripe
using Order = WebMVC.Models.OrderModels.Order;
// When adding the Stripe.net nuget package to the WebMVC project
// Can click the link to the project page in the description to see what their
// apis are like: https://github.com/stripe/stripe-dotnet


// First thing you need is a javascript ui that can accept payment
// Second, you need backend code to make a call to stripe, external api

namespace WebMVC.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IEventCartApiService _cartSvc;
        private readonly IOrderService _orderSvc;
        private readonly IIdentityService<ApplicationUser> _identitySvc;
        private readonly ILogger<OrderController> _logger;
        private readonly IConfiguration _config;


        public OrderController(IConfiguration config,
            ILogger<OrderController> logger,
            IOrderService orderSvc,
            IEventCartApiService cartSvc,
            IIdentityService<ApplicationUser> identitySvc)
        {
            _identitySvc = identitySvc;
            _orderSvc = orderSvc;
            _cartSvc = cartSvc;
            _logger = logger;
            _config = config;
        }


        // We get redirected here to OrderController.Create
        // From CartController.Index when a Cart is posted back to that method
        // after the [ Checkout ] button is clicked
        //if (action == "[ Checkout ]") // action is the text of the button
        //{
        //    // Redirect the user to the OrderController, specifically to the Create action...go there now
        //    return RedirectToAction("Create", "Order");
        //}
        public async Task<IActionResult> Create() // When redirected from Cart
        {
            // When on the Order page, we are "in the Create"
            // Preview ahead to Views\Order\Create.cshtml...

            var user = _identitySvc.Get(HttpContext.User);
            var cart = await _cartSvc.GetCart(user);
            var order = _cartSvc.MapCartToOrder(cart);
            ViewBag.StripePublishableKey = _config["StripePublicKey"];
            return View(order);
        }


        // When you click the Pay button a post comes here
        [HttpPost]
        public async Task<IActionResult> Create(Order frmOrder) // Create order
        {
            // The code "to actually make a payment"


            if (ModelState.IsValid)
            {
                // Checking token, making sure you're logged in
                var user = _identitySvc.Get(HttpContext.User);

                // Get all the data from the form
                Order order = frmOrder;

                // Recall UserName and BuyerId are [BindNever] on the Order class
                // These are not populated from the form on the page
                // After we get the user back based on the token, we're putting the
                // user's email on the order as UserName and BuyerId
                order.UserName = user.Email;
                order.BuyerId = user.Email;


                // Now this is how you create a Stripe payment
                var options = new RequestOptions // For stripe project
                {
                    ApiKey = _config["StripePrivateKey"] // Look up key from yaml
                };

                // Create a new charge
                var chargeOptions = new ChargeCreateOptions()

                {
                    //required
                    Amount = (int)(order.OrderTotal * 100), // Why * 100? want to round it to two decimal places, then the int cast truncates it
                    Currency = "usd",
                    // Remember we have two keys, the one we're putting in Source here is the public key that came in from the form
                    Source = order.StripeToken, // Comes from form itself
                    //optional
                    Description = string.Format("Order Payment {0}", order.UserName),
                    ReceiptEmail = order.UserName,

                };


                var chargeService = new ChargeService();

                Charge stripeCharge = null;
                try
                {                // Constructed charge object, now fire it off by calling the ChargeService.Create method using the ChargeCreateOptions and the RequestOptions we defined above
                    stripeCharge = chargeService.Create(chargeOptions, options); // You are making call
                    // They will now if dev or prod based on which key used
                    _logger.LogDebug("Stripe charge object creation" + stripeCharge.StripeResponse.ToString());
                    // You get back a stripeCharge, you can look at the StripeResponse
                }
                catch (StripeException stripeException)
                {
                    _logger.LogDebug("Stripe exception " + stripeException.Message);
                    ModelState.AddModelError(string.Empty, stripeException.Message);
                    return View(frmOrder);
                }


                try
                {

                    if (stripeCharge.Id != null) // If has value, then worked
                    {
                        //_logger.LogDebug("TransferID :" + stripeCharge.Id);
                        order.PaymentAuthCode = stripeCharge.Id; // Id tracking, id match to transaction

                        //_logger.LogDebug("User {userName} started order processing", user.UserName);

                        // HERE, FINALLY IS THE CALL TO OUR OrderApi microservice
                        // Since the order is now completed
                        // Then the order complete page happens: Views\Order\Complete.cshtml
                        int orderId = await _orderSvc.CreateOrder(order); // Now firing API (our create order ms, order now completed)
                                                                          // write order to db, record order api
                                                                          //_logger.LogDebug("User {userName} finished order processing  of {orderId}.", order.UserName, order.OrderId);


                        // DIRECTLY CALLING MS:
                        //await _cartSvc.ClearCart(user);
                        // ^ EXAMPLE OF WHAT NOT TO DO


                        return RedirectToAction("Complete", new { id = orderId, userName = user.UserName });

                    }

                    else
                    {
                        ViewData["message"] = "Payment cannot be processed, try again";
                        return View(frmOrder);
                    }

                }
                catch (BrokenCircuitException)
                {
                    ModelState.AddModelError("Error", "It was not possible to create a new order, please try later on. (Business Msg Due to Circuit-Breaker)");
                    return View(frmOrder);
                }
            }
            else
            {
                return View(frmOrder);
            }
        }


        public IActionResult Complete(int id, string userName)
        {

            _logger.LogInformation("User {userName} completed checkout on order {orderId}.", userName, id);
            return View(id);

        }


        public async Task<IActionResult> Detail(string orderId)
        {
            var user = _identitySvc.Get(HttpContext.User);

            var order = await _orderSvc.GetOrder(orderId);
            return View(order);
        }

        public async Task<IActionResult> Index()
        {
            var vm = await _orderSvc.GetOrders();
            return View(vm);
        }






        //public async Task<IActionResult> Orders()
        //{


        //    var vm = await _orderSvc.GetOrders();
        //    return View(vm);
        //}


        private decimal GetTotal(List<Models.OrderModels.OrderItem> orderItems)
        {
            return orderItems.Select(p => p.UnitPrice * p.Units).Sum();

        }
    }

}