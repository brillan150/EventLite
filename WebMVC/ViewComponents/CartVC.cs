using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;
using WebMVC.Services;
using WebMVC.ViewModels;

namespace WebMVC.ViewComponents
{
    // Recall that we've used cshtml partial components
    // for the catalog
    // This view component is "purely C#"
    public class CartVC : ViewComponent
    {
        private readonly IEventCartApiService _cartSvc;

        public CartVC(IEventCartApiService cartSvc) => _cartSvc = cartSvc;


        // Invoke means:
        // When this code is placed in your html, run this code
        public async Task<IViewComponentResult> InvokeAsync(ApplicationUser user)
        {

            // Create a CartComponentViewModel
            var vm = new CartComponentViewModel();
            try
            {
                // "Cart list will give me the details"
                // That is, the CartListVC, the other Cart-related
                // view component
                var cart = await _cartSvc.GetCart(user);
                // QUESTION:
                // Seems to me like the view (this view component) should not be accessing the "model"/data directly, as it does here by making a call on the CartApi service
                // I think that the controller that creates the page which contains this view component should
                // Make this call to the CartApi and pass along
                // the data to this view component

                vm.ItemsInCart = cart.Items.Count;
                vm.TotalCost = cart.Total();
                return View(vm);
            }
            catch (BrokenCircuitException)
            {
                // Catch error when CartApi is in open circuit mode
                ViewBag.IsBasketInoperative = true;
            }

            return View(vm);
        }

    }
}
