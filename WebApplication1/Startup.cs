using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Unity;
using WebApplication1.App_Code;
using WebApplication1.Interfaces;
//using Unity;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //public void ConfigureContainer(IUnityContainer container)
        //{
        //    // Could be used to register more types
        //    container.RegisterType<IMyWeatherService, MyWeatherService>();
        //}

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddHttpClient();

            //services.JSON; AddNewtonsoftJson()
            services.AddDbContext<DbContextForApi>();
            services.AddTransient<IMyWeatherService, MyWeatherService>();
            //// Creating the UnityServiceProvider
            //var unityServiceProvider = new UnityServiceProvider();

            //IUnityContainer container = unityServiceProvider.UnityContainer;

            //// Adding the Controller Activator
            //// Caution!!! Do this before you Build the ServiceProvider !!!
            //services.AddSingleton<IControllerActivator>(new UnityControllerActivator(container));

            ////Now build the Service Provider
            //var defaultProvider = services.BuildServiceProvider();

            //// Configure UnityContainer
            //// #region Unity

            ////Add the Fallback extension with the default provider
            //container.AddExtension(new UnityFallbackProviderExtension(defaultProvider));

            //// Register custom Types here

            //container.RegisterType<ITest, Test>();

            //container.RegisterType<HomeController>();
            //container.RegisterType<AuthController>();

            //// #endregion Unity

            //return unityServiceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseMvcWithDefaultRoute();

            app.UseCors(options =>
            {
                options.WithOrigins("http://localhost:4200",
                                            "https://localhost")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                 name: "getCityAutocomplete",
                 template: "api/values/get/{text}",
                 defaults: new { controller = "Values", action = "Get", text = RouteParameter.Optional });


                routes.MapRoute(
                 name: "getCityWeatherInfo",
                 template: "api/values/getinfobycity/{idCityKey}",
                 defaults: new { controller = "Values", action = "GetInfoByCity", idCityKey = RouteParameter.Optional });

                routes.MapRoute(
                 name: "getAllFavCities",
                 template: "api/values/getAllFavCities",
                 defaults: new { controller = "Values", action = "GetAllFavCities" });
                

                routes.MapRoute(
                 name: "post1",
                 template: "api/favourite/{action}/{id}",
                 defaults: new { controller = "Favourite", action = "Post", id = RouteParameter.Optional });

            });
        }
    }
}
