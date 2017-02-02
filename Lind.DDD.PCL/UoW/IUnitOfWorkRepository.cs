using Lind.DDD.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.UoW
{
    /// <summary>
    /// 工作单元中仓储接口CRUD操作
    /// 需要使用工作单元的仓储，需要实现本接口（IRepository,IExtensionRepository)
    /// </summary>
    public interface IUnitOfWorkRepository
    {
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="item"></param>
        void UoWInsert(IEntity item);
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="item"></param>
        void UoWUpdate(IEntity item);
        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="item"></param>
        void UoWDelete(IEntity item);
    }
}
