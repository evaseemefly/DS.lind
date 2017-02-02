using Lind.DDD.ConfigConstants;
using Lind.DDD.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.Utils
{
    /// <summary>
    /// 程序集相关帮助类
    /// </summary>
    public class AssemblyHelper
    {
        /// <summary>
        /// 白名单
        /// </summary>
        static string[] Blacklist = ConfigManager.Config.AutoLoadDLL_BlackList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        static object lockObj = new object();
        static IEnumerable<Type> modelList;
        static AssemblyHelper()
        {
            lock (lockObj)
            {
                if (modelList == null)
                {
                    lock (lockObj)
                    {
                        var pathList = new List<string> { AppDomain.CurrentDomain.BaseDirectory };
                        List<Assembly> AssemblyList = new List<Assembly>();
                        string path = AppDomain.CurrentDomain.BaseDirectory;
                        if (!path.Contains("bin") && Directory.Exists(path))//如果当前程序目录没有bin，就添加bin，并且需要这个bin真实存在
                        {
                            pathList.Add(Path.Combine(path, "bin"));
                        }

                        try
                        {
                            foreach (var _path in pathList)
                            {
                                Lind.DDD.Logger.LoggerFactory.Instance.Logger_Info("Plugin load folder:" + _path);

                                foreach (var dir in Directory.GetFiles(_path)
                                                 .Where(i => i.EndsWith("dll", StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    var dll = Assembly.LoadFrom(dir);
                                    if (Blacklist.Where(j => dll.FullName.StartsWith(j, StringComparison.CurrentCultureIgnoreCase)).Count() == 0)
                                        AssemblyList.Add(dll);
                                }
                            }
                            modelList = AssemblyList.SelectMany(a => a.GetTypes()).Where(i => i.IsClass && !i.IsAbstract);
                            Lind.DDD.Logger.LoggerFactory.Instance.Logger_Info("Plugin load dll\r\n" + string.Join("\r\n", AssemblyList.Select(i => i.FullName)));
                        }
                        catch (Exception ex)
                        {
                            Lind.DDD.Logger.LoggerFactory.Instance.Logger_Info("Plugin error\r\n" + ex.Message + ex.StackTrace);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 得到BIN下面加载程序集里，实现某接口的类型集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypesByInterfaces(Type @interface)
        {
            //泛型
            var linq = modelList.Where(x => x.GetInterfaces()
                    .Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == @interface));
            //非泛型
            if (linq == null || linq.Count() == 0)
                linq = modelList.Where(i => @interface.IsAssignableFrom(i));

            return linq;
        }

        /// <summary>
        /// 得到BIN下面加载程序集里，实现某接口的类型名称的集合
        /// </summary>
        /// <param name="interface"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetTypeNamesByInterfaces(Type @interface)
        {
            return GetTypesByInterfaces(@interface).Select(i => i.Name);
        }

        /// <summary>
        /// 得到BIN下面加载程序里的指定实体类
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static Type GetEntityTypeByName(string className)
        {
            return modelList.Where(i => i.GetInterfaces().Contains(typeof(IEntity)) && i.Name == className).FirstOrDefault();
        }

        /// <summary>
        /// 根据接口，返回程序运行时里所有实现它的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="interface"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetCurrentTypesByInterfaces<T>(Type @interface)
        {
            var handlers = @interface.Assembly.GetExportedTypes()
                .Where(x => x.GetInterfaces()
                    .Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == @interface))
                    .Where(h => h.GetInterfaces().Any(ii => ii.GetGenericArguments()
                        .Any(aa => aa == typeof(T))))
                        .ToList();
            return handlers;
        }

    }
}
