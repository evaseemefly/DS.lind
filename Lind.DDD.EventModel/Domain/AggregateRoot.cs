using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lind.DDD.Domain_Aggregate_Event.Domain
{
    /// <summary>
    /// 聚合根基类
    /// </summary>
    public abstract class AggregateRoot:EntityBase
    {
        public override string ToString()
        {
            return string.Format("聚合根:" + this.GetType().Name);
        }
    }
}
