using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.Logger.Implements
{
    /// <summary>
    /// 日志核心基类
    /// 模版方法模式，对InputLogger开放，对其它日志逻辑隐藏，InputLogger可以有多种实现
    /// </summary>
    internal abstract class LoggerBase : ILogger
    {
        private string _defaultLoggerName = DateTime.Now.ToString("yyyyMMddhh") + ".log";

        /// <summary>
        /// 日志文件地址
        /// 优化级为mvc方案地址，网站方案地址，console程序地址
        /// </summary>
        protected string FileUrl
        {
            get
            {
                try
                {
                    //在当前进程的应用程序域中找到LoggerDir目录
                    return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LoggerDir");
                }
                catch (Exception)
                {

                    try
                    {
                        return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "LoggerDir");
                    }
                    catch (Exception)
                    {
                        return Path.Combine(System.Web.HttpContext.Current.Request.PhysicalApplicationPath, "LoggerDir");
                    }
                }

            }

        }

        /// <summary>
        /// 日志持久化的方法，派生类必须要实现自己的方式
        /// </summary>
        /// <param name="message"></param>
        protected abstract void InputLogger(string message);

        #region ILogger 成员

        public void Logger_Timer(string message, Action action)
        {
            StringBuilder str = new StringBuilder();
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            str.Append(message);
            action();
            str.Append("Logger_Timer:代码段运行时间(" + sw.ElapsedMilliseconds + "毫秒)");
            InputLogger(str.ToString());
            sw.Stop();
        }

        public void Logger_Exception(string message, Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                InputLogger("Logger_Exception:" + message + "代码段出现异常,信息为" + ex.Message);
            }
        }

        public virtual void Logger_Info(string message)
        {
            InputLogger("Info:" + message);
        }

        public virtual void Logger_Error(Exception ex)
        {
            InputLogger("Error:" + ex.Message);
        }

        public virtual void Logger_Debug(string message)
        {
            InputLogger("Debug:" + message);
        }

        public virtual void Logger_Fatal(string message)
        {
            InputLogger("Fatal:" + message);
        }

        public virtual void Logger_Warn(string message)
        {
            InputLogger("Warn" + message);
        }

        #endregion
    }
}
