using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
                                            connected_players.Add(room_player);

                                            Logger.ServerLogInfo(ToString() + ": Player with id: " + player.ClientId + " connected");

                                            if (on_success != null)
                                                on_success.Invoke();

                                            Task.Factory.StartNew(() => OnPlayerConnected(room_player, join_data)).
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
                            if (player_wants_to_connect_task.Exception != null)
                            {
                                Logger.ServerLogError(ToString() + " OnPlayerWantsToConnect(): " + player_wants_to_connect_task.Exception);
                            }
                            else
                            {
                                Logger.ServerLogError(ToString() + " OnPlayerWantsToConnect(): " + "Task faulted or cancelled");
                            }

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

                    if(curr_player.ServerPlayer.ClientId == player.ClientId)
                    {
                        connected_players.RemoveAt(i);

                        Logger.ServerLogInfo(ToString() + ": Player with id: " + player.ClientId + " disconnected");

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
                    Task.Factory.StartNew(() => OnRoomClosed())
                    .ContinueWith(delegate(Task room_closed_task)
                    {
                        if (room_closed_task.IsFaulted || room_closed_task.IsCanceled)
                        {
                            if (room_closed_task.Exception != null)
                            {
                                Logger.ServerLogError(ToString() + " OnRoomClosed(): " + room_closed_task.Exception);
                            }
                            else
                            {
                                Logger.ServerLogError(ToString() + " OnRoomClosed(): " + "Task faulted or cancelled");
                            }
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
                if (update_task.IsFaulted || update_task.IsCanceled)
                {
                    if (update_task.Exception != null)
                    {
                        Logger.ServerLogError(ToString() + " OnUpdate(): " + update_task.Exception);
                    }
                    else
                    {
                        Logger.ServerLogError(ToString() + " OnUpdate(): " + "Task faulted or cancelled");
                    }
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
                ContinueWith(delegate (Task update_task)
                {
                    if (update_task.IsFaulted || update_task.IsCanceled)
                    {
                        if (update_task.Exception != null)
                        {
                            Logger.ServerLogError(ToString() + " OnMessageReceived(): " + update_task.Exception);
                        }
                        else
                        {
                            Logger.ServerLogError(ToString() + " OnMessageReceived(): " + "Task faulted or cancelled");
                        }
                    }

                    finished_updating = true;
                }); ;
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

                    if (curr_player.ServerPlayer.ClientId == player.ClientId)
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
