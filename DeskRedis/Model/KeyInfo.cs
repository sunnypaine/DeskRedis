using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskRedis.Model
{
    public class KeyInfo
    {
        public string Header { get; set; }

        public string Key { get; set; }

        public bool IsKey { get; set; }

        public List<KeyInfo> Keys { get; set; }
    }
}
