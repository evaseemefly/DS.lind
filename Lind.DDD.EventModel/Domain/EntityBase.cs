using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lind.DDD.Domain_Aggregate_Event.Domain
{
    /// <summary>
    /// 领域对象基类-聚合根和实体都继承它
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public Guid Id
        {
            get
            {
                return Guid.NewGuid();
            }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
