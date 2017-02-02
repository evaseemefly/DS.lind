using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lind.DDD.ConfigConstants;
using StackExchange.Redis;

namespace Lind.DDD.RedisClient
{
    /// <summary>
    /// StackExchange.Redis管理者
    /// 注意：这个客户端没有连接池的概念，而是有了多路复用技术
    /// </summary>
    public class RedisManager
    {
        /// <summary>
        /// 锁对象
        /// </summary>
        private static object _locker = new object();
        /// <summary>
        /// StackExchange.Redis对象
        /// </summary>
        private static ConnectionMultiplexer instance;

        /// <summary>
        /// 得到StackExchange.Redis单例对象
        /// </summary>
        public static ConnectionMultiplexer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_locker)
                    {
                        if (instance != null)
                            return instance;

                        instance = GetManager();
                        return instance;
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// 构建链接,返回对象
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static ConnectionMultiplexer GetManager()
        {
            string connectionString = ConfigConstants.ConfigManager.Config.Redis.Host;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("请配置Redis连接串！");
            }
            return ConnectionMultiplexer.Connect(connectionString);
        }

    }
}
