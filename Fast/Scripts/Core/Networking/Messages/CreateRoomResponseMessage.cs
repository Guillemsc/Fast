using System;

namespace Fast.Networking
{
    [System.Serializable]
    class CreateRoomResponseMessage : Message
    {
        private bool success = false;
        private string room_id = "";

        public CreateRoomResponseMessage(bool success, string room_id) : base(MessageType.CREATE_ROOM_RESPONSE)
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
