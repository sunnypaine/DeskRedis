using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskRedis.Util
{
    public static class JsonConfigUtil
    {
        public static T GetConfigObject<T>(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string json = sr.ReadToEnd();
            sr.Close();

            T connections = JsonConvert.DeserializeObject<T>(json);
            return connections;
        }

        public static void SetConfigObject<T>(string path, T obj)
        {
            string connections = JsonConvert.SerializeObject(obj,Formatting.Indented);

            StreamWriter sw = new StreamWriter(path, false, Encoding.Default);
            sw.Write(connections);
            sw.Flush();
            sw.Close();
        }
    }
}
