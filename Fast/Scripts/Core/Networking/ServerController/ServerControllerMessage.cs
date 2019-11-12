using System;

namespace Fast.Networking
{
    [System.Serializable]
    public class ServerControllerMessage
    {
        private ServerControllerMessageType type = new ServerControllerMessageType();

        public ServerControllerMessage(ServerControllerMessageType type)
        {
            this.type = type;
        }

        public ServerControllerMessageType Type
        {
            get { return type; }
        }
    }
}
