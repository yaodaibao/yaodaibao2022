using iTR.LibCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YRBApp.Filters
{
    /// <summary>
    /// redis缓存层,尽量使用list存储和查询，根据业务不同保存不同类型的redids数据类型
    /// </summary>
    public class RedisCacheFilter : Attribute, IActionFilter
    {
        private readonly IRedisClient _redis;
        public RedisCacheFilter(IRedisClient redisClient)
        {
            _redis = redisClient;
        }
        public async void OnActionExecuted(ActionExecutedContext context)
        {
 
        }
        /// <summary>
        /// 根据查询条件判断是否有查询缓存，如果有的话返回缓存数据，加快查询速度
        /// </summary>
        /// <param name="context"></param>
        public async void OnActionExecuting(ActionExecutingContext context)
        {
            string jsonString = "";
            string jsonChache = "";
            using (StreamReader reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8))
            {
                jsonString = await reader.ReadToEndAsync();
            }
            jsonChache = _redis.GetStringKey<string>(jsonString);
            if (!string.IsNullOrEmpty(jsonChache))
            {
                ContentResult content = new();
                content.Content = jsonChache;
                context.Result = content;
                return;
            }


 
        }
    }
}
