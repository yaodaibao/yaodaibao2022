using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace YRB.Infrastructure.CustomValid
{
    public class ValidateResultEntity
    {
        public bool IsValidateSuccess { get; set; }
        public List<FieldEntity> ValidateMessage { get; set; }
    }
    /// <summary>
    /// 字段信息
    /// </summary>
    public class FieldEntity
    {
        /// <summary>
        /// 字段名称
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }   
        /// <summary>
        /// 字段类型
        /// </summary>
        public string FieidType { get; set; }
        /// <summary>
        /// 验证错误时提示信息
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
