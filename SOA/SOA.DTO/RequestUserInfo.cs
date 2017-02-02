using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lind.DDD.EntityValidation;
using Lind.DDD.SOA;
namespace SOA.DTO
{
    /// <summary>
    /// 用户－请求参数
    /// 输入参数各属性都是可空的,为空时不去验证,并且查询时不去构造查询条件
    /// </summary>
    public class RequestUserInfo : RequestBase
    {
        public string Id { get; set; }
        [MaxLength(10, ErrorMessage = "用户名最多为10个字符")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "Email地址不是合法的")]
        public string Email { get; set; }
        [MaxLength(20, ErrorMessage = "用户名最多为20个字符")]
        public string RealName { get; set; }
    }
}
