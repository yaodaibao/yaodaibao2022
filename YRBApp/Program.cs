using Confluent.Kafka;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using YRB.Infrastructure;

namespace YRBApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            Global.ServiceProviderRoot = host.Services;
            //var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //var configuration = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    .Build();
            //Log.Logger = new LoggerConfiguration()
            //    .Enrich.FromLogContext()
            //    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            //    {
            //        AutoRegisterTemplate = true,
            //        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower()}-{DateTime.UtcNow:yyyy-MM}"
            //    })
            //    .Enrich.WithProperty("Environment", environment)
            //    .ReadFrom.Configuration(configuration)
            //    .CreateLogger();
            host.Run();
        }
            public static IHostBuilder CreateHostBuilder(string[] args) =>
               Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostcontext, configbuilder) => {
                    configbuilder.AddJsonFile("appsettings.json",
                    optional: true,
                    reloadOnChange: true);
                    Global.HostingEnvironment = hostcontext.HostingEnvironment;
                }) 
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog((hostingContext, loggerConfiguration) =>
            loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

    } 
}