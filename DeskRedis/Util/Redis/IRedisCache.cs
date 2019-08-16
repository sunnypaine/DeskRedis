using DeskRedis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskRedis.Util.Redis
{
    public interface IRedisCache
    {
        /// <summary>
        /// 测试连接。
        /// </summary>
        /// <returns>SUCCESS:成功。其它内容：失败信息。</returns>
        string ConnectTest();

        /// <summary>
        /// 获取数据库个数。
        /// </summary>
        /// <returns></returns>
        int GetDataBaseCount();

        /// <summary>
        /// 获取所有的键。
        /// </summary>
        /// <param name="index">数据库索引</param>
        /// <returns></returns>
        List<string> GetAllKeys(int index = 0);

        /// <summary>
        /// 获取数据库数量。
        /// </summary>
        /// <returns></returns>
        long GetKeyCount(int index = 0);

        /// <summary>
        /// 重命名键。
        /// </summary>
        /// <param name="oldKey"></param>
        /// <param name="newKey"></param>
        void RenameKey(string oldKey, string newKey, int index = 0);

        /// <summary>
        /// 清除指定数据库的所有数据。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        void FlushDb(long index);

        /// <summary>
        /// 清除所有数据库的所有数据。
        /// </summary>
        /// <returns></returns>
        void FlushAll();

        /// <summary>
        /// 仅在缓存为空时在指定的缓存密钥中添加一个新项到缓存中。
        /// </summary>
        /// <typeparam name="T">添加的值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <returns></returns>
        bool Add<T>(string key, T value, int index = 0);

        /// <summary>
        /// 仅在缓存为空时在指定的缓存密钥中添加一个新项到缓存中。
        /// </summary>
        /// <typeparam name="T">添加的值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiresAt">数据的生命周期。</param>
        /// <returns></returns>
        bool Add<T>(string key, T value, DateTime expiresAt, int index = 0);

        /// <summary>
        /// 仅在缓存为空时在指定的缓存密钥中添加一个新项到缓存中。
        /// </summary>
        /// <typeparam name="T">添加的值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param> 
        /// <param name="expiresIn">数据的生命周期。</param>
        /// <returns></returns>
        bool Add<T>(string key, T value, TimeSpan expiresIn, int index = 0);

        /// <summary>
        /// 按给定的数值递增指定键的值。操作是原子的, 并在服务器上发生。 一个不存在的键值从0开始。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="amount">递增的数量</param>
        /// <returns></returns>
        long Increment(string key, uint amount, int index = 0);

        /// <summary>
        /// 按给定的数值递减指定键的值。操作是原子的, 并在服务器上发生。 一个不存在的键值从0开始。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="amount">递减的数量</param>
        /// <returns></returns>
        long Decrement(string key, uint amount, int index = 0);

        /// <summary>
        /// 获取指定键的值（字符串形式）。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        RedisValue Get(string key, int index = 0);

        /// <summary>
        /// 获取指定键的值。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <returns></returns>
        T Get<T>(string key, int index = 0);

        /// <summary>
        /// 获取多个键的值。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="keys">表示多个键的集合。</param>
        /// <returns></returns>
        IDictionary<string, T> GetAll<T>(IEnumerable<string> keys, int index = 0);

        /// <summary>
        /// 移除一个键/值。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>如果移除成功，则返回true。如果指定键在Redis中不存在或移除失败，则返回false。</returns>
        bool Remove(string key, int index = 0);

        /// <summary>
        /// 移除多个键/值。
        /// </summary>
        /// <param name="keys">表示多个键的集合。</param>
        void RemoveAll(IEnumerable<string> keys, int index = 0);

        /// <summary>
        /// 仅当某个项存在于该位置时，才将该项目替换为指定的cachekey。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <returns></returns>
        bool Replace<T>(string key, T value, int index = 0);

        /// <summary>
        /// 仅当某个项存在于该位置时，才将该项目替换为指定的cachekey。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiresAt">数据的生命周期。</param>
        /// <returns></returns>
        bool Replace<T>(string key, T value, DateTime expiresAt, int index = 0);

        /// <summary>
        /// 仅当某个项存在于该位置时，才将该项目替换为指定的cachekey。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiresIn">数据的生命周期。</param>
        /// <returns></returns>
        bool Replace<T>(string key, T value, TimeSpan expiresIn, int index = 0);

        /// <summary>
        /// 在指定的缓存键中设置一个项目，不管它是否已经存在。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <returns></returns>
        bool Set<T>(string key, T value, int index = 0);

        /// <summary>
        /// 在指定的缓存键中设置一个项目，不管它是否已经存在。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiresAt">数据的生命周期。</param>
        /// <returns></returns>
        bool Set<T>(string key, T value, DateTime expiresAt, int index = 0);

        /// <summary>
        /// 在指定的缓存键中设置一个项目，不管它是否已经存在。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiresIn">数据的生命周期。</param>
        /// <returns></returns>
        bool Set<T>(string key, T value, TimeSpan expiresIn, int index = 0);

        /// <summary>
        /// 将多个值设置到缓存中。
        /// </summary>
        /// <typeparam name="T">值得数据类型。</typeparam>
        /// <param name="values">以“键-值”形式表示的多个数据的集合。</param>
        void SetAll<T>(IDictionary<string, T> values, int index = 0);
    }
}
