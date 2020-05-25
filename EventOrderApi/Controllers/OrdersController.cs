using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For [Authorize] attribute
using Microsoft.AspNetCore.Authorization;

// For ILogger
using Microsoft.Extensions.Logging;

// For IConfiguration
using Microsoft.Extensions.Configuration;

using EventOrderApi.Data;

// For QueryTrackingBehavior
using Microsoft.EntityFrameworkCore;

// For HttpStatusCode
using System.Net;
using EventOrderApi.Models;
using Common.Messaging;
using MassTransit;

namespace EventOrderApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersContext _ordersContext;

        private readonly IConfiguration _config;

        private readonly ILogger<OrdersController> _logger; // Create a logger
        // What type of class will write to this logger

            // IPublishEndpoint is from MassTransit
        private IPublishEndpoint _bus;
        public OrdersController(OrdersContext ordersContext,
            ILogger<OrdersController> logger,
            IConfiguration config
            , IPublishEndpoint bus // Inject message bus
                                   // Order is a publisher
                                   // This is how distingush publisher and subscriber
            )
        {
            _config = config;
            _ordersContext = ordersContext ?? throw new ArgumentNullException(nameof(ordersContext));
            // Nifty syntax for assign this if it is not null, otherwise throw

            ordersContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            _bus = bus; // Handle to the bus as a publisher
            // you don't need to konw a queue name
            // not tied to a queue
            // only is exchange
            // rec side requires a queue
            _logger = logger;
        }

        // POST api/v1/Order/new
        [Route("new")]
        [HttpPost]
        // CreateOrder can produce two types of responses: Accepted, or BadRequest
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            // Somewhere in the UI,
            // someone is creating that order (the WebMVC) then they are sending that data to you


            // Picked up here on Sat 4/25
            order.OrderStatus = OrderStatus.Preparing;
            order.OrderDate = DateTime.UtcNow;

            _logger.LogInformation(" In Create Order");
            _logger.LogInformation(" Order" + order.UserName);
            
            // Write order to orders table of the db
            _ordersContext.Orders.Add(order);

            // Recall that order can include more than one order item
            // AddRange adds mulitple rows
            _ordersContext.OrderItems.AddRange(order.OrderItems);
            // OrderItems is already an enumerable
            // EF gives us AddRange so we dont' have to loop and call Add repeatedly

            // In the db world, it is not enough to add
            // Must also save, saving is a seperate step, and saving can fail,
            // so wrap it in a try/catch block

            _logger.LogInformation(" Order added to context");
            _logger.LogInformation(" Saving........");
            try
            {
                // In a relational database (such as SQLServer) save is when
                // changes get committed
                // add goes to temporary buffer in memory. save committs that change


                // Try catch in case something fails writing to db
                // Tell user can't complete, try again
                await _ordersContext.SaveChangesAsync();
                // in relational db
                // finaly write to db
                // when use EF, must save to save record to db
                // add to temp buffer in memory
                // now if worked, goes to db



                _logger.LogWarning("BuyerId is: " + order.BuyerId);
                _bus.Publish(new OrderCompletedEvent(order.BuyerId)).Wait(); // Wait is like await
                // Wait for publish to finish



                // 1) install nuget packages as a publisher
                // 2) class represents event (namespace!)
                // 3) startup config, kick off mass transit
                // 4) set up di for publisher class (order)
                // 5) call pubslish on bus and wait
                // 6) set up yaml file for rabbit
                // Now go to consumer/subscriber
                // logger later discuss



                return Ok(new { order.OrderId }); // order id generated from db
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("An error occored during Order saving .." + ex.Message);
                return BadRequest(); // can include a message here optionally
            }

        }

        [HttpGet("{id}", Name = "GetOrder")]
        //[Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetOrder(int id) // give me an order by id
        { // not used in project tho

            // orders
            // orders is a table



            // so is orderitems


            // these tables related in a way
            // each order item related to an order

            // get me an order of 1 (order number 1)
            // query order table only get that row
            // but also want the related items from the order items table
            // two tables in one query

            // Goal of this linq query is to get data from multiple related tables

            var item = await _ordersContext.Orders
                .Include(x => x.OrderItems) // !! can only use include statement if tables are related
                                            // filter for you OrderItems that match from that table
                                            // don't need to write complex queries, use linq
                .SingleOrDefaultAsync(ci => ci.OrderId == id); // give me an order 

            // recall Order and OrderId from OrderItem.cs
            //public Order Order (but not virutal?)
            if (item != null)
            {
                return Ok(item); //if found order
            }

            return NotFound();

        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetOrders()
        {
            // get all orders
            // but not use in this project
            // all db orders?
            // not use

            var orders = await _ordersContext.Orders.ToListAsync();


            return Ok(orders);
        }

        // ok that was the controller for orders
        // now what changes in startup
        // Done dockerfile also
        // Everytime make new ms

        // ** How to make a ms:
        // Models
        // Context (Data folder)
        // Add-Migration
        // Controllers
        // - APIs
        // Startup
        // - config
        // Dockerfile
        // Then finally yaml file
        // (WebMVC is optional if you are only writing ms)

        // Do dockerfile now, then
        // ** To Startup

    }
}