using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lind.DDD.Domain_Aggregate_Event.Bus
{
    public class EntityDeletedEventData<T> : EventData where T : class
    {
        public EntityDeletedEventData(T eventModel)
        {
            base.EventModel = eventModel;
        }
    }
}
