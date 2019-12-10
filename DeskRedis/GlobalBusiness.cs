using DeskRedis.Exceptions;
using DeskRedis.Model.Configs;
using DeskRedis.Util;
using DeskRedis.Util.Redis;
using System.Collections.Generic;
using System.Linq;

namespace DeskRedis
{
    public static class GlobalBusiness
    {
        #region 私有变量
        #endregion


        #region 公共属性
        public static string PathConnections { get; set; }

        public static IDictionary<string, ConnectionConfig> DictConnectionConfig { get; set; }

        /// <summary>
        /// Redis缓存对象。
        /// </summary>
        public static IDictionary<string, IRedisCache> RedisCaches { get; }
        #endregion


        #region 构造方法
        static GlobalBusiness()
        {
            PathConnections = "connections.json";

            DictConnectionConfig = new Dictionary<string, ConnectionConfig>();
            RedisCaches = new Dictionary<string, IRedisCache>();
            List<ConnectionConfig> configs = GetAllConnectionConfig();
            configs.ForEach(p =>
            {
                DictConnectionConfig.Add(p.Id, p);

                string host = (string.IsNullOrEmpty(p.Password) ? "" : $"{p.Password}@") + $"{p.IP}:{p.Port}";
                string[] hosts = new string[] { host };
                RedisCaches.Add(p.Id, new RedisCache(hosts, hosts));
            });
        }
        #endregion


        #region 公共方法
        /// <summary>
        /// 获取所有配置信息
        /// </summary>
        /// <returns></returns>
        public static List<ConnectionConfig> GetAllConnectionConfig()
        {
            List<ConnectionConfig> configs = JsonConfigUtil.GetConfigObject<List<ConnectionConfig>>(PathConnections);
            return configs;
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="id">配置id</param>
        /// <returns></returns>
        public static ConnectionConfig GetConnectionConfig(string id)
        {
            AssertUtil.IsNullOrEmpty("指定的参数 {0}  为空或null。", id);

            List<ConnectionConfig> configs = GetAllConnectionConfig();
            ConnectionConfig config = configs?.Find(p => p.Id.Equals(id));
            return config;
        }

        /// <summary>
        /// 保存配置信息。
        /// </summary>
        /// <param name="config"></param>
        /// <exception cref="DuplicateMemberException">成员重复异常</exception>
        public static void SaveConfig(ConnectionConfig config)
        {
            List<ConnectionConfig> configs = JsonConfigUtil.GetConfigObject<List<ConnectionConfig>>(PathConnections);
            if (configs == null)
            {
                configs = new List<ConnectionConfig>();
            }
            if (configs.Exists(p => p.Name.Equals(config.Name)))
            {
                throw new DuplicateMemberException($"已存在名为 {config.Name} 的配置信息。");
            }
            configs.Add(config);

            JsonConfigUtil.SetConfigObject<List<ConnectionConfig>>(PathConnections, configs);

            DictConnectionConfig.Add(config.Id, config);
            string host = (string.IsNullOrEmpty(config.Password) ? "" : $"{config.Password}@") + $"{config.IP}:{config.Port}";
            string[] hosts = new string[] { host };
            RedisCaches.Add(config.Id, new RedisCache(hosts, hosts));
        }

        public static void UpdateConfig(ConnectionConfig config)
        {
            List<ConnectionConfig> configs = JsonConfigUtil.GetConfigObject<List<ConnectionConfig>>(PathConnections);
            //检查旧名称
            if (configs == null || !configs.Exists(p => p.Id.Equals(config.Id)))
            {
                throw new KeyNotFoundException($"配置文件中已丢失原配置信息。");
            }
            //检查新名称
            List<ConnectionConfig> tmps = configs.Where(p => p.Id.Equals(config.Id) == false).ToList();
            if (tmps != null && tmps.Exists(p => p.Name.Equals(config.Name)))
            {
                throw new DuplicateMemberException($"已存在名为 {config.Name} 的配置信息。");
            }

            ConnectionConfig tmp = configs.Find(p => p.Id.Equals(config.Id));
            tmp.IP = config.IP;
            tmp.Name = config.Name;
            tmp.Password = config.Password;
            tmp.Port = config.Port;

            JsonConfigUtil.SetConfigObject<List<ConnectionConfig>>(PathConnections, configs);
            DictConnectionConfig[config.Id] = config;
            string host = (string.IsNullOrEmpty(config.Password) ? "" : $"{config.Password}@") + $"{config.IP}:{config.Port}";
            string[] hosts = new string[] { host };
            RedisCaches[config.Id]= new RedisCache(hosts, hosts);
        }

        /// <summary>
        /// 移除配置信息。
        /// </summary>
        /// <param name="id"></param>
        public static void RemoveConfig(string id)
        {
            List<ConnectionConfig> configs = JsonConfigUtil.GetConfigObject<List<ConnectionConfig>>(PathConnections);
            int index = configs.FindIndex(p => p.Id.Equals(id));
            configs.RemoveAt(index);
            JsonConfigUtil.SetConfigObject<List<ConnectionConfig>>(PathConnections, configs);

            DictConnectionConfig.Remove(id);
            RedisCaches.Remove(id);
        }
        #endregion
    }
}
