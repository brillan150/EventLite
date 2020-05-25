using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebMVC.Infrastructure;
using WebMVC.Services;

// Required installing nuget packages required to use the TokenServiceApi:
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;
using WebMVC.Models;

namespace WebMVC
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

            IdentityModelEventSource.ShowPII = true;

            // Token
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddControllersWithViews();


            // Dependency Injection
            services.AddSingleton<IHttpClient, CustomHttpClient>();

            services.AddTransient<IEventCatalogApiService, EventCatalogApiService>();

            // EventCartApi Integration
            // This is the service that allows you to get the token from the httpcontext browser section
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // Service for logging in and logging out...
            // But I didn't have these at the point in the vid when Kal showed adding these
            // lines to Startup: https://youtu.be/3NjVcWju7EE?list=PLdbymrfiqF-wmh3VsbxysBsu2O9w6Z3Ks&t=194
            services.AddTransient<IIdentityService<ApplicationUser>, IdentityService>();
            // DI for cart service
            services.AddTransient<IEventCartApiService, EventCartApiService>();

            // Nearly missed this part of OrderApi ms integration
            services.AddTransient<IOrderService, OrderService>();



            // More Token
            var identityUrl = Configuration.GetValue<string>("IdentityUrl");
            var callBackUrl = Configuration.GetValue<string>("CallBackUrl");
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                // options.DefaultAuthenticateScheme = "Cookies";
            })
            .AddCookie()
            .AddOpenIdConnect(options => {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.Authority = identityUrl.ToString();
                options.SignedOutRedirectUri = callBackUrl.ToString();
                options.ClientId = "mvc";
                options.ClientSecret = "secret";
                options.ResponseType = "code id_token";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.RequireHttpsMetadata = false;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("offline_access");

                // Added for CartApi integration
                // Extend your scope to add basket
                options.Scope.Add("basket");
                // Those are all of the changes in Startup for integrating CartApi

                // This didn't come up either in Kal's walkthrough of Order integration
                options.Scope.Add("order");

                // Next are views, the UI
                // Created Cart Component and CartList Component


                options.TokenValidationParameters = new TokenValidationParameters()
                {

                    NameClaimType = "name",
                    RoleClaimType = "role"
                };


            });

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");

                // For TokenService
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
                app.UseStaticFiles();

                app.UseRouting();


                // For TokenService
                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseCookiePolicy();


            // app.UseAuthentication(); MUST BE BEFORE app.UseAuthorization();
            // Else, you will get into a nasty redirect loop
            // https://www.scottbrady91.com/OpenID-Connect/Help-Im-Stuck-in-a-Redirect-Loop#comment-4633319851

                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=RandomEvents}/{action=Index}/{id?}");
                });
            }
        }
    }
