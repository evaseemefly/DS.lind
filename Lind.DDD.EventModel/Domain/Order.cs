using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lind.DDD.Domain_Aggregate_Event.Domain
{
    /// <summary>
    /// 聚合根－订单
    /// 由多个实体和多个值对象组成,全局唯一标识
    /// </summary>
    public class Order : AggregateRoot
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public decimal TotalFee { get; set; }
        public int OrderStatus { get; set; }

        public Shipping Shipping { get; set; }
        public Address Address { get; set; }
        public IEnumerable<OrderDetail> OrderDetail { get; set; }

        public void Paid()
        {
            this.OrderStatus = 1;
        }

        public void Signed()
        {
            this.OrderStatus = 2;
        }
    }
    /// <summary>
    /// 实体-订单项
    /// 由字段和值对象组成,在某个聚合根下有唯一标识
    /// </summary>
    public class OrderDetail : EntityBase
    {
        public Guid OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal SalePrice { get; set; }
        public int SaleCount { get; set; }
    }
    /// <summary>
    /// 值对象－快递与状态
    /// 没有标识列
    /// </summary>
    public class Shipping
    {
        public string Name { get; set; }
        public string Info { get; set; }
        public string Status { get; set; }
    }
    /// <summary>
    /// 值对象-收获地址
    /// </summary>
    public class Address
    {
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public override string ToString()
        {
            return string.Format("{0}-{1}-{2}", Province, City, District);
        }
    }
}
