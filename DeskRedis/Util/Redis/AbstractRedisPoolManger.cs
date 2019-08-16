using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskRedis.Util.Redis
{
    public abstract class AbstractRedisPoolManger : IDisposable
    {
        #region 私有变量
        /// <summary>
        /// Redis客户端连接池管理器
        /// </summary>
        protected readonly PooledRedisClientManager pooledRedisClientManager;
        #endregion


        #region 构造方法
        /// <summary>
        /// 使用指定的参数创建实例。
        /// </summary>
        /// <param name="readWriteHosts">可读写的Redis主机地址。</param>
        /// <param name="readOnlyHosts">只读的Redis主机地址。</param>
        /// <exception cref="ArgumentException">Redis主机地址不合法时。</exception>
        protected AbstractRedisPoolManger(string[] readWriteHosts, string[] readOnlyHosts)
        {
            //WriteServerList：可写的Redis链接地址。  
            //ReadServerList：可读的Redis链接地址。  
            //MaxWritePoolSize：最大写链接数。  
            //MaxReadPoolSize：最大读链接数。  
            //AutoStart：自动重启。  
            //LocalCacheTime：本地缓存到期时间，单位:秒。  
            //RecordeLog：是否记录日志,该设置仅用于排查redis运行时出现的问题,如redis工作正常,请关闭该项。  
            //RedisConfigInfo类是记录redis连接信息，此信息和配置文件中的RedisConfig相呼应  

            this.ValidateHosts(readWriteHosts);
            this.ValidateHosts(readOnlyHosts);
            // 支持读写分离，均衡负载   
            this.pooledRedisClientManager = new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
            {
                MaxWritePoolSize = 5, // “写”链接池链接数   
                MaxReadPoolSize = 5, // “读”链接池链接数   
                AutoStart = true
            });
        }
        #endregion


        #region 私有方法
        /// <summary>
        /// 解析Redis主机地址。
        /// </summary>
        /// <param name="hosts">Redis主机地址。</param>
        /// <returns></returns>
        private void ValidateHosts(string[] hosts)
        {
            foreach (string item in hosts)
            {
                if (item.Contains(":") == false)
                {
                    throw new ArgumentException("给定的Redis主机地址不合法。请使用合法地址。如“127.0.0.1:6379”，或者具有身份校验的“123456@127.0.0.1:6379”。");
                }
            }
        }
        #endregion


        #region 公共方法，IDispose成员
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.pooledRedisClientManager.Dispose();
        }
        #endregion
    }
}
