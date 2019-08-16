using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskRedis.Model.Configs
{
    public class ConnectionConfig
    {
        /// <summary>
        /// 配置id。唯一。
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 连接名。唯一。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// IP地址。
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 端口。默认6379
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 密码。可为空。
        /// </summary>
        public string Password { get; set; }
    }
}
