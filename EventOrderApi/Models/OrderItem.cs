using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For Key
using System.ComponentModel.DataAnnotations;

// For DatabaseGenerated
using System.ComponentModel.DataAnnotations.Schema;
using EventOrderApi.Infrastructure.Exceptions;

namespace EventOrderApi.Models
{
    // See how similar OrderItem is to CartItem
    public class OrderItem
    {
        // TODO: After integration:
        // Change names in this file (and propagate throughout solution):
        // ProductName -> EventTitle
        // UnitPrice -> TicketPrice
        // Units -> TicketCount
        // ProductId -> EventId

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ProductName { get; set; }
        public string PictureUrl { get; set; }


        // Only need UnitPrice, don't need old price as we did with the cart
        public decimal UnitPrice { get; set; }

        public int Units { get; set; }
        public int ProductId { get; private set; }

        protected OrderItem() { }
        public Order Order { get; set; }
        public int OrderId { get; set; }



        public OrderItem(int productId, string productName, decimal unitPrice, string pictureUrl, int units = 1)
        {
            if (units <= 0)
            {
                // Example of a custom exception
                throw new OrderingDomainException("Invalid number of units");


                // Could use ArgumentException here
                // But if a trycatch block someplace else...
                // What if ArgumentException is being thrown by different methods?
                // Essentially, ArgumentException could mean different things
            }

            ProductId = productId;

            ProductName = productName;
            UnitPrice = unitPrice;

            Units = units;
            PictureUrl = pictureUrl;
        }

        // This is for aesthetics
        // If PictureUrl is broken, don't show broken image box
        // it looks bad. Simply show nothing instead.
            
            // Models done, now data
        public void SetPictureUri(string pictureUri)
        {
            if (!String.IsNullOrWhiteSpace(pictureUri))
            {
                PictureUrl = pictureUri;
            }
        }
        


        public void AddUnits(int units)
        {
            if (units < 0)
            {
                throw new OrderingDomainException("Invalid units");
            }

            Units += units;
        }
    }
}
