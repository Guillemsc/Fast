using System;

namespace Fast.Networking
{
    [System.Serializable]
    public class DisconnectedFromRoomMessage : ServerControllerMessage
    {
        public DisconnectedFromRoomMessage() : base(ServerControllerMessageType.DISCONNECTED_FROM_ROOM)
        {

        }
    }
}
