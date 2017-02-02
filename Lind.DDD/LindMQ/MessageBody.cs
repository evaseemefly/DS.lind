using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.LindQueue
{
    /// <summary>
    /// 客户端对于Topic队列的消费进度
    /// </summary>
    public class Client_TopicQueue_Offset
    {
        /// <summary>
        /// 客户端消费者标识，可能是ＩＰ地址
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Topic名称
        /// </summary>
        public string TopicName { get; set; }
        /// <summary>
        /// 队列编号
        /// </summary>
        public int QueueId { get; set; }
        /// <summary>
        /// 队列消费的偏移量
        /// </summary>
        public int Offset { get; set; }
    }
    /// <summary>
    /// 消息协议
    /// </summary>
    [Serializable]
    public class MessageBody
    {
        public MessageBody()
        {
            CreateTime = DateTime.Now;
        }
        /// <summary>
        /// 消息所属Topic，每种Topic有一种类型的Body
        /// </summary>
        public string Topic { get; set; }
        /// <summary>
        /// 消息内容，Redis里存储为Json
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 消息所属的队列ID
        /// </summary>
        public int QueueId { get; set; }
        /// <summary>
        /// 消息在所属队列的序号
        /// </summary>
        public long QueueOffset { get; set; }
        /// <summary>
        /// 消息的存储时间
        /// </summary>
        public DateTime CreateTime { get; private set; }
        /// <summary>
        /// 将消息对象序列化成字符
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Utils.SerializeMemoryHelper.SerializeToJson<MessageBody>(this);
        }
    }




}
