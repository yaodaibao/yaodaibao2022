using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YRB.Infrastructure.CustomValid
{
    [AttributeUsage(AttributeTargets.Property,Inherited = true)]
 public abstract  class AbstractCustomAttribute:Attribute
    {
        /// <summary>
        /// 定义校验抽象方法
        /// </summary>
        /// <param name="value">需要校验的值</param>
        /// <returns></returns>
        public abstract FieldEntity Validate(object value);
    }
}
