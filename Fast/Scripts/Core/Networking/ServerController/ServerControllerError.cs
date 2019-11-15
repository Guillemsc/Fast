using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public enum ServerControllerError
    {
        EMPTY,

        PLAYER_IS_NOT_CONNECTED,
        ROOM_DOES_NOT_EXIST,
        ROOM_TYPE_DOES_NOT_EXIST,
        ROOM_ALREADY_EXIST,

        CONNECTION_TO_ROOM_DENIED,
        ROOM_EXCEPTION,
    }

    public class ServerControllerErrorToString
    {
        public static string Get(ServerControllerError error)
        {
            string ret = "";

            switch (error)
            {
                case ServerControllerError.EMPTY:
                    {
                        break;
                    }
            }

            return ret;
        }
    }
}
