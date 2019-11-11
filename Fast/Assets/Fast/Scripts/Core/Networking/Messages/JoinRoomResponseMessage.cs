using System;

namespace Fast.Networking
{
    [System.Serializable]
    class JoinRoomResponseMessage : Message
    {
        private bool success = false;
        private string room_id = "";

        public JoinRoomResponseMessage(bool success, string room_id) : base(MessageType.JOIN_ROOM_RESPONSE)
        {
            this.success = success;
            this.room_id = room_id;
        }

        public bool Success
        {
            get { return success; }
        }

        public string RoomId
        {
            get { return room_id; }
        }
    }
}
