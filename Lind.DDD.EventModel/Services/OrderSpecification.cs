using Lind.DDD.Domain_Aggregate_Event.Domain;
using Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lind.DDD.Domain_Aggregate_Event.Services
{
    /// <summary>
    /// 订单规约
    /// </summary>
    public class OrderSpecification : Specification<Order>
    {
        DateTime? _fromDate;
        DateTime? _toDate;
        string _userName;
        int? _age;
        public OrderSpecification(DateTime? fromDate, DateTime? toDate, string userName, int age = 0)
        {
            _fromDate = fromDate;
            _toDate = toDate;
            _userName = userName;
            _age = age;
        }
        public override System.Linq.Expressions.Expression<Func<Order, bool>> SatisfiedBy()
        {
            Specification<Order> spec = new TrueSpecification<Order>();
            if (_fromDate.HasValue)
                spec &= new DirectSpecification<Order>(o => o.CreateTime >= _fromDate);
            if (_toDate.HasValue)
            {
                var val = _toDate.Value.AddDays(1);
                spec &= new DirectSpecification<Order>(o => o.CreateTime < val);
            }
            if (!string.IsNullOrWhiteSpace(_userName))
                spec &= new DirectSpecification<Order>(o => o.UserName == _userName);
            if (_age > 0)
            {

            }
            return spec.SatisfiedBy();
        }

    }
}
