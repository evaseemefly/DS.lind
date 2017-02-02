using Lind.DDD.Domain_Aggregate_Event.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lind.DDD.Domain_Aggregate_Event.Services
{
    /// <summary>
    /// 订单服务
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// 确实订单
        /// </summary>
        /// <param name="order"></param>
        void GeneratorOrder(Order order);
        /// <summary>
        /// 订单付款
        /// </summary>
        /// <param name="order"></param>
        void PaidOrder(Order order);
        /// <summary>
        /// 订单发货
        /// </summary>
        /// <param name="order"></param>
        void ShippingOrder(Order order);
        /// <summary>
        /// 订单签收
        /// </summary>
        /// <param name="order"></param>
        void SignedOrder(Order order);
        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        IList<Order> GetOrder(OrderSpecification spec);
    }
}
