using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public enum PlayerLeaveRoomCause
    {
        ROOM_CHANGE,
        ROOM_DISCONNECTED_CLIENT,
        CLIENT_REQUESTED_TO_LEAVE,
        CLIENT_DISCONNECTED,
        UNKNOWN,
    }
}
