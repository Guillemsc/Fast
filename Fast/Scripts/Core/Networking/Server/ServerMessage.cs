using System;

namespace Fast.Networking
{
    public class ServerMessage
    {
        private int client_id = 0;
        private byte[] data = null;

        public ServerMessage(int client_id, byte[] data)
        {
            this.client_id = client_id;
            this.data = data;
        }

        public int ClientId
        {
            get { return client_id; }
        }

        public byte[] Data
        {
            get { return data; }
        }
    }
}
