using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lind.DDD.PublishSubscribe
{
    /// <summary>
    /// 发布与订阅的管理者
    /// 说明：分布式的pub/sub模式，订阅者与发布者不需要是同一系统，同一平台，只需要实现某些Provider驱动即可
    /// Function:Distributed Pub/Sub Pattern.
    /// </summary>
    public class PubSubManager : IPubSub
    {
        #region Constructors & Fields
        private IPubSub pubSub;
        private PubSubManager()
        {
            pubSub = new RedisProvider();
        }
        /// <summary>
        /// 单实例
        /// </summary>
        private static PubSubManager instance;
        /// <summary>
        /// 重复次数
        /// </summary>
        private static int repeatNum = ConfigConstants.ConfigManager.Config.Pub_Sub.RepeatNum;
        /// <summary>
        /// 时间间隔，失败后的等待时间，单位毫秒
        /// </summary>
        private static int interval = ConfigConstants.ConfigManager.Config.Pub_Sub.Interval;
        /// <summary>
        /// 发布/订阅单例
        /// </summary>
        public static PubSubManager Instance
        {
            get
            {
                return instance ?? (instance = new PubSubManager());
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 输出控制台和日志
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="num"></param>
        private void Write(string channel, int num)
        {
            string msg = string.Format("队列管道:{0},成功找到订阅者，当前是第{1}次重试", channel, num);
            var @default = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ForegroundColor = @default;
            Logger.LoggerFactory.Instance.Logger_Warn(msg);
        }
        /// <summary>
        /// 失败后的重试操作
        /// </summary>
        /// <param name="action"></param>
        private void RepeatAction(string channel, Func<bool> action)
        {
            for (int i = 1; i <= repeatNum; i++)
            {
                Thread.Sleep(interval);
                string msg = string.Format("队列管道:{0},发布者发展订阅者有问题，正在重试第{1}次", channel, i);
                Console.WriteLine(msg);
                Logger.LoggerFactory.Instance.Logger_Warn(msg);
                if (action())
                    break;
            }
        }
        #endregion

        #region IPubSub 成员

        #region 同步版本
        public long Publish(string channel, string value)
        {
            if (pubSub.Publish(channel, value) == 0)
            {
                //订阅者为0，或者订阅者出现异常，我们可以重试
                RepeatAction(channel, () =>
                {
                    return pubSub.Publish(channel, value) > 0;
                });
            }
            return 0;
        }
        public void Subscribe(string channel, Action<string> action)
        {
            pubSub.Subscribe(channel, action);
        }
        public long PublishByte(string channel, byte[] value)
        {
            if (pubSub.PublishByte(channel, value) == 0)
            {
                //订阅者为0，或者订阅者出现异常，我们可以重试
                RepeatAction(channel, () =>
                {
                    return pubSub.PublishByte(channel, value) > 0;
                });
            }
            return 0;
        }
        public void SubscribeByte(string channel, Action<byte[]> action)
        {
            pubSub.SubscribeByte(channel, action);
        }
        public long Publish<T>(string channel, T value)
        {
            if (pubSub.Publish<T>(channel, value) == 0)
            {
                //订阅者为0，或者订阅者出现异常，我们可以重试
                RepeatAction(channel, () =>
                {
                    return pubSub.Publish<T>(channel, value) > 0;
                });
            }
            return 0;
        }
        public void Subscribe<T>(string channel, Action<T> action)
        {
            pubSub.Subscribe(channel, action);
        }
        #endregion

        #region 异步版本
        public long PublishAsync(string channel, string value)
        {
            if (pubSub.PublishAsync(channel, value) == 0)
            {
                //订阅者为0，或者订阅者出现异常，我们可以重试
                RepeatAction(channel, () =>
                {
                    return pubSub.PublishAsync(channel, value) > 0;
                });
            }
            return 0;
        }

        public void SubscribeAsync(string channel, Action<string> action)
        {
            Lind.DDD.Utils.ThreadManager.Run(() =>
             {
                 pubSub.SubscribeAsync(channel, action);
             });
        }

        public long PublishByteAsync(string channel, byte[] value)
        {
            Lind.DDD.Utils.ThreadManager.Run(() =>
            {
                if (pubSub.PublishByteAsync(channel, value) == 0)
                {
                    RepeatAction(channel, () =>
                    {
                        return pubSub.PublishByteAsync(channel, value) > 0;
                    });
                }

            });
            return 0;
        }

        public void SubscribeByteAsync(string channel, Action<byte[]> action)
        {
            Lind.DDD.Utils.ThreadManager.Run(() =>
            {
                pubSub.SubscribeByteAsync(channel, action);
            });
        }

        public long PublishAsync<T>(string channel, T value)
        {
            Lind.DDD.Utils.ThreadManager.Run(() =>
            {

                if (pubSub.PublishAsync(channel, value) == 0)
                {
                    RepeatAction(channel, () =>
                    {
                        return pubSub.PublishAsync(channel, value) > 0;
                    });
                }

            });
            return 0;
        }

        public void SubscribeAsync<T>(string channel, Action<T> action)
        {
            Lind.DDD.Utils.ThreadManager.Run(() =>
            {
                pubSub.SubscribeAsync(channel, action);
            });
        }

        #endregion

        public void UnSubscribe(string channel)
        {
            pubSub.UnSubscribe(channel);
        }

        public void UnSubscribeAll()
        {
            pubSub.UnSubscribeAll();
        }

        #endregion
    }
}
