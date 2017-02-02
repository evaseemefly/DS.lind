using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.Domain_Aggregate_Event.Events
{
    /// <summary>
    /// 订单被签收的事件源
    /// </summary>
    public class OrderSigned : EventData
    {
        public string SignUserName { get; set; }
        public DateTime SignTime { get; set; }
        public string OrderId { get; set; }
    }
}
