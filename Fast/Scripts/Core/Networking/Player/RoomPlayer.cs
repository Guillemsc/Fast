using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public class RoomPlayer
    {
        private ServerController server = null;
        private int client_id = 0;

        public void Init(ServerController server, int client_id)
        {
            this.server = server;
            this.client_id = client_id;
        }

        public int ClientId
        {
            get { return client_id; }
        }

        public void SendMessage(object obj_to_send)
        {
            if(server != null)
            {
                server.SendDataMessage(client_id, obj_to_send);
            }
        }

        public void Disconnect()
        {
            if (server != null)
            {
                server.PlayerLeaveRoom(client_id);
            }
        }
    }
}
