using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public class RoomPlayer
    {
        private RoomsServerModule rooms_module = null;
        private Player player = null;

        private object join_data = null;

        public void Init(RoomsServerModule rooms_module, Player player, object join_data)
        {
            this.rooms_module = rooms_module;
            this.player = player;
            this.join_data = join_data;
        }

        public Player Player
        {
            get { return player; }
        }

        public object JoinData
        {
            get { return join_data; }
        }

        public void SendMessage(object obj_to_send)
        {
            if(rooms_module != null && player != null)
            {
                rooms_module.PlayerSendMessage(player, obj_to_send);
            }
        }

        public void Disconnect()
        {
            if (rooms_module != null && player != null)
            {
                rooms_module.PlayerLeaveRoom(player, PlayerLeaveRoomCause.ROOM_DISCONNECTED_CLIENT);
            }
        }
    }
}
