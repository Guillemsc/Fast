using System;

namespace Fast.Networking
{
    [System.Serializable]
    public class CreateJoinRoomMessage : ServerControllerMessage
    {
        private string room_name = "";
        private string room_id = "";
        private object join_data_object = null;

        public CreateJoinRoomMessage(string room_name, string room_id, object join_data_object) : base(ServerControllerMessageType.CREATE_JOIN_ROOM)
        {
            this.room_name = room_name;
            this.room_id = room_id;
            this.join_data_object = join_data_object;
        }

        public string RoomName
        {
            get { return room_name; }
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
