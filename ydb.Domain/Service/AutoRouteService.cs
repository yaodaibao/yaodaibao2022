using iTR.LibCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ydb.Domain.Interface;
using ydb.Domain.Models;

namespace ydb.Domain.Service
{
   public class AutoRouteService : IAutoRouteService
    {
        public async Task<ResponseModel> SaveAutoRouteDataAsync([FromBody] MiniLocationPoint miniLocationPoint)
        {
            ResponseModel responseModel = await Task.Run(() =>
            {
                try
                {
                    byte[] guidArray = Guid.NewGuid().ToByteArray();
                    long lguid = BitConverter.ToInt64(guidArray, 0);

                    SQLServerHelper serverHelper = new();
                    string sql = $"insert into [yaodaibao].[dbo].[Auto_RouteData](ID,EmployeeID,[Location],[Name],[Longitude],[Lantitute]) values('{lguid}','{miniLocationPoint.EmployeeID}','{miniLocationPoint.Location}','{miniLocationPoint.Name}','{miniLocationPoint.Location.Split(",")[0]}','{miniLocationPoint.Location.Split(",")[1]}')";
                    foreach (MiniSurrAddress surrAddress in miniLocationPoint.MiniSurrAddress)
                    {
                        //和插入主表一次执行完
                        sql += $";insert into [yaodaibao].[dbo].[Auto_RouteData_Detail](LocationID,[Distance],[Name],[Location],[LocationType],[TypeCode],[Address],[Longitude],[Lantitute]) values('{lguid}','{surrAddress.Distance}','{surrAddress.Name}','{surrAddress.Location}','{surrAddress.LocationType}','{surrAddress.TypeCode}','{surrAddress.Address}','{surrAddress.Location.Split(",")[0]}','{surrAddress.Location.Split(",")[1]}')";
                    }
                    serverHelper.ExecuteSql(sql);
                }
                catch (Exception ex)
                {
                    throw;
                }
                return new ResponseModel() { Description = "保存成功！" };
            });
            return responseModel;
        }
    }
}
