using System;

namespace Fast.Networking
{
    class DisconnectedFromRoomMessage : Message
    {
        public DisconnectedFromRoomMessage() : base(MessageType.DISCONNECTED_FROM_ROOM)
        {

        }
    }
}
