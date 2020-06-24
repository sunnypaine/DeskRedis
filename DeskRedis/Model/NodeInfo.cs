using DeskRedis.Enums;
using DeskRedis.Model.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskRedis.Model
{
    public class NodeInfo
    {
        /// <summary>
        /// 配置id
        /// </summary>
        public string ConfigId { get; set; }

        /// <summary>
        /// 键全名
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 数据库索引
        /// </summary>
        public int DbIndex { get; set; }

        /// <summary>
        /// 菜单选项类型
        /// </summary>
        public MenuItemType Type { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        public NodeType NodeType { get; set; }
    }
}
