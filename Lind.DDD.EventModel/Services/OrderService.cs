using Lind.DDD.Domain_Aggregate_Event.Domain;
using Lind.DDD.Domain_Aggregate_Event.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lind.DDD.Domain_Aggregate_Event.Services
{
    /// <summary>
    /// 订单服务
    /// </summary>
    public class OrderService : IOrderService
    {
        IOrderRepository _orderRepository;
        public OrderService()
        {
            _orderRepository = new OrderRepository();
        }
        public void GeneratorOrder(Order order)
        {
            order.TotalFee = order.OrderDetail.Sum(i => i.SaleCount * i.SalePrice);
            order.OrderStatus = 1;
            _orderRepository.InsertOrder_Detail(order);
            EventBus.Instance.Publish(new Events.OrderCreated
            {
                OrderId = order.Id.ToString(),
                UserName = order.UserName
            });
        }

        public void PaidOrder(Order order)
        {
            order.OrderStatus = 2;
            _orderRepository.UpdateOrder(order);
            EventBus.Instance.Publish(new Events.OrderPaid
            {
                OrderId = order.Id.ToString(),

            });

        }

        public void ShippingOrder(Order order)
        {
            order.OrderStatus = 3;
            _orderRepository.UpdateOrder(order);
            EventBus.Instance.Publish(new Events.OrderDelivered
            {
                OrderId = order.Id.ToString()
            });
        }

        public void SignedOrder(Order order)
        {
            order.OrderStatus = 4;
            _orderRepository.UpdateOrder(order);
            EventBus.Instance.Publish(new Events.OrderSigned
            {
                SignTime = DateTime.Now,
                SignUserName = order.UserName,
                OrderId = order.Id.ToString()
            });
        }
        public IList<Order> GetOrder(Expression<Func<Order, bool>> spec) { return null; }

        public IList<Order> GetOrder(OrderSpecification spec)
        {
            return _orderRepository.GetModel(spec.SatisfiedBy())
                                   .ToList();

        }
    }
}
