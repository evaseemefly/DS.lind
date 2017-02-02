using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.Domain_Aggregate_Event.Events
{
    /// <summary>
    /// 订单被确认的事件源
    /// </summary>
    [Serializable]
    public class OrderDelivered : EventData
    {
        public override string ToString()
        {
            return "订单已经发货";
        }
        public string OrderId { get; set; }
        public string ShippingTime { get; set; }


    }

}
