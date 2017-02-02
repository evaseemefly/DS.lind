using Lind.DDD.Domain;
using Lind.DDD.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.UoW
{
    /// <summary>
    /// 工作单元
    /// 所有数据上下文对象都应该继承它，面向仓储的上下文应该与具体实现（存储介质,ORM）无关
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 向工作单元中注册变更
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="type"></param>
        /// <param name="repository"></param>
        void RegisterChangeded(IEntity entity, SqlType type, IUnitOfWorkRepository repository);
        /// <summary>
        /// 向数据库提交变更
        /// </summary>
        void Commit();
    }
}
