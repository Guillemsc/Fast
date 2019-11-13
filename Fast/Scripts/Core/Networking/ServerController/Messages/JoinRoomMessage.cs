using System;

namespace Fast.Networking
{
    [System.Serializable]
    public class JoinRoomMessage : ServerControllerMessage
    {
        private string room_id = "";
        private object join_data = null;

        public JoinRoomMessage(string room_id, object join_data) : base(ServerControllerMessageType.JOIN_ROOM)
        {
            this.room_id = room_id;
            this.join_data = join_data;
        }

        public string RoomId
        {
            get { return room_id; }
        }

        public object JoinData
        {
            get { return join_data; }
        }
    }
}
