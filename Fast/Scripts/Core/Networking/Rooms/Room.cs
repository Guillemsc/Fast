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

        public override string ToString()
        {
            return "[Name:" + room_name + "][Id:" + room_id + "][Connected Players:" + connected_players.Count + "]";
        }

        public string RoomName
        {
            get { return room_name; }
        }

        public string RoomId
        {
            get { return room_id; }
        }

        public ServerController ServerController
        {
            get { return server; }
        }

        public void PlayerConnect(int client_id, object join_data, Action on_success, Action<ServerControllerError> on_error)
        {
            RoomPlayer player = new RoomPlayer();
            player.Init(server, client_id, join_data);

            lock (connected_players)
            {
                Task.Factory.StartNew(() => OnPlayerWantsToConnect(player)).
                ContinueWith(delegate (Task<bool> player_wants_to_connect_task)
                {
                    if (player_wants_to_connect_task.IsCompleted && !player_wants_to_connect_task.IsFaulted && 
                    !player_wants_to_connect_task.IsCanceled)
                    {
                        if (player_wants_to_connect_task.Result)
                        {
                            if (connected_players.Count == 0)
                            {
                                Task.Factory.StartNew(() => OnRoomOpened()).
                                ContinueWith(delegate (Task room_opened_task)
                                {
                                    if (room_opened_task.IsCompleted && !room_opened_task.IsFaulted && !room_opened_task.IsCanceled)
                                    {
                                        connected_players.Add(player);

                                        Logger.ServerLogInfo(ToString() + ": Player with id: " + client_id + " connected");

                                        Task.Factory.StartNew(() => OnPlayerConnected(player)).
                                        ContinueWith(delegate (Task player_connected_task)
                                        {
                                            if (player_connected_task.IsFaulted || player_connected_task.IsCanceled)
                                            {
                                                if (player_connected_task.Exception != null)
                                                {
                                                    Logger.ServerLogError(ToString() + " OnPlayerConnected(): " + player_connected_task.Exception);
                                                }
                                                else
                                                {
                                                    Logger.ServerLogError(ToString() + " OnPlayerConnected(): " + "Task faulted or cancelled");
                                                }
                                            }

                                            if (on_success != null)
                                                on_success.Invoke();
                                        });
                                    }
                                    else
                                    {
                                        if (room_opened_task.Exception != null)
                                        {
                                            Logger.ServerLogError(ToString() + " OnRoomOpened(): " + room_opened_task.Exception);
                                        }
                                        else
                                        {
                                            Logger.ServerLogError(ToString() + " OnRoomOpened(): " + "Task faulted or cancelled");
                                        }

                                        if (on_error != null)
                                            on_error.Invoke(ServerControllerError.EMPTY);
                                    }
                                });
                            }
                            else
                            {
                                connected_players.Add(player);

                                Task.Factory.StartNew(() => OnPlayerConnected(player)).
                                ContinueWith(delegate (Task player_connected_task)
                                {
                                    if (player_connected_task.IsCompleted && !player_connected_task.IsFaulted && !player_connected_task.IsCanceled)
                                    {
                                        Logger.ServerLogInfo(ToString() + ": Player with id: " + client_id + " connected");

                                        if (on_success != null)
                                            on_success.Invoke();
                                    }
                                    else
                                    {
                                        if (player_connected_task.Exception != null)
                                        {
                                            Logger.ServerLogError(ToString() + " OnPlayerConnected(): " + player_connected_task.Exception);
                                        }
                                        else
                                        {
                                            Logger.ServerLogError(ToString() + " OnPlayerConnected(): " + "Task faulted or cancelled");
                                        }

                                        if (on_error != null)
                                            on_error.Invoke(ServerControllerError.EMPTY);
                                    }
                                });
                            }
                        }
                        else
                        {
                            if (on_error != null)
                                on_error.Invoke(ServerControllerError.EMPTY);
                        }
                    }
                    else 
                    {
                        if (player_wants_to_connect_task.Exception != null)
                        {
                            Logger.ServerLogError(ToString() + " OnPlayerWantsToConnect(): " + player_wants_to_connect_task.Exception);
                        }
                        else
                        {
                            Logger.ServerLogError(ToString() + " OnPlayerWantsToConnect(): " + "Task faulted or cancelled");
                        }

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

                        Logger.ServerLogInfo(ToString() + ": Player with id: " + client_id + " disconnected");

                        Task.Factory.StartNew(() => OnPlayerDisconnected(curr_player)).
                        ContinueWith(delegate (Task player_disconnected_task)
                        {
                            if(player_disconnected_task.IsFaulted || player_disconnected_task.IsCanceled)
                            {
                                if(player_disconnected_task.Exception != null)
                                {
                                    Logger.ServerLogError(ToString() + " OnPlayerDisconnected(): " + player_disconnected_task.Exception);
                                }
                                else
                                {
                                    Logger.ServerLogError(ToString() + " OnPlayerDisconnected(): " + "Task faulted or cancelled");
                                }
                            }
                        }); 

                        break;
                    }
                }

                if(connected_players.Count == 0)
                {
                    Task.Factory.StartNew(() => OnRoomClosed());
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
                Task.Factory.StartNew(() => OnMessageReceived(player, message_obj));
            }
        }

        public void Log(string log)
        {
            Logger.RoomLogInfo(this, log);
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

        protected virtual void Update()
        {

        }
    }
}
