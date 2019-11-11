using System;

namespace Fast.Networking
{
    [System.Serializable]
    public class DataMessage : Message
    {
        private object message_obj = null;

        public DataMessage(object message_obj) : base(MessageType.DATA)
        {
            this.message_obj = message_obj;
        }

        public object MessageObj
        {
            get { return message_obj; }
        }
    }
}
