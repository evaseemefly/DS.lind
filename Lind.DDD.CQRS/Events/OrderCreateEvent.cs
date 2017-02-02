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
        public Guid OrderId { get; set; }
        public string ProductName { get; set; }
        public int UserId { get; set; }
    }
}
