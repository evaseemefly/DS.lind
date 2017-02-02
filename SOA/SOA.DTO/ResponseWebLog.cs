using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOA.DTO
{
    public class ResponseWebLog : Lind.DDD.SOA.ResponseBase
    {
        public int Type { get; set; }
        public string Message { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Operator { get; set; }
    }
}
