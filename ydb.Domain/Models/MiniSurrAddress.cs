using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ydb.Domain.Models
{
    public class MiniSurrAddress
    {
        public string ID { get; set;}
        public string Distance { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string LocationType { get; set; }
        public string TypeCode { get; set; }

        public string Address { get; set; }
 
    }
}
