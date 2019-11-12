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
        private ServerController server = null;

        private string room_name = "";
        private string room_id = "";

        private List<RoomPlayer> connected_players = new List<RoomPlayer>();

        public void Init(ServerController server, string room_name, string room_id)
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

        public void PlayerConnect(int client_id, Action on_success, Action<ServerControllerError> on_error)
        {
            RoomPlayer player = new RoomPlayer();
            player.Init(server, client_id);

            lock (connected_players)
            {
                Task.Factory.StartNew(() => OnPlayerWantsToConnect(player)).
                ContinueWith(delegate (Task<bool> player_wants_to_connect_task)
                {
                    if (player_wants_to_connect_task.Result)
                    {                    
                        if (connected_players.Count == 0)
                        {
                            Task.Factory.StartNew(() => OnRoomOpened()).
                            ContinueWith(delegate (Task room_opened_task)
                            {
                                connected_players.Add(player);

                                Task.Factory.StartNew(() => OnPlayerConnected(player)).
                                ContinueWith(delegate (Task player_connected_task)
                                {
                                    if (on_success != null)
                                        on_success.Invoke();
                                });
                            });
                        }
                        else
                        {
                            Task.Factory.StartNew(() => OnPlayerConnected(player)).
                            ContinueWith(delegate (Task player_connected_task)
                            {
                                if (on_success != null)
                                    on_success.Invoke();
                            });
                        }
                    }
                    else
                    {
                        if (on_error != null)
                            on_error.Invoke(ServerControllerError.EMPTY);
                    }
                });
            }
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

        protected virtual bool OnPlayerWantsToConnect(RoomPlayer player)
        {
            return false;
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
