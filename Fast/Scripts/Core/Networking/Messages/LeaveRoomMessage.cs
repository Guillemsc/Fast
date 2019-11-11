using System;

namespace Fast.Networking
{
    class LeaveRoomMessage : Message
    {
        public LeaveRoomMessage() : base(MessageType.LEAVE_ROOM)
        {

        }
    }
}
