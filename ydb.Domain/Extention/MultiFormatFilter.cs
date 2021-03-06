using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using YRB.Infrastructure.CustomValid;

namespace ydb.Domain.Extention
{
    /// <summary>
    /// 把控制器的返回值做统一格式过滤器，可以自定义各种过滤器，根据前端需要
    /// </summary>
    public class MultiFormatFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            ResponseModel responseModel = (ResponseModel)((ObjectResult)filterContext.Result).Value;
            responseModel.DataRows = $@"{{""DataRow"":[{responseModel.DataRow }]}}";
            string data = $"{{\"Result\":\"{responseModel.Result.ToString().ToLower()}\",\"Description\":\"{responseModel.Description}\",\"DataRows\":{{\"DataRow\":[{responseModel.DataRow }]}} }}";
            //ResponseModel response = JsonConvert.DeserializeObject<ResponseModel>(jsonData.ToString());
            //JObject data = JObject.FromObject(responseModel);
            //string data = JsonConvert.SerializeObject(responseModel);
            filterContext.Result = new ContentResult { Content = data, ContentType = "application/json" };

            base.OnActionExecuted(filterContext);
        }
    }
}
