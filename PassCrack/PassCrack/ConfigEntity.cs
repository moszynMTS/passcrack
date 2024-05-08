using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassCrack.Host
{
    public class ConfigEntity
    {
        public int ClientsCount { get; set; } = 1;
        public int Method { get; set; } = 2;
        public int Hash { get; set; } = 1;
        public string IP { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 5000;
        public string WordListPath { get; set; }
        public string CharacterKeys { get; set; } = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789,./;'[]\\-=<>?:\"{}|_+!@#$%^&*() ";
        public int PackageSize { get; set; } = 2000;
    }
}
