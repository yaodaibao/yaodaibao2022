using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ydb.Domain.Extention
{
    /// <summary>
    /// xml格式参数检查访问权限
    /// </summary>
    public class AuthCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var xmlstr = ((ObjectResult)context.Result).Value;
            if (false)
            {
                return;
            }
        }
    }
}
