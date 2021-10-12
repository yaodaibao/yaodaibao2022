﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using iTR.Lib;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ydb.Report;

namespace ydb.WebService
{
    /// <summary>
    /// ReportDataInvoke 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class ReportDataInvoke : System.Web.Services.WebService
    {

        [WebMethod]
        public string GetCallReport1(string xmlMessage)
        {
           string result ="<GetData>" +
                          "<Result>False</Result>" +
                          "<Description></Description></GetData>";
            string logID = Guid.NewGuid().ToString();
            try
            {

                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", "GetCallReport1");

                if (Helper.CheckAuthCode("GetData", xmlMessage))
                {
                    CallRpt rpt = new CallRpt();
                    result = rpt.GetCallRepotr1(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "" +
                          "<GetData>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></GetData>";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", "GetCallReport1");
            return result;
        }


        [WebMethod]
        public string GetCallReport1Json(string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetData");
            string result = GetCallReport1(xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetData");
            return result;
        }

        [WebMethod]
        public string GetCallReport2(string xmlMessage)
        {
            string result = "<GetData>" +
                           "<Result>False</Result>" +
                           "<Description></Description></GetData>";
            string logID = Guid.NewGuid().ToString();
            try
            {

                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", "GetCallRepotr2");

                if (Helper.CheckAuthCode("GetData", xmlMessage))
                {
                    CallRpt rpt = new CallRpt();
                    result = rpt.GetCallRepotr2(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "" +
                          "<GetData>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></GetData>";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", "GetCallRepotr2");
            return result;
        }


        [WebMethod]
        public string GetCallReport2Json(string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetData");
            string result = GetCallReport2(xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetData");
            return result;
        }

        [WebMethod]
        public string ExportCallReport(string xmlMessage)
        {
            string result = "<GetData>" +
                           "<Result>False</Result>" +
                           "<Description></Description></GetData>";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", "ExportCallReport");

                if (Helper.CheckAuthCode("GetData", xmlMessage))
                {
                    CallRpt rpt = new CallRpt();
                    result = rpt.ExportCallReport(xmlMessage);
                }
 
            }
            catch (Exception err)
            {
                result = "" +

                          "<GetData>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></GetData>";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", "ExportCallReport");
            return result;
        }

        [WebMethod]
        public string ExportCallReportJson(string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetData");
            string result = ExportCallReport(xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetData");
            return result;
        }
        [WebMethod]
        public string GetPersonSummaryReport(string JsonMessage)
        {
            string result = "";
            result = GetCompassReport(JsonMessage, "GetPersonSummaryReport");
            return result;
        }

        //报表统一入口
        public string GetCompassReport(string JsonMessage, string callType)
        {

            ResultEntity resultEntity = new ResultEntity();
            ResultContent resultContent = new ResultContent();
            string content = "";

            CompassRpt compass = new CompassRpt();
            string result = "";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + JsonMessage, 1, "", callType);
                if (Helper.CheckAuthCode("GetData", JsonMessage, "json"))
                {
                    string mainresult = "";
                    if (callType == "GetPersonSummaryReport")
                    {
                        CompassRpt routeRpt = new CompassRpt();
                        mainresult = routeRpt.GetPersonPerReport(JsonMessage);
                    }
                    resultContent.DataRows = mainresult;
                    resultContent.Result = "True";
                }
            }
            catch (Exception err)
            {
                resultContent.Description = err.Message;
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            content = JsonConvert.SerializeObject(resultContent);
            resultEntity.GetData = content;
            result = JsonConvert.SerializeObject(content).Replace("\\", "");
            result = result.Substring(1);
            result = result.Substring(0, result.Length - 1);
            result = "{\"" + callType + "\":" + result + "}";
            return result;
        }
    }
    public class ResultEntity
    {
        public string GetData { get; set; }
    }
    public class ResultContent
    {
        public string Result { get; set; } = "False";
        public string Description { get; set; }
        public Object DataRows { get; set; }
    }
}
