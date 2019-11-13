using System;

namespace Fast.Networking
{
    [System.Serializable]
    public class JoinRoomMessage : ServerControllerMessage
    {
        private string room_id = "";
        private object join_data_object = null;

        public JoinRoomMessage(string room_id, object join_data_object) : base(ServerControllerMessageType.JOIN_ROOM)
        {
            this.room_id = room_id;
            this.join_data_object = join_data_object;
        }

        public string RoomId
        {
            get { return room_id; }
        }

        public object JoinDataObject
        {
            get { return join_data_object; }
        }
    }
}
