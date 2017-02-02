using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lind.DDD.Events;
namespace Lind.DDD.Events.Demo
{
    public class SendSMSEventHandler :
          IEventHandler<OrderEvent>,
          IEventHandler<UserEvent>
    {
        #region IEventHandler<OrderEvent> 成员

        public void Handle(OrderEvent evt)
        {
            Console.WriteLine("为订单处理发短信");
        }

        #endregion

        #region IEventHandler<UserEvent> 成员

        public void Handle(UserEvent evt)
        {
            Console.WriteLine("为用户管理发短信");
        }

        #endregion
    }
}
