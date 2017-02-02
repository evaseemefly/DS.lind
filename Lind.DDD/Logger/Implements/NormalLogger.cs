using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lind.DDD.Logger.Implements
{
    /// <summary>
    /// 以普通的文字流的方式写日志
    /// </summary>
    internal class NormalLogger : LoggerBase
    {
        static readonly object objLock = new object();
        protected override void InputLogger(string message)
        {
            string filePath = Path.Combine(FileUrl, DateTime.Now.ToLongDateString() + "_" + System.Diagnostics.Process.GetCurrentProcess().Id + ".log");

            if (!System.IO.Directory.Exists(FileUrl))
                System.IO.Directory.CreateDirectory(FileUrl);

            lock (objLock)//防治多线程读写冲突
            {
                using (System.IO.StreamWriter srFile = new System.IO.StreamWriter(filePath, true))
                {
                    srFile.WriteLine(string.Format("{0}{1}{2}"
                        , DateTime.Now.ToString().PadRight(20)
                        , ("[ThreadID:" + Thread.CurrentThread.ManagedThreadId.ToString() + "]").PadRight(14)
                        , message));
                }
            }
        }

    }
}
