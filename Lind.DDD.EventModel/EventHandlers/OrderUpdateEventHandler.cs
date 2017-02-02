using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.Domain_Aggregate_Event.EventHandlers
{
    /// <summary>
    /// 订单被更新的事件处理程序
    /// </summary>
    [Serializable]
    public class OrderUpdateEventHandler :
         IEventHandler<Events.OrderPaid>,
         IEventHandler<Events.OrderSigned>,
         IEventHandler<Events.OrderDelivered>
    {
        #region IEventHandler<OrderPaid> 成员

        public void Handle(Events.OrderPaid evt)
        {
            Console.WriteLine("订单付款");

        }

        #endregion

        #region IEventHandler<OrderSigned> 成员

        public void Handle(Events.OrderSigned evt)
        {
            Console.WriteLine("订单签收");

        }

        #endregion

        #region IEventHandler<OrderShipping> 成员

        public void Handle(Events.OrderDelivered evt)
        {
            Console.WriteLine("订单发货");
        }

        #endregion
    }
}
