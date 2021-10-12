using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ydb.Domain.Interface
{
   public  interface IQueryItem
    {
        public ResponseModel GetItems(string employeeID, string itemType,  string queryValue="",int pageIndex=1,int pageSize=100);
    }
}
