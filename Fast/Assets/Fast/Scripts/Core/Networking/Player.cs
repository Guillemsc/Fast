using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public class Player
    {
        private int client_id = 0;

        private bool connected_to_room = false;
        private string room_id = "";

        public Player(int client_id)
        {
            this.client_id = client_id;
        }

        public int ClientId
        {
            get { return client_id; }
        }

        public bool ConnectedToRoom
        {
            get { return connected_to_room; }
            set { connected_to_room = value; }
        }

        public string RoomId
        {
            get { return room_id; }
            set { room_id = value; }
        }
    }
}
