using Lind.DDD.FastSocket.Client;
using Lind.DDD.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.LindQueue
{
    /// <summary>
    /// 生产者相关配置
    /// </summary>
    public class ProducerSetting
    {
        public string BrokerName { get; set; }
        public string BrokerAddress { get; set; }
        public int BrokerPort { get; set; }
        public int Timeout { get; set; }
    }
    /// <summary>
    /// 生产者
    /// </summary>
    public class ProducerManager
    {

        ProducerSetting _producerSetting;
        AsyncBinarySocketClient _client;
        public ProducerManager(ProducerSetting producerSetting)
        {
            _producerSetting = producerSetting;
            _client = new AsyncBinarySocketClient(8192, 8192, _producerSetting.Timeout, _producerSetting.Timeout);
            _client.RegisterServerNode(_producerSetting.BrokerName, new IPEndPoint(IPAddress.Parse(_producerSetting.BrokerAddress), _producerSetting.BrokerPort));

        }
        /// <summary>
        /// 推入消息
        /// </summary>
        /// <param name="body"></param>
        public void Push(MessageBody body)
        {
            _client.Send(
                "LindQueue_Push",
                SerializeMemoryHelper.SerializeToBinary(body),
                res => res.Buffer).ContinueWith(c =>
                {
                    if (c.IsFaulted)
                    {
                        throw c.Exception;
                    }
                    Console.WriteLine(SerializeMemoryHelper.DeserializeFromBinary(c.Result));
                    Logger.LoggerFactory.Instance.Logger_Debug("LindQueue_Push发送结果：" + Encoding.UTF8.GetString(c.Result));
                }).Wait();
        }



    }
}
