using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;
using Lind.DDD.RedisClient;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
namespace Lind.DDD.UnitTest
{
    class VoteModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreateTime { get; set; }
        public override bool Equals(object obj)
        {
            return this.UserID == ((VoteModel)obj).UserID;
        }
    }
    [TestClass]
    public class Redis
    {
        static object lockObj = new object();
        static ConnectionMultiplexer conn = ConnectionMultiplexer.Connect("127.0.0.1:6379");
        RedisValue[] getCity()
        {
            return conn.GetDatabase().SetMembers("testlong");
        }
        RedisValue[] getCityAsync()
        {
            return conn.GetDatabase().SetMembersAsync("testlong").Result;
        }

        [TestMethod]
        public void TW()
        {
            //连接TW服务器
            ConfigurationOptions sentinelConfig = new ConfigurationOptions();
            sentinelConfig.EndPoints.Add("192.168.1.190:22121");
            sentinelConfig.Proxy = Proxy.Twemproxy;
            ConnectionMultiplexer conn = ConnectionMultiplexer.Connect(sentinelConfig);
            conn.GetDatabase().StringSet("zzltest", "test");
        }

        [TestMethod]
        public void testRedis()
        {
            Console.WriteLine("Redis sync & async testing!");
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            for (int i = 0; i < 100000; i++)
            {
                conn.GetDatabase().SetAddAsync("async_testlong", i.ToString());
            }
            sw.Stop();
            long timer2 = sw.ElapsedMilliseconds;

            sw.Restart();
            for (int i = 0; i < 100000; i++)
            {
                conn.GetDatabase().SetAdd("testlong", i.ToString());
            }
            sw.Stop();
            long timer1 = sw.ElapsedMilliseconds;

            sw.Restart();
            var a2 = getCityAsync();
            sw.Stop();
            long timer4 = sw.ElapsedMilliseconds;

            sw.Restart();
            var a1 = getCity();
            sw.Stop();
            long timer3 = sw.ElapsedMilliseconds;


            Console.WriteLine("SetAdd Timer:" + timer1 + "\r\nSetAddAsync async timer:" + timer2 + "\r\nSetMembers Timer:" + timer3 + "\r\nSetMembersAsync Timer:" + timer4);

        }

        [TestMethod]
        public void AppendValue()
        {

            //连接sentinel服务器
            ConfigurationOptions sentinelConfig = new ConfigurationOptions();
            sentinelConfig.ServiceName = "master1";
            sentinelConfig.EndPoints.Add("192.168.2.3", 26379);
            sentinelConfig.EndPoints.Add("192.168.2.3", 26380);
            sentinelConfig.TieBreaker = "";//这行在sentinel模式必须加上
            sentinelConfig.CommandMap = CommandMap.Sentinel;

            // Need Version 3.0 for the INFO command?
            sentinelConfig.DefaultVersion = new Version(3, 0);


            ConnectionMultiplexer conn = ConnectionMultiplexer.Connect(sentinelConfig);




            ISubscriber sub = conn.GetSubscriber();
            sub.Subscribe("+switch-master", (o, i) =>
            {
                Console.WriteLine(o + "-hello pub/sub-" + i);
                var arr = i.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var oldServer = arr[0];
                var newServer = arr[1];
                var conf = "/usr/local/twemporxy/conf/nutcracker.yml";
                lock (lockObj)
                {
                    var result = ReadTxt(conf);
                    result = result.Replace(oldServer, newServer);
                    WriteTxt(conf, result);
                }
            });

            Console.ReadKey();

        }

        [TestMethod]
        public void Redis_Async()
        {
            List<Action> actionList = new List<Action>();
            actionList.Add(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    RedisClient.RedisManager.Instance.GetDatabase().SetAdd("test01", DateTime.Now.ToString());
                    Thread.Sleep(100);
                    Console.WriteLine("test011" + i);
                }
            });
            actionList.Add(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    RedisClient.RedisManager.Instance.GetDatabase().SetAdd("test02", DateTime.Now.ToString());
                    Thread.Sleep(10);
                    Console.WriteLine("test012" + i);
                }
            });
            Parallel.Invoke(actionList.ToArray());
        }

        static string ReadTxt(string fileName)
        {
            string msg = string.Empty;
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {

                using (StreamReader sw = new StreamReader(fs, Encoding.UTF8))
                {
                    msg = sw.ReadToEnd();
                }

            }
            return msg;
        }

        static void WriteTxt(string fileName, string obj)
        {

            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {

                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(obj);
                }
            }

        }


        [TestMethod]
        public void Redis_ModelList()
        {

            //   RedisClient.RedisManager.Instance.GetDatabase().SetAdd("ModelList", Utils.SerializeMemoryHelper.SerializeToJson(new People(1, "zzl", 1)));
            //   RedisClient.RedisManager.Instance.GetDatabase().SetAdd("ModelList", Utils.SerializeMemoryHelper.SerializeToJson(new People(2, "zzl2", 2)));
            var list = new List<People>();
            list.Add(new People(1, "zzl", 1));
            list.Add(new People(2, "zzl2", 2));


            string old = Lind.DDD.Utils.SerializeMemoryHelper.SerializeToJson(list);
            var news = Lind.DDD.Utils.SerializeMemoryHelper.DeserializeFromJson<List<People>>(old);


            RedisClient.RedisManager.Instance.GetDatabase().Set("ModelEntity", list);
            var result = RedisClient.RedisManager.Instance.GetDatabase().Get<List<People>>("ModelEntity");
        }

        [TestMethod]
        public void VoteBigData()
        {
            for (var i = 0; i < 1000000; i++)
            {
                var entity = new VoteModel
                {
                    UserID = i,
                    ProjectID = 1,
                    ProjectName = "tel",
                    UserName = "zzl" + i,
                    CreateTime = DateTime.Now
                };
                RedisClient.RedisManager.Instance.GetDatabase().HashSet("VoteList", entity.UserID, Utils.SerializeMemoryHelper.SerializeToJson(entity));
                //空间换时间的索引UserName
                RedisClient.RedisManager.Instance.GetDatabase().HashSet("VoteList_UserName", entity.UserName, entity.UserID);

            }
        }


        [TestMethod]
        public void FindBigData()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var name = RedisClient.RedisManager.Instance.GetDatabase().HashGet("VoteList_UserName", "zzl15");//找到用户ID
            if (name.HasValue)
            {
                var val = RedisClient.RedisManager.Instance.GetDatabase().HashGet("VoteList", name);//找到用户实体
                Console.WriteLine("name={0},value={1}", name, val);
            }
            else
            {
                Console.WriteLine("没有发现这个Key");
            }
            sw.Stop();
            Console.WriteLine("查询需要的时间：" + sw.ElapsedMilliseconds + "ms");
        }

    }


}
