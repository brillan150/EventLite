using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models.OrderModels
{
    // Brought over properties from OrderItem class on OrderApi
    public class OrderItem
    {
        // TODO: After integration:
        // Change names in this file (and propagate throughout solution):
        // ProductName -> EventTitle
        // UnitPrice -> TicketPrice
        // Units -> TicketCount
        // ProductId -> EventId
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }


        // Only need UnitPrice, don't need old price as we did with the cart
        public decimal UnitPrice { get; set; }

        public int Units { get; set; }

        // ProductId not private set here on the WebMVC side (unlike on the OrderApi side)
        public int ProductId { get; /*private*/ set; }



        // These members not carried over
        // from the OrderApi model:
        //public int Id { get; set; }

        //protected OrderItem() { }
        //public Order Order { get; set; }
        //public int OrderId { get; set; }

    }
}
