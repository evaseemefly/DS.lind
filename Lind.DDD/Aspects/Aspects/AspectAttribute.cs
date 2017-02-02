using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lind.DDD.Aspects
{
    /// <summary>
    /// 方法拦截基类
    /// </summary>
    public abstract class AspectAttribute : Attribute
    {
        /// <summary>
        /// 拦截行为，子类根据自己逻辑去重写
        /// </summary>
        /// <param name="context">方法对象上下文</param>
        public virtual void Action(InvokeContext context) { }
        /// <summary>
        /// 返回对象
        /// </summary>
        public virtual object Result { get; set; }
    }
    /// <summary>
    /// 方法执行前拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class BeforeAspectAttribute : AspectAttribute
    {
    }
    /// <summary>
    /// 方法执行后拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class AfterAspectAttribute : AspectAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public abstract class PropertyAspectAttribute : AspectAttribute
    {

    }

    /// <summary>
    /// 出现异常时拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ExceptionAspectAttribute : AspectAttribute
    {
    }
}
