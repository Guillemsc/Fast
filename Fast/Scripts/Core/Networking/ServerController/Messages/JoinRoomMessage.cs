using System;

namespace Fast.Networking
{
    [System.Serializable]
    public class JoinRoomMessage : ServerControllerMessage
    {
        private string room_id = "";

        public JoinRoomMessage(string room_id) : base(ServerControllerMessageType.JOIN_ROOM)
        {
            this.room_id = room_id;
        }

        public string RoomId
        {
            get { return room_id; }
        }
    }
}
