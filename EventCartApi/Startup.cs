using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventCartApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace EventCartApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // set up di for redis
            services.AddTransient<ICartRepository, RedisCartRepository>();

            // actual cache is just one
            services.AddSingleton<ConnectionMultiplexer>(cm =>
            {
                // where is cache located

                // save off the ConfigurationOptions object
                // so that we can retry connecting if we fail
                var configuration =
                    ConfigurationOptions.Parse(Configuration["ConnectionString"], true);
                // if given as a domain name, resolve it to an ip address
                // redis needs to connect to an actual machine
                configuration.ResolveDns = true;
                configuration.AbortOnConnectFail = false;
                // try mulitiple times before eventually fail

                // can specify how many times to attempt retry
                return ConnectionMultiplexer.Connect(configuration);
                // this is how you connect to a ConnectionMultiplexer
                // returns a ConnectionMultiplexer object
            });
            // note that we are not seeding cart data
            // see how much easier this than EF and SQL server
            // Will set up Cart controllers tomorrow
            // but first must integrate cart with tokenservice
            // (need to be logged in to have a cart)

            // Takeaways:
            // Redis cache is dict storage
            // replicated, high availabilty data store
            // cache storage geodistributed
            // high availabilty
            // partition features


            // https://www.youtube.com/watch?v=-rRHFUA5dDQ&feature=youtu.be&list=PLdbymrfiqF-wmh3VsbxysBsu2O9w6Z3Ks&t=4190
            // *** How to Write a Microservice ***
            // 1) define models
            // 2) data repository, your storage (redis, ef, etc.)
            // 3) write apis (controllers)
            // that's it at high level

            // most important part of a microservice is...
            // startup file!
            // this is where you bring everything together
            // a) dependency injection(s) di
            // b) data (configuration of your data repo)
            // c) identiy server integration (authorizatoin and authentication)
            // This is the next thing that we'll do tomorrow
            // d) swashbuckle/swagger (documentation generator)

            // 5) dockerfile
            // 6) integrate with webmvc (who is customer, how will they use the microservice)
            // ApiPath
            // Service (that represents the ms to the webmvc app)
            // Controller (WebMVC controllers that have the methods that serve the pages)
            // 7) Docker (compose) yaml

            // In the real world, microservice developers don't do step 6
            // that is front-end work
            // You, as microservice devloper, do steps 1-5 and 7 and deploy it

            // User of your microservice does step #6

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
