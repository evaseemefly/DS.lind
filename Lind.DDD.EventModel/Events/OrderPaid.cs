using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.Domain_Aggregate_Event.Events
{
    /// <summary>
    /// 订单被付款的事件源
    /// </summary>
    [Serializable]
    public class OrderPaid : EventData
    {
        public override string ToString()
        {
            return "订单已经付款";
        }
        public string OrderId { get; set; }

    }
}
