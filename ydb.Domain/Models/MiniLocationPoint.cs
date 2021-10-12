using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ydb.Domain.Models
{
   public  class MiniLocationPoint
    {
        public string ID { get; set; }
        public string EmployeeID { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        //自动匹配Json 子级数据
        public List<MiniSurrAddress> MiniSurrAddress { get; set; }
    }
}
