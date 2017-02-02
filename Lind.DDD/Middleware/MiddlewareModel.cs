using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.Middleware
{
    /// <summary>
    /// 中间件模型
    /// </summary>
    [Serializable]
    public class MiddlewareModel
    {
        /// <summary>
        /// 回调方法
        /// </summary>
        public Action Behavor { get; set; }

    }
}
