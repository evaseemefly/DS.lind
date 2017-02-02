using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lind.DDD.Events
{
    /// <summary>
    /// 领域事件实体基类[实体接口]
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// 领域事件实体的聚合根，生命周期在会话结束后消失
        /// </summary>
        Guid AggregateRoot { get; }
    }
}
