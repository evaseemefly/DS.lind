using Lind.DDD.Domain_Aggregate_Event.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lind.DDD.Domain_Aggregate_Event.Bus
{
    public class EntityChangedEventHandler<T> :
        IEventHandler<EntityCreatedEventData<T>>,
        IEventHandler<EntityDeletedEventData<T>>,
        IEventHandler<EntityUpdatedEventData<T>> where T:class
    {

        #region IEventHandler<EntityCreatedEventData<T>> 成员

        public void Handle(EntityCreatedEventData<T> evt)
        {
            Console.WriteLine("建立对象");
        }

        #endregion

        #region IEventHandler<EntityDeletedEventData<T>> 成员

        public void Handle(EntityDeletedEventData<T> evt)
        {
            Console.WriteLine("删除对象");
        }

        #endregion

        #region IEventHandler<EntityUpdatedEventData<T>> 成员

        public void Handle(EntityUpdatedEventData<T> evt)
        {
            Console.WriteLine("更新对象");
        }

        #endregion
    }

}
