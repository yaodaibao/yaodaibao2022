using Confluent.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YRBApp.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;
        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
               _logger.LogError(ex.Message.Replace("\r\n", "") + ex.InnerException?.ToString().Replace("\r\n", ""));
               httpContext.Response.ContentType = "text/json";
                //return httpContext.Response.WriteAsync("服务异常,请稍后再试！", Encoding.UTF8);
               await httpContext.Response.WriteAsync($"{{Result:false,Description:\"{ex.Message}\",DataRows:\"\"}}");
 
            }

        }
        private Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            //_producer.ProduceAsync("logstash", new Message<Null, string>() { Value = $"请求串: {httpContext.Request.GetEncodedUrl()}    异常状态码: {httpContext.Response.StatusCode  }     异常信息:{ex.Message},堆栈跟踪{ex.StackTrace}" });
            httpContext.Response.ContentType = "text/json";
            //return httpContext.Response.WriteAsync("服务异常,请稍后再试！", Encoding.UTF8);
            return httpContext.Response.WriteAsync($"\"Result\":\"false\",\"Description\":\"服务异常！\",\"DataRows\":\"\"");
            //httpContext.Response.Redirect("/Error");
            //return Task.CompletedTask;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}
