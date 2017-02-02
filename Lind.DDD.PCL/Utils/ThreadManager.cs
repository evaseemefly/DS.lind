using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.Utils
{
    /// <summary>
    /// 线程管理
    /// </summary>
    public class ThreadManager
    {
        /// <summary>
        /// 将在线程池上运行的指定工作排队
        /// </summary>
        /// <param name="action"></param>
        public static void Run(Action action)
        {
            Task.Factory.StartNew(action);
        }
        /// <summary>
        /// 将在线程池上运行的指定工作排队，并返回 function 返回的 Task(TResult) 的代理项
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="function"></param>
        /// <returns></returns>
        public static Task<TResult> Run<TResult>(Func<TResult> function)
        {
            return Task.Factory.StartNew(function);
        }

    }
}
