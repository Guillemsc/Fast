using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    class Logger
    {
        public static void ServerLogInfo(string to_log)
        {
            Console.WriteLine("[Fast.Networking] SERVER: " + to_log);
        }
    }
}
