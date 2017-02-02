using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.Logger.Implements
{
    /// <summary>
    /// 空日志实现者
    /// </summary>
    internal class EmptyLogger : LoggerBase
    {
        protected override void InputLogger(string message)
        {
            Console.WriteLine(message);
        }
    }
}
