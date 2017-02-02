using Lind.DDD.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.Aspects
{

    /// <summary>
    /// 方法执行前拦截，并记录日志
    /// </summary>
    public class LoggerAspectAttribute : BeforeAspectAttribute
    {
        public override void Action(InvokeContext context)
        {
            Console.WriteLine(context.Method.MethodName + " run start!");
            Lind.DDD.Logger.LoggerFactory.Instance.Logger_Info(context.Method.MethodName + "这个方法开始执行");
        }
    }

    /// <summary>
    /// 方法执行完成后拦截，并记录日志
    /// </summary>
    public class LoggerEndAspectAttribute : AfterAspectAttribute
    {
        public override void Action(InvokeContext context)
        {
            Console.WriteLine(context.Method.MethodName + " run end!");
            Lind.DDD.Logger.LoggerFactory.Instance.Logger_Info(context.Method.MethodName + "这个方法开始执行");
        }
    }
}
