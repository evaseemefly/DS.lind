using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lind.DDD.Events;
namespace Lind.DDD.Events.Demo
{
    /// <summary>
    /// 发邮件功能[某个事件的行为]
    /// </summary>
    [HandlesAsynchronouslyAttribute]
    public class SendEmailEventHandler :
        IEventHandler<OrderEvent>,
        IEventHandler<UserEvent>
    {

        #region IEventHandler<OrderEvent> 成员

        public void Handle(OrderEvent evt)
        {
            Console.WriteLine("生成和确认订单{0}时发Email", evt.OrderId);
        }

        #endregion

        #region IEventHandler<UserEvent> 成员

        public void Handle(UserEvent evt)
        {
            Console.WriteLine("建立用户后发Email，用户ＩＤ{0}", evt.UserId);
        }

        #endregion
    }
}
