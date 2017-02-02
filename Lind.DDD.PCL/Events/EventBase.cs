using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.Events
{
    /// <summary>
    /// 事件对象[实体核心]
    /// </summary>
    public abstract class EventBase : IEvent
    {
        #region IEvent Members
        /// <summary>
        /// 获取事实范围内的唯一标识，生命周期在本事件会话内有效
        /// </summary>
        public Guid AggregateRoot
        {
            get { return Guid.NewGuid(); }
        }
        /// <summary>
        /// 事件发生的时间
        /// </summary>
        public DateTime EventDate
        {
            get { return DateTime.Now; }
        }

        #endregion

    }
}
