using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lind.DDD.Redis.TestClient
{
    class Logger
    {
        public static void Info(string msg)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToLongDateString() + ".log");
            using (System.IO.StreamWriter srFile = new System.IO.StreamWriter(filePath, true))
            {
                string warn = string.Format("{0}{1}{2}"
                    , DateTime.Now.ToString().PadRight(20)
                    , ("[ThreadID:" + Thread.CurrentThread.ManagedThreadId.ToString() + "]").PadRight(14)
                    , msg);
                srFile.WriteLine(warn);
                Console.WriteLine(warn);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string[] emailServices = { "LessonOnLine", "Ordered", "MoneyIn", "MoneyOut", "zzl" };
            foreach (var service in emailServices)
            {
                Lind.DDD.PublishSubscribe.PubSubManager.Instance.Subscribe(service, (msg) =>
                {
                    Logger.Info(service + "->" + msg);

                });
            }
            Console.ReadKey();
        }
    }
}
