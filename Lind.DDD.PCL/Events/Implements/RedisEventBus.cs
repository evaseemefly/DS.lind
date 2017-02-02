﻿using Lind.DDD.RedisClient;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.Events
{
    internal class RedisEventBus : IEventBus
    {
        /// <summary>
        /// 模式锁
        /// </summary>
        private static object _objLock = new object();
        /// <summary>
        /// 对于事件数据的存储，目前采用内存字典
        /// </summary>
        private readonly IRedisClient _redisClient = RedisManager.GetClient();
        private const string REDIS_EVENT_BUS = "RedisEventBus";
        /// <summary>
        /// 比较两个委托Handler是否相同，以免添加重复事件
        /// </summary>
        private readonly Func<object, object, bool> _eventHandlerEquals = (o1, o2) =>
        {
            var o1Type = o1.GetType();
            var o2Type = o2.GetType();
            if (o1Type.IsGenericType
                && o1Type.GetGenericTypeDefinition() == typeof(ActionDelegatedEventHandler<>)
                && o2Type.IsGenericType
                && o2Type.GetGenericTypeDefinition() == typeof(ActionDelegatedEventHandler<>))
                return o1.Equals(o2);
            return o1Type == o2Type;
        };

        #region IEventBus 成员

        #region 事件订阅 & 取消订阅，可以扩展
        /// <summary>
        /// 订阅事件列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="subTypeList"></param>
        public void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent
        {
            using (var tbl = _redisClient.GetTypedClient<Dictionary<Type, List<object>>>())
            {
                lock (_objLock)
                {
                    if (tbl.Lists[REDIS_EVENT_BUS] == null || tbl.Lists[REDIS_EVENT_BUS].Count == 0)
                    {
                        tbl.Lists[REDIS_EVENT_BUS].Add(new Dictionary<Type, List<object>>());
                    }
                    var _eventHandlers = (Dictionary<Type, List<object>>)tbl.Lists[REDIS_EVENT_BUS][0];
                    var eventType = typeof(TEvent);
                    if (_eventHandlers.ContainsKey(eventType))
                    {
                        var handlers = _eventHandlers[eventType];
                        if (handlers != null)
                        {
                            if (!handlers.Exists(deh => _eventHandlerEquals(deh, eventHandler)))
                                handlers.Add(eventHandler);
                        }
                        else
                        {
                            handlers = new List<object>();
                            handlers.Add(eventHandler);
                        }
                    }
                    else
                        _eventHandlers.Add(eventType, new List<object> { eventHandler });
                }
            }

        }
        /// <summary>
        /// 订阅事件实体
        /// 装饰模式
        /// </summary>
        /// <param name="type"></param>
        /// <param name="subTypeList"></param>
        public void Subscribe<TEvent>(Action<TEvent> eventHandlerFunc)
            where TEvent : class, IEvent
        {
            Subscribe<TEvent>(new ActionDelegatedEventHandler<TEvent>(eventHandlerFunc));
        }
        public void Subscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : class, IEvent
        {
            foreach (var eventHandler in eventHandlers)
                Subscribe<TEvent>(eventHandler);
        }
        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="subType"></param>
        public void Unsubscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent
        {
            using (var tbl = _redisClient.GetTypedClient<Dictionary<Type, List<object>>>())
            {
                lock (_objLock)
                {
                    var _eventHandlers = (Dictionary<Type, List<object>>)tbl.Lists[REDIS_EVENT_BUS][0];
                    var eventType = typeof(TEvent);
                    if (_eventHandlers.ContainsKey(eventType))
                    {
                        var handlers = _eventHandlers[eventType];
                        if (handlers != null
                            && handlers.Exists(deh => _eventHandlerEquals(deh, eventHandler)))
                        {
                            var handlerToRemove = handlers.First(deh => _eventHandlerEquals(deh, eventHandler));
                            handlers.Remove(handlerToRemove);
                        }
                    }
                }
            }
        }
        public void Unsubscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
          where TEvent : class, IEvent
        {
            foreach (var eventHandler in eventHandlers)
                Unsubscribe<TEvent>(eventHandler);
        }
        public void Unsubscribe<TEvent>(Action<TEvent> eventHandlerFunc)
            where TEvent : class, IEvent
        {
            Unsubscribe<TEvent>(new ActionDelegatedEventHandler<TEvent>(eventHandlerFunc));
        }
        /// <summary>
        /// 取消指定事件的所有订阅
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlerFunc"></param>
        public void UnsubscribeAll<TEvent>()
        where TEvent : class, IEvent
        {
            using (var tbl = _redisClient.GetTypedClient<Dictionary<Type, List<object>>>())
            {
                lock (_objLock)
                {
                    var _eventHandlers = (Dictionary<Type, List<object>>)tbl.Lists[REDIS_EVENT_BUS][0];
                    var eventType = typeof(TEvent);

                    if (_eventHandlers.ContainsKey(eventType))
                    {
                        var handlers = _eventHandlers[eventType];
                        if (handlers != null)
                            handlers.Clear();
                    }
                }
            }
        }
        /// <summary>
        /// 取消所有事件的所有订阅
        /// </summary>
        public void UnsubscribeAll()
        {
            using (var tbl = _redisClient.GetTypedClient<Dictionary<Type, List<object>>>())
            {
                lock (_objLock)
                {
                    var arr = (Dictionary<Type, List<object>>)tbl.Lists[REDIS_EVENT_BUS][0];
                    if (arr != null)
                        arr.Clear();
                }
            }
        }
        #endregion

        #region 事件发布
        /// <summary>
        /// 发布事件，支持异步事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="evnt"></param>
        public void Publish<TEvent>(TEvent @event)
           where TEvent : class, IEvent
        {
            using (var tbl = _redisClient.GetTypedClient<Dictionary<Type, List<object>>>())
            {
                lock (_objLock)
                {
                    var _eventHandlers = (Dictionary<Type, List<object>>)tbl.Lists[REDIS_EVENT_BUS][0];

                    if (@event == null)
                        throw new ArgumentNullException("event");
                    var eventType = @event.GetType();
                    if (_eventHandlers.ContainsKey(eventType)
                        && _eventHandlers[eventType] != null
                        && _eventHandlers[eventType].Count > 0)
                    {
                        var handlers = _eventHandlers[eventType];
                        foreach (var handler in handlers)
                        {
                            var eventHandler = handler as IEventHandler<TEvent>;
                            if (eventHandler.GetType().IsDefined(typeof(HandlesAsynchronouslyAttribute), false))
                            {
                                Task.Factory.StartNew((o) => eventHandler.Handle((TEvent)o), @event);
                            }
                            else
                            {
                                eventHandler.Handle(@event);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 发布事件
        /// event参数为关键字,所以加了@符
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        /// <param name="callback"></param>
        /// <param name="timeout"></param>
        public void Publish<TEvent>(TEvent @event, Action<TEvent, bool, Exception> callback, TimeSpan? timeout = null)
           where TEvent : class, IEvent
        {
            using (var tbl = _redisClient.GetTypedClient<Dictionary<Type, List<object>>>())
            {
                lock (_objLock)
                {
                    var _eventHandlers = (Dictionary<Type, List<object>>)tbl.Lists[REDIS_EVENT_BUS][0];
                    if (@event == null)
                        throw new ArgumentNullException("event");
                    var eventType = @event.GetType();
                    if (_eventHandlers.ContainsKey(eventType) &&
                        _eventHandlers[eventType] != null &&
                        _eventHandlers[eventType].Count > 0)
                    {
                        var handlers = _eventHandlers[eventType];
                        List<Task> tasks = new List<Task>();
                        try
                        {
                            foreach (var handler in handlers)
                            {
                                var eventHandler = handler as IEventHandler<TEvent>;
                                if (eventHandler.GetType().IsDefined(typeof(HandlesAsynchronouslyAttribute), false))
                                {
                                    tasks.Add(Task.Factory.StartNew((o) => eventHandler.Handle((TEvent)o), @event));
                                }
                                else
                                {
                                    eventHandler.Handle(@event);
                                }
                            }
                            if (tasks.Count > 0)
                            {
                                if (timeout == null)
                                    Task.WaitAll(tasks.ToArray());
                                else
                                    Task.WaitAll(tasks.ToArray(), timeout.Value);
                            }
                            callback(@event, true, null);
                        }
                        catch (Exception ex)
                        {
                            callback(@event, false, ex);
                        }
                    }
                    else
                        callback(@event, false, null);
                }
            }
        }
        /// <summary>
        /// 显式的异步发布事件,不需要为处理程序加HandlesAsynchronouslyAttribute
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event">具体事件执行者</param>
        public void PublishAsync<TEvent>(TEvent @event)
           where TEvent : class, IEvent
        {
            Task.Factory.StartNew(() => Publish(@event));
        }
        /// <summary>
        /// 显式的异步发布事件,不需要为处理程序加HandlesAsynchronouslyAttribute
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event">具体事件执行者</param>
        /// <param name="callback">回调</param>
        /// <param name="timeout">超时</param>
        public void PublishAsync<TEvent>(TEvent @event, Action<TEvent, bool, Exception> callback, TimeSpan? timeout = null)
          where TEvent : class, IEvent
        {
            Task.Factory.StartNew(() => Publish(@event, callback, timeout));
        }
        #endregion


        #endregion
    }
}
