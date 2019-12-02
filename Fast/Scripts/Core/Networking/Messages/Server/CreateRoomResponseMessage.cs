using System;

namespace Fast.Networking
{
    [System.Serializable]
    public class CreateRoomResponseMessage : ServerControllerMessage
    {
        private bool success = false;
        private string room_id = "";

        private ServerControllerError error = ServerControllerError.EMPTY;

        public CreateRoomResponseMessage(bool success, string room_id) : base(ServerControllerMessageType.CREATE_ROOM_RESPONSE)
        {
            this.success = success;
            this.room_id = room_id;
        }

        public CreateRoomResponseMessage(bool success, ServerControllerError error) : base(ServerControllerMessageType.CREATE_ROOM_RESPONSE)
        {
            this.success = success;
            this.error = error;
        }

        public bool Success
        {
            get { return success; }
        }

        public string RoomId
        {
            get { return room_id; }
        }

        public ServerControllerError Error
        {
            get { return error; }
        }
    }
}
