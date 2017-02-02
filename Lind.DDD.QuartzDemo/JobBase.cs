using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lind.DDD.QuartzDemo
{
    [DisallowConcurrentExecution]
    public abstract class JobBase : Quartz.IJob
    {
        /// <summary>
        /// log4日志对象
        /// </summary>
        protected log4net.ILog Logger
        {
            get
            {
                return log4net.LogManager.GetLogger(this.GetType());//得到当前类类型（当前实实例化的类为具体子类）
            }
        }

        /// <summary>
        /// 基类去实现接口，由派生类处理
        /// 这个方法里定义算法的骨架，而核心的ExcuteJob抽象方法，由各个具体类去实现
        /// </summary>
        /// <param name="context"></param>
        public void Execute(Quartz.IJobExecutionContext context)
        {

            foreach (var item in context.JobDetail.JobDataMap)
            {
                Console.WriteLine("{0}.JobDataMap   key={1},value={2}", context.JobDetail.Key.Name, item.Key, item.Value);
            }
            ExcuteJob(context);
        }

        /// <summary>
        /// 具体业务层的JOB
        /// 派生类实现这个方法，这类似于模版方法里的具体方法
        /// </summary>
        protected abstract void ExcuteJob(Quartz.IJobExecutionContext context);

    }
}