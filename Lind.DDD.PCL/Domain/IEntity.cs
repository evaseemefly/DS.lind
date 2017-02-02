using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.Domain
{
    /// <summary>
    /// 实体类规范
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 建立时间
        /// </summary>
        DateTime CreateDateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime UpdateDateTime { get; set; }
        /// <summary>
        /// 实体状态
        /// </summary>
        Status Status { get; set; }
    }
}
