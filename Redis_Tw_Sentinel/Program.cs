using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Redis_Tw_Sentinel
{
    class Program
    {
        public static T GetCachedAndModifyWithLock<T>(string key, Func<T> retrieveDataFunc, TimeSpan timeExpiration, Func<T, bool> modifyEntityFunc,
   TimeSpan? lockTimeout = null, bool isSlidingExpiration = false) where T : class
        {
            ConnectionMultiplexer Connection = ConnectionMultiplexer.Connect("localhost:6379");
            int lockCounter = 0;//for logging in case when too many locks per key
            Exception logException = null;

            var cache = Connection.GetDatabase();
            var lockToken = Guid.NewGuid().ToString(); //unique token for current part of code
            var lockName = key + "_lock"; //unique lock name. key-relative.
            T tResult = null;

            while (lockCounter < 20)
            {
                //check for access to cache object, trying to lock it
                if (!cache.LockTake(lockName, lockToken, lockTimeout ?? TimeSpan.FromSeconds(10)))
                {
                    lockCounter++;
                    Thread.Sleep(100); //sleep for 100 milliseconds for next lock try. you can play with that
                    continue;
                }

                try
                {
                    RedisValue result = RedisValue.Null;

                    if (isSlidingExpiration)
                    {
                        //in case of sliding expiration - get object with expiry time
                        var exp = cache.StringGetWithExpiry(key);

                        //check ttl.
                        if (exp.Expiry.HasValue && exp.Expiry.Value.TotalSeconds >= 0)
                        {
                            //get only if not expired
                            result = exp.Value;
                        }
                    }
                    else //in absolute expiration case simply get
                    {
                        result = cache.StringGet(key);
                    }

                    //"REDIS_NULL" is for cases when our retrieveDataFunc function returning null (we cannot store null in redis, but can store pre-defined string :) )
                    if (result.HasValue && result == "REDIS_NULL") return null;
                    //in case when cache is epmty
                    if (!result.HasValue)
                    {
                        //retrieving data from caller function (from db from example)
                        tResult = retrieveDataFunc();

                        if (tResult != null)
                        {
                            //trying to modify that entity. if caller modifyEntityFunc returns true, it means that caller wants to resave modified entity.
                            if (modifyEntityFunc(tResult))
                            {
                                //json serialization
                                var json = JsonConvert.SerializeObject(tResult);
                                cache.StringSet(key, json, timeExpiration);
                            }
                        }
                        else
                        {
                            //save pre-defined string in case if source-value is null.
                            cache.StringSet(key, "REDIS_NULL", timeExpiration);
                        }
                    }
                    else
                    {
                        //retrieve from cache and serialize to required object
                        tResult = JsonConvert.DeserializeObject<T>(result);
                        //trying to modify
                        if (modifyEntityFunc(tResult))
                        {
                            //and save if required
                            var json = JsonConvert.SerializeObject(tResult);
                            cache.StringSet(key, json, timeExpiration);
                        }
                    }

                    //refresh exiration in case of sliding expiration flag
                    if (isSlidingExpiration)
                        cache.KeyExpire(key, timeExpiration);
                }
                catch (Exception ex)
                {
                    logException = ex;
                }
                finally
                {
                    cache.LockRelease(lockName, lockToken);
                }
                break;
            }

            if (lockCounter >= 20 || logException != null)
            {
                //log it
            }

            return tResult;
        }


        static void Main(string[] args)
        {
            //连接TW服务器
            var config = new ConfigurationOptions();
            config.EndPoints.Add("192.168.1.190:22121");
            config.Proxy = Proxy.Twemproxy;
            config.Password = "Tsingda#rdsZXC#$%198.x";
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(config);
            redis.GetDatabase().StringSet("zzltest", "test" + DateTime.Now.ToLongTimeString());
            Console.WriteLine(redis.GetDatabase().StringGet("zzltest"));

            var db = redis.GetDatabase();
            db.StringSet("addingNumber", 1);
            var transaction = db.CreateTransaction();
            transaction.AddCondition(Condition.StringEqual("addingNumber", ""));
            var val = transaction.StringGetAsync("addingNumber").Result;
            transaction.StringSetAsync("addingNumber", val + 1);
            transaction.Execute();

            Console.WriteLine("addingNumber:{0}", db.StringGet("addingNumber"));

            //RedisValue token = Environment.MachineName;
            //if (db.LockTake("key1", token, new TimeSpan(0, 0, 1)))
            //{
            //    try
            //    {
            //        // you have the lock do work
            //    }
            //    finally
            //    {
            //        db.LockRelease("key1", token);
            //    }
            //}

        }
    }
}
