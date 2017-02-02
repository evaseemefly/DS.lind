using Lind.DDD.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
namespace Lind.DDD.Repositories.Dapper
{
    /// <summary>
    /// 使用Dapper实现的仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class DapperRepository<TEntity> : IRepositories.IRepository<TEntity> where TEntity : Entity
    {
        IDbConnection conn;
        string connString;
        string tableName;
        public DapperRepository(string connString)
        {
            this.connString = connString;
            conn = new SqlConnection(connString);
            tableName = typeof(TEntity).Name;
        }

        #region Private Methods

        /// <summary>
        /// 插入语句SQL
        /// </summary>
        public string GetInsertSql(TEntity entity)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append(string.Format("INSERT INTO {0}(", tableName));
            var colums = typeof(TEntity).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Where(i => i.Name != "Id").ToList();
            for (int i = 0; i < colums.Count(); i++)
            {
                if (i == 0) sql.Append(colums[i].Name);
                else sql.Append(string.Format(",{0}", colums[i].Name));
            }
            sql.Append(")");
            sql.Append(" VALUES(");
            for (int i = 0; i < colums.Count(); i++)
            {
                var val = colums[i].GetValue(entity);
                if (val.GetType().BaseType == typeof(Enum))
                {
                    val = Convert.ToInt32(val);
                }
                if (i == 0)
                    sql.Append(string.Format("'{0}'", val));
                else
                    sql.Append(string.Format(",'{0}'", val));
            }
            sql.Append(") ");
            return sql.ToString();

        }
        /// <summary>
        /// 删除SQL
        /// </summary>
        public string GetDeleteSql(TEntity entity)
        {

            return string.Format("DELETE FROM {0} where id={1}", tableName, entity.Id);

        }
        /// <summary>
        /// 修改SQL
        /// </summary>
        public string GetUpdateSql(TEntity entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(string.Format("UPDATE {0} SET", tableName));
            var colums = typeof(TEntity).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Where(i => i.Name != "Id").ToList();
            for (int i = 0; i < colums.Count(); i++)
            {
                var val = colums[i].GetValue(entity);
                if (val.GetType().BaseType == typeof(Enum))
                {
                    val = Convert.ToInt32(val);
                }

                if (i != 0) sql.Append(",");
                sql.Append(" ");
                sql.Append(colums[i].Name);
                sql.Append("=");
                sql.Append("'");
                sql.Append(val);
                sql.Append("'");
            }
            sql.Append(string.Format(" where id= '{0}'", entity.Id));
            return sql.ToString();

        }

        #endregion

        #region IRepository<TEntity> 成员

        public TEntity Find(params object[] id)
        {
            return conn.Query<TEntity>("select * from " + tableName + " where id=" + id[0]).FirstOrDefault();
        }

        public IQueryable<TEntity> GetModel()
        {
            return conn.Query<TEntity>("select * from " + tableName).AsQueryable();
        }

        public void SetDataContext(object db)
        {
            throw new NotImplementedException();
        }

        public void Insert(TEntity item)
        {
            conn.Execute(GetInsertSql(item));
        }

        public void Update(TEntity item)
        {
            conn.Execute(GetUpdateSql(item));
        }

        public void Delete(TEntity item)
        {
            conn.Execute(GetDeleteSql(item));
        }

        #endregion

        #region IUnitOfWorkRepository 成员

        public void UoWInsert(Domain.IEntity item)
        {
            throw new NotImplementedException();
        }

        public void UoWUpdate(Domain.IEntity item)
        {
            throw new NotImplementedException();
        }

        public void UoWDelete(Domain.IEntity item)
        {
            throw new NotImplementedException();
        }

        public void UoWInsert(IEnumerable<Domain.IEntity> list)
        {
            throw new NotImplementedException();
        }

        public void UoWUpdate(IEnumerable<Domain.IEntity> list)
        {
            throw new NotImplementedException();
        }

        public void UoWDelete(IEnumerable<Domain.IEntity> list)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
