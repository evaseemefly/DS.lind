using Lind.DDD.SOA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOA.DTO
{
    /// <summary>
    /// 用户－响应结果
    /// </summary>

    public class ResponseUserInfo : ResponseBase
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
    }
}
