using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    [System.Serializable]
    public class Message
    {
        private MessageType type = new MessageType();

        public Message(MessageType type)
        {
            this.type = type;
        }

        public MessageType Type
        {
            get { return type; }
        }
    }
}
