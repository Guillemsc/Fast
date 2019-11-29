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
            string log = "[Fast.Networking] SERVER INFO: " + to_log;

            UnityEngine.Debug.Log(log);
        }

        public static void ServerLogError(string to_log)
        {
            string log = "[Fast.Networking] SERVER ERROR: " + to_log;

            UnityEngine.Debug.Log(log);
        }

        public static void ClientLogInfo(string to_log)
        {
            string log = "[Fast.Networking] CLIENT: " + to_log;
;
            UnityEngine.Debug.Log(log);
        }

        public static void RoomLogInfo(BaseRoom room, string to_log)
        {
            if (room != null)
            {
                string log = "[Fast.Networking] ROOM " + room.ToString() + " " + to_log;

                UnityEngine.Debug.Log(log);
            }
        }
    }
}
