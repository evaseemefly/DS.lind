using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lind.DDD.QuartzDemo
{
    public class OrderJob : JobBase
    {
        protected override void ExcuteJob(Quartz.IJobExecutionContext context)
        {
            //从Notice_Info表获取消息
            //使用Lind.DDD.Messaging发送消息 
            Console.WriteLine(DateTime.Now + "OrderJob订单统计被执行...");
            Thread.Sleep(5000);
            Console.WriteLine(DateTime.Now + "OrderJob订单统计完成...");

        }
    }
}
