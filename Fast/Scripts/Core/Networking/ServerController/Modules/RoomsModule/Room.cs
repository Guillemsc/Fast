using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fast.Networking
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RoomSettings : Attribute
    {
        private string name = "";
        private bool updatable = false;

        public RoomSettings(string name, bool updatable)
        {
            this.name = name;
            this.updatable = updatable;
        }

        public string Name
        {
            get { return name; }
        }

        public bool Updatable
        {
            get { return updatable; }
        }
    }

    public class Room<P> : BaseRoom where P : RoomPlayer, new()
    {
        private List<P> connected_players = new List<P>();

        public override string ToString()
        {
            return "[Name:" + RoomName + "][Id:" + RoomId + "][Connected Players:" + connected_players.Count + "]";
        }

        public override bool IsEmpty()
        {
            return connected_players.Count == 0;
        }

        public override void PlayerConnect(Player player, object join_data, Action on_success, Action<ServerControllerError> on_error)
        {
            if (player != null)
            {
                P room_player = new P();
                room_player.Init(RoomsServerModule, player, join_data);

                lock (connected_players)
                {
                    Task.Factory.StartNew(() => OnPlayerWantsToConnect(room_player, join_data)).
                    ContinueWith(delegate (Task<bool> player_wants_to_connect_task)
                    {
                        string error_msg = "";
                        Exception exception = null;

                        bool has_errors = player_wants_to_connect_task.HasErrors(out error_msg, out exception);

                        if (!has_errors)
                        {
                            if (player_wants_to_connect_task.Result)
                            {
                                if (connected_players.Count == 0)
                                {
                                    Task.Factory.StartNew(() => OnRoomOpened()).
                                    ContinueWith(delegate (Task room_opened_task)
                                    {
                                        has_errors = room_opened_task.HasErrors(out error_msg, out exception);

                                        if (!has_errors)
                                        {
                                            connected_players.Add(room_player);

                                            Logger.ServerLogInfo(ToString() + ": Player with id: " + player.ClientId + " connected");

                                            if (on_success != null)
                                                on_success.Invoke();

                                            Task.Factory.StartNew(() => OnPlayerConnected(room_player, join_data)).
                                            ContinueWith(delegate (Task player_connected_task)
                                            {
                                                has_errors = player_connected_task.HasErrors(out error_msg, out exception);

                                                if (has_errors)
                                                {
                                                    Logger.ServerLogError(ToString() + " OnPlayerConnected(): " + exception);
                                                }
                                            });
                                        }
                                        else
                                        {
                                            Logger.ServerLogError(ToString() + " OnRoomOpened(): " + error_msg);

                                            if (on_error != null)
                                                on_error.Invoke(ServerControllerError.ROOM_EXCEPTION);
                                        }
                                    });
                                }
                                else
                                {
                                    connected_players.Add(room_player);

                                    Logger.ServerLogInfo(ToString() + ": Player with id: " + player.ClientId + " connected");

                                    if (on_success != null)
                                        on_success.Invoke();

                                    Task.Factory.StartNew(() => OnPlayerConnected(room_player, join_data)).
                                    ContinueWith(delegate (Task player_connected_task)
                                    {
                                        has_errors = player_connected_task.HasErrors(out error_msg, out exception);

                                        if (has_errors)
                                        {
                                            Logger.ServerLogError(ToString() + " OnPlayerConnected(): " + error_msg);

                                            if (on_error != null)
                                                on_error.Invoke(ServerControllerError.ROOM_EXCEPTION);
                                        }
                                    });
                                }
                            }
                            else
                            {
                                if (on_error != null)
                                    on_error.Invoke(ServerControllerError.CONNECTION_TO_ROOM_DENIED);
                            }
                        }
                        else
                        {
                            Logger.ServerLogError(ToString() + " OnPlayerWantsToConnect(): " + error_msg);

                            if (on_error != null)
                                on_error.Invoke(ServerControllerError.ROOM_EXCEPTION);
                        }
                    });
                }
            }
            else
            {
                if (on_error != null)
                    on_error.Invoke(ServerControllerError.PLAYER_IS_NOT_CONNECTED);
            }
        }

        public override void PlayerDisconnect(Player player)
        {
            lock (connected_players)
            {
                for (int i = 0; i < connected_players.Count; ++i)
                {
                    P curr_player = connected_players[i];

                    if(curr_player.ServerClientId == player.ClientId)
                    {
                        connected_players.RemoveAt(i);

                        Logger.ServerLogInfo(ToString() + ": Player with id: " + player.ClientId + " disconnected");

                        Task.Factory.StartNew(() => OnPlayerDisconnected(curr_player)).
                        ContinueWith(delegate (Task player_disconnected_task)
                        {
                            string error_msg = "";
                            Exception exception = null;

                            bool has_errors = player_disconnected_task.HasErrors(out error_msg, out exception);

                            if (has_errors)
                            {
                                Logger.ServerLogError(ToString() + " OnPlayerDisconnected(): " + error_msg);
                            }
                        }); 

                        break;
                    }
                }

                if(connected_players.Count == 0)
                {
                    Task.Factory.StartNew(() => OnRoomClosed())
                    .ContinueWith(delegate(Task room_closed_task)
                    {
                        string error_msg = "";
                        Exception exception = null;

                        bool has_errors = room_closed_task.HasErrors(out error_msg, out exception);

                        if (has_errors)
                        {
                            Logger.ServerLogError(ToString() + " OnRoomClosed(): " + error_msg);
                        }
                    });
                }
            }
        }

        public override void Update()
        {
            finished_updating = false;

            Task.Factory.StartNew(() => OnUpdate()).
            ContinueWith(delegate (Task update_task)
            {
                string error_msg = "";
                Exception exception = null;

                bool has_errors = update_task.HasErrors(out error_msg, out exception);

                if (has_errors)
                {
                    Logger.ServerLogError(ToString() + " OnUpdate(): " + error_msg);
                }

                finished_updating = true;
            });
        }

        public override void MessageReceived(Player player, object message_obj)
        {
            P room_player = GetConnectedPlayer(player);

            if (room_player != null)
            {
                Task.Factory.StartNew(() => OnMessageReceived(room_player, message_obj)).
                ContinueWith(delegate (Task message_received_task)
                {
                    string error_msg = "";
                    Exception exception = null;

                    bool has_errors = message_received_task.HasErrors(out error_msg, out exception);

                    if (has_errors)
                    {
                        Logger.ServerLogError(ToString() + " OnMessageReceived(): " + error_msg);
                    }

                    finished_updating = true;
                }); 
            }
        }

        public P GetConnectedPlayer(Player player)
        {
            P ret = null;

            lock (connected_players)
            {
                for (int i = 0; i < connected_players.Count; ++i)
                {
                    P curr_player = connected_players[i];

                    if (curr_player.ServerClientId == player.ClientId)
                    {
                        ret = curr_player;

                        break;
                    }
                }
            }

            return ret;
        }

        public int ConnectedPlayersCount
        {
            get { return connected_players.Count; }
        }

        public IEnumerable<P> ConnectedPlayers
        {
            get { return connected_players.AsEnumerable(); }
        }

        public void Log(string log)
        {
            Logger.RoomLogInfo(this, log);
        }

        public void BroadcastMessage(object message_obj)
        {
            lock (connected_players)
            {
                for (int i = 0; i < connected_players.Count; ++i)
                {
                    connected_players[i].SendMessage(message_obj);
                }
            }
        }

        protected virtual void OnRoomOpened()
        {

        }

        protected virtual bool OnPlayerWantsToConnect(P player, object join_data)
        {
            return true;
        }

        protected virtual void OnPlayerConnected(P player, object join_data)
        {

        }

        protected virtual void OnPlayerDisconnected(P player)
        {

        }

        protected virtual void OnRoomClosed()
        {

        }

        protected virtual void OnMessageReceived(P player, object message_obj)
        {

        }

        protected virtual void OnUpdate()
        {

        }
    }
}
