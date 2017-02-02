using Lind.DDD.Domain_Aggregate_Event.Bus;
using Lind.DDD.Domain_Aggregate_Event.Domain;
using Lind.DDD.Domain_Aggregate_Event.EventHandlers;
using Lind.DDD.Domain_Aggregate_Event.Events;
using Lind.DDD.Domain_Aggregate_Event.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.Domain_Aggregate_Event
{

    class Class1<T>
    {
        public void Test(T t)
        {
            Console.WriteLine(t);
        }
    }
    class Program
    {
        static void InvokeTest(Type t, params object[] args)
        {
            Type type = typeof(Class1<>);
            type = type.MakeGenericType(t);
            object o = Activator.CreateInstance(type);
            type.InvokeMember("Test", BindingFlags.Default | BindingFlags.InvokeMethod, null, o, args);
        }
        static void Main(string[] args)
        {


            InvokeTest(typeof(int), 1);


            Console.WriteLine("订阅所有的IEventHandler处理程序");
            //订阅所有的IEventHandler处理程序
            EventBus.Instance.SubscribeAll();
            EventBus.Instance.Publish(new OrderCreated { });
            EventBus.Instance.Publish(new OrderDelivered { });

            EventBus.Instance.Publish(new EntityCreatedEventData<Order>(new Order { }));
            EventBus.Instance.Publish(new EntityDeletedEventData<Order>(new Order { }));
            EventBus.Instance.Publish(new EntityUpdatedEventData<Order>(new Order { }));


            IOrderService orderService = new OrderService();
            var detail = new List<OrderDetail>();
            detail.Add(new OrderDetail { ProductId = 1, ProductName = "tel", SaleCount = 10, SalePrice = 999 });
            detail.Add(new OrderDetail { ProductId = 1, ProductName = "pad", SaleCount = 5, SalePrice = 1999 });

            var obj = detail;
            Console.WriteLine("ReferenceEquals:" + ReferenceEquals(detail, obj));



            var order = new Order
            {
                CreateTime = DateTime.Now,
                Address = new Address { Province = "北京", City = "大興", District = "西紅門" },
                UpdateTime = DateTime.Now,
                UserId = 1,
                UserName = "xiaohong",
                OrderDetail = detail,
            };
            // step1
            orderService.GeneratorOrder(order);
            // step2
            orderService.PaidOrder(order);
            // step3
            orderService.ShippingOrder(order);
            // step4
            orderService.SignedOrder(order);
            Console.ReadKey();

        }
    }
}
