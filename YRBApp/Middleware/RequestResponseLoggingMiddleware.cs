using Confluent.Kafka;
using iTR.LibCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using ydb.Domain;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace YRBApp.Middleware
{
    /// <summary>
    /// 控制web接口只返回Json格式数据中间件
    /// </summary>
    public static class RequestResponseLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }

    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        //private readonly KafkaDependentProducer<Null, string> _producer;

        public RequestResponseLoggingMiddleware(RequestDelegate next,
            ILogger<RequestResponseLoggingMiddleware> logger/*, KafkaDependentProducer<Null, string> producer*/)
        {
            _next = next;
            _logger = logger;

            //_producer = producer;
        }
        /*
        public async Task Invoke(HttpContext context)
        {
            var existingBody = context.Response.Body;
            using (var ms = new MemoryStream())
            {
                bool auth = await FormatRequest(context.Request);
                if (!auth)
                {
                    context.Response.ContentType = "text/plain; charset=utf-8";
                    await context.Response.WriteAsync("无效请求！", Encoding.UTF8);
                }
                else
                {

                    //var orgBodyStream = context.Response.Body;
                    //context.Response.Body = ms;
                    //await _next(context);
                    //context.Response.Body.Seek(0, SeekOrigin.Begin);
                    ////得到Action的返回值
                    //var actionJsonResult = await new StreamReader(context.Response.Body, Encoding.UTF8).ReadToEndAsync();
                    //context.Response.Body.Seek(0, SeekOrigin.Begin);
                    //ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(actionJsonResult);
                    ////处理成统一的返回格式
                    ////string responJson = $"{{\"Result\":\"{responseModel.Result.ToString().ToLower()}\",\"Description\":\"{responseModel.Description}\",\"DataRows\":{{\"DataRow\":[{responseModel.DataRow }]}}}}";
                    //string responJson = $"{{\"Result\":\"{responseModel.Result.ToString().ToLower()}\",\"Description\":\"{responseModel.Description}\",\"DataRows12323413241324\"}}";
                    ////必须把原始流给responsebody
                    //context.Response.Body = orgBodyStream;
                    //// context.Response.ContentType = "text/json";
                    //_logger.LogInformation($"服务消息:{responJson}");
                    ////await context.Response.WriteAsync("resdafdf", Encoding.UTF8);
                    ////显示修改后的数据 
                    //await context.Response.WriteAsync(responJson.ToString(), Encoding.UTF8);
                    // We set the response body to our stream so we can read after the chain of middlewares have been called.
                    context.Response.Body = ms;
                    await _next(context);
                    // Reset the body so nothing from the latter middlewares goes to the output.
                    context.Response.Body = new MemoryStream();
                    ms.Seek(0, SeekOrigin.Begin);
                    context.Response.Body = existingBody;
                    var newContent = await new StreamReader(ms).ReadToEndAsync();
                    // newContent += "asdfffffffffffffffffffffffffffffffff";
                    await context.Response.WriteAsync(newContent);

                    //var orgBodyStream = context.Response.Body;
                    //context.Response.Body = ms;
                    //await _next(context);
                    //context.Response.Body.Seek(0, SeekOrigin.Begin);
                    ////得到Action的返回值
                    //var actionJsonResult = await new StreamReader(context.Response.Body, Encoding.UTF8).ReadToEndAsync();
                    //context.Response.Body.Seek(0, SeekOrigin.Begin);
                    //ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(actionJsonResult);
                    ////处理成统一的返回格式
                    ////string responJson = $"{{\"Result\":\"{responseModel.Result.ToString().ToLower()}\",\"Description\":\"{responseModel.Description}\",\"DataRows\":{{\"DataRow\":[{responseModel.DataRow }]}}}}";
                    //string responJson = $"{{\"Result\":\"{responseModel.Result.ToString().ToLower()}\",\"Description\":\"{responseModel.Description}\",\"DataRows12323413241324\"}}";
                    ////必须把原始流给responsebody
                    //context.Response.Body = orgBodyStream;
                    //// context.Response.ContentType = "text/json";
                    //_logger.LogInformation($"服务消息:{responJson}");
                    ////await context.Response.WriteAsync("resdafdf", Encoding.UTF8);
                    ////显示修改后的数据 
                    //await context.Response.WriteAsync(responJson.ToString(), Encoding.UTF8);
                }
            }
        }
        */

        public async Task Invoke(HttpContext context)
        {
            await _next.Invoke(context);
            //using (var buffer = new MemoryStream())
            //{
            //    var stream = context.Response.Body;
            //    context.Response.Body = buffer;
            //    await _next.Invoke(context);
            //    buffer.Seek(0, SeekOrigin.Begin);
            //    var reader = new StreamReader(buffer);
            //    using (var bufferReader = new StreamReader(buffer))
            //    {
            //        string body = await bufferReader.ReadToEndAsync();



            //        // Commented below lines.
            //        // byte[] bytess = Encoding.ASCII.GetBytes(jsonString);
            //        // var data = new MemoryStream(bytess);
            //        // context.Response.Body = data;

            //        // Added new code
            //        await context.Response.WriteAsync(" byte[] bytess = Encoding.ASCII.GetBytes(jsonString); byte[] bytess = Encoding.ASCII.GetBytes(jsonString); byte[] bytess = Encoding.ASCII.GetBytes(jsonString); byte[] bytess = Encoding.ASCII.GetBytes(jsonString);");
            //        context.Response.Body.Seek(0, SeekOrigin.Begin);
            //        await context.Response.Body.CopyToAsync(stream);
            //        context.Response.Body = stream;
            //    }
            //}
        }
        private async Task<bool> FormatRequest(HttpRequest request)
        {

            request.EnableBuffering();
            request.Body.Seek(0, SeekOrigin.Begin);
            var boReader = new StreamReader(request.Body);
            var bodyAsText = await boReader.ReadToEndAsync();
            _logger.LogInformation($"请求地址:{request.GetDisplayUrl() }");
            _logger.LogInformation($"post数据:{bodyAsText.Replace("\r\n", "") }");
            _logger.LogInformation($"User-Agent:{request.Headers["User-Agent"]}");

            request.Body.Seek(0, SeekOrigin.Begin);


            try
            {
                var requstJson = JObject.Parse(bodyAsText);
                //使用前台写死authcode方案。。 不判断路由统一拦截
                if (requstJson["AuthCode"]?.ToString() != "0uyV7aK3cvAWcDRZowdyP3AMH27pvw0dmuvqfsJy5+XO/vl6Wdi9Cg==")
                {
                    return false;
                }
                else
                {
                    return true;
                }
                ////无序判断路由
                //if (request.GetDisplayUrl().ToLower().Contains("api/login"))
                //{
                //    if (request.GetDisplayUrl().Contains("api/login/SetPwd"))
                //    {
                //        if (requstJson["smsmd5"]?.ToString() != "5F1B87CACCE747E5DD5F813FBC9163E5")
                //        {
                //            return false;
                //        }

                //    }
                //    if (request.GetDisplayUrl().Contains("api/login/ChangePwd"))
                //    {

                //        if (requstJson["AuthCode"]?.ToString() != "0uyV7aK3cvAWcDRZowdyP3AMH27pvw0dmuvqfsJy5+XO/vl6Wdi9Cg==")
                //        {
                //            return false;
                //        }
                //    }
                //    return true;
                //}
                //else
                //{
                //    if (requstJson["AuthCode"]?.ToString() == "0uyV7aK3cvAWcDRZowdyP3AMH27pvw0dmuvqfsJy5+XO/vl6Wdi9Cg==")
                //    {
                //        return true;
                //    }
                //    else
                //    {
                //        return false;
                //    }
                //}
            }
            catch (Exception)
            {
                return false;
            }
            //return $"Url:  {request.GetEncodedUrl()}   Method: {request.Method}   Token: {request.Headers["Authorization"]}      消息体:  {bodyAsText} ";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var responText = await new StreamReader(response.Body, Encoding.UTF8).ReadToEndAsync();
            _logger.LogInformation($"服务消息:{responText.Replace("\r\n", "") }");
            response.Body.Seek(0, SeekOrigin.Begin);
            return $" {responText}    返回状态码:{response.StatusCode}";
        }
    }
}
