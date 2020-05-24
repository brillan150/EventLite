using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventCartApi.Models
{
    public class Cart
    {
        public string BuyerId { get; set; }
        public List<CartItem> Items { get; set; }

        // Aha! Thank you Richa
        // Looks like Kal probably had the 500 error when testing her code
        // after she first introduced the Cart model class
        public Cart()
        { }

        public Cart(string cartId)
        {
            BuyerId = cartId;
            Items = new List<CartItem>();
        }
    }
}
