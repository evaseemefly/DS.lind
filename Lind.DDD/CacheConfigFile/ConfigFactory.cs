using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.IO;
using System.Web;
using System.Configuration;

namespace Lind.DDD.CacheConfigFile
{
    /// <summary>
    /// 配置信息生产工厂
    /// </summary>
    public class ConfigFactory : Singleton<ConfigFactory>
    {
        private ConfigFactory()
        {

        }

        #region 公开的属性
        /// <summary>
        /// 得到ＷＥＢ网站下的指定文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetConfig<T>() where T : IConfiger
        {
            return GetConfig<T>(null);
        }
        /// <summary>
        /// 可以根据绝对路径得到文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configPath">配置文件的完整路径</param>
        /// <returns></returns>
        public T GetConfig<T>(string configPath) where T : IConfiger
        {
            string configFilePath = configPath;
            string filename = typeof(T).Name;

            HttpContext context = HttpContext.Current;
            string siteVirtrualPath = string.IsNullOrEmpty(ConfigurationManager.AppSettings["SiteVirtrualPath"]) ?
                "/" : ConfigurationManager.AppSettings["SiteVirtrualPath"];

            if (string.IsNullOrWhiteSpace(configFilePath))
            {
                if (context != null)
                {
                    configFilePath = context.Server.MapPath(string.Format("{0}/Configs/{1}.Config", siteVirtrualPath, filename));
                }
                else
                {
                    configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Configs\{0}.Config", filename));
                }

                if (!File.Exists(configFilePath))
                {
                    Logger.LoggerFactory.Instance.Logger_Warn("WEB目录下没有正确的.Config文件，目录为：" + configFilePath);
                    throw new Exception("WEB目录下没有正确的.Config文件");
                }

            }
            return (T)ConfigFilesManager.Instance.LoadConfig(configFilePath, typeof(T));
        }
        #endregion

    }
}
