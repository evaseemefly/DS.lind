using Lind.DDD.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOA.Entity
{
    public class UserInfo : NoSqlEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RealName { get; set; }
    }
}
