using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RoomName : Attribute
    {
        private string name = "";

        public RoomName(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }
    }

    public class Room
    {
        private Server server = null;

        private string room_name = "";
        private string room_id = "";

        private List<RoomPlayer> connected_players = new List<RoomPlayer>();

        public void Init(Server server, string room_name, string room_id)
        {
            this.server = server;
            this.room_name = room_name;
            this.room_id = room_id;
        }

        public string RoomName
        {
            get { return room_name; }
        }

        public string RoomId
        {
            get { return room_id; }
        }

        public void PlayerConnect(int client_id)
        {
            RoomPlayer player = new RoomPlayer();
            player.Init(server, client_id);

            lock (connected_players)
            {
                if(connected_players.Count == 0)
                {
                    OnRoomOpened();
                }

                connected_players.Add(player);
            }

            OnPlayerConnected(player);
        }

        public void PlayerDisconnect(int client_id)
        {
            lock (connected_players)
            {
                for (int i = 0; i < connected_players.Count; ++i)
                {
                    RoomPlayer curr_player = connected_players[i];

                    if(curr_player.ClientId == client_id)
                    {
                        connected_players.RemoveAt(i);

                        OnPlayerDisconnected(curr_player);

                        break;
                    }
                }

                if(connected_players.Count == 0)
                { 
                    OnRoomClosed();
                }
            }
        }

        public int ConnectedPlayersCount
        {
            get { return connected_players.Count; }
        }

        public RoomPlayer GetConnectedPlayer(int client_id)
        {
            RoomPlayer ret = null;

            lock (connected_players)
            {
                for (int i = 0; i < connected_players.Count; ++i)
                {
                    RoomPlayer curr_player = connected_players[i];

                    if (curr_player.ClientId == client_id)
                    {
                        ret = curr_player;

                        break;
                    }
                }
            }

            return ret;
        }

        public void MessageReceived(int client_id, object message_obj)
        {
            RoomPlayer player = GetConnectedPlayer(client_id);

            if(player != null)
            {
                OnMessageReceived(player, message_obj);
            }
        }

        protected virtual void OnRoomOpened()
        {

        }

        protected virtual void OnPlayerConnected(RoomPlayer player)
        {

        }

        protected virtual void OnPlayerDisconnected(RoomPlayer player)
        {

        }

        protected virtual void OnRoomClosed()
        {

        }

        protected virtual void OnMessageReceived(RoomPlayer player, object message_obj)
        {

        }
    }
}
