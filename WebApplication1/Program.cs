using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unity;
using Unity.Microsoft.DependencyInjection;
using WebApplication1.App_Code;
using WebApplication1.Interfaces;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //WebHost.CreateDefaultBuilder(args)
        //.UseStartup<Startup>();
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        { //=>
            ////var container = new UnityContainer();
            //container.RegisterType<IRepository, Repository>();
            //    // Could be used to register more types
          /////  container.RegisterType<IMyWeatherService, MyWeatherService>();


            return WebHost.CreateDefaultBuilder(args)
           // .UseUnityServiceProvider(container)
            .UseStartup<Startup>();
        }
    }
}
