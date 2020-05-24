using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models.CartModels
{

    // After finished with CustomHttpClient

    // Time to bring over models from CartApi
    // exact copy

    public class CartItem
    {
        public string Id { get; set; }
        public string EventId { get; set; }
        public string EventTitle { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal OldUnitPrice { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }
    }
}
