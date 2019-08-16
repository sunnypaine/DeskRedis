using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskRedis.Model
{
    public class RedisValue
    {
        public TimeSpan? TTL { get; set; }

        public string Value { get; set; }
    }
}
