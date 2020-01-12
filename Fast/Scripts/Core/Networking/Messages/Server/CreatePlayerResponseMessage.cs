using System;

namespace Fast.Networking
{
    [System.Serializable]
    class CreatePlayerResponseMessage : ServerControllerMessage
    {
        private bool success = false;
        private int client_id = 0;

        public CreatePlayerResponseMessage(bool success, int client_id) : base(ServerControllerMessageType.CREATE_PLAYER_RESPONSE)
        {
            this.success = success;
            this.client_id = client_id;
        }

        public bool Success
        {
            get { return success; }
        }

        public int ClientId
        {
            get { return client_id; }
        }
    }
}
