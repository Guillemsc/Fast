using System;

namespace Fast.Networking
{
    public class DisconnectedFromRoomMessage : ServerControllerMessage
    {
        public DisconnectedFromRoomMessage() : base(ServerControllerMessageType.DISCONNECTED_FROM_ROOM)
        {

        }
    }
}
