using System;

namespace Fast.Networking
{
    [System.Serializable]
    public class LeaveRoomMessage : ServerControllerMessage
    {
        public LeaveRoomMessage() : base(ServerControllerMessageType.LEAVE_ROOM)
        {

        }
    }
}
