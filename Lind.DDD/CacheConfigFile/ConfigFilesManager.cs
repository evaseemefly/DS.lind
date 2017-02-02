using System;
using System.Web;
using System.IO;
using System.Collections.Generic;
namespace Lind.DDD.CacheConfigFile
{
    /// <summary>
    /// 基本文件配置信息管理者
    /// </summary>
    public class ConfigFilesManager : Singleton<ConfigFilesManager>
    {

        #region 私有

        /// <summary>
        /// 锁对象
        /// </summary>
        object lockHelper = new object();

        /// <summary>
        /// 配置文件修改时间,以文件名为键，修改时间为值
        /// </summary>
        public static Dictionary<string, DateTime> fileChangeTime;

        /// <summary>
        /// 配置文件类型
        /// </summary>
        Type configType = null;

        #endregion

        #region 构造方法
        static ConfigFilesManager()
        {
            fileChangeTime = new Dictionary<string, DateTime>();
        }
        /// <summary>
        /// 私用无参方法，实例单例时用
        /// </summary>
        private ConfigFilesManager()
        {
        }

        #endregion

        #region 配置操作

        #region 加载配置类
        /// <summary>
        /// 从配置文件中读取
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        private IConfiger LoadConfigFile(string fileName, Type type)
        {
            this.configType = type;
            fileChangeTime[fileName] = File.GetLastWriteTime(fileName);//得到配置文件的最后修改改时间    
            return ConfigSerialize.DeserializeInfo(fileName, this.configType);
        }
        /// <summary>
        /// 加载配置文件(直接从文件加载）
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="type">实体类型</param>
        /// <returns></returns>
        internal IConfiger LoadConfig(string fileName, Type type)
        {
            return LoadConfig(fileName, type, true);
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="type">实体类型</param>
        /// <param name="isCache">是否从缓存加载</param>
        /// <returns></returns>
        internal IConfiger LoadConfig(string fileName, Type type, bool isCache)
        {
            if (!isCache)
                return LoadConfigFile(fileName, type);
            lock (lockHelper)
            {
                if (DataCache.GetCache(fileName) == null)
                    DataCache.SetCache(fileName, LoadConfigFile(fileName, type));
                DateTime newfileChangeTime = File.GetLastWriteTime(fileName);
                if (!newfileChangeTime.Equals(fileChangeTime[fileName]))
                {
                    DataCache.SetCache(fileName, LoadConfigFile(fileName, type));
                    return LoadConfigFile(fileName, type);
                }
                else
                {
                    return DataCache.GetCache(fileName) as IConfiger;
                }
            }
        }
        #endregion 
        #endregion

    }
}
