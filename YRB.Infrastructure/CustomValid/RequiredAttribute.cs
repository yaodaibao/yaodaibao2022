using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YRB.Infrastructure.CustomValid
{
    public class MyRequiredAttribute:AbstractCustomAttribute
    {
 
        private string errorMessage { get; set; }
        public MyRequiredAttribute(string ErrorMessage)
        {
            errorMessage = ErrorMessage;
        }
        public override FieldEntity Validate(object value)
        {

            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                return null;
            }
            else
            {
                return new FieldEntity() { ErrorMessage = string.IsNullOrEmpty(errorMessage) ?"字段不能为空！": errorMessage };
            }
        }
    }
}
