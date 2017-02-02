using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.Domain_Aggregate_Event.EventHandlers
{

    /// <summary>
    /// 订单被插入时的处理程序
    /// </summary>
    [Serializable]
    public class OrderInsertEventHandler :
        IEventHandler<Events.OrderCreated>,
          IEventHandler<Events.OrderDelivered>
    {
        #region IEventHandler<OrderSigned> 成员

        public void Handle(Events.OrderCreated evt)
        {
            Console.WriteLine("订单确认,下单用户:" + evt.UserName);
        }

        #endregion

        public void Handle(Events.OrderDelivered evt)
        {
            Console.WriteLine("OrderDelivered,下单用户:" + evt.OrderId);

        }
    }
}
