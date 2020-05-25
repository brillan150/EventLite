using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For [BindNever]
using Microsoft.AspNetCore.Mvc.ModelBinding;

// For [DisplayFormat]
using System.ComponentModel.DataAnnotations;


namespace WebMVC.Models.OrderModels
{
    public class Order
    {
        // TODO: Reorder the members in this file
        // to match the order of the Order class
        // in OrderApi


        // Recall CreateOrder api
        // Order object
        // When you send json over, must match
        // Deserialize will fail
        // In Webmvc
        // Still need this in the json schema

        // Sometimes:
        // "Don't want this data to come from anywhere"
        // Or maybe "don't need it at all"

        // But still need this format to make up a json
        // BindNever properties will only ever appear as zero on the WebMVC side
        // Server will create this data for me
        [BindNever]
        public int OrderId { get; set; }

        [BindNever]
        public DateTime OrderDate { get; set; }


        // Attributes belong to DataAnnotations namespace
        // Annotating on top of an existying property in C#
        // This means
        // Display an order total show to two decimal places
        // Every time 
        [DisplayFormat(DataFormatString = "{0:N2}")]
        // I think this works when using the Order class type as a viewmodel for a cshtml file
        public decimal OrderTotal { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }

        // We'll pass this through the token
        [BindNever]
        public string UserName { get; set; }
        // We'll pass this through the token, too
        [BindNever]
        public string BuyerId { get; set; }
        public string StripeToken { get; set; }

        // Done copying over models, then onward to Services: 



        public OrderStatus OrderStatus { get; set; }

        // See the property initializer syntax below. This
        // initializes the compiler generated field for this
        // auto-implemented property.
        public List<OrderItem> OrderItems { get; } = new List<OrderItem>();


        public string PaymentAuthCode { get; set; }
        // public PaymentInfo Payment { get; internal set; }
        //  public Guid RequestId { get;  set; }
    }
    public enum OrderStatus
    {
        Preparing = 1,
        Shipped = 2,
        Delivered = 3
    }

}
