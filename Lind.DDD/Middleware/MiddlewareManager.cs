using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.Middleware
{


    /// <summary>
    /// 中间件管理者
    /// </summary>
    public class MiddlewareManager
    {
        /// <summary>
        /// 向中间件添加操作行为
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="entity">数据体</param>
        /// <param name="action">数据处理程序－会被异步调用</param>
        public static void AddBehavor(MiddlewareModel data)
        {
            var dataPackage = Lind.DDD.Utils.SerializeMemoryHelper.SerializeToBinary(data);
            Lind.DDD.CachingQueue.QueueManager.Instance.Push("Middleware", dataPackage);
        }

        /// <summary>
        /// 执行一个最早的行为
        /// </summary>
        public static void DoBehavor()
        {
            var result = Lind.DDD.CachingQueue.QueueManager.Instance.Pop("Middleware");
            if (result != null)
            {
                var data = Lind.DDD.Utils.SerializeMemoryHelper.DeserializeFromBinary(result) as MiddlewareModel;
                Task.Run(() => data.Behavor());
            }
        }
    }
}
