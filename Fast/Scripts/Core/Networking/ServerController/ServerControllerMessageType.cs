using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public enum ServerControllerMessageType
    {
        DATA,

        CREATE_PLAYER,
        CREATE_PLAYER_RESPONSE,

        CREATE_ROOM,
        CREATE_ROOM_RESPONSE,

        JOIN_ROOM,
        JOIN_ROOM_RESPONSE,

        CREATE_JOIN_ROOM,
        CREATE_JOIN_ROOM_RESPONSE,

        LEAVE_ROOM,

        DISCONNECTED_FROM_ROOM,
    }
}
