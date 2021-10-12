using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using iTR.LibCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using YRB.Infrastructure;

namespace ydb.Domain
{
    public class RewardDomainService : IRewardDomainService
    {
        /// <summary>
        /// 获取奖金
        /// </summary>
        /// <param name="id"></param>
        /// <param name="month">查询月份</param>
        /// <returns></returns>
        public async Task<ResponseModel> GetReward(string id, string startMonth = "", string endMonth = "")
        {
            SQLServerHelper sqlServer = new SQLServerHelper();
            Task<ResponseModel> jsonResult = Task<ResponseModel>.Factory.StartNew(() =>
           {
               var nameList = Global.ConfigurationRoot.GetSection("RewardNames").AsEnumerable(true);

               string queryCondition = "";
 
               string filterMonth = "";
               ResponseModel responseModel = new();
               List<string> bonusList = new List<string>();
               List<string> listMonth = new List<string>();
               //获取需要查询的奖金
               foreach (var name in nameList)
               {
                   string field = name.Key;
                   string viewName = name.Value;
                   System.Diagnostics.Debug.WriteLine($"field{field},name{viewName}");
                   if (!string.IsNullOrEmpty(viewName))
                   {
                       queryCondition += $"t2.{field} '{viewName}',";
                   }
               }
               queryCondition = queryCondition.Remove(queryCondition.Length - 1);
               if (!string.IsNullOrEmpty(startMonth) && !string.IsNullOrEmpty(endMonth))
               {
                   filterMonth = $"  and left(convert(varchar,t1.FMonth, 120),10)  between '{startMonth}' and '{endMonth}'";
               }
               else
               {
                   filterMonth = $"  and left(convert(varchar,t1.FMonth, 120),10) = '{DateTime.Now.AddMonths(-1).ToString("yyyy-MM")}'";
               }
               //查询奖金
               DataTable dt = sqlServer.ExecuteSql(@$"select t1.FMonth, {queryCondition} from yaodaibao.dbo.Reward t1 left join yaodaibao.dbo.RewardNames t2 on t1.FID = t2.FID left join yaodaibao.dbo.t_Items t3 on t1.FCode = t3.FNumber WHERE t3.FID ='{id}' {filterMonth} ");
               
               //拼接返回格式
               foreach (DataRow row in dt.Rows)
               {
                   string monthValue = "";
                   int monthCount=0;
                   string rowValues = "";
                   foreach (DataColumn column in dt.Columns)
                   {
                       if (column.ColumnName.ToString()=="FMonth")
                       {
                           monthValue = $"\"month\":\"{row["FMonth"]}\"";
                       }
                       else
                       {
                           if (!string.IsNullOrEmpty(row[column.ColumnName].ToString()))
                           {
                               //拼接单条奖金类型数据
                               monthCount += int.Parse(row[column.ColumnName].ToString());
                               rowValues = $"{{MONTHCOUNT,\"bonusName\":\"{column.ColumnName}\",\"bonus\":{row[column.ColumnName]}}}";
                               bonusList.Add(rowValues);
                           }
                       }
                   }
                   for (int i = 0; i < bonusList.Count; i++)
                   {
                       bonusList[i]=bonusList[i].Replace("MONTHCOUNT", $"{monthValue},\"count\":{monthCount}");
                   }
                   //拼接返回的list数据
                   listMonth.Add($"{{ \"list\" :[{string.Join(',',bonusList.ToArray())}]}}");
                   bonusList.Clear();
               }

               if (listMonth.Count>0) 
               {
                   responseModel.DataRow = $"{string.Join(',', listMonth.ToArray())}";
               }

               return responseModel;
           });
            return jsonResult.Result;
        }
        /// <summary>
        /// 查询产品的销售额 ,默认查询上个月份
        /// </summary>
        /// <param name="id">EmployeeID</param>
        /// <param name="startMonth">开始月份</param>
        /// <param name="endMonth">结束月份</param>
        /// <returns></returns>
        public async Task<ResponseModel> GetSaleProducts(string id,string startMonth="",string endMonth="",string product="")
        {
            Task<ResponseModel> result = Task<ResponseModel>.Factory.StartNew(()=> {
                return GetSaleResult(id,"product", startMonth, endMonth,product);
            });
            return result.Result;
        }

        /// <summary>
        /// 查询医院的销量，默认查询上个月的
        /// </summary>
        /// <param name="id">EmployeeID</param>
        /// <param name="productID">产品ID</param>
        /// <param name="startMonth">开始月份</param>
        /// <param name="endMonth">结束月份</param>
        /// <returns></returns>
        public async Task<ResponseModel> GetSalesHospitals(string id,string productID,string startMonth,string endMonth)
        {
            Task<ResponseModel> result = Task<ResponseModel>.Factory.StartNew(() => {
              return   GetSaleResult(id,"hospital",startMonth,endMonth,productID);
            });
            return result.Result;
        }
        /// <summary>
        /// 获取销量展示结果
        /// </summary>
        /// <param name="id">EmployeeID</param>
        /// <param name="queryType">查询类型</param>
        /// <param name="startMonth">开始月份</param>
        /// <param name="endMonth">结束月份</param>
        /// <param name="productID">产品ID</param>
        /// <returns></returns>
        public ResponseModel GetSaleResult(string id, string queryType, string startMonth="", string endMonth="",string productID= "")
        {
            ResponseModel responseModel = new();
            if (string.IsNullOrEmpty(startMonth))
            {
                throw new Exception("请查询具体日期！");
            }
            //最近一个月的
            if (string.IsNullOrEmpty(endMonth))
            {
                endMonth = startMonth;
            }
            startMonth = startMonth.Replace("-", "");
            endMonth = endMonth.Replace("-", "");
            SQLServerHelper serverHelper = new();
            //查询产品销量
            if (queryType=="product")
            {
                string productFilter = "";
                if (!string.IsNullOrEmpty(productID))
                {
                    productFilter = $" and Ffield0014=  '{productID}'";
                }
                
                DataTable dt = serverHelper.ExecuteSql(@$"SELECT  SUM(CONVERT(DECIMAL,Ffield0020)) amount,  Ffield0003 productID,Ffield0005,Ffield0011 FMonth, Ffield0014  productName FROM   yaodaibao.dbo.formmain_8728  WHERE Ffield0008 ='{id}' and Ffield0018 ='人员' and  CONVERT(INT,Ffield0011) BETWEEN CONVERT(INT,'{startMonth}') AND CONVERT(INT,'{endMonth}') {productFilter} GROUP BY Ffield0006,Ffield0003,Ffield0011,Ffield0014,Ffield0005 ");
                List<string> productList = new List<string>();
                List<string> listMonth = new List<string>();
                //拼接返回格式
                foreach (DataRow row in dt.Rows)
                {
                    string rowValues = "";
                    rowValues = $"{{\"month\":\"{row["FMonth"]}\",\"salesName\":\"{row["productName"]}\",\"productID\":\"{row["productID"]}\",\"sales\":\"{row["amount"]}\",\"unit\":\"{row["Ffield0005"]}\"}}";
                    productList.Add(rowValues);
                    //如果是同一个月份的产品放到同一个list里面
                    if (dt.Rows.IndexOf(row) == dt.Rows.Count - 1)
                        {
                        listMonth.Add($"{{ \"list\" :[{string.Join(',', productList.ToArray())}]}}");
                        productList.Clear();
                        }
                        else
                        {
                            if (row["FMonth"] == dt.Rows[dt.Rows.IndexOf(row) + 1]["FMonth"])
                            {

                                continue;
                            }
                            else
                            {
                                    listMonth.Add($"{{ \"list\" :[{string.Join(',', productList.ToArray())}]}}");
                                    productList.Clear();
                            }
                        }
 
                }
                    responseModel.DataRow = $"{string.Join(',', listMonth.ToArray())}";
            }
            //查询医院销量
            else if (queryType=="hospital")
            {
                List<string> hospitalList = new List<string>();
                DataTable dt = serverHelper.ExecuteSql(@$"SELECT  SUM(CONVERT(DECIMAL,Ffield0020)) amount,ti.FName,  Ffield0003,Ffield0014   productName FROM    yaodaibao.dbo.formmain_8728 t1 LEFT JOIN yaodaibao.dbo.t_Items ti ON t1.Ffield0002 = ti.FID   WHERE Ffield0008 ='{id}' and Ffield0003='{productID}' and Ffield0018 ='医院'  and  CONVERT(INT,Ffield0011) BETWEEN CONVERT(INT,'{startMonth}') AND CONVERT(INT,'{endMonth}')   GROUP BY Ffield0006,Ffield0003,Ffield0011,Ffield0014, Ffield0018,ti.FName ");
                //拼接返回的Json数据格式
                foreach (DataRow row in dt.Rows)
                {
                    string rowValues = $"{{\"sales\":\"{row["amount"]}\",\"hospital\":\"{row["FName"]}\"}}";
                    hospitalList.Add(rowValues);
                }
            responseModel.DataRow = $"{string.Join(',', hospitalList.ToArray())}";
            }
            return responseModel;
        }

        public async Task<ResponseModel> GetTotal(string id)
        {

            var nameList = Global.ConfigurationRoot.GetSection("RewardNames").AsEnumerable(true);
            string queryCondition = "";
            string result = "";
            //获取需要查询的奖金
            foreach (var name in nameList)
            {
                string key = name.Key;
                if (!string.IsNullOrEmpty(key))
                {
                    queryCondition += $"CONVERT(INT,ISNULL(t2.{key} ,''))+";
                }
            }
            SQLServerHelper sqlServerHelper = new SQLServerHelper();
            queryCondition = queryCondition.Remove(queryCondition.Length - 1);
            Task<ResponseModel> jsonResult = Task<ResponseModel>.Factory.StartNew(() =>
            {
                ResponseModel responseModel = new();
                //查询总奖金
                DataTable dt = sqlServerHelper.ExecuteSql(@$"select {queryCondition} Total from yaodaibao.dbo.Reward t1 left join yaodaibao.dbo.RewardNames t2 on t1.FID = t2.FID left join yaodaibao.dbo.t_Items t3 on t1.FCode = t3.FNumber WHERE t3.FID ='{id}' and left(convert(varchar,t1.FMonth, 120),10) = '{DateTime.Now.AddMonths(-1).ToString("yyyy-MM")}'  ");
                //添加$符号之后 {{ 表示{
                //查询有销售额的月份而且销售额 最高的产品
                DataTable dtproduct = sqlServerHelper.ExecuteSql(@$"SELECT  SUM(CONVERT(DECIMAL,Ffield0020)) amount,Ffield0003 ,Ffield0005,Ffield0011      FMonth, Ffield0014  productName  FROM  yaodaibao.dbo.formmain_8728  WHERE Ffield0008 = '{id}' and Ffield0018 ='人员' and Ffield0011 >= '{DateTime.Now.AddMonths(-6).ToString("yyyyMM")}'  GROUP BY Ffield0014,Ffield0008,Ffield0005,Ffield0011,Ffield0018,Ffield0003 ,Ffield0014 ORDER BY Ffield0011 DESC, amount DESC  ");
                responseModel.DataRow = $"{{ \"count\": {(dtproduct.Rows.Count != 0 ? dtproduct.Rows[0]["amount"] : 0)},\"id\":1,\"name\":\"{(dtproduct.Rows.Count != 0 ? dtproduct.Rows[0]["productName"] : "暂无数据")}\",\"month\":\"{(dtproduct.Rows.Count != 0 ? dtproduct.Rows[0]["FMonth"] : "")}\",\"unit\":\"{(dtproduct.Rows.Count != 0 ? dtproduct.Rows[0]["Ffield0005"] : "")}\"  }},{{ \"count\":{(dt.Rows.Count != 0 ? dt.Rows[0]["Total"] : 0)},\"id\":1,\"name\":\"奖金\"  }}";
                return responseModel;

            });
            return jsonResult.Result;
        }
    }
}