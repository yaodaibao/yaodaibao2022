using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ydb.Domain.Models;

namespace ydb.Domain.Interface
{
    public interface ILoginService
    {
        public ResponseModel Login([FromBody] Login login);

        public ResponseModel GetRegStatusByMobile([FromBody] Login login);

        public ResponseModel ChangePwd([FromBody] Login login);
        public ResponseModel SetPwd([FromBody] Login login);

        public ResponseModel SendVCode([FromBody] Login login);
        public ResponseModel CheckVCode([FromBody] Login login);
    }
}
