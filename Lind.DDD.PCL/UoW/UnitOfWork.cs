using Lind.DDD.Domain;
using Lind.DDD.IoC;
using Lind.DDD.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lind.DDD.UoW
{

    /// <summary>
    /// 工作单元，主要用于管理事务性操作
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields
        /// <summary>
        /// 操作行为字典
        /// </summary>
        private IDictionary<IEntity, IUnitOfWorkRepository> insertEntities;
        private IDictionary<IEntity, IUnitOfWorkRepository> updateEntities;
        private IDictionary<IEntity, IUnitOfWorkRepository> deleteEntities;

        #endregion

        #region Constructor

        public UnitOfWork()
        {
            insertEntities = new Dictionary<IEntity, IUnitOfWorkRepository>();
            updateEntities = new Dictionary<IEntity, IUnitOfWorkRepository>();
            deleteEntities = new Dictionary<IEntity, IUnitOfWorkRepository>();
        }

        #endregion

        #region IUnitOfWork 成员
        /// <summary>
        /// 事务提交
        /// </summary>
        public void Commit()
        {
            try
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {

                    foreach (var entity in insertEntities.Keys)
                    {
                        insertEntities[entity].UoWInsert(entity);
                    }
                    foreach (var entity in updateEntities.Keys)
                    {
                        updateEntities[entity].UoWUpdate(entity);
                    }
                    foreach (var entity in deleteEntities.Keys)
                    {
                        deleteEntities[entity].UoWDelete(entity);
                    }
                    transactionScope.Complete();//提交事务，程序中如果出错，这行无法执行，即事务不会被提交，这就类似于rollback机制
                }
            }
            catch (Exception ex)
            {
                Logger.LoggerFactory.Instance.Logger_Error(ex);
            }

        }



        /// <summary>
        /// 注册数据变更
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="type"></param>
        public void RegisterChangeded(IEntity entity, SqlType type, IUnitOfWorkRepository repository)
        {
            switch (type)
            {
                case SqlType.Insert:
                    insertEntities.Add(entity, repository);
                    break;
                case SqlType.Update:
                    updateEntities.Add(entity, repository);
                    break;
                case SqlType.Delete:
                    deleteEntities.Add(entity, repository);
                    break;
                default:
                    throw new ArgumentException("you enter reference is error.");
            }
        }
        #endregion


    }
}
