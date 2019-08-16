using DeskRedis.Model;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskRedis.Util.Redis
{
    public class RedisCache : AbstractRedisPoolManger, IRedisCache
    {
        #region 构造方法
        /// <summary>
        /// 使用指定的参数创建实例。主机地址示例，“127.0.0.1:6379”，或者带有身份校验的“123456@127.0.0.1:6379”。
        /// </summary>
        /// <param name="readWriteHosts">可读写Redis主机的地址。</param>
        /// <param name="readOnlyHosts">只读Redis主机的地址。</param>
        /// <exception cref="ArgumentException">Redis主机地址不合法时。</exception>
        public RedisCache(string[] readWriteHosts, string[] readOnlyHosts)
            : base(readWriteHosts, readOnlyHosts)
        { }
        #endregion


        #region 公共方法，IRedisAccess成员
        /// <summary>
        /// 连接测试。
        /// </summary>
        /// <returns></returns>
        public string ConnectTest()
        {
            IRedisClient client = null;
            try
            {
                client = base.pooledRedisClientManager.GetClient();
            }
            catch (Exception e)
            {
                return e.InnerException.Message;
            }
            finally
            {
                client?.Dispose();
                client = null;
            }
            return "SUCCESS";
        }

        public int GetDataBaseCount()
        {
            int count;
            using (IRedisClient readOnlyClient = base.pooledRedisClientManager.GetReadOnlyClient())
            {
                string a = readOnlyClient.GetConfig("databases");
                count = Convert.ToInt32(a);
            }
            return count;
        }

        /// <summary>
        /// 获取所有键
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllKeys(int index = 0)
        {
            List<string> keys;
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                client.Db = index;
                keys = client.GetAllKeys();
            }
            return keys;
        }

        /// <summary>
        /// 当前数据库的 key 的数量。
        /// </summary>
        /// <returns></returns>
        public long GetKeyCount(int index = 0)
        {
            long count;
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                client.Db = index;
                count = client.DbSize;
            }
            return count;
        }

        /// <summary>
        /// 重命名键
        /// </summary>
        /// <param name="oldKey"></param>
        /// <param name="newKey"></param>
        public void RenameKey(string oldKey, string newKey, int index = 0)
        {
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                client.Db = index;
                client.RenameKey(oldKey, newKey);
            }
        }

        /// <summary>
        /// 清除指定数据库的所有数据。
        /// </summary>
        /// <param name="index"></param>
        public void FlushDb(long index)
        {
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                client.Db = index;
                client.FlushDb();
            }
        }

        /// <summary>
        /// 清除所有数据库的所有数据。
        /// </summary>
        public void FlushAll()
        {
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                client.FlushAll();
            }
        }

        /// <summary>
        /// 仅在缓存为空时在指定的缓存密钥中添加一个新项到缓存中。
        /// </summary>
        /// <typeparam name="T">添加的值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiresIn">数据的生命周期。</param>
        /// <returns></returns>
        public bool Add<T>(string key, T value, TimeSpan expiresIn, int index = 0)
        {
            bool result;
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                client.Db = index;
                result = client.Add<T>(key, value, expiresIn);
            }
            return result;
        }

        /// <summary>
        /// 仅在缓存为空时在指定的缓存密钥中添加一个新项到缓存中。
        /// </summary>
        /// <typeparam name="T">添加的值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiresAt">数据的生命周期。</param>
        /// <returns></returns>
        public bool Add<T>(string key, T value, DateTime expiresAt, int index = 0)
        {
            bool result;
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                client.Db = index;
                result = client.Add<T>(key, value, expiresAt);
            }
            return result;
        }

        /// <summary>
        /// 仅在缓存为空时在指定的缓存密钥中添加一个新项到缓存中。
        /// </summary>
        /// <typeparam name="T">添加的值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param> 
        /// <returns></returns>
        public bool Add<T>(string key, T value, int index = 0)
        {
            bool result;
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                client.Db = index;
                result = client.Add<T>(key, value);
            }
            return result;
        }

        /// <summary>
        /// 按给定的数值递增指定键的值。操作是原子的, 并在服务器上发生。 一个不存在的键值从0开始。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="amount">递增的数量</param>
        /// <returns></returns>
        public long Increment(string key, uint amount, int index = 0)
        {
            long result;
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                client.Db = index;
                result = client.Increment(key, amount);
            }
            return result;
        }

        /// <summary>
        /// 按给定的数值递减指定键的值。操作是原子的, 并在服务器上发生。 一个不存在的键值从0开始。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="amount">递减的数量</param>
        /// <returns></returns>
        public long Decrement(string key, uint amount, int index = 0)
        {
            long result;
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                client.Db = index;
                result = client.Decrement(key, amount);
            }
            return result;
        }

        /// <summary>
        /// 获取指定键的值（字符串形式）。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValue Get(string key, int index = 0)
        {
            RedisValue redisValue;
            using (IRedisClient readOnlyClient = base.pooledRedisClientManager.GetReadOnlyClient())
            {
                redisValue = new RedisValue();

                readOnlyClient.Db = index;
                redisValue.TTL = readOnlyClient.GetTimeToLive(key);
                redisValue.Value = readOnlyClient.GetValue(key);
            }
            return redisValue;
        }

        /// <summary>
        /// 获取指定键的值。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <returns></returns>
        public T Get<T>(string key, int index = 0)
        {
            T result;
            using (IRedisClient readOnlyClient = base.pooledRedisClientManager.GetReadOnlyClient())
            {
                readOnlyClient.Db = index;
                result = readOnlyClient.Get<T>(key);
            }
            return result;
        }

        /// <summary>
        /// 获取多个键的值。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="keys">表示多个键的集合。</param>
        /// <returns></returns>
        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys, int index = 0)
        {
            IDictionary<string, T> all;
            using (IRedisClient readOnlyClient = base.pooledRedisClientManager.GetReadOnlyClient())
            {
                readOnlyClient.Db = index;
                all = readOnlyClient.GetAll<T>(keys);
            }
            return all;
        }

        /// <summary>
        /// 移除一个键/值。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns></returns>
        public bool Remove(string key, int index = 0)
        {
            bool result;
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                client.Db = index;
                result = client.Remove(key);
            }
            return result;
        }

        /// <summary>
        /// 移除多个键/值。
        /// </summary>
        /// <param name="keys">表示多个键的集合。</param>
        public void RemoveAll(IEnumerable<string> keys, int index = 0)
        {
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                client.Db = index;
                client.RemoveAll(keys);
            }
        }

        /// <summary>
        /// 仅当某个项存在于该位置时，才将该项目替换为指定的cachekey。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiresIn">数据的生命周期。</param>
        /// <returns></returns>
        public bool Replace<T>(string key, T value, TimeSpan expiresIn, int index = 0)
        {
            bool result;
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                client.Db = index;
                result = client.Replace<T>(key, value, expiresIn);
            }
            return result;
        }

        /// <summary>
        /// 仅当某个项存在于该位置时，才将该项目替换为指定的cachekey。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiresAt">数据的生命周期。</param>
        /// <returns></returns>
        public bool Replace<T>(string key, T value, DateTime expiresAt, int index = 0)
        {
            bool result;
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                client.Db = index;
                result = client.Replace<T>(key, value, expiresAt);
            }
            return result;
        }

        /// <summary>
        /// 仅当某个项存在于该位置时，才将该项目替换为指定的cachekey。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <returns></returns>
        public bool Replace<T>(string key, T value, int index = 0)
        {
            bool result;
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                client.Db = index;
                result = client.Replace<T>(key, value);
            }
            return result;
        }

        /// <summary>
        /// 在指定的缓存键中设置一个项目，不管它是否已经存在。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiresIn">数据的生命周期。</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, TimeSpan expiresIn, int index = 0)
        {
            bool result;
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                if (value == null)
                {
                    result = false;
                }
                else
                {
                    client.Db = index;
                    result = client.Set<T>(key, value, expiresIn);
                }
            }
            return result;
        }

        /// <summary>
        /// 在指定的缓存键中设置一个项目，不管它是否已经存在。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiresAt">数据的生命周期。</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, DateTime expiresAt, int index = 0)
        {
            bool result;
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                if (value == null)
                {
                    result = false;
                }
                else
                {
                    client.Db = index;
                    result = client.Set<T>(key, value, expiresAt);
                }
            }
            return result;
        }

        /// <summary>
        /// 在指定的缓存键中设置一个项目，不管它是否已经存在。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, int index = 0)
        {
            bool result;
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                if (value == null)
                {
                    result = false;
                }
                else
                {
                    client.Db = index;
                    result = client.Set<T>(key, value);
                }
            }
            return result;
        }

        /// <summary>
        /// 将多个值设置到缓存中。
        /// </summary>
        /// <typeparam name="T">值得数据类型。</typeparam>
        /// <param name="values">以“键-值”形式表示的多个数据的集合。</param>
        public void SetAll<T>(IDictionary<string, T> values, int index = 0)
        {
            using (IRedisClient client = base.pooledRedisClientManager.GetClient())
            {
                if (values.Count != 0)
                {
                    client.Db = index;
                    client.SetAll<T>(values);
                }
            }
        }
        #endregion
    }
}
