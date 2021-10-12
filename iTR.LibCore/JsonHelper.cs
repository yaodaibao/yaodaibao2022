using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTR.LibCore
{
    class JsonHelper
    {
        public string JsonFormat(string jsonString)
        {
            string result = "";
            jsonString = @"{
   Result: true,
   Description:,
   DataRows:
            {
            DataRow:
                [
               {
                list:
                    [
                   {

                    month: 2021 - 04
                   },
               {
                    bonusName: 自营增长奖,
                  bonus: 4
               },
               {
                    bonusName: KPI奖金额,
                  bonus: 3
               },
               {
                    bonusName: 招商提成奖金额,
                  bonus: 2
               },
               {
                    bonusName: 自营提成奖金额,
                  bonus: 1
               }
            ]
         },
         {
                list:
                    [
                   {
                    month: 2021 - 05
                   },
               {
                    bonusName: KPI奖金额,
                  bonus: 3
               },
               {
                    bonusName: 招商提成奖金额,
                  bonus: 2
               },
               {
                    bonusName: 自营提成奖金额,
                  bonus: 1
               }
            ]
         }
      ]
   }
        }
";
            var jsonobj = JObject.Parse(jsonString);
            foreach (var item in jsonobj)
            {
                IEnumerable<JProperty> jProperties = jsonobj.Properties();
            }
            return result;
        }
    }
}
