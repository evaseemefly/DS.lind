//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lind.DDD.Test
{
    using System;
    using System.Collections.Generic;
    
    public partial class Roles :Lind.DDD.Domain.IEntity
    {
        public Roles()
        {
            this.Users = new HashSet<Users>();
        }
    
        public System.Guid ApplicationId { get; set; }
        public System.Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
    
        public virtual Applications Applications { get; set; }
        public virtual ICollection<Users> Users { get; set; }
    }
}
