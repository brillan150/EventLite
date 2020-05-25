using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventOrderApi.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventOrderApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Splitting out Build and Run
            // So that we can ensure that the database is created
            // ...EnsureCreated is actually kind of badly named...
            // it ensures created and then also runs Migrate (UpdateDatabase)
            // so better name would be good
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var serviceProviders = scope.ServiceProvider;
                var context = serviceProviders.GetRequiredService<OrdersContext>();
                MigrateDatabase.EnsureCreated(context);
            }
            host.Run();

            // Out of time for today to do Controller
            // Next week: What kind of apis to write
            // Get order based on order id
            // Create an order
            // Set up constructor with all the injections

            // Then finally integrate order with webmvc
            // Very similar to what we did with cart


            // ToDo list for Order microservice:
            // Controlller
            // Startup.cs
            //  -Authentication

            // ToDo list for WebMVC
            // ApiPaths
            // Service
            //  -IOrderService
            //  -OrderService
            // Models (copy models over from ms)
            // Controllers
            // Views
            // Startup.cs (of WebMVC)

            // Final piece of orders is calling the Stripe API\
            
            // Oh and also Messaging


            // Next stop: OrdersController.cs (OrderApi)

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
