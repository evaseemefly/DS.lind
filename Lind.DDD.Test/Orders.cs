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
    
    public partial class Orders :Lind.DDD.Domain.IEntity
    {
        public System.Guid Id { get; set; }
        public System.Guid UserId { get; set; }
        public string UserName { get; set; }
        public decimal TotalFee { get; set; }
        public Nullable<int> OrderStatus { get; set; }
    }
}