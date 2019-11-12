﻿using System;

namespace Fast.Networking
{
    [System.Serializable]
    public class CreateRoomMessage : ServerControllerMessage
    {
        private string room_name = "";
        private string room_id = "";

        public CreateRoomMessage(string room_name, string room_id) : base(ServerControllerMessageType.CREATE_ROOM)
        {
            this.room_name = room_name;
            this.room_id = room_id;
        }

        public string RoomName
        {
            get { return room_name; }
        }

        public string RoomId
        {
            get { return room_id; }
        }
    }
}