using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskRedis.Model
{
    public class KeyInfo
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 键的名称。
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 是否是键。true：是键，false：文件夹。
        /// </summary>
        public bool IsKey { get; set; }

        public List<KeyInfo> Keys { get; set; }
    }
}
