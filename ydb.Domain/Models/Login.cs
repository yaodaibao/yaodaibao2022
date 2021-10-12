using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ydb.Domain.Models
{
    public class Login
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Savepwd { get; set; }
        public string Mobile { get; set; }
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
}
