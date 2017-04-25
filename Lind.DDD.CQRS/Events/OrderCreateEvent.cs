using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.CQRS.Events
{

    /// <summary>
    /// 订单被建立，事件源
    /// </summary>
    public class OrderCreateEvent:EventBase
    {
        /// <summary>
        /// 订单id
        /// </summary>
        public Guid OrderId { get; set; }
        /// <summary>
        /// 商品名字
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 购买者
        /// </summary>
        public int UserId { get; set; }
    }
}
