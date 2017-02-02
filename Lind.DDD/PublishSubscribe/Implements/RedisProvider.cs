using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lind.DDD.PublishSubscribe
{
    /// <summary>
    /// 使用redis实现分布式的pub/sub模式,它不会做序列化操作
    /// 在多系统之间实现发布、订阅模式
    /// publish返回值为订阅的数量，为0表示没有任务订阅项，本消息为发布失败的消息，应该进行二次处理
    /// </summary>
    internal class RedisProvider : IPubSub
    {
        /// <summary>
        /// 订阅者与发布者对象
        /// </summary>
        static ISubscriber sub = StackExchange.Redis.ConnectionMultiplexer.Connect(ConfigConstants.ConfigManager.Config.Redis.Host).GetSubscriber();

        #region IPubSub 成员

        #region String对象
        public long Publish(string channel, string value)
        {
            return sub.Publish(channel, value);
        }

        public void Subscribe(string channel, Action<string> action)
        {
            sub.Subscribe(channel, (c, m) =>
            {
                action(m);
            });

        }

        public long PublishAsync(string channel, string value)
        {
            return sub.PublishAsync(channel, value).Result;
        }

        public void SubscribeAsync(string channel, Action<string> action)
        {
            sub.SubscribeAsync(channel, (c, m) =>
            {
                action(m);
            });
        }

        #endregion

        #region Byte[]对象
        public long PublishByte(string channel, byte[] value)
        {
            return sub.Publish(channel, value);
        }

        public void SubscribeByte(string channel, Action<byte[]> action)
        {
            sub.Subscribe(channel, (c, m) =>
            {
                action(m);
            });
        }

        public long PublishByteAsync(string channel, byte[] value)
        {
            return sub.PublishAsync(channel, value).Result;
        }

        public void SubscribeByteAsync(string channel, Action<byte[]> action)
        {
            sub.SubscribeAsync(channel, (c, m) =>
            {
                action(m);
            });
        }

        #endregion

        #region 泛型对象
        public long Publish<T>(string channel, T value)
        {
            return sub.Publish(channel, Utils.SerializeMemoryHelper.SerializeToBinary(value));
        }

        public void Subscribe<T>(string channel, Action<T> action)
        {
            sub.Subscribe(channel, (c, m) =>
            {
                action((T)Utils.SerializeMemoryHelper.DeserializeFromBinary(m));
            });
        }

        public long PublishAsync<T>(string channel, T value)
        {
            return sub.PublishAsync(channel, Utils.SerializeMemoryHelper.SerializeToBinary(value)).Result;
        }

        public void SubscribeAsync<T>(string channel, Action<T> action)
        {
            sub.SubscribeAsync(channel, (c, m) =>
            {
                action((T)Utils.SerializeMemoryHelper.DeserializeFromBinary(m));
            });
        }
        #endregion

        #endregion

        #region IPubSub 成员


        public void UnSubscribe(string channel)
        {
            sub.Unsubscribe(channel);
        }

        public void UnSubscribeAll()
        {
            sub.UnsubscribeAll();
        }

        #endregion
    }
}
