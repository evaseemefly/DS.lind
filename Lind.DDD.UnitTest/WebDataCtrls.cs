//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lind.DDD.UnitTest
{
    using System;
    using System.Collections.Generic;
    
    public partial class WebDataCtrls :Lind.DDD.Domain.IEntity
    {
        public WebDataCtrls()
        {
            this.WebDataSettings = new HashSet<WebDataSettings>();
        }
    
        public int ID { get; set; }
        public string DataCtrlName { get; set; }
        public string DataCtrlType { get; set; }
        public string DataCtrlField { get; set; }
        public string DataCtrlApi { get; set; }
        public string Description { get; set; }
        public System.DateTime DataCreateDateTime { get; set; }
        public System.DateTime DataUpdateDateTime { get; set; }
        public int DataStatus { get; set; }
    
        public virtual ICollection<WebDataSettings> WebDataSettings { get; set; }
    }
}
