using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.CQRS.EventHandlers
{
    /// <summary>
    /// 事件处理程序的接口规范
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEventHandler<T> where T : class
    {
        void Action(T t);
    }
}
