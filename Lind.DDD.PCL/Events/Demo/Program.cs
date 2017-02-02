using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lind.DDD.Events;
namespace Lind.DDD.Events.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            EventBus.Instance.Subscribe<OrderEvent>(new SendEmailEventHandler());
            EventBus.Instance.Subscribe<OrderEvent>(new SendSMSEventHandler());
            EventBus.Instance.Subscribe<UserEvent>(new SendSMSEventHandler());

            var entity = new OrderEvent { OrderId = Guid.NewGuid() };
            Console.WriteLine("生成一个订单，单号为{0}", entity.OrderId);
            EventBus.Instance.Publish(entity);
            Console.ReadKey();
        }
    }
}
