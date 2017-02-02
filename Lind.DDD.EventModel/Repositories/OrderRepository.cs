using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Lind.DDD.Domain_Aggregate_Event.Repositories
{
    /// <summary>
    /// 订单仓储
    /// </summary>
    public class OrderRepository : IOrderRepository
    {
        public void InsertOrder_Detail(Domain.Order order)
        {
            using (var trans = new TransactionScope())
            {
                //订单表
                //订单明细表
                trans.Complete();
            }
        }

        public void InsertUserAddress(Domain.Address address)
        {
            throw new NotImplementedException();
        }

        public IList<Domain.Order> GetModel(System.Linq.Expressions.Expression<Func<Domain.Order, bool>> predicate)
        {
            throw new NotImplementedException();
        }


        public void UpdateOrder(Domain.Order order)
        {
            //更新
        }
    }
}
