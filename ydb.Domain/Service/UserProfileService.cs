using iTR.LibCore;
using System.Collections.Generic;
using System.Data;
using ydb.Domain.Interface;
using ydb.Domain.Models;

namespace ydb.Domain.Service
{
    public class UserProfileService : IUserProfileService
    {
        public ResponseModel SaveUserProfile(UserProfile userProfile)
        {
            SQLServerHelper runner = new SQLServerHelper();
            string sql = $"select  * from  [yaodaibao].[dbo].[Profile] where FEmployeeID='{userProfile.EmployeeID}'";
            DataTable dataTable = runner.ExecuteSql(sql);
            if (dataTable.Rows.Count > 0)
            {
                sql = $"update [yaodaibao].[dbo].[Profile] set [FRouteStatus]='{userProfile.AutoState}'";
            }
            else
            {
                sql = $"insert into [yaodaibao].[dbo].[Profile]([FEmployeeID],[FRouteStatus]) values('{userProfile.EmployeeID}','{userProfile.AutoState}')";
            }

            runner.ExecuteSqlNone(sql);
            return new ResponseModel() { Description = "保存成功！" };
        }

        public ResponseModel GetUserProfile(UserProfile userProfile)
        {
            SQLServerHelper runner = new SQLServerHelper();
            string sql = $"SELECT   [FRouteStatus] FROM  [yaodaibao].[dbo].[Profile] where FEmployeeID='{userProfile.EmployeeID}'";
            DataTable dataTable = runner.ExecuteSql(sql);
            if (dataTable.Rows.Count == 0)
            {
                return new ResponseModel() { DataRow = "" };
            }
            else
            {
                return new ResponseModel() { DataRow = $"{dataTable.Rows[0]["FRouteStatus"]}" };
            }
        }

        public ResponseModel GetAutoHis(RouteQuery routeQuery)
        {
            SQLServerHelper runner = new SQLServerHelper();
            string sql = @$"select CONVERT(varchar(100), CreateTime, 24) time,[Address]  name ,[name] viewName from  [dbo].[Auto_RouteData_Detail] t3 right join
         (SELECT   MIN(T1.ID) ID ,   T1.[LocationID],  (SELECT TOP 1 Distance FROM [dbo].[Auto_RouteData_Detail]  T2 WHERE T2.ID = MIN(T1.ID)) distance
          FROM  [dbo].[Auto_RouteData_Detail]  T1 right join [dbo].[Auto_RouteData] t5 on T1.[LocationID]= t5.ID where  CONVERT(varchar(100), t5.CreateTime, 23) = '{routeQuery.QueryTime}' and t5.EmployeeID='{routeQuery.EmployeeID}'   GROUP BY  [LocationID]) t4  on t3.ID = t4.ID   order by CreateTime desc";
            DataTable dataTable = runner.ExecuteSql(sql);
            if (dataTable.Rows.Count == 0)
            {
                return new ResponseModel() { DataRow = "" };
            }
            else
            {
                List<string> autoList = new List<string>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (string.IsNullOrEmpty(dataRow["name"]?.ToString()))
                    {
                        autoList.Add($"{{\"name\":\"{dataRow["viewName"]}\",\"time\":\"{dataRow["time"]}\"}}");
                    }
                    else
                    {
                        autoList.Add($"{{\"name\":\"{dataRow["name"]}\",\"time\":\"{dataRow["time"]}\"}}");
                    }

                }
                return new ResponseModel() { DataRow = $"[{string.Join(",", autoList.ToArray())}]" };
            }
        }
    }
}
