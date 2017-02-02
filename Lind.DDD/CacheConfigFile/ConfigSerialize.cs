using System.IO;
using System.Xml.Serialization;
using System;

namespace Lind.DDD.CacheConfigFile
{
    /// <summary>
    /// 配置序列化操作类
    /// </summary>
    internal class ConfigSerialize
    {
        #region 反序列化指定的类

        /// <summary>
        /// 反序列化指定的类
        /// </summary>
        /// <param name="configfilepath">config 文件的路径</param>
        /// <param name="configtype">相应的类型</param>
        /// <returns></returns>
        public static IConfiger DeserializeInfo(string path, Type type)
        {

            IConfiger iconfiginfo;
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                iconfiginfo = (IConfiger)serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }

            return iconfiginfo;
        }



        #endregion

        #region 保存(序列化)指定路径下的配置文件

        /// <summary>
        /// 保存(序列化)指定路径下的配置文件
        /// </summary>
        /// <param name="configFilePath">指定的配置文件所在的路径(包括文件名)</param>
        /// <param name="configinfo">被保存(序列化)的对象</param>
        /// <returns></returns>
        public static bool Serializer(string path, IConfiger Iconfiginfo)
        {
            bool succeed = false;
            FileStream fs = null;
            XmlSerializer serializer = null;
            try
            {
                fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                serializer = new XmlSerializer(Iconfiginfo.GetType());
                serializer.Serialize(fs, Iconfiginfo);
                //成功则将会返回true
                succeed = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    serializer = null;
                }
            }

            return succeed;
        }

        #endregion
    
    }
}
