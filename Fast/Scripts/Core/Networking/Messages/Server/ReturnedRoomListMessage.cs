using System;

namespace Fast.Networking
{
    [System.Serializable]
    public class ReturnedRoomListMessage : ServerControllerMessage
    {
        public ReturnedRoomListMessage() : base(ServerControllerMessageType.RETURNED_ROOM_LIST)
        {
            
        }
    }
}
