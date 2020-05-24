using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models.CartModels
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public string BuyerId { get; set; }

        // New method on the WebMVC side
        // Total is a calculated value, not something that needs to be stored
        // in the CartApi data store (currently redis)
        // Idea is that it would be good to show the total in the UI
        public decimal Total()
        {
            // For each item in Items, add up this
            // A real money thing
            return Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity), 2);
        }
        // My question about whether it was important that Total was a method
        // Kal says that it could just as well have been a property
        // Property is nothing but a method wrapped around a variable
        // No performance difference, it's the same
    }
}
