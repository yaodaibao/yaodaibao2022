using Elasticsearch.Net;
using iTR.LibCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ydb.BLL;
using ydb.Domain.Interface;
using ydb.Domain.Models;

namespace ydb.Domain.Service
{
    //小程序登陆
    public class LoginService : ILoginService
    {
        public ResponseModel ChangePwd([FromBody] Login login)
        {
            if (string.IsNullOrEmpty(login.Mobile))
            {
                return new ResponseModel() { Result = false, Description = "手机号码不能为空" };
            }

            if (string.IsNullOrEmpty(login.Password))
            {
                return new ResponseModel() { Result = false, Description = "密码不能为空" };
            }
            string sql = $"Select FID From t_Employees Where FMobile='{login.Mobile}' OR FLoginName='{login.UserName}'";
            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);
            if (login.Password != login.ConfirmPassword)
            {
                return new ResponseModel() { Result = false, Description = "输入密码不一致" };
            }
            string loginpwd = login.Password;
            if (dt.Rows.Count == 0)
                return new ResponseModel() { Result = false, Description = "用户不存在" };

            loginpwd = Common.EncryptDES(loginpwd, Common.DesKey);
            sql = "Update t_Employees Set FLoginPwd='" + loginpwd + $"',FRegisted=1  Where FMobile='{login.Mobile}' OR FLoginName='{login.UserName}'";

            if (runner.ExecuteSqlNone(sql) < 1)
            {
                return new ResponseModel() { Result = false };
            }
            return new ResponseModel();
        }

        public ResponseModel CheckVCode([FromBody] Login login)
        {
            string curTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            string sql = $"Select FExpireTime From VCodes Where FMobile='{login.Mobile}' and FCode ='{login.Code}' and FStatus =0 and  '" + curTime + "' Between FCreateTime and FExpireTime";

            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);
            if (dt.Rows.Count == 0)//
            {
                return new ResponseModel() { Result = false, Description = "错误或已过期" };

            }
            else
            {
                sql = $"Update VCodes Set FStatus =1 Where  FMobile='{login.Mobile}' and FCode ='{login.Code}' and FStatus =0 and  '" + curTime + "' Between FCreateTime and FExpireTime";
                runner.ExecuteSqlNone(sql);
            }

            return new ResponseModel() { DataRow = "5F1B87CACCE747E5DD5F813FBC9163E5" };
        }

        public ResponseModel GetRegStatusByMobile([FromBody] Login login)
        {
            if (string.IsNullOrEmpty(login.Mobile))
            {
                return new ResponseModel() { Result = false, Description = "用户名不能为空" };
            }


            DataTable dt = new DataTable();
            string status = "0", queryStatus = "False", employeeID = "-1", id = "-1";
            SQLServerHelper runner = new SQLServerHelper();
            string sql = $"Select t1.FID,t1.FRegisted,Isnull(t2.FID,'') As FEmployeeID" +
                     " From Reg_Application t1" +
                     " Left Join t_Employees t2 On t1.FMobile= t2.FMobile" +
                     $" Where t1.FDeleted=0 and t1.FMobile='{login.Mobile}'";
            dt = runner.ExecuteSql(sql);
            if (dt.Rows.Count > 0)
            {
                status = "1";
                if (bool.Parse(dt.Rows[0]["FRegisted"].ToString()))
                    status = "2";
                else
                    status = "3";
                id = dt.Rows[0]["FID"].ToString();
                employeeID = dt.Rows[0]["FEmployeeID"].ToString();
                queryStatus = "True";
            }
            else
            {
                sql = $@"Select FID As FEmployeeID,FRegisted  from t_Employees Where FMobile='{login.Mobile}' and FIsDeleted=0";
                dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)
                {
                    id = "-1";
                    employeeID = dt.Rows[0]["FEmployeeID"].ToString();
                    queryStatus = "True";
                    if (bool.Parse(dt.Rows[0]["FRegisted"].ToString()))
                        status = "2";
                    else
                        status = "1";
                }
            }

            return new ResponseModel() { DataRow = $"{status }" };
        }

        public ResponseModel Login([FromBody] Login login)
        {
            string pwd = Common.EncryptDES(login.Password, Common.DesKey);
            string sql = "";
            if (string.IsNullOrEmpty(login.UserName) && string.IsNullOrEmpty(login.Mobile))
            {
                return new ResponseModel() { Result = false, Description = "用户名不能为空" };
            }

            if (string.IsNullOrEmpty(login.Password))
            {
                return new ResponseModel() { Result = false, Description = "密码不能为空" };
            }

            if (!string.IsNullOrEmpty(login.Mobile))
            {
                sql = "Select t1.*,t2.FName As FEmployeeName from " +
                      " t_Employees t1 " +
                      " Left Join t_Items t2 On t1.FID= t2.FID";
                sql = sql + " Where t1.FMobile='" + login.Mobile + "' and t1.FLoginPwd='" + pwd + "' And t1.FIsDeleted=0";
            }
            else if (!string.IsNullOrEmpty(login.UserName))
            {

                sql = "Select t1.*,t2.FName As FEmployeeName from " +
                      " t_Employees t1 " +
                      " Left Join t_Items t2 On t1.FID= t2.FID";
                sql = sql + " Where t1.FLoginPwd='" + pwd + "' And  t1.FLoginName ='" + login.UserName + "'  and  t1.FIsDeleted=0";
            }


            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);
            if (dt.Rows.Count < 1)
            {
                return new ResponseModel { Result = false, Description = "账号或者密码错误" };
            }
            else
            {
                //登陆完成交换 登陆秘钥
                //return new ResponseModel { Result = true, DataRow = "0uyV7aK3cvAWcDRZowdyP3AMH27pvw0dmuvqfsJy5+XO/vl6Wdi9Cg==|" + dt.Rows[0]["FID"].ToString() };
                return new ResponseModel() { Result = true, Description = "登陆完成", DataRow = $"{{\"CompanyID\":\"{ dt.Rows[0]["FCompanyID"]}\",\"EmployeeID\":\"{ dt.Rows[0]["FID"]}\",\"RoleID\":\"{ dt.Rows[0]["FRoleID"]}\",\"PageID\":\"{ dt.Rows[0]["FPageID"]}\",\"DeptID\":\"{ dt.Rows[0]["FDeptID"]}\",\"EmployeeName\":\"{ dt.Rows[0]["FEmployeeName"]}\",\"Mobile\":\"{ dt.Rows[0]["FMobile"]}\",\"Profile\":\"{ dt.Rows[0]["FProfile"]}\"}}" };
            }


        }

        public ResponseModel SendVCode([FromBody] Login login)
        {
            string curTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string vCode = "";
            string sql = "Select FCode from VCodes Where '" + curTime + $"' Between FCreateTime and FExpireTime and FStatus =0 and FMobile='{login.Mobile}'";
            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);
            if (dt.Rows.Count > 0)//存在未验证且在有限期内
            {
                vCode = dt.Rows[0]["FCode"].ToString();
            }
            else
            {
                Random ran = new Random();
                vCode = ran.Next(1000, 9999).ToString();
            }
            AliDayuSMS smsSender = new AliDayuSMS();
            if (smsSender.SendSms(vCode, login.Mobile) == "1" && dt.Rows.Count == 0)//发送成功,且不存在该记录
            {
                DateTime expireTime = DateTime.Now.AddMinutes(5);
                sql = $"Insert Into VCodes(FMobile,FCode)Values('{login.Mobile}','" + vCode + "')";
                runner = new SQLServerHelper();
                if (runner.ExecuteSqlNone(sql) < 0)
                {
                    return new ResponseModel() { Result = false };
                }
            }

            return new ResponseModel();
        }

        public ResponseModel SetPwd([FromBody] Login login)
        {
            string sql = $"Select FID From t_Employees Where FMobile='{login.Mobile}' OR FLoginName='{login.UserName}'";
            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);
            if (login.Password != login.ConfirmPassword)
            {
                return new ResponseModel() { Result = false, Description = "输入密码不一致" };
            }

            if (string.IsNullOrEmpty(login.Password))
            {
                return new ResponseModel() { Result = false, Description = "密码不能为空" };
            }
            string loginpwd = login.Password;
            loginpwd = Common.EncryptDES(loginpwd, Common.DesKey);
            sql = "Update t_Employees Set FLoginPwd='" + loginpwd + $"',FRegisted=1  Where FMobile='{login.Mobile}' OR FLoginName='{login.UserName}'";

            if (runner.ExecuteSqlNone(sql) < 1)
            {
                return new ResponseModel() { Result = false, Description = "重置密码失败" };
            }
            return new ResponseModel();
        }
    }
}
