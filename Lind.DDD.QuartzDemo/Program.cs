using Quartz;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.QuartzDemo
{
    static class Program
    {

        private static log4net.ILog Logger;
        private static IScheduler scheduler = null;
        static void Init()
        {
            Logger = log4net.LogManager.GetLogger("test");//得到当前类类型（当前实实例化的类为具体子类）
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Log4Net.config"));
        }



        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[] 
            //{ 
            //    new Service1() 
            //};
            //ServiceBase.Run(ServicesToRun);

            Init();
            #region job配置
            NameValueCollection properties = new NameValueCollection();
            // 远程输出配置
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "777";
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp";
            properties["quartz.scheduler.instanceName"] = "DataCenterInstance";

            // set thread pool info
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "2";//整个JOB系统可用的JOB数
            properties["quartz.threadPool.threadPriority"] = "Normal";

            // job initialization plugin handles our xml reading, without it defaults are used
            properties["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz";
            properties["quartz.plugin.xml.fileNames"] = System.AppDomain.CurrentDomain.BaseDirectory + "quartz_jobs.xml";
            ISchedulerFactory sf = new Quartz.Impl.StdSchedulerFactory(properties);
            scheduler = sf.GetScheduler();
            scheduler.Start();
          
            #endregion
        }
    }
}
