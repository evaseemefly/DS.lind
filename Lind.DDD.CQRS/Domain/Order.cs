using Lind.DDD.CQRS.EventHandlers;
using Lind.DDD.CQRS.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.CQRS.Domain
{
    public enum OrderStatus
    {
        Create,
        Confirm,
        Paid,
        Send,
        Receive,
    }
    /// <summary>
    /// 订单顶，值对象
    /// </summary>
    public class OrderItem
    {
        public Guid ItemId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
    }
    /// <summary>
    /// 订单，实体
    /// POCO实体是只有属性和修饰自身状态的方法
    /// </summary>
    public class Order
    {
        event Action<OrderCreateEvent> handle;
        event Action<OrderConfirmEvent> handle2;
        public Order()
        {

            //事件订阅
            handle += Order_handle;
            handle2 += Order_handle2;
        }

        void Order_handle2(OrderConfirmEvent obj)
        {
            new SendEmailEventHandler().Action(obj);
        }

        void Order_handle(OrderCreateEvent obj)
        {
            new SendEmailEventHandler().Action(obj);
        }

        #region 属性
        public Guid OrderId { get; set; }
        public DateTime CreateDate { get; set; }
        public int UserId { get; set; }
        public OrderStatus Status { get; set; }
        #endregion

        #region 导航属性
        public IList<OrderItem> OrderItem { get; set; }
        #endregion

        #region 领域事件
        public void Create()
        {
            //事件发布
            handle(new OrderCreateEvent { OrderId = Guid.NewGuid(), UserId = 1, ProductName = "电脑" });
        }

        public void Paid() { }

        public void Confirm()
        {
            handle2(new OrderConfirmEvent { OrderId = Guid.NewGuid(), UserId = 1, ConfirmDate = DateTime.Now });
        }

        public void Send() { }

        public void Recivie() { }
        #endregion


    }
}
