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

        private object join_data_object = null;

        public void Init(ServerController server, int client_id, object join_data_object)
        {
            this.server = server;
            this.client_id = client_id;
            this.join_data_object = join_data_object;
        }

        public int ClientId
        {
            get { return client_id; }
        }

        public object JoinDataObject
        {
            get { return join_data_object; }
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
                server.PlayerLeaveRoom(client_id, PlayerLeaveRoomCause.ROOM_DISCONNECTED_CLIENT);
            }
        }
    }
}
