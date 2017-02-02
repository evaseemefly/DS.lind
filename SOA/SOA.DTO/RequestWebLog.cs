using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOA.DTO
{
    public class RequestWebLog : Lind.DDD.SOA.RequestBase
    {
        [Range(1, 5,ErrorMessage="参数取值在1和5之间")]
        public int Type { get; set; }
    }
}
