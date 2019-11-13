using System;

namespace Fast.Networking
{
    [System.Serializable]
    class CreatePlayerMessage : ServerControllerMessage
    {
        private object join_data = null;

        public CreatePlayerMessage(object join_data) : base(ServerControllerMessageType.CREATE_PLAYER)
        {
            this.join_data = join_data;
        }

        public object JoinData
        {
            get { return join_data; }
        }
    }
}
