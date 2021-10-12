using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YRB.Infrastructure.CustomValid
{
    /// <summary>
    /// 自定义必填属性，
    /// </summary>
  public   static  class CustomValidateExtend
    {
        /// <summary>
        /// 校验含有必填属性的字段，
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="igorePros">忽略必填属性，填入不需要检测的属性名</param>
        /// <returns></returns>
        public static bool Validate<T>(this T entity,params string[] igorePros) where T : class
        {
            ValidateResultEntity validateResult = new ValidateResultEntity();
            validateResult.IsValidateSuccess = true;
            List<FieldEntity> fieldList = new List<FieldEntity>();
            Type type = entity.GetType();
            foreach (var item in type.GetProperties())
            {
                //跳过必填检查
                if (igorePros.Contains(item.Name))
                {
                    continue;
                }
                if (item.IsDefined(typeof(AbstractCustomAttribute),true))
                {
                    foreach (AbstractCustomAttribute attribute in item.GetCustomAttributes(typeof(AbstractCustomAttribute),true))
                    {
                        if (attribute==null)
                        {
                            throw new Exception("自定义属性未实现");
                        }
                        var result = attribute.Validate(item.GetValue(entity));
                        if (result!=null)
                        {
                            result.FieldName = item.Name;
                            result.FieidType = item.PropertyType.Name;
                            fieldList.Add(result);
                        }
                    }
                }
            }
            if (fieldList.Count>0)
            {
                return false;
            }
            return true;
        }
    }
}
