using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lind.DDD.CQRS.Logger
{
    /// <summary>
    /// 以普通的文字流的方式写日志
    /// </summary>
    public class NormalLogger
    {
        static readonly object objLock = new object();
        public static void Info(string message)
        {
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LoggerDir");
            string filePath = Path.Combine(dir, DateTime.Now.ToLongDateString() + ".log");

            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            lock (objLock)//防治多线程读写冲突
            {
                using (System.IO.StreamWriter srFile = new System.IO.StreamWriter(filePath, true))
                {
                    srFile.WriteLine(string.Format("{0}{1}{2}"
                        , DateTime.Now.ToString().PadRight(20)
                        , ("[ThreadID:" + Thread.CurrentThread.ManagedThreadId.ToString() + "]").PadRight(14)
                        , message));
                    srFile.Close();
                    srFile.Dispose();
                }
            }
        }

    }
}
