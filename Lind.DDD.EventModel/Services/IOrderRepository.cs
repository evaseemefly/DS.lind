using Lind.DDD.Domain_Aggregate_Event.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lind.DDD.Domain_Aggregate_Event.Repositories
{
    /// <summary>
    /// 订单仓储
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// 插入订单和订单项
        /// </summary>
        /// <param name="order"></param>
        void InsertOrder_Detail(Order order);
        /// <summary>
        /// 插入订单－用户常用地址
        /// </summary>
        /// <param name="address"></param>
        void InsertUserAddress(Address address);
        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="order"></param>
        void UpdateOrder(Order order);
        /// <summary>
        /// 得到订单列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IList<Order> GetModel(Expression<Func<Order, bool>> predicate);
    }
}
