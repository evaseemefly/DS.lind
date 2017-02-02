using Lind.DDD.FastSocket.Server;
using Lind.DDD.FastSocket.Server.Command;
using Lind.DDD.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.LindQueue
{
    /// <summary>
    /// 队列端消息管理者
    /// key/value:topic/queueId
    /// key/value:topic_queueId/MessageBody
    /// key/value:topic_consumerId/offset
    /// LindQueue中的broker负责消息的中转，即接收producer发送过来的消息，然后持久化消息到磁盘，
    /// 然后接收consumer发送过来的拉取消息的请求，然后根据请求拉取相应的消息给consumer。所以，broker可以理解为消息队列服务器，提供消息的接收、存储、拉取服务。可见，broker对于equeue来说是核心，它绝对不能挂，一旦挂了，那producer，consumer就无法实现publish-subscribe了。
    /// </summary>
    public class BrokerManager
    {
        #region consts
        /// <summary>
        ///负载均衡的取模数,N表示N+1个queue管道 
        /// </summary>
        const int CONFIG_QUEUECOUNT = 5;
        /// <summary>
        /// LindMQ统一键前缀
        /// </summary>
        const string LINDMQKEY = "LindMQ_";
        /// <summary>
        /// LindMQ所有Topic需要存储到这个键里
        /// </summary>
        const string LINDMQ_TOPICKEY = LINDMQKEY + "Topic";
        /// <summary>
        /// 每个消费者的消费进度
        /// </summary>
        const string QUEUEOFFSETKEY = LINDMQKEY + "ConsumerOffset";
        /// <summary>
        /// 消息自动回收的周期（小时）
        /// </summary>
        const int AutoEmptyForHour = 24;
        #endregion

        #region Public Methods
        /// <summary>
        /// 在队列中的消息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<MessageBody> GetMessageBody(string topic)
        {
            foreach (var item in RedisClient.RedisManager.Instance.GetDatabase().SetMembers(GetRedisKey(topic)))
            {
                foreach (var sub in RedisClient.RedisManager.Instance.GetDatabase().ListRange(item.ToString()))
                {
                    yield return Utils.SerializeMemoryHelper.DeserializeFromJson<MessageBody>(sub);
                }
            }

        }

        /// <summary>
        /// 拿到消息进度
        /// </summary>
        /// <returns></returns>
        public static IDictionary<string, int> GetQueueOffset()
        {
            var ret = RedisClient.RedisManager
                                 .Instance
                                 .GetDatabase()
                                 .HashGetAll(QUEUEOFFSETKEY)
                                 .ToDictionary(i => (string)i.Name, i => (int)i.Value);

            return ret;
        }

        /// <summary>
        /// 开始拉消息的服务,从FastSocket的配置文件里拿数据
        /// 服务端配置信息在Broken的宿主app.config
        /// </summary>
        public static void Start()
        {
            SocketServerManager.Init();
            SocketServerManager.Start();
            Console.ReadLine();
        }

        /// <summary>
        /// 自动清除过期的消息，清楚昨天的任务
        /// </summary>
        /// <returns></returns>
        public static void AutoRemoveQueue()
        {
            var topicList = RedisClient.RedisManager.Instance.GetDatabase().SetMembers(LINDMQ_TOPICKEY);
            foreach (var topic in topicList)
            {
                var queueList = RedisClient.RedisManager.Instance.GetDatabase().SetMembers(LINDMQKEY + topic);
                foreach (var queue in queueList)
                {
                    var removeKey = LINDMQKEY + queue + "_" + DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                    RedisClient.RedisManager.Instance.GetDatabase().KeyDelete(removeKey);
                }
            }
        }

        #endregion

        #region Internal & Private Methods
        /// <summary>
        /// 连接LindMQ在redis存储的键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static string GetRedisKey(string key)
        {
            return LINDMQKEY + key;
        }
        static string GetRedisDataKey(string key)
        {
            return GetRedisKey(key) + "_" + DateTime.Now.ToString("yyyyMMdd");
        }
        /// <summary>
        /// 推入消息
        /// </summary>
        /// <param name="body">消息体</param>
        internal static void Push(MessageBody body)
        {
            //存储当前Topic
            RedisClient.RedisManager.Instance.GetDatabase().SetAdd(LINDMQ_TOPICKEY, body.Topic);

            //要存储到哪个队列
            body.QueueId = Math.Abs(body.Body.GetHashCode() % BrokerManager.CONFIG_QUEUECOUNT);
            var dataKey = body.Topic + body.QueueId;
            RedisClient.RedisManager.Instance.GetDatabase().SetAdd(GetRedisKey(body.Topic), dataKey);

            //记录偏移
            var offset = RedisClient.RedisManager.Instance.GetDatabase().SortedSetLength(GetRedisDataKey(dataKey));
            body.QueueOffset = offset + 1;

            //存储消息
            RedisClient.RedisManager.Instance.GetDatabase().SortedSetAdd(
                GetRedisDataKey(dataKey),
                Utils.SerializeMemoryHelper.SerializeToJson(body),
                score: body.QueueOffset);
        }

        /// <summary>
        /// 拉出消息
        /// </summary>
        /// <param name="topicQueueId">队列</param>
        /// <param name="offset">偏移量</param>
        /// <returns></returns>
        internal static MessageBody Pull(string topic, string topicQueueId, int offset = 0)
        {
            if (topicQueueId == null)
                return null;

            //通过消费进度，拿指定进度的消息
            var entity = RedisClient.RedisManager.Instance.GetDatabase().SortedSetRangeByScore(GetRedisDataKey(topicQueueId), start: offset).FirstOrDefault();
            if (!entity.HasValue)
            {
                //当前客户端已经消费完成
                return null;
            }

            return Utils.SerializeMemoryHelper.DeserializeFromJson<MessageBody>(entity);
        }

        /// <summary>
        /// 拉出来消息后，处理消息
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="aciton">处理程序</param>
        /// <param name="offset">偏移量</param>
        internal static void Pull(string topic, string topicQueueId, Action<MessageBody> aciton, int offset = 0)
        {
            var entity = Pull(topic, topicQueueId, offset);
            if (entity != null)
                aciton(entity);
        }

        /// <summary>
        /// 返回当前消费者的消息偏移量
        /// </summary>
        /// <param name="connection">消费者连接</param>
        /// <param name="topic">主题</param>
        /// <param name="topicQueueId">当前队列</param>
        /// <param name="after">回调</param>
        /// <returns></returns>
        internal static int GetConsumerQueueOffset(
            FastSocket.SocketBase.IConnection connection,
            string topic,
            ref string topicQueueId,
            ref Action after)
        {
            int offset = 0;
            var queueList = RedisClient.RedisManager.Instance.GetDatabase().SetMembers(GetRedisKey(topic));
            topicQueueId = queueList.OrderByNewId().FirstOrDefault().ToString();
            if (topicQueueId == null)
                return 0;

            //消费者标识
            string connectionId = GetRedisKey(topicQueueId + "_" + connection.RemoteEndPoint.Address.Address);
            if (!RedisClient.RedisManager.Instance.GetDatabase().HashExists(QUEUEOFFSETKEY, connectionId))
            {
                RedisClient.RedisManager.Instance.GetDatabase().HashSet(QUEUEOFFSETKEY, connectionId, 1);
            }

            //算出当前队列的偏移量
            offset = (int)RedisClient.RedisManager.Instance.GetDatabase().HashGet(QUEUEOFFSETKEY, connectionId) + 1;
            //业务层的回调
            after = () =>
            {
                //更新消费端的消费量
                RedisClient.RedisManager.Instance.GetDatabase().HashSet(QUEUEOFFSETKEY, connectionId, offset);
            };

            return offset;
        }
        #endregion

    }




}
