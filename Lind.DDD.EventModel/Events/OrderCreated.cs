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
    public class OrderCreated : EventData
    {
        public override string ToString()
        {
            return "订单已经确认";
        }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal TotalFee { get; set; }
        /// <summary>
        /// 购买者ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 购买者
        /// </summary>
        public string UserName { get; set; }

        public string OrderId { get; set; }

    }

}
