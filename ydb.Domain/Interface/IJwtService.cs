using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ydb.Domain.Interface
{
    public interface IJwtService
    {
        public string GenerateToken(string username, string password);
    }
}
