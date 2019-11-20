using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskRedis.Enums
{
    /// <summary>
    /// 节点类型
    /// </summary>
    public enum NodeType
    {
        /// <summary>
        /// 根节点，连接名
        /// </summary>
        Connection,
        /// <summary>
        /// 二级节点，数据库
        /// </summary>
        DataBase,
        /// <summary>
        /// 三级或四级节点，键
        /// </summary>
        Key,
        /// <summary>
        /// 三级或四级节点，键文件夹
        /// </summary>
        Folder
    }
}
