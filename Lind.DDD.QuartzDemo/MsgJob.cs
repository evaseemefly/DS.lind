using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lind.DDD.QuartzDemo
{
    public class MsgJob : JobBase
    {
        protected override void ExcuteJob(Quartz.IJobExecutionContext context)
        {


            //从Notice_Info表获取消息
            //使用Lind.DDD.Messaging发送消息 
            Console.WriteLine(DateTime.Now + "MsgJob消息正在发送...");
            Thread.Sleep(5000);
            Console.WriteLine(DateTime.Now + "MsgJob消息发送完成...");

        }
    }
}
