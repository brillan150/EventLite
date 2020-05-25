using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using EventOrderApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// For OpenApiSecurityScheme
using Microsoft.OpenApi.Models;

namespace EventOrderApi
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

            services.AddControllers()
                                        // ** Need this for Cart and Order
                                        // In order for POST to work
                                        .AddNewtonsoftJson();
            // Also need to install nuget package:
            // Microsoft.AspNetCore.Mvc.NewtonsoftJson
            // Recall in WebMVC,
            // it is taking all data on form and posting to your service
            // body of the data contains the cart
            // This new .AddNewtonsoftJson() call is a new requirement
            // Was not a thing in dotnetcore 2.0 and 2.1
            // It's a new thing to dotnetcore 3.1
            // Evidently dotnetcore 3.1 is a major change, a lot of breaking changes

            // Recall that NewtonsoftJson was most popular library to use back in dotnetcore 2.0, 2.1
            // In dotnetcore 3.1 MS has their own version of json: System.Text.Json
            // From 3.1 onward, dotnetcore not using Newtonsoft behind the scenes,
            // rather, expects you to create json in the System.Text.Json format
            // Kal tried to migrate, it was not a smooth process
            // Recall from Post method in CustomHttpClient:
            // Console.WriteLine(JsonConvert.SerializeObject(item));
            // If we want to keep using Newtonsoft instead of migrating to System.Text.Json,
            // that's why we need to add the call to .AddNewtonsoftJson() to our
            // services.AddControllers() call
            // This is backwards compatability with NewtonsoftJson
            // The .AddNewtonsoftJson() call means:
            // "Whenever you deal with my controllers, make sure you are communicating
            // using NewtonsoftJson
            // If you forget, then you will get an Http exception and have a bad time
            // Not descriptive error, Kal spent a lot of time trying to figure it out
            // She used Fiddler to try and figure out why post not getting sent over the wire


            // ...moving on...ok, now what?
            // Copied a bunch of things over from CartApi.. drop down to *** below


            // Kal copypastaed this from CatalogApi startup
            var server = Configuration["DatabaseServer"];
            var database = Configuration["DatabaseName"];
            var user = Configuration["DatabaseUser"];
            var password = Configuration["DatabasePassword"];
            var connectionString = $"Server={server};Database={database};User Id={user};Password={password}";
            //var connectionString = Configuration["ConnectionString"];
            services.AddDbContext<OrdersContext>(options =>
                options.UseSqlServer(connectionString));
            // Arrived here from the MigrateDatabase.cs file
            // where we set up the class for automating UpdateDatabase
            // Then Kal realized that PM> Add-Migration Initial wasn't working
            // because we hadn't set up the dependency injection here in Startup.cs
            // So here we are.
            // Added the above lines, then Add-Migration worked
            // Of note is that, while Add-Migration did work, there is still something
            // missing that would keep this from running.
            // Add-Migration needed the OrdersContext class to be set up here with AddDbContext<>
            // But the connectionString is not used until the project actually starts up
            // But we haven't yet put all of the needed values in docker-compose.yml
            // (The environment variables that we are looking up in the Configuration
            // dictionary above)

            // Now head to Program.cs to set up the UpdateDatabase automation



            // *** Recall that Kal wrote ConfigureAuthService as a helper, for refactoring
            // back in Cart, so bring that over here to Order
            ConfigureAuthService(services);



            // Must install nuget package Swashbuckle.AspNetCore
            // Copypasta from CartApi
            // (Mind your curly braces, parenthesis and semicolons at the end
            services.AddSwaggerGen(options =>
            {
                //options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "EventLite - Event Order API", // <-- CHANGE from Cart ms
                    Version = "v1",
                    Description = "Event cart microservice" // <-- CHANGE from Cart ms
                });
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{Configuration["IdentityUrl"]}/connect/authorize"),
                            TokenUrl = new Uri($"{Configuration["IdentityUrl"]}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {"order", "Order Api" } // <-- CHANGE from Cart ms
                            }
                        }
                    }

                });
            }); // I know, it looks really funny, but curly brace first then parenthesis is
            // normal for an init/lambda statement block inside a method call

            // After done integrating Swagger, assume done working on OrderApi ms
            // (Will come back to messaging, logging and payment later)

            // Now on to how to integrate OrderApi with the WebMVC
            // Start with Apipaths let's goooo.....

        }

        private void ConfigureAuthService(IServiceCollection services)
        {
            // ADD from copypasta of this method from Cart:
            // Kal: "prevent from mapping "sub" claim to nameidentifier."
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            // Also noted while Kal was covering Order, that she had added this
            // line back to Cart as well, so here we go to do that...


            // where is id server located
            // read our config
            var identityUrl = Configuration["IdentityUrl"];
            // add auth to existing service
            services.AddAuthentication(options =>
            // add auth for my service
            {
                // jwt json token
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                // challenge refers to username and pwd
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                // both of these refer to your id
                // token is who you are
                // then decode the token to determine what permissions you have

            }).AddJwtBearer(options =>
            {
                // authority is who issues the token
                // auth is provided by service at id url:
                options.Authority = identityUrl;
                // route user to id url

                // in production, this should be true
                options.RequireHttpsMetadata = false;

                // who you are
                // who is allowed to ask for token
                options.Audience = "order"; // <-- CHANGE from copypasta of this method from Cart
                                            // go back to tokenserviceapi
                                            // see mvc, basket, order
                                            // apis must identify themselves when ask for token
                                            // must be on the list

            // "I am cart" I can ask for a token from the tokenapiserver

            // Can look back at tokenservice's Config.cs to see where these Audience names are used:
            //    public static IEnumerable<ApiResource> GetApiResources()
            //    {
            //        return new List<ApiResource>
                    //{
                    //     new ApiResource("basket", "Shopping Cart Api"),
                    //     new ApiResource("order", "Ordering Api"),
                    //};
            //    }

            });
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
