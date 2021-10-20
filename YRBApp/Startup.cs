using iTR.LibCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ydb.Domain;
using ydb.Domain.Interface;
using ydb.Domain.Service;
using YRB.Infrastructure;
using YRBApp.Middleware;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace YRBApp
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
            Global.ConfigurationRoot = (IConfigurationRoot)Configuration;
            services.AddControllers();
            //services.AddSingleton<IRedisClient>(_ => new RedisClient(Configuration));
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "YRBApp", Version = "v1" });
            //});
            services.AddScoped<IRewardDomainService, RewardDomainService>();
            services.AddScoped<IAuthHospitalService, AuthHospitalService>();
            services.AddScoped<IQueryItem, QueryItem>();
            services.AddScoped<IAutoRouteService, AutoRouteService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            services.AddControllers()
        //全局配置Json序列化处理
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            //不使用驼峰样式的可以
            options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseSerilogRequestLogging();

            app.UseRequestResponseLogging();
            app.UseCustomExceptionMiddleware();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "YRBApp v1"));
            //}

            app.UseRouting();

            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
