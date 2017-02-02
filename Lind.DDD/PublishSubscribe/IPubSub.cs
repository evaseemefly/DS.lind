using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.PublishSubscribe
{
    /// <summary>
    /// 发布订阅的接口规则
    /// </summary>
    public interface IPubSub
    {
        /// <summary>
        /// 发布，有顺序，对象源是字符串
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="value"></param>
        /// <returns>订阅者总数</returns>
        long Publish(string channel, string value);
        /// <summary>
        /// 订阅，对象源是字符串
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="action"></param>
        void Subscribe(string channel, Action<string> action);
        /// <summary>
        /// 异步发布，无顺序，对象源是字符串
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="value"></param>
        /// <returns>订阅者总数</returns>
        long PublishAsync(string channel, string value);
        /// <summary>
        /// 异步订阅，无顺序，对象源是字符串
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="action"></param>
        void SubscribeAsync(string channel, Action<string> action);

        /// <summary>
        /// 发布，有顺序，对象源是Byte[]
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="value"></param>
        /// <returns>订阅者总数</returns>
        long PublishByte(string channel, byte[] value);
        /// <summary>
        /// 订阅，对象源是Byte[]
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="action"></param>
        void SubscribeByte(string channel, Action<byte[]> action);
        /// <summary>
        /// 异步发布，有顺序，对象源是Byte[]
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="value"></param>
        /// <returns>订阅者总数</returns>
        long PublishByteAsync(string channel, byte[] value);
        /// <summary>
        /// 异步订阅，对象源是Byte[]
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="action"></param>
        void SubscribeByteAsync(string channel, Action<byte[]> action);

        /// <summary>
        /// 发布，有顺序，对象源是泛型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="value"></param>
        /// <returns>订阅者总数</returns>
        long Publish<T>(string channel, T value);
        /// <summary>
        /// 订阅，对象源是泛型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="action"></param>
        void Subscribe<T>(string channel, Action<T> action);
        /// <summary>
        /// 异步发布，有顺序，对象源是泛型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="value"></param>
        /// <returns>订阅者总数</returns>
        long PublishAsync<T>(string channel, T value);
        /// <summary>
        /// 异步订阅，对象源是泛型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="action"></param>
        void SubscribeAsync<T>(string channel, Action<T> action);
        /// <summary>
        /// 取消指定订阅
        /// </summary>
        /// <param name="channel"></param>
        void UnSubscribe(string channel);
        /// <summary>
        /// 取消所有订阅
        /// </summary>
        /// <param name="channel"></param>
        void UnSubscribeAll();
    }
}
