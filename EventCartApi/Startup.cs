using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using EventCartApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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
            services.AddControllers()
                                        // ** Need this for Cart and Order
                                        // In order for POST to work
                                        .AddNewtonsoftJson();
                                        // Also need to install nuget package:
                                        // Microsoft.AspNetCore.Mvc.NewtonsoftJson

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


            // We defined this as a helper for refactoring purposes
            // since this ConfigureServices method is getting really bloated
            ConfigureAuthService(services);
            // VS generated method for us from inferring signature


            // Pasted here from catalog api, updated naming
            services.AddSwaggerGen(options =>
            {
                //options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                // this v1 here in this string must match the v1 down in the
                // e.SwaggerEndpoint exactly (lowercase 'v')
                {
                    Title = "EventLite - Event Cart API",
                    Version = "v1",
                    Description = "Event cart microservice"
                });
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());


                // So what's different about swagger for cart vs. swagger for catalog?
                // potential for big security hole
                // swagger will show the content of your database
                // swagger must be authenticated too for protection:
                // AddSecurityDefinition
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {// swagger secured by what? by oauth2

                    // what does scheme look like?

                    // type of scheme oauth2 (as enum)
                    Type = SecuritySchemeType.OAuth2,

                    // next is flows
                    // flows are 
                    // id can be obtained in many ways
                    // this is a backend api
                    // no login page (vs. webmvc)

                    // this is implicit flow
                    // don't expect another handshake
                    // id server to validate token for me as good or not
                    // 2fa is a different flow
                    Flows = new OpenApiOAuthFlows
                    // notice how kal in the video was figuring out using vs intellisense
                    // whether the value that Flows was going to get would be an enum
                    // or a class type
                    {
                        // when api communicates to id server behind the scenes is an implicit flow
                        Implicit = new OpenApiOAuthFlow
                        // note that you can look in OpenApiOAuthFlows to see the the properties
                        // that you can set to specify which flow you want
                        // you set the property of the flow you want and create an instance of
                        // OpenApiOAuthFlow to set that Flows property to
                        {
                            // take this token and pass it on to someone else
                            // read the same id server url
                            // tell where auth comes from
                            AuthorizationUrl = new Uri($"{Configuration["IdentityUrl"]}/connect/authorize"),
                            // token url
                            // token is id
                            TokenUrl = new Uri($"{Configuration["IdentityUrl"]}/connect/token"),
                            // the bits that come after the IdentityUrl
                            // (/connect/authorize and /connect/token )
                            // these are specified by Identity Server


                            // what is the scope
                            // who is going to be calling this
                            Scopes = new Dictionary<string, string>
                            {
                                // key is basket
                                // it must match inside the tokenapi exactly
                                // if doesn't match, tokenserver will say you can't have a token
                                // you aren't on the list of ppl who can have tokens
                                {"basket", "Basket Api" }
                                // this is my Basket api
                                // the value doesn't matter can be anything
                            }


                            // Summing up CartApi
                            // At this point, pretty much done with Cart api
                            // have models
                            // one controller with three methods in it
                            // dockerfile
                            // startup is hooked up to id server
                            // swashbuckle/swagger integration

                            // last piece is docker yaml
                            // 
                            // note that you can't put comments in a yaml file

                            // after done making yaml file:
                            // next week we'll test swagger with the right client id
                            // then we'll integrate the cart with the webmvc
                            // then we'll see it working from the webmvc all the way on down
                            // kal notes that that the order api is very similar
                            // so maybe she'll just give us the code for that
                            // I think she did, because I remember enjoying
                            // writing my class notes as comments directly into the
                            // prepared-in-advance code


                            // (I think) Sathis asked why
                            // cart api is listed as both backend and frontend network
                            // Kal: cartapi is back end for catalog service,
                            // but front end for id server
                            // hrm...not sure I quite grok what this really all means
                            // idea to ask DMM to help clarify sometime next when we
                            // hang out


                            // on swagger authorize for cartapi
                            // client_id
                            // remember that this is implicit flow
                            // (no username/pwd)
                            // need to know what the client's name is
                            // evidently "basket" isn't the right one
                            // fires call to id server
                            // authorize you 
                            // client_id is about the scope that you want to make the call to
                            // the token will xfer over

                            // Kal will show the swagger working with the correct client_id
                            // at the start of class (next weekend Sat 4/18) 

                            // also note that this is when Kal made a nod to
                            // continuing to integrate microservices along with class
                            // "keep building"
                            // https://youtu.be/znOAfdL15No?list=PLdbymrfiqF-wmh3VsbxysBsu2O9w6Z3Ks&t=4408


                        }
                    }

                });



            });


        }
        private void ConfigureAuthService(IServiceCollection services)
        {
            // While Kal was covering Order, she copypasta-ed this ConfigureAuthService
            // method from here in Cart over to Order
            // I noticed that she had added this line back to Cart as well,
            // So here we are:
            // Kal: "prevent from mapping "sub" claim to nameidentifier."
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


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
                options.Audience = "basket";
                // go back to tokenserviceapi
                // see mvc, basket, order
                // apis must identify themselves when ask for token
                // must be on the list

                // "I am basket" I can ask for a token from the tokenapiserver

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

            // !!! UseAuthentication() MUST come before UseAuthorization() !!!
            app.UseAuthentication();
            // We added UseAuthentication(), app.UseAuthorization() was already here by default

            // At this point, we have integrated our cart with id server
            // when we do order api it will be the same thing:
            // JWT token
            // id url
            // and finally UseAuthentication()
            // that's it

            // While here in Startup.cs, let's do swagger

            // Hrm, swagger was already integrated in the product catalog api
            // by this point in class (but hasn't been to our project yet)

            // Installed Swashbuckle.AspNetCore
            // added here to cart ms and also to catalog ms while at it





            app.UseAuthorization();



            // Second change to Startup.cs for Swagger
            app.UseSwagger()
                .UseSwaggerUI(e =>
                {
                    e.SwaggerEndpoint("/swagger/v1/swagger.json", "EventCartAPI V1");
                }); // this v1 here in this string must match the v1 up in the
            // options.SwaggerDoc exactly (lowercase 'v')

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
