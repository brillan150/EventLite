using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;
using WebMVC.Models.CartModels;
using WebMVC.Models.OrderModels;

namespace WebMVC.Services
{
    public interface IEventCartApiService
    {
        // These are all the actions that we expect the user would be doing from
        // the web side

        // For every call to the service, passing in the user
        // When make a call to the cart service, expect the user info to be passed
        // as the first parameter
        // (this must be what the     [Authorize] attribute does at the class-level for
        // the CartController class on the EventCartApi microservice project
        // as an implicit first param, essentially
        // Probably expected/parsed/enforced by the routing (or before/after the routing?)
        // before the body of our api methods are executed

        Task<Cart> GetCart(ApplicationUser user);
        Task AddItemToCart(ApplicationUser user, CartItem product);
        Task<Cart> UpdateCart(Cart Cart);
        Task<Cart> SetQuantities(ApplicationUser user, Dictionary<string, int> quantities);
        
        // Added during integration of OrderApi with WebMVC
        Order MapCartToOrder(Cart Cart);
        
        Task ClearCart(ApplicationUser user);


    }
}
