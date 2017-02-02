using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lind.DDD.Aspects;
using Lind.DDD.IRepositories;
using Lind.DDD.IoC;
using Lind.DDD.Domain;
using System.Reflection;
using System.Threading;
using Lind.DDD.Events;
using Lind.DDD.Caching;
using Lind.DDD.ConfigConstants;
using Lind.DDD.PublishSubscribe;
using Lind.DDD.UoW;
using System.Transactions;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Services;
using System.Net.Http;
using System.IO;
using System.Linq.Expressions;
using Lind.DDD.LinqExtensions;
using Lind.DDD.CachingQueue;
using Lind.DDD.MongoDbClient;
using System.Net;
using Lind.DDD.CatClientPur;
using System.Web.Http.SelfHost;
using System.Web.Http;
using Lind.DDD.Commons;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Lind.DDD.RedisClient;
using Lind.DDD.FastDFS;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Xml;
using Lind.DDD.Utils;
using System.Collections.Specialized;
using FastSocket.SocketBase;
using System.Diagnostics;
using Quartz;
using System.Text.RegularExpressions;
using StackExchange.Redis;
using Autofac;
using MongoDB.Driver;
namespace Lind.DDD.Test
{
    #region 实体

    public class Shop : NoSqlEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public Shop Parent { get; set; }

        public void Hello()
        {
            Console.WriteLine("shop方法被调用了");
        }
    }
    public partial class User : NoSqlEntity
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string UserName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
    }
    public class User1 : DisposableBase
    {
        public string UserName { get; set; }
        public int Age { get; set; }
        protected override void Finalize(bool disposing)
        {
            if (disposing)
            {
                //清除托管
            }
            //清除非托管
        }
    }

    public class User_DataSet_Policies
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 策略所需字段
        /// </summary>
        public string PolicyField { get; set; }
        /// <summary>
        /// 策略所需要值
        /// </summary>
        public string PolicyValue { get; set; }
        /// <summary>
        /// 策略操作符
        /// </summary>
        public string PolicyOperation { get; set; }
    }
    public interface ITEST<T> where T : class { }
    [Serializable]
    public class TestList : ITEST<Shop>
    {


    }
    #endregion

    #region AOP方法拦截
    /// <summary>
    /// 业务层基类
    /// </summary>
    public class BLLBase : MarshalByRefObject
    {

    }
    /// <summary>
    /// 方法拦截的测试
    /// </summary>
    public class AopUserBLL : BLLBase
    {
        public void Hello()
        {
            Console.WriteLine("Hello world!");
        }

        public void Insert(User entity)
        {
            Console.WriteLine("插动作" + entity.UserName);
        }
        public void Update(User entity)
        {
            Console.WriteLine("更新动作" + entity.UserName);
        }
    }
    /// <summary>
    /// AOP拦截行为
    /// </summary>
    public class Logging : Lind.DDD.IoC.Interception.InterceptionBase
    {

        public override Microsoft.Practices.Unity.InterceptionExtension.IMethodReturn Invoke(Microsoft.Practices.Unity.InterceptionExtension.IMethodInvocation input, Microsoft.Practices.Unity.InterceptionExtension.GetNextInterceptionBehaviorDelegate getNext)
        {

            foreach (var item in input.Arguments)
            {
                Console.WriteLine("用户参数" + item.GetType().Name);
            }
            Console.WriteLine("你好，世界，前");
            var methodReturn = getNext().Invoke(input, getNext);
            Console.WriteLine("你好，世界，后");
            return methodReturn;
        }
    }
    #endregion

    #region AOP设计
    public class LogStartAttribute : BeforeAspectAttribute
    {
        public override void Action(InvokeContext context)
        {
            Console.WriteLine("log start!");
        }
    }

    public class LogEndAttribute : AfterAspectAttribute
    {
        public override void Action(InvokeContext context)
        {
            Console.WriteLine("log end!");
        }
    }

    public class LogExAttribute : ExceptionAspectAttribute
    {
        public override void Action(InvokeContext context)
        {
            Console.WriteLine("log exception!");
        }
    }

    public interface ICalc
    {
        void Add(int a, int b);
        void Divide(int a, int b);
    }

    public class Calculater : ICalc
    {
        [LogStart]
        [LogEnd]
        public virtual void Add(int a, int b)
        {
            Console.WriteLine("a+b=" + (a + b).ToString());
        }

        [LogEx]
        public virtual void Divide(int a, int b)
        {
            Console.WriteLine("a/b=" + (a / b).ToString());
        }
    }
    #endregion

    #region IOC接口测试

    public interface IHello
    {
        void HelloWorld();
    }

    public class China : IHello
    {

        public void HelloWorld()
        {
            Console.WriteLine("你好，世界！");
        }
    }

    public class USA : IHello
    {

        public void HelloWorld()
        {
            Console.WriteLine("Hello World！");
        }
    }


    #endregion

    /// <summary>
    /// 多线程工作任务
    /// </summary>
    public class WorkOfTask
    {
        /// <summary>
        /// 默认构造方法
        /// </summary>
        /// <param name="maxThreadCount">产生的最大线程数</param>
        /// <param name="processTask">处理的任务</param>
        public WorkOfTask(
            int maxThreadCount,
            Func<int, bool> processTask)
            : this(maxThreadCount, processTask, null)
        { }

        /// <summary>
        /// 带有完成加调的构造方法
        /// </summary>
        /// <param name="maxThreadCount">产生的最大线程数</param>
        /// <param name="processTask">处理的任务</param>
        /// <param name="successTask">完成后的回调方法</param>
        public WorkOfTask(
            int maxThreadCount,
            Func<int, bool> processTask,
            Action successTask)
        {
            if (maxThreadCount > 2048)
                throw new ArgumentException("最大线程数不能超过2048个");

            this._maxThreadCount = maxThreadCount;
            this._processTask = processTask;
            this._successTask = successTask;
        }
        /// <summary>
        /// 终止事件
        /// </summary>
        private event Action OnExit;
        /// <summary>
        /// 线程池
        /// </summary>
        private readonly List<Thread> ThreadCollection = new List<Thread>();
        /// <summary>
        /// 任务中产生的最大线程数
        /// </summary>
        private readonly int _maxThreadCount;
        /// <summary>
        /// 成功后要干的事
        /// </summary>
        private readonly Action _successTask;
        /// <summary>
        /// 线程核心代码
        /// </summary>
        private readonly Func<int, bool> _processTask;
        /// <summary>
        /// 要做的事，及終止事件
        /// </summary>
        private void TaskRun()
        {
            while (true)
            {
                Monitor.Enter(this);//锁定，保持同步     
                var res = _processTask(Thread.CurrentThread.ManagedThreadId);
                #region 任务终于事件触发
                if (res)
                {
                    OnExit();
                }
                #endregion
                Monitor.Exit(this);//取消锁定     
            }
        }

        /// <summary>
        /// 建立線程池子,订阅线程终止条件
        /// </summary>
        private void CreateThreadPool()
        {
            //控制純種
            for (int i = 0; i < _maxThreadCount; i++)
            {
                Thread mythread = new Thread(TaskRun);
                mythread.Name = string.Format("{0}", i);
                ThreadCollection.Add(mythread);
            }

            OnExit += () =>
            {
                Console.ForegroundColor = ConsoleColor.Red;

                if (this._successTask != null)
                    this._successTask();

                foreach (Thread thread in ThreadCollection)
                {
                    thread.Abort();
                }
            };
        }

        public void Action()
        {
            CreateThreadPool();
            foreach (Thread thread in ThreadCollection)
            {
                thread.Start();
            }
        }
    }
    public interface IoCTest
    {
        void Hello();
    }
    public class IoCTestChina : IoCTest
    {

        public void Hello()
        {
            Console.WriteLine("世界你好！");
        }
    }
    public class IoCTestEnglish : IoCTest
    {
        public void Hello()
        {
            Console.WriteLine("Hello world!");
        }
    }
    public interface IoCTest<T>
    {
        void Hello();
    }
    public class IoCTestChina<T> : IoCTest<T>
    {

        public void Hello()
        {
            Console.WriteLine("世界你好！" + typeof(T).Name);
        }
    }
    #region EventModel
    /// <summary>
    /// 订单被确认的事件源
    /// </summary>
    [Serializable]
    public class OrderCreated : EventData
    {
        public override string ToString()
        {
            return "订单已经确认";
        }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal TotalFee { get; set; }
        /// <summary>
        /// 购买者ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 购买者
        /// </summary>
        public string UserName { get; set; }

        public string OrderId { get; set; }

    }
    [Serializable]
    public class OrderInsertEventHandler : IEventHandler<OrderCreated>
    {
        #region IEventHandler<OrderSigned> 成员

        public void Handle(OrderCreated evt)
        {
            Console.WriteLine("订单确认,下单用户:" + evt.UserName);
        }

        #endregion


    }
    #endregion

    class Program
    {

        static void Adds(PureCat.Context.CatContext context)
        {
            PureCat.CatClient.LogRemoteCallServer(context);
            PureCat.CatClient.LogEvent("printevent", "helloevent", "0", "hello distribute api234");
        }

        #region FastDFS分块上传
        static int FaildCount = 0;
        static int MaxFaildCount = 5;
        static string DFSGroupName = "tsingda";
        static int bufferSize = 1024 * 10;

        /// <summary>
        /// 网络可用为True,否则为False
        /// </summary>
        /// <returns></returns>
        static bool NetCheck()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }
        /// <summary>
        /// 断点续传
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="serverShortName"></param>
        static void ContinueUploadPart(Stream stream, string serverShortName)
        {
            var serviceFile = FastDFSClient.GetFileInfo(FastDFSClient.GetStorageNode(DFSGroupName), serverShortName);
            stream.Seek(serviceFile.FileSize, SeekOrigin.Begin);
            BeginUploadPart(stream, serverShortName);
        }


        /// <summary>
        /// 从指定位置开始上传文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="beginOffset"></param>
        /// <param name="serverShortName"></param>
        static void BeginUploadPart(Stream stream, string serverShortName)
        {
            try
            {
                byte[] content = new byte[bufferSize];

                while (stream.Position < stream.Length)
                {
                    stream.Read(content, 0, bufferSize);

                    var result = FastDFSClient.AppendFile(DFSGroupName, serverShortName, content);
                    Console.WriteLine("开始上传，当前上传了" + content.Length + "字节...");
                    if (result.Length == 0)
                    {
                        FaildCount = 0;
                        continue;
                    }
                }
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("上传完成");
                Console.ForegroundColor = oldColor;
            }
            catch (Exception ex)
            {
                Console.WriteLine("上传文件中断！" + ex.Message);
                if (NetCheck())
                {
                    //重试
                    if (FaildCount < MaxFaildCount)
                    {
                        FaildCount++;
                        ContinueUploadPart(stream, serverShortName);
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Console.WriteLine("已达到失败重试次数仍没有上传成功"); ;
                    }
                }
                else
                {
                    Console.WriteLine("当前网络不可用");
                }
            }
        }
        #endregion
        /// <summary>
        /// volatile 关键字用于通知编译器，将有多个线程访问 _shouldStop 数据成员，因此它不应当对此成员的状态做任何优化假设
        ///通过将 volatile 与 _shouldStop 数据成员一起使用，
        ///可以从多个线程安全地访问此成员，而不需要使用正式的线程同步技术，
        ///但这仅仅是因为 _shouldStop 是 bool。这意味着只需要执行单个原子操作就能修改 _shouldStop。
        ///但是，如果此数据成员是类、结构或数组，那么，从多个线程访问它可能会导致间歇的数据损坏。
        ///假设有一个更改数组中的值的线程。Windows 定期中断线程，以便允许其他线程执行，
        ///因此线程会在分配某些数组元素之后和分配其他元素之前被中断。这意味着，数组现在有了一个程序员从不想要的状态，
        ///因此，读取此数组的另一个线程可能会失败。
        /// </summary>
        public class Worker
        {
            public void DoWork()
            {
                while (!_shouldStop)
                {
                    Console.WriteLine("Worker thread: working...");
                }
                Console.WriteLine("Worker thread: terminating gracefully.");
            }
            public void RequestStop()
            {
                _shouldStop = true;
            }
            // Keyword volatile is used as a hint to the compiler that this data
            // member is accessed by multiple threads.
            private volatile bool _shouldStop;
        }



        static void Main(string[] args)
        {
            #region MiddlewareManager
            Lind.DDD.Middleware.MiddlewareManager.DoBehavor();
            #endregion

            #region EventBus.SubscribeAll
            Events.EventBus.Instance.SubscribeAll();
            Events.EventBus.Instance.Publish(new OrderCreated() { UserName = "zzl" });
            #endregion

            #region BulkUpdate
            var db = new Lind.DDD.Repositories.EF.EFRepository<Order>(new EF7_testEntities());
            var order1 = db.GetModel().First();
            order1.IsDeleted = true;
            db.Update(order1);
            var orderList = new List<Order>();
            orderList = db.GetModel().ToList();
            orderList.ForEach(i => i.Name = "U" + i.Name);
            new Lind.DDD.Repositories.EF.EFRepository<Order>(new EF7_testEntities()).BulkUpdate(orderList, "name");

            #endregion

            #region Cat实时监控
            PureCat.CatClient.Initialize();

            var p1 = PureCat.CatClient.NewTransaction("zzlTest", "Test1");
            PureCat.CatClient.LogMetricForCount("order", 1);
            PureCat.CatClient.LogMetricForSum("order seller", 20);
            PureCat.CatClient.LogMetricForSum("order seller", 21);
            PureCat.CatClient.LogMetricForSum("order seller", 22);

            p1.Complete();
            Console.WriteLine("统计完成");
            Console.ReadKey();
            #region 单独的事件
            PureCat.CatClient.NewEvent("outer", "It is outter with Do");　//它不在内部，与Do事务是独立的
            #endregion

            #region 单独的事务
            var p = PureCat.CatClient.NewTransaction("Zzl3", "Test1");
            p.Complete();
            #endregion

            Console.WriteLine("单独事务和事件执行完成");
            Console.ReadKey();

            #region 定义一个嵌套事务
            var context = PureCat.CatClient.DoTransaction("Do", "Test", () =>
            {
                PureCat.CatClient.NewEvent("Do1", "Test1");
                PureCat.CatClient.NewEvent("Do2", "Test2");
                PureCat.CatClient.NewEvent("Do3", "Test3");
            });


            #endregion

            #region 本程序中的嵌套
            var c2 = PureCat.CatClient.DoTransaction("DoInner", "TestInner", () =>
            {
                PureCat.CatClient.LogRemoteCallServer(context);
                PureCat.CatClient.LogEvent("printevent", "helloevent", "0", "hello distribute api234");
            });
            PureCat.CatClient.DoTransaction("DoInner3", "TestInner3", () =>
            {
                PureCat.CatClient.LogRemoteCallServer(c2);
                PureCat.CatClient.LogEvent("printevent", "helloevent", "0", "hello distribute api234");
            });
            #endregion

            Console.WriteLine("本地嵌套事务执行完成");
            Console.ReadKey();

            #region 引用api，将context序列化发过去
            var url = "http://localhost:4532/home/index";
            CatHttpClient.Get(url);
            #endregion

            Console.WriteLine("分布式嵌套事务执行完成");
            Console.ReadKey();

            #endregion

            #region 统一的服务定位器
            ServiceLocator.Instance.GetService<IHello>().HelloWorld();
            #endregion

            #region IoC容器，依赖注入的框架，用来映射依赖，管理对象创建和生存周期（DI框架）

            //全局入口注册
            string implementType = "Lind.DDD.Test.IoCTestEnglish,Lind.DDD.Test";
            IoCFactory.Instance.CurrentContainer.RegisterType(
                typeof(IoCTest),
                Type.GetType(implementType));





            //具体使用
            var helloIoC = IoCFactory.Instance.CurrentContainer.Resolve<IoCTest>();
            helloIoC.Hello();

            #endregion

            #region Entity验证
            var user1 = new User();
            foreach (var item in user1.GetRuleViolations())
                Console.WriteLine(item.PropertyName + "=" + item.ErrorMessage);
            #endregion

            #region 异步实时队列

            for (var i = 0; i < 10; i++)
                RedisQueueManager.Push<string>("zzlqueue", "dudu" + i);
            RedisQueueManager.DoQueue<string>((msg) =>
            {
                Console.WriteLine("redis queue msg:" + msg);
                Thread.Sleep(2000);//执行时候为２秒
            }, "zzlqueue");
            #endregion

            #region 并行Task
            Console.WriteLine("Task开始" + DateTime.Now);
            var task = Task.WhenAll(Task.Run(() =>
            {
                Thread.Sleep(1000);
            }), Task.Run(() =>
            {
                Thread.Sleep(2000);
            }));//多个task并行执行，不阻塞
            task.ContinueWith((ctw) =>//当task完成后，执行这个回调
            {
                Console.WriteLine("并行完成" + DateTime.Now);
            });
            Console.WriteLine("Task结束" + DateTime.Now);
            #endregion

            #region 并行Parallel
            Console.WriteLine(DateTime.Now);
            Parallel.Invoke(() =>
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(1000);
            }, () =>
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(5000);
            });
            Console.WriteLine(DateTime.Now);

            #endregion

            #region 新线程添加到线程池上排队执行(非阻塞)
            Task.Run(() =>
            {
                Thread.Sleep(3000);
                Console.WriteLine("等待了３秒");
            });
            #endregion

            #region 异步与回调(非阻塞)
            Task.WhenAll(Task<string>.Run(() =>
           {
               Console.WriteLine("Task.WhenAll开始执行");
               Thread.Sleep(5000);
               return "我完成了1";
           }),
            Task<string>.Run(() =>
           {
               Console.WriteLine("Task.WhenAll开始执行");
               Thread.Sleep(5000);
               return "我完成了2";
           })).ContinueWith((ctw) =>
           {
               Console.WriteLine("Task.WhenAll使用完成了,returnValue=" + string.Join(",", ctw.Result));
           });
            #endregion

            #region volatile的使用（每次都从内存中拿，而不是从cache里拿）
            Worker workerObject = new Worker();
            Thread workerThread = new Thread(workerObject.DoWork);

            // Start the worker thread.
            workerThread.Start();
            Console.WriteLine("Main thread: starting worker thread...");

            while (!workerThread.IsAlive) ;//线程没有终断之前反复执行它

            // 子线程执行1秒，主线程到这里等待1秒
            Thread.Sleep(1);

            // 主线程更新某个值，由于多个线程共同访问它，所有声明为volatile
            workerObject.RequestStop();

            //阻塞主线程
            workerThread.Join();
            Console.WriteLine("Main thread: worker thread has terminated.");
            #endregion

            #region quartz job配置
            NameValueCollection properties = new NameValueCollection();
            // 远程输出配置
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "667";
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp";
            properties["quartz.scheduler.instanceName"] = "DataCenterInstance";

            // set thread pool info
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "10";//单个任务所对应的线程，为1表示只有一个线程工作，这样避免多个线程改同一条数据
            properties["quartz.threadPool.threadPriority"] = "Normal";

            // job initialization plugin handles our xml reading, without it defaults are used
            properties["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz";
            properties["quartz.plugin.xml.fileNames"] = System.AppDomain.CurrentDomain.BaseDirectory + "quartz_jobs.xml";
            ISchedulerFactory sf = new Quartz.Impl.StdSchedulerFactory(properties);
            var scheduler = sf.GetScheduler();
            scheduler.Start();
            #endregion

            #region 正则表达式从字符串拿到连接的Ｎ位数字
            string exp = @"\d{4,}";//４位以上
            string str = "abcde125";
            if (Regex.Match(str, exp).ToString().Length <= 6)
            {
                Console.WriteLine(Regex.Match(str, exp).ToString());
            }
            else
            {
                Console.WriteLine("数字不是４－６位连续的");
            }
            #endregion

            #region 字符串的二讲制
            byte[] testa = System.Text.Encoding.ASCII.GetBytes("a");
            foreach (var item in testa)
                Console.WriteLine(item.ToString("x2"));//两位的16进制数
            #endregion

            #region 装载全局配置项Lind.DDD
            var config1 = ConfigConstants.ConfigManager.Config;
            #endregion

            #region 数据包
            //包部长度（int32），版本号长度(byte)，命令长度(byte)，主体内容（byte[],版本号+命令+主体内容）

            string cmd = "insert";
            string version = Guid.NewGuid().ToString();
            var message = SerializeHelper.SerializeToBinary(new ProductInfo { ProductName = "repositoryUncle" });
            int bodyLength = 4 + 2 + 2 + cmd.Length + version.Length + message.Length;
            byte[] buffer = new byte[bodyLength];

            #region （发送端）写入字节流
            //write message length
            Buffer.BlockCopy(BitConverter.GetBytes(bodyLength), 0, buffer, 0, 4);
            //write version len.
            Buffer.BlockCopy(BitConverter.GetBytes(version.Length), 0, buffer, 4, 2);
            //write response cmd len.
            Buffer.BlockCopy(BitConverter.GetBytes(cmd.Length), 0, buffer, 6, 2);

            //write response version.
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(version), 0, buffer, 8, version.Length);
            //write response version.
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(cmd), 0, buffer, 8 + version.Length, cmd.Length);
            //write response message.
            Buffer.BlockCopy(message, 0, buffer, 8 + version.Length + cmd.Length, message.Length);

            #endregion

            #region （接收端）读字节流

            var messageLength = BitConverter.ToInt32(buffer, 0);
            var version2 = BitConverter.ToInt16(buffer, 4);
            var cmd2 = BitConverter.ToInt16(buffer, 6);
            var versional = Encoding.ASCII.GetString(buffer, 8, version2);
            var cmdVal = Encoding.ASCII.GetString(buffer, 8 + version2, cmd2);
            var dataLength = messageLength - 8 - cmd2 - version2;
            byte[] body = new byte[dataLength];
            Buffer.BlockCopy(buffer, 8 + cmd2 + version2, body, 0, dataLength);
            var obj = (ProductInfo)SerializeHelper.DeserializeFromBinary(body);
            #endregion

            #endregion

            #region 排序字典与NameValue字典
            SortedDictionary<string, object> sortDic = new SortedDictionary<string, object>();

            sortDic.Add("zzl", "name");
            sortDic.Add("name", "zzl");
            sortDic.Add("age", "15");
            sortDic.Add("tel", "13521972991");
            foreach (var item in sortDic)
                Console.WriteLine("key:{0},value:{1}", item.Key, item.Value);

            var xmlData = DictionaryExtensions.ToXml(sortDic);
            var strData = DictionaryExtensions.FromXml(xmlData);


            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("color", "red");
            nvc.Add("color", "green");
            nvc.Add("age", "15");
            foreach (string key in nvc.Keys)
                Console.WriteLine("key:{0},value:{1}", key, nvc[key]);

            #endregion

            #region 对象与byte[]

            JsonConvert.SerializeObject(new
            {
                name = "zzl"
            });


            Utils.SerializeMemoryHelper.SerializeToBinary(new
            {
                name = "zzl"
            });

            Shop byteShop = new Shop() { Name = "zzl test" };
            var resultByte = Utils.SerializeMemoryHelper.SerializeToBinary(byteShop);
            #endregion

            #region Byte[] & Stream
            //1字节＝８位（bit）,byte:0-255,(2^8=256，它由２５６个数组成，取值０－２５５)
            byte[] testByte = System.Text.Encoding.ASCII.GetBytes("abcdef");//将字符串转为byte[]，进行数据传输
            Stream testStream = new MemoryStream(testByte);//将byte[]数组初始化到内存流里
            byte[] testBuffer = new byte[1024];//定义缓冲区
            testStream.Read(testBuffer, 0, 6);//将流读到缓冲区里
            testStream.Close();
            #endregion

            #region FastDFS

            #region 上传
            byte[] content = null;
            if (File.Exists(@"d:\video.mp4"))
            {
                FileStream streamUpload = new FileStream(@"d:\video.mp4", FileMode.Open);
                using (BinaryReader reader = new BinaryReader(streamUpload))
                {
                    content = reader.ReadBytes((int)streamUpload.Length);//5*1024*1024byte
                }
            }
            //string fileName = FastDFSClient.UploadAppenderFile(node, content, "mdb");
            var fileName = FastDFSClient.UploadFile(FastDFSClient.GetStorageNode("tsingda"), content, "mp4", null, (name) =>
            {
                Console.WriteLine(name);
            });

            #endregion

            #region 分块上传
            content = new byte[bufferSize];
            var streamUp = new FileStream(@"d:\video.mp4", FileMode.Open);
            streamUp.Read(content, 0, bufferSize);
            string shortName = FastDFSClient.UploadAppenderFile(FastDFSClient.GetStorageNode(DFSGroupName), content, "mp4");
            BeginUploadPart(streamUp, shortName);
            streamUp.Close();
            #endregion

            #region 下载
            byte[] buffer1 = FastDFSClient.DownloadFile(FastDFSClient.GetStorageNode("tsingda"), fileName, 0L, 0L);
            var streamDown = new FileStream(@"D:\down.mp4", FileMode.Create);
            using (BinaryWriter write = new BinaryWriter(streamDown, Encoding.BigEndianUnicode))
            {
                write.Write(buffer1);
                write.Close();
            }
            streamDown.Close();
            #endregion

            #region 删除
            FastDFSClient.RemoveFile("tsingda", fileName);
            #endregion

            Console.Read();
            #endregion

            #region MongoDb操作
            MongoManager<Shop>.Instance.InsertOneAsync(new Shop
            {
                Code = "x1",
                Name = "zzl",
            });
            var model = MongoManager<Shop>.Instance.FindAsync(Builders<Shop>.Filter.Eq("Code", "x1"));
            foreach (var i in model.Result.ToListAsync().Result)
                Console.WriteLine(i.Code);
            #endregion

            #region Lind.DDD.RedisClient
            //string,字符
            //hash,对象
            //list,双向链表
            //set,去重集合
            //sortset,根据权重值排序
            #region set
            RedisManager.Instance.GetDatabase().SetAdd("set", "zzl");
            RedisManager.Instance.GetDatabase().SetAdd("set", "zzllr");
            RedisManager.Instance.GetDatabase().SetAdd("set", "zzl");
            #endregion

            #region list
            RedisManager.Instance.GetDatabase().ListLeftPush("list", "zzl");
            RedisManager.Instance.GetDatabase().ListLeftPush("list", "zzl");
            RedisManager.Instance.GetDatabase().ListLeftPush("list", "lr");
            #endregion

            #region hash,添加过就不进行添加
            RedisManager.Instance.GetDatabase().HashSet("User_Redis", "name", "zzl");
            RedisManager.Instance.GetDatabase().HashSet("User_Redis", "sex", "1");

            #endregion

            #region sort set
            //值越小越在前
            RedisManager.Instance.GetDatabase().SortedSetAdd("sort", "zzl", 1);
            RedisManager.Instance.GetDatabase().SortedSetAdd("sort", "lr", 2);
            RedisManager.Instance.GetDatabase().SortedSetAdd("sort", "dudu", 0.5);
            RedisManager.Instance.GetDatabase().SortedSetAdd("sort", "zzz", -1);
            #endregion

            #endregion

            #region Redis并发锁机制

            RedisManager.Instance.GetDatabase().StringSet("addingNumber", 1);
            var transaction = RedisManager.Instance.GetDatabase().CreateTransaction();
            //  transaction.AddCondition(Condition.StringEqual("key","")); //the API here could maybe be simplified
            var val = transaction.StringGetAsync("addingNumber").Result; //notably, this is not async because you would have to get the result immediately - it would only work on watched keys
            transaction.StringSetAsync("addingNumber", val + 1);
            transaction.Execute();
            #endregion

            #region 缓存队列
            QueueManager.Instance.Push("zzl",Utils.SerializeMemoryHelper.SerializeToBinary("zzl hello!"));
            string msgReturn = (string)Utils.SerializeMemoryHelper.DeserializeFromBinary(QueueManager.Instance.Pop("zzl"));
            Console.WriteLine(msgReturn);
            #endregion

            #region 分布式的Pub/Sub(可以做成队列服务）[长连接，不能保证数据完整性]

            PubSubManager.Instance.SubscribeAsync("SendNotify", (msg) =>
            {
                Thread.Sleep(3000);
                //订阅者A，进行Email的发送
                Logger.LoggerFactory.Instance.Logger_Info("1-Email发送消息" + msg);
                //Console.WriteLine("1-Email发送消息{0}", msg);
            });
            PubSubManager.Instance.SubscribeAsync("SendNotify", (msg) =>
            {
                Thread.Sleep(2000);
                //订阅者B，进行SMS的发送
                Logger.LoggerFactory.Instance.Logger_Info("2-SMS发送消息" + msg);
                //   Console.WriteLine("2-SMS发送消息{0}", msg);
            });
            PubSubManager.Instance.SubscribeAsync("SendNotify", (msg) =>
            {
                Thread.Sleep(1000);
                //订阅者B，进行XMPP的发送
                Logger.LoggerFactory.Instance.Logger_Info("3-XMPP发送消息" + msg);
                //  Console.WriteLine("3-XMPP发送消息{0}", msg);
            });
            #endregion

            #region 分布式的Pub/Sub发布事件[长连接，不能保证数据完整性]

            //订阅的主题
            string[] emailServices = { "LessonOnLine", "Ordered", "MoneyIn", "MoneyOut" };
            foreach (var sevice in emailServices)
            {
                PubSubManager.Instance.Subscribe(sevice, (msg) =>
                {
                    //注册所有发送Email的服务
                    Console.WriteLine(msg);
                    Logger.LoggerFactory.Instance.Logger_Info(msg);
                });
            }

            //用户登陆模块
            PubSubManager.Instance.PublishAsync("SendNotify", "占岭这个用户登录了");
            PubSubManager.Instance.PublishAsync("SendNotify", "张三这个用户建立了");

            //发布动作在具体业务里
            PubSubManager.Instance.Publish("LessonOnLine", "大叔，你的课程已经审核通过");
            PubSubManager.Instance.Publish("Ordered", "大叔，有学生购买了您的课程");
            PubSubManager.Instance.Publish("MoneyIn", "大叔，钱来了");
            PubSubManager.Instance.Publish("MoneyOut", "大叔，你的1000元已经提出成功了");
            Logger.LoggerFactory.Instance.Logger_Info("应该不阻塞");
            Console.ReadKey();
            #endregion

            #region Unity IoC Dynamic Instace
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IRepository<Shop>, Lind.DDD.Repositories.Xml.XmlRepository<Shop>>();
                var repository = container.Resolve<IRepository<Shop>>();
                var shoplist = repository.GetModel().ToList();
            }

            //IOC上下文，使用using自动进行Dispose
            using (IUnityContainer container = new UnityContainer())
            {
                string helloType = "Lind.DDD.Test.China1";//从配置文件或者数据库里读取信息

                if (Type.GetType(helloType) == null)
                    throw new TypeLoadException("没有这个类型");

                container.RegisterType(typeof(IHello), Type.GetType(helloType));//注意类型与实现的关系
                var hello = container.Resolve<IHello>();//生产对象
                hello.HelloWorld();//调用方法
            }
            #endregion

            #region 根据类型字符，IOC构建对象
            //var t = Type.GetType("Lind.DDD.ConfigConstants.ConfigModel,Lind.DDD");
            //var tGeneric = Type.GetType("Lind.DDD.Test.China1`1[Lind.DDD.Test.Shop]");//当前程序集中，只要写完整的类名称，否则需要在后面加程序集的名称，用逗号用开

            //var a = DynamicIoCFactory.GetService<IHello>("Lind.DDD.Test.China,Lind.DDD.Test");
            //a.HelloWorld();

            //var xml = DynamicIoCFactory.GetService<IRepository<Shop>>("Lind.DDD.Repositories.Xml.XmlRepository`1[[Lind.DDD.Test.Shop,Lind.DDD.Test]],Lind.DDD.Repositories.Xml");
            //xml.GetModel().ToList().ForEach(i =>
            //{
            //    Console.WriteLine(i.Name);
            //});


            //using (IUnityContainer container2 = new UnityContainer())
            //{
            //    tGeneric = Type.GetType("Lind.DDD.Repositories.Xml.XmlRepository`1[[Lind.DDD.ConfigConstants.ConfigModel,Lind.DDD]],Lind.DDD.Repositories.Xml");//拿到泛型类型
            //    //   tGeneric = tGeneric.MakeGenericType(typeof(Lind.DDD.ConfigConstants.ConfigModel));//注册泛型叁数
            //    container2.RegisterType(typeof(IRepository<ConfigModel>), tGeneric);//注意类型与实现的关系
            //    var repositoryXml = container2.Resolve<IRepository<ConfigModel>>();//生产对象
            //    repositoryXml.GetModel();
            //}
            #endregion

            #region 并发写日志
            for (int i = 0; i < 10000; i++)
            {
                Task.Run(() =>
                {
                    Logger.LoggerFactory.Instance.Logger_Info(string.Format("线程：{0},时间:{1}", Thread.CurrentThread.ManagedThreadId, DateTime.Now));
                });
            }
            Task.WaitAll();
            Console.WriteLine("线程执行完成");
            Console.Read();
            #endregion

            #region Web Api监听（自宿主）
            Assembly.Load("Lind.DDD.TestApi");  //手工加载某个api程序集的controller
            var config = new HttpSelfHostConfiguration("http://localhost:3333");
            config.Routes.MapHttpRoute("default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            var server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();
            Console.WriteLine("Server is opened");
            #endregion

            #region 引用类型
            var shop = new Shop { Name = "zzl" };
            var selfShop = shop;
            selfShop.Name = "zhz";
            Console.WriteLine("shop.Name={0},selfShop.Name={1}", shop.Name, selfShop.Name);
            #endregion

            #region 数据缓存
            CacheManager.Instance.Put("time", "时间缓存：" + DateTime.Now);
            Thread.Sleep(5000);
            Console.WriteLine(CacheManager.Instance.Get("time"));
            #endregion

            #region 生成时间计算
            for (var dt = new DateTime(2015, 10, 1); dt < new DateTime(2015, 11, 1); dt = dt.AddDays(1))
            {
                for (int x = 1; x < 206; x++)
                {
                    Random random2 = new Random((int)(DateTime.Now.Ticks));
                    var timer = dt.AddHours(7);

                    int totalTask = 262;//当天任务之和
                    int seconds = 15 * 60 * 60;//7-22点秒数之和
                    int stepSeconds = random2.Next(10, seconds / totalTask);//产生平均时间段，并随机产生
                    var beforeTime2 = timer.AddSeconds(stepSeconds * x);
                    Console.WriteLine(beforeTime2);
                    Lind.DDD.Logger.LoggerFactory.Instance.Logger_Info(beforeTime2.ToString());
                }
            }
            #endregion

            #region 随机的时间
            Random random = new Random((int)(DateTime.Now.Ticks));
            for (int i = 0; i < 100; i++)
            {

                int hour = random.Next(8, 22);
                int minute = random.Next(0, 60);
                int second = random.Next(0, 60);
                DateTime day = DateTime.Now;
                var beforeTime = new DateTime(
                    day.Year,
                    day.Month,
                    day.Day,
                    hour,
                    minute,
                    second);
                Console.WriteLine(beforeTime);
            }
            #endregion

            #region EF Update AutoMapper操作
            Guid id = Guid.Parse("3ba82f12-877c-4cd7-87f7-0a8abe8fd443");
            var usersRepository = new Lind.DDD.Repositories.EF.EFRepository<Users>();
            usersRepository.SetDataContext(new testEntities());

            var users = usersRepository.GetModel(i => i.UserId == id)
                                       .Select(i => new Users_Ext
                                       {
                                           UserId = i.UserId,
                                           UserName = i.UserName,
                                           ApplicationId = i.ApplicationId,
                                           LastActivityDate = i.LastActivityDate,
                                       }).ToList();//必须tolist()到本地，这时datareader才关闭

            foreach (var item in users)
            {
                Users user = item.MapTo<Users>();
                user.UserName = "zzl";
                usersRepository.Update(user);
            }
            #endregion

            #region aop方法拦截
            var test = ServiceLocator.Instance.GetService<AopUserBLL>();
            test.Hello();
            test.Insert(new User { UserName = "zzl" });
            test.Update(new User { UserName = "zql" });
            #endregion

            #region UoW
            //IUnitOfWork unitOfWork = new UnitOfWork();
            //var userRepository = new Lind.DDD.Repositories.EF.EFRepository<UserInfo>();
            //var productRepository = new Lind.DDD.Repositories.EF.EFRepository<ProductInfo>();
            //unitOfWork.RegisterChangeded(new UserInfo { UserName = "zzl3" }, SqlType.Insert, userRepository);
            //unitOfWork.Commit();
            #endregion


            #region EF6.0的In操作，性能不错
            var user_Product = new Lind.DDD.Repositories.EF.EFRepository<User_Product>();
            var productInfo = new Lind.DDD.Repositories.EF.EFRepository<ProductInfo>();
            user_Product.SetDataContext(new testEntities());
            productInfo.SetDataContext(new testEntities());
            //EF5 IQueryable<T>集合可以解释成Exist语句，而List<T>只能解释成in语句
            var idArr = user_Product.GetModel(i => i.UserId == 1).Select(i => i.ProductId);
            var product = user_Product.GetModel(i => idArr.Contains(i.ProductId)).ToList();
            #endregion


            #region 动态构建表达式树
            IEnumerable<User> userList = new List<User> 
            { 
                new  User{UserName="zzl",Age=12},
                new  User{UserName="zhz",Age=12},
                new  User{UserName="zhang",Age=13},
            };

            Expression<Func<User, bool>> exe = ExpressionExtensions.GenerateExpression<User>(
                new string[] { "Age", "UserName" },
                new object[] { "12", "zzl" },
                new string[] { "=", "=" });
            userList.Where(exe.Compile()).ToList().ForEach(i =>
            {
                Console.WriteLine(i.UserName);
            });
            #endregion

            #region AutoMapper
            User1 uc1 = new User1 { UserName = "zzl", Age = 3 };
            User DUDU = new User { Age = 1, UserName = "lr", Address = "dudu" };
            uc1.MapTo<User>(DUDU);
            #endregion

            #region Redis & Sqlserver事务机制
            var redis = new Lind.DDD.Repositories.Redis.RedisRepository<User>();
            var redisClient = RedisManager.Instance.GetDatabase();
            redis.SetDataContext(redisClient);

            //先执行redis,再执行sql,sql出错,事务正常
            Lind.DDD.RedisClient.RedisTransactionManager.Transaction(() =>
            {
                redis.Insert(new User { UserName = "gogod111" });
                redis.Insert(new User { UserName = "gogod211" });
            }, () =>
            {
                //   userRepository.Insert(new UserInfo { UserName = "zzl3" });
            });
            #endregion

            #region 日志Lind.DDD
            Logger.LoggerFactory.Instance.Logger_Debug("1");
            Logger.LoggerFactory.Instance.Logger_Fatal("1");
            Logger.LoggerFactory.Instance.Logger_Info("1");
            Logger.LoggerFactory.Instance.Logger_Warn("1");
            #endregion

            #region 反射调用程序集的方法
            //Users u = new Users();
            //IRepository<Users> bb = new LindDbEfRepository<Users>();
            //var cc = Activator.CreateInstance("Lind.DDD.Web", "Lind.DDD.Web.LindDbEfRepository`1[Lind.DDD.Web.Users]");
            //var d = cc.Unwrap();
            //var w = d.GetType();
            //MethodInfo methodInfo = w.GetMethod("Insert", new Type[] { typeof(Users) });
            //var s = methodInfo.Invoke(d, new Object[] { u });
            //Console.ReadKey();
            #endregion

        }
    }
}


