using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public class RoomsServerModule : ServerModule
    {
        private Dictionary<RoomSettings, Type> room_types = new Dictionary<RoomSettings, Type>();

        private List<BaseRoom> rooms = new List<BaseRoom>();

        public RoomsServerModule(ServerController server_controller) : base(server_controller)
        {

        }

        public override void Start()
        {
            GetRoomsTypes();
        }

        public override void Update()
        {
            UpdateRooms();
        }

        private void GetRoomsTypes()
        {
            room_types.Clear();

            System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            for (int i = 0; i < assemblies.Length; ++i)
            {
                System.Reflection.Assembly curr_assembly = assemblies[i];

                Type[] types = curr_assembly.GetTypes();

                for (int y = 0; y < types.Length; ++y)
                {
                    Type curr_type = types[y];

                    object[] attributes_room = curr_type.GetCustomAttributes(typeof(RoomSettings), true);

                    if (attributes_room != null && attributes_room.Length > 0)
                    {
                        List<RoomSettings> room_settings = attributes_room.Cast<RoomSettings>().ToList();

                        if (room_settings.Count > 0)
                        {
                            room_types.Add(room_settings[0], curr_type);
                        }
                    }
                }
            }

            string log_rooms = "Room types loaded: ";

            int counter = 0;
            foreach (KeyValuePair<RoomSettings, Type> entry in room_types)
            {
                log_rooms += entry.Key.Name;

                if (counter < room_types.Count - 1)
                {
                    log_rooms += ", ";
                }

                ++counter;
            }

            Logger.ServerLogInfo(log_rooms);
        }

        public override void OnMessageReceived(Player player, ServerControllerMessage message)
        {
            switch(message.Type)
            {
                case ServerControllerMessageType.CREATE_ROOM:
                    {
                        CreateRoomMessage create_room_message = (CreateRoomMessage)message;

                        PlayerCreateRoom(player, create_room_message.RoomName, create_room_message.RoomId,
                            create_room_message.JoinData,
                        delegate ()
                        {
                            ServerController.SendMessage(player, new CreateRoomResponseMessage(true, create_room_message.RoomId));
                        }
                        , delegate (ServerControllerError error)
                        {
                            ServerController.SendMessage(player, new CreateRoomResponseMessage(false, error));
                        });

                        break;
                    }

                case ServerControllerMessageType.JOIN_ROOM:
                    {
                        JoinRoomMessage join_room_message = (JoinRoomMessage)message;

                        PlayerJoinRoom(player, join_room_message.RoomId, join_room_message.JoinData,
                        delegate ()
                        {
                            ServerController.SendMessage(player, new JoinRoomResponseMessage(true, join_room_message.RoomId));
                        }
                        , delegate (ServerControllerError error)
                        {
                            ServerController.SendMessage(player, new JoinRoomResponseMessage(false, error));
                        });

                        break;
                    }

                case ServerControllerMessageType.CREATE_JOIN_ROOM:
                    {
                        CreateJoinRoomMessage create_join_room_message = (CreateJoinRoomMessage)message;

                        PlayerCreateJoinRoom(player, create_join_room_message.RoomName,
                        create_join_room_message.RoomId, create_join_room_message.JoinData,
                        delegate ()
                        {
                            ServerController.SendMessage(player, new CreateJoinRoomResponseMessage(true, create_join_room_message.RoomId));
                        }
                        , delegate (ServerControllerError error)
                        {
                            ServerController.SendMessage(player, new CreateJoinRoomResponseMessage(false, error));
                        });

                        break;
                    }

                case ServerControllerMessageType.LEAVE_ROOM:
                    {
                        PlayerLeaveRoom(player, PlayerLeaveRoomCause.CLIENT_REQUESTED_TO_LEAVE);

                        break;
                    }

                case ServerControllerMessageType.ROOM_MESSAGE:
                    {
                        RoomMessage room_message = (RoomMessage)message;

                        PlayerMessageReceived(player, room_message.MessageObj);

                        break;
                    }
            }
        }

        private BaseRoom CreateRoom(string room_name, string room_id)
        {
            BaseRoom ret = null;

            Type type = null;
            RoomSettings settings = null;

            bool exists = false;

            foreach (KeyValuePair<RoomSettings, Type> entry in room_types)
            {
                if (entry.Key.Name == room_name)
                {
                    type = entry.Value;
                    settings = entry.Key;

                    exists = true;

                    break;
                }
            }

            if (exists)
            {
                ret = (BaseRoom)Activator.CreateInstance(type);

                ret.Init(this, settings, room_id);

                lock (rooms)
                {
                    rooms.Add(ret);
                }

                Logger.ServerLogInfo(ret.ToString() + " Room created");
            }

            return ret;
        }

        private void RemoveRoom(string room_id)
        {
            lock (rooms)
            {
                for (int i = 0; i < rooms.Count; ++i)
                {
                    BaseRoom curr_room = rooms[i];

                    if (curr_room.RoomId == room_id)
                    {
                        rooms.RemoveAt(i);

                        Logger.ServerLogInfo(curr_room.ToString() + " Room removed");

                        break;
                    }
                }
            }
        }

        private BaseRoom GetRoom(string room_id)
        {
            BaseRoom ret = null;

            for (int i = 0; i < rooms.Count; ++i)
            {
                if (rooms[i].RoomId == room_id)
                {
                    ret = rooms[i];

                    break;
                }
            }

            return ret;
        }

        private void UpdateRooms()
        {
            for (int i = 0; i < rooms.Count; ++i)
            {
                BaseRoom curr_room = rooms[i];

                if (curr_room._Updatable)
                {
                    if (curr_room._FinishedUpdating)
                    {
                        curr_room.Update();
                    }

                    break;
                }
            }
        }

        public void PlayerCreateRoom(Player player, string room_name, string room_id, object join_data,
            Action on_succes, Action<ServerControllerError> on_fail)
        {            
            BaseRoom room_test = GetRoom(room_id);

            if (room_test == null)
            {
                BaseRoom room = CreateRoom(room_name, room_id);

                if (room != null)
                {
                    if (player.ConnectedToRoom)
                    {
                        PlayerLeaveRoom(player, PlayerLeaveRoomCause.ROOM_CHANGE);
                    }

                    player.ConnectedToRoom = true;
                    player.RoomId = room.RoomId;

                    room.PlayerConnect(player, join_data,
                    delegate ()
                    {
                        if (on_succes != null)
                            on_succes.Invoke();
                    },
                    delegate (ServerControllerError error)
                    {
                        player.ConnectedToRoom = false;
                        player.RoomId = "";

                        if (room.IsEmpty())
                        {
                            RemoveRoom(room.RoomId);
                        }

                        if (on_fail != null)
                            on_fail.Invoke(error);
                    });
                }
                else
                {
                    if (on_fail != null)
                        on_fail.Invoke(ServerControllerError.ROOM_TYPE_DOES_NOT_EXIST);
                }
            }
            else
            {
                if (on_fail != null)
                    on_fail.Invoke(ServerControllerError.ROOM_ALREADY_EXIST);
            }      
        }

        public void PlayerJoinRoom(Player player, string room_id, object join_data, Action on_succes, Action<ServerControllerError> on_fail)
        {            
            BaseRoom room = GetRoom(room_id);

            if (room != null)
            {
                if (player.ConnectedToRoom)
                {
                    PlayerLeaveRoom(player, PlayerLeaveRoomCause.ROOM_CHANGE);
                }

                player.ConnectedToRoom = true;
                player.RoomId = room.RoomId;

                room.PlayerConnect(player, join_data,
                delegate ()
                {
                    if (on_succes != null)
                        on_succes.Invoke();
                },
                delegate (ServerControllerError error)
                {
                    player.ConnectedToRoom = false;
                    player.RoomId = "";

                    if (room.IsEmpty())
                    {
                        RemoveRoom(room.RoomId);
                    }

                    if (on_fail != null)
                        on_fail.Invoke(error);
                });
            }
            else
            {
                if (on_fail != null)
                    on_fail.Invoke(ServerControllerError.ROOM_DOES_NOT_EXIST);
            }          
        }

        public void PlayerCreateJoinRoom(Player player, string room_name, string room_id, object join_data,
            Action on_succes, Action<ServerControllerError> on_fail)
        {
            bool tried_to_create_room = false;

            BaseRoom room = GetRoom(room_id);

            if (room == null)
            {
                tried_to_create_room = true;

                room = CreateRoom(room_name, room_id);
            }

            if (room != null)
            {
                if (player.ConnectedToRoom)
                {
                    PlayerLeaveRoom(player, PlayerLeaveRoomCause.ROOM_CHANGE);
                }

                player.ConnectedToRoom = true;
                player.RoomId = room.RoomId;

                room.PlayerConnect(player, join_data,
                delegate ()
                {
                    if (on_succes != null)
                        on_succes.Invoke();
                },
                delegate (ServerControllerError error)
                {
                    player.ConnectedToRoom = false;
                    player.RoomId = "";

                    if (room.IsEmpty())
                    {
                        RemoveRoom(room.RoomId);
                    }

                    if (on_fail != null)
                        on_fail.Invoke(error);
                });
            }
            else
            {
                if (tried_to_create_room)
                {
                    if (on_fail != null)
                        on_fail.Invoke(ServerControllerError.ROOM_TYPE_DOES_NOT_EXIST);
                }
                else
                {
                    if (on_fail != null)
                        on_fail.Invoke(ServerControllerError.ROOM_DOES_NOT_EXIST);
                }
            }            
        }

        public void PlayerLeaveRoom(Player player, PlayerLeaveRoomCause cause)
        {            
            if (player.ConnectedToRoom)
            {
                BaseRoom room = GetRoom(player.RoomId);

                if (room != null)
                {
                    room.PlayerDisconnect(player);

                    player.ConnectedToRoom = false;
                    player.RoomId = "";

                    ServerController.SendMessage(player, new DisconnectedFromRoomMessage(cause));

                    if (room.IsEmpty())
                    {
                        RemoveRoom(room.RoomId);
                    }
                }
            }
        }

        public void PlayerSendMessage(Player player, object message_obj)
        {
            if (player.ConnectedToRoom)
            {
                ServerController.SendMessage(player, new RoomMessage(message_obj));
            }
        }

        public void PlayerMessageReceived(Player player, object message_obj)
        {
            if (player.ConnectedToRoom)
            {
                BaseRoom room = GetRoom(player.RoomId);

                if (room != null)
                {
                    room.MessageReceived(player, message_obj);
                }
            }
        }
    }
}
