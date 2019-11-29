using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public enum ServerControllerMessageType
    {
        CREATE_PLAYER,
        CREATE_PLAYER_RESPONSE,

        CREATE_ROOM,
        CREATE_ROOM_RESPONSE,

        JOIN_ROOM,
        JOIN_ROOM_RESPONSE,

        CREATE_JOIN_ROOM,
        CREATE_JOIN_ROOM_RESPONSE,

        LEAVE_ROOM,

        ROOM_MESSAGE,

        DISCONNECTED_FROM_ROOM,

        RETRIEVE_ROOMS_LIST,
    }
}
