using Lind.DDD.CachingDataSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.Aspects
{
    /// <summary>
    /// 表示用于Caching特性的缓存方式。
    /// </summary>
    public enum CachingMethod
    {
        /// <summary>
        /// 表示需要从缓存中获取对象。如果缓存中不存在所需的对象，系统则会调用实际的方法获取对象，
        /// 然后将获得的结果添加到缓存中。
        /// </summary>
        Get,
        /// <summary>
        /// 表示需要将对象存入缓存。此方式会调用实际方法以获取对象，然后将获得的结果添加到缓存中，
        /// 并直接返回方法的调用结果。
        /// </summary>
        Put,
        /// <summary>
        /// 表示需要将对象从缓存中移除。
        /// </summary>
        Remove
    }
    /// <summary>
    /// 缓存拦截器
    /// </summary>
    public class CachingAspectAttribute : BeforeAspectAttribute
    {
        /// <summary>
        /// 缓存方式
        /// </summary>
        CachingMethod cachingMethod;

        /// <summary>
        /// 初始化缓存拦截器
        /// </summary>
        /// <param name="cachingMethod"></param>
        public CachingAspectAttribute(CachingMethod cachingMethod)
        {
            this.cachingMethod = cachingMethod;
        }

        /// <summary>
        /// 方法拦截动作
        /// </summary>
        /// <param name="context"></param>
        public override void Action(InvokeContext context)
        {
            var method = context.Method;
            string prefix = "Lind_";
            //键名，在put和get时使用
            var key = prefix + method.MethodName;
            Console.WriteLine(key);
            switch (cachingMethod)
            {
                case CachingMethod.Remove:
                    //……
                    break;
                case CachingMethod.Get:
                    //……
                    context.SetResult("zzl");
                    break;
                case CachingMethod.Put:
                    //……
                    break;
                default:
                    throw new InvalidOperationException("无效的缓存方式。");

            }
        }
    }
}
