using System;

namespace Fast.Networking
{
    [System.Serializable]
    class CreatePlayerResponseMessage : ServerControllerMessage
    {
        private bool success = false;

        public CreatePlayerResponseMessage(bool success) : base(ServerControllerMessageType.CREATE_PLAYER_RESPONSE)
        {
            this.success = success;
        }

        public bool Success
        {
            get { return success; }
        }
    }
}
