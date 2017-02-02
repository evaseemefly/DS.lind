using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lind.DDD.PublishSubscribe;
using System.Threading;
using System.Diagnostics;

namespace Lind.DDD.UnitTest
{
    [TestClass]
    public class PubSub
    {
        [TestMethod]
        public void SendEmail()
        {

            //－－－－－－－－－－－－－－－－－－－－－服务平台开始－－－－－－－－－－－－－－－－－－
            //订阅的主题
            string[] emailServices = { "LessonOnLine", "Ordered", "MoneyIn", "MoneyOut", "zzl" };
            foreach (var sevice in emailServices)
            {
                PubSubManager.Instance.Subscribe(sevice, (msg) =>
                {
                    //注册所有发送Email的服务
                    Console.WriteLine(sevice + ":" + msg);
                    Debug.WriteLine(sevice + ":" + msg);
                });
            }
            //－－－－－－－－－－－－－－－－－－－－－－服务平台结束－－－－－－－－－－－－－－－－－－


            //－－－－－－－－－－－－－－－－－－－－－业务平台开始－－－－－－－－－－－－－－－－－－
            foreach (var item in emailServices)
            {
                //发布
                PubSubManager.Instance.Publish(item, "张大叔");
                Thread.Sleep(1000);
            }
            //－－－－－－－－－－－－－－－－－－－－业务平台结束－－－－－－－－－－－－－－－－－－

        }
    }
}
