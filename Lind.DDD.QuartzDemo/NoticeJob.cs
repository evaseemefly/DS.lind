using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lind.DDD.QuartzDemo
{
    public class NoticeJob : JobBase
    {
        protected override void ExcuteJob(Quartz.IJobExecutionContext context)
        {

            //从Notice_Info表获取消息
            //使用Lind.DDD.Messaging发送消息 
            Console.WriteLine(DateTime.Now + "NoticeJob通知发送……");
            foreach (var item in context.JobDetail.JobDataMap)
            {
                Console.WriteLine("Key:" + item.Key + ",Value:" + item.Value);
            }
            Thread.Sleep(3000);
            Console.WriteLine(DateTime.Now + "NoticeJob通知完成……");
        }
    }
}
