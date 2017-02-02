using Lind.DDD.FastSocket.Client;
using Lind.DDD.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lind.DDD.LindQueue
{
    /// <summary>
    /// 消费模型
    /// </summary>
    public class ConsumerSetting
    {
        /// <summary>
        /// 中间件别名
        /// </summary>
        public string BrokenName { get; set; }
        /// <summary>
        /// 中间件地址
        /// </summary>
        public IPEndPoint BrokenAddress { get; set; }
        /// <summary>
        /// 消费topic和对应的处理程序，一般来说，每个topic都有自己的处理程序
        /// </summary>
        public Dictionary<string, Action<MessageBody>> Callback { get; set; }
        /// <summary>
        /// 客户端连接
        /// </summary>
        public AsyncBinarySocketClient Client { get; set; }

    }
    /// <summary>
    /// 消费者,它只是从Broken拿消息，而不关系消息存储的地方
    /// </summary>
    public class ConsumerManager
    {
        /// <summary>
        /// 消费者配置
        /// </summary>
        List<ConsumerSetting> _allConsumers;
        List<AsyncBinarySocketClient> _clientList;
        public ConsumerManager(List<ConsumerSetting> consumerModel)
        {
            _allConsumers = consumerModel;
            _clientList = new List<AsyncBinarySocketClient>();

            foreach (var item in consumerModel)
            {
                item.Client = new AsyncBinarySocketClient(8192, 8192, 3000, 3000);
                item.Client.RegisterServerNode(item.BrokenName, item.BrokenAddress);
            }
        }
        /// <summary>
        /// 消费者开始消费消息
        /// 单线程消费，减少并发冲突
        /// </summary>
        public void Start()
        {
            while (true)
            {
                foreach (var item in _allConsumers)
                {
                    foreach (var key in item.Callback.Keys)
                    {
                        item.Client.Send("LindQueue_Pull", SerializeMemoryHelper.SerializeToBinary(key), res => res.Buffer).ContinueWith(c =>
                        {
                            if (c.IsFaulted)
                            {
                                throw c.Exception;
                            }

                            if (c.Result == null)
                            {
                                Console.WriteLine("指定topic队列为空，服务挂起15秒！");
                                Thread.Sleep(15000);
                            }
                            else
                            {
                                var entity = SerializeMemoryHelper.DeserializeFromBinary(c.Result) as MessageBody;
                                item.Callback[key](entity);
                            }
                        }).Wait();
                    }

                }
            }
        }
    }
}
