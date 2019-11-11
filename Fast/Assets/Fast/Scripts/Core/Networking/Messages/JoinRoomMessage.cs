using System;

namespace Fast.Networking
{
    [System.Serializable]
    class JoinRoomMessage : Message
    {
        private string room_id = "";

        public JoinRoomMessage(string room_id) : base(MessageType.JOIN_ROOM)
        {
            this.room_id = room_id;
        }

        public string RoomId
        {
            get { return room_id; }
        }
    }
}
