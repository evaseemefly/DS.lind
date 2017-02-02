using DemoFastSocket.Models;
using Lind.DDD.LindQueue;
using Lind.DDD.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoFastSocket.Client
{

    class Program
    {
        static void Main(string[] args)
        {


            //var pm = new ProducerManager(new ProducerSetting
            //{
            //    BrokerAddress = "127.0.0.1",
            //    BrokerName = "test",
            //    BrokerPort = 8406,
            //    Timeout = 1000,
            //});
            //for (int i = 0; i < 10; i++)
            //{
            //    pm.Push(new LindMQ
            //    {
            //        Topic = "zzl",
            //        Body = Lind.DDD.Utils.SerializeMemoryHelper.SerializeToJson(new { Name = "zzl", SortNumber = 10 })
            //    });
            //}

            //Console.ReadKey();


            #region Client-LindMQ
            var consumer = new ConsumerSetting
            {
                BrokenName = "test",
                BrokenAddress = new System.Net.IPEndPoint(IPAddress.Parse("192.168.2.71"), 8406),
                Callback = new Dictionary<string, Action<MessageBody>>() { 
                {"zzl",(o)=>{
                    Console.WriteLine(o.ToString());
                    Thread.Sleep(1000);
                }},
                {"zhz",(o)=>{
                    Console.WriteLine(o.ToString());
                    Thread.Sleep(2000);
                }}
                }
            };
            var consumerClient = new ConsumerManager(new List<ConsumerSetting> { consumer });
            consumerClient.Start();
            #endregion

            Console.ReadKey();


            #region Socket-SendQueue
            //var client2 = new AsyncBinarySocketClient(8192, 8192, 3000, 3000);
            //client2.RegisterServerNode("127.0.0.1:8404", new System.Net.IPEndPoint(System.Net.IPAddress.Parse("127.0.0.1"), 8404));
            //client2.Send("SendQueue", SerializeMemoryHelper.SerializeToBinary("zzl"), res => res.Buffer).ContinueWith(c =>
            //{
            //    if (c.IsFaulted)
            //    {
            //        throw c.Exception;
            //    }
            //    var entity = SerializeMemoryHelper.DeserializeFromBinary(c.Result) as User;
            //    Console.WriteLine(entity.Name);
            //}).Wait();
            #endregion

            #region 数据连接
            //var client = new DSSBinarySocketClient(8192, 8192, 3000, 3000);
            //List<Action> actionList = new List<Action>();

            //for (int i = 0; i < 100; i++)
            //{
            //    actionList.Add(() =>
            //    {
            //        #region  Socket-DataInsert
            //        //注册服务器节点，这里可注册多个(name不能重复）
            //        client.RegisterServerNode("192.168.2.71:8403", new System.Net.IPEndPoint(System.Net.IPAddress.Parse("192.168.2.71"), 8403));
            //        client.Send("UserInsert", 1, "zzl", 1, "test"
            //            , SerializeMemoryHelper.SerializeToBinary("hello world!"), res => res.Buffer)
            //            .ContinueWith(c =>
            //            {
            //                if (c.IsFaulted)
            //                {
            //                    throw c.Exception;
            //                }
            //                Console.WriteLine(DateTime.Now + "result:" + Encoding.UTF8.GetString(c.Result));
            //            }).Wait();
            //        Thread.Sleep(1000);
            //        #endregion
            //    });
            //}
            //Parallel.Invoke(actionList.ToArray());
            #endregion

            Console.ReadKey();
        }

    }
}
