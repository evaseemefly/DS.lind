using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lind.DDD.Domain_Aggregate_Event.Bus
{
    public class EntityUpdatedEventData<T> : EventData where T : class
    {
        public EntityUpdatedEventData(T eventModel)
        {
            base.EventModel = eventModel;
        }
    }
}
