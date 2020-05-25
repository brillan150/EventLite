using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Messaging // <-- NOTE NAMESPACE
                           // IMPORTANT !!
                           // When rabbit mq was created, was before microservices were a thing
                           // when someone publishes a message
                           // your message type would normally be named after your folders
                           // OrderApi.OrderCompletedEvent
                           // in cart, need to write the same event, so that the types match
                           // CartApi.OrderCompletedEvent
                           // Type has to be the same message type (namespace and all)
                           // If messages not of the same type, you won't get it
{
    public class OrderCompletedEvent // Messages are just types
    {
        // Put whatever you want in the message
        // Now to cart
        public string BuyerId { get; set; }
        public OrderCompletedEvent(string buyerId)
        {
            BuyerId = buyerId;
        }
    }
}
