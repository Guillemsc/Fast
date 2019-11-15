using System;

namespace Fast.Networking
{
    [System.Serializable]
    public class RoomMessage : ServerControllerMessage
    {
        private object message_obj = null;

        public RoomMessage(object message_obj) : base(ServerControllerMessageType.ROOM_MESSAGE)
        {
            this.message_obj = message_obj;
        }

        public object MessageObj
        {
            get { return message_obj; }
        }
    }
}
