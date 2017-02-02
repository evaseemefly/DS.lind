using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.CQRS.Events
{
    /// <summary>
    /// 事件源，基类
    /// </summary>
    public class EventBase
    {
        public EventBase()
        {
            this.RootId = Guid.NewGuid();
            this.EventDate = DateTime.Now;

        }
        public Guid RootId { get; private set; }

        public DateTime EventDate { get; private set; }
    }
}
