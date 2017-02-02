using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.CQRS.EventHandlers
{
    /// <summary>
    /// 发Email的事件处理程序
    /// </summary>
    public class SendEmailEventHandler :
        IEventHandler<Events.OrderCreateEvent>,
        IEventHandler<Events.OrderConfirmEvent>
    {

        public void Action(Events.OrderCreateEvent t)
        {
            Console.WriteLine("Email:这个订单" + t.OrderId + "被" + t.UserId + "建立了,购买了商品" + t.ProductName);
            Logger.NormalLogger.Info("Email:这个订单" + t.OrderId + "被" + t.UserId + "建立了,购买了商品" + t.ProductName);
        }

        public void Action(Events.OrderConfirmEvent t)
        {
            Console.WriteLine("Email:这个订单" + t.OrderId + "被" + t.UserId + "在" + t.ConfirmDate + "被确认了");
            Logger.NormalLogger.Info("Email:这个订单" + t.OrderId + "被" + t.UserId + "在" + t.ConfirmDate + "被确认了");
        }
    }
}
