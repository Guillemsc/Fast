using System;

namespace Fast.Networking
{
    public class LeaveRoomMessage : ServerControllerMessage
    {
        public LeaveRoomMessage() : base(ServerControllerMessageType.LEAVE_ROOM)
        {

        }
    }
}
