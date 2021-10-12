using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ydb.Domain.Models;

namespace ydb.Domain.Interface
{
    public  interface IAutoRouteService
    {
        public   Task<ResponseModel> SaveAutoRouteDataAsync([FromBody] MiniLocationPoint miniLocationPoint);
    }
}
