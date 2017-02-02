using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.Test.EventHandlers
{
    /// <summary>
    /// 订单库存处理程序
    /// </summary>
    //public class ProductInventoryEventHandler : Lind.DDD.Events.IEventHandler<ProductInfo>
    //{
    //    #region IEventHandler<ProductInfo> 成员

    //    public void Handle(ProductInfo evt)
    //    {
    //        var productRepository = new Lind.DDD.Repositories.EF.EFRepository<ProductInfo>();
    //        productRepository.SetDataContext(new testEntities());
    //        var entity = productRepository.Find(i => i.ProductId == evt.ProductId);
    //        if (entity != null)
    //        {
    //            if (entity.Inventory > 0)
    //                entity.Inventory -= 1;
    //            productRepository.Update(entity);
    //        }
    //    }

    //    #endregion
    //}
}
