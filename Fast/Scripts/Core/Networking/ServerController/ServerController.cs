using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public class ServerController
    {
        Server server = null;

        private Dictionary<string, Type> room_types = new Dictionary<string, Type>();
        private Dictionary<string, Type> player_cluster_types = new Dictionary<string, Type>();

        private PlayerCluster player_cluster = null;
        private List<Room> rooms = new List<Room>();
        private List<Player> players = new List<Player>();

        public ServerController(int server_port, string player_cluster_name)
        {
            Logger.ServerLogInfo("Starting server with port: " + server_port + " and player cluster: " + player_cluster_name);

            server = new Server(server_port);

            server.OnClientConnected.Subscribe(OnClientConnected);
            server.OnClientDisconnected.Subscribe(OnClientDisconnected);
            server.OnMessageReceived.Subscribe(OnMessageReceived);

            GetDataTypes();

            player_cluster = CreatePlayerCluster(player_cluster_name);
        }

        public void Start()
        {
            server.Start();
        }

        public void Update()
        {
            server.ReadMessages();
        }

        private void OnClientConnected(int client_id)
        {

        }

        private void OnClientDisconnected(int client_id)
        {
            RemovePlayer(client_id, PlayerLeaveRoomCause.CLIENT_DISCONNECTED);
        }

        public void SendDataMessage(int client_id, object message_obj)
        {
            DataMessage message = new DataMessage(message_obj);

            byte[] data = Parsers.ByteParser.ComposeObject(message);

            server.SendMessage(client_id, data);
        }

        private void OnMessageReceived(ServerMessage server_message)
        {
            ServerControllerMessage message = Parsers.ByteParser.ParseObject<ServerControllerMessage>(server_message.Data);

            switch (message.Type)
            {
                case ServerControllerMessageType.CREATE_PLAYER:
                    {
                        CreatePlayerMessage create_player_message = (CreatePlayerMessage)message;

                        Player player = CreatePlayer(server_message.ClientId, create_player_message.JoinData);

                        bool success = player != null;

                        byte[] data = Parsers.ByteParser.ComposeObject(new CreatePlayerResponseMessage(success));

                        server.SendMessage(server_message.ClientId, data);

                        break;
                    }

                case ServerControllerMessageType.DATA:
                    {
                        DataMessage data_message = (DataMessage)message;

                        ClientMessageReceived(server_message.ClientId, data_message.MessageObj);

                        break;
                    }

                case ServerControllerMessageType.CREATE_ROOM:
                    {
                        CreateRoomMessage create_room_message = (CreateRoomMessage)message;

                        PlayerCreateRoom(server_message.ClientId, create_room_message.RoomName, create_room_message.RoomId, 
                            create_room_message.JoinData,
                        delegate ()
                        {
                            byte[] data = Parsers.ByteParser.ComposeObject(new CreateRoomResponseMessage(true, create_room_message.RoomId));

                            server.SendMessage(server_message.ClientId, data);
                        }
                        , delegate(ServerControllerError error)
                        {
                            byte[] data = Parsers.ByteParser.ComposeObject(new CreateRoomResponseMessage(false, error));

                            server.SendMessage(server_message.ClientId, data);
                        });

                        break;
                    }

                case ServerControllerMessageType.JOIN_ROOM:
                    {
                        JoinRoomMessage join_room_message = (JoinRoomMessage)message;

                        PlayerJoinRoom(server_message.ClientId, join_room_message.RoomId, join_room_message.JoinData,
                        delegate ()
                        {
                            byte[] data = Parsers.ByteParser.ComposeObject(new JoinRoomResponseMessage(true, join_room_message.RoomId));

                            server.SendMessage(server_message.ClientId, data);
                        }
                        , delegate(ServerControllerError error)
                        {
                            byte[] data = Parsers.ByteParser.ComposeObject(new JoinRoomResponseMessage(false, error));

                            server.SendMessage(server_message.ClientId, data);
                        });

                        break;
                    }

                case ServerControllerMessageType.CREATE_JOIN_ROOM:
                    {
                        CreateJoinRoomMessage create_join_room_message = (CreateJoinRoomMessage)message;

                        PlayerCreateJoinRoom(server_message.ClientId, create_join_room_message.RoomName, 
                        create_join_room_message.RoomId, create_join_room_message.JoinData,
                        delegate ()
                        {
                            byte[] data = Parsers.ByteParser.ComposeObject(new CreateJoinRoomResponseMessage(true, create_join_room_message.RoomId));

                            server.SendMessage(server_message.ClientId, data);
                        }
                        , delegate(ServerControllerError error)
                        {
                            byte[] data = Parsers.ByteParser.ComposeObject(new CreateJoinRoomResponseMessage(false, error));

                            server.SendMessage(server_message.ClientId, data);
                        });

                        break;
                    }

                case ServerControllerMessageType.LEAVE_ROOM:
                    {
                        PlayerLeaveRoom(server_message.ClientId, PlayerLeaveRoomCause.CLIENT_REQUESTED_TO_LEAVE);

                        break;
                    }
            }
        }

        private void GetDataTypes()
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

                    // Room 
                    object[] attributes_room = curr_type.GetCustomAttributes(typeof(RoomName), true);

                    if (attributes_room != null && attributes_room.Length > 0)
                    {
                        List<RoomName> room_name = attributes_room.Cast<RoomName>().ToList();

                        if (room_name.Count > 0)
                        {
                            room_types.Add(room_name[0].Name, curr_type);
                        }
                    }

                    // Player cluster
                    object[] attributes_player_cluster = curr_type.GetCustomAttributes(typeof(PlayerClusterName), true);

                    if (attributes_player_cluster != null && attributes_player_cluster.Length > 0)
                    {
                        List<PlayerClusterName> player_cluster_name = attributes_player_cluster.Cast<PlayerClusterName>().ToList();

                        if (player_cluster_name.Count > 0)
                        {
                            player_cluster_types.Add(player_cluster_name[0].Name, curr_type);
                        }
                    }
                }
            }

            string log_rooms = "Room types loaded: ";

            int counter = 0;
            foreach (KeyValuePair<string, Type> entry in room_types)
            {
                log_rooms += entry.Key;

                if (counter < room_types.Count - 1)
                {
                    log_rooms += ", ";
                }

                ++counter;
            }

            Logger.ServerLogInfo(log_rooms);

            string log_player_clusters = "Player clusters loaded: ";

            counter = 0;
            foreach (KeyValuePair<string, Type> entry in player_cluster_types)
            {
                log_player_clusters += entry.Key;

                if (counter < player_cluster_types.Count - 1)
                {
                    log_player_clusters += ", ";
                }

                ++counter;
            }

            Logger.ServerLogInfo(log_player_clusters);
        }

        private PlayerCluster CreatePlayerCluster(string player_cluster_name)
        {
            PlayerCluster ret = null;

            Type type = null;

            bool exists = player_cluster_types.TryGetValue(player_cluster_name, out type);

            if (exists)
            {
                ret = (PlayerCluster)Activator.CreateInstance(type);

                ret.Init(this);
            }

            return ret;
        }

        private Room CreateRoom(string room_name, string room_id)
        {
            Room ret = null;

            Type type = null;

            bool exists = room_types.TryGetValue(room_name, out type);

            if(exists)
            {
                ret = (Room)Activator.CreateInstance(type);

                ret.Init(this, room_name, room_id);

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
                    Room curr_room = rooms[i];

                    if (curr_room.RoomId == room_id)
                    {
                        rooms.RemoveAt(i);

                        Logger.ServerLogInfo(curr_room.ToString() + " Room removed");

                        break;
                    }
                }
            }
        }

        private Room GetRoom(string room_id)
        {
            Room ret = null;

            for (int i = 0; i < rooms.Count; ++i)
            {
                if(rooms[i].RoomId == room_id)
                {
                    ret = rooms[i];

                    break;
                }
            }

            return ret;
        }

        public ReadOnlyCollection<Player> Players
        {
            get { return players.AsReadOnly(); }
        }

        public Player CreatePlayer(int client_id, object join_data)
        {
            Player ret = new Player(client_id);

            lock(players)
            {
                players.Add(ret);

                Logger.ServerLogInfo("Player created with id: " + client_id + " | Players count: " + players.Count);
            }

            if(player_cluster != null)
            {
                player_cluster.PlayerConnected(ret, join_data);
            }

            return ret;
        }

        public void RemovePlayer(int client_id, PlayerLeaveRoomCause cause)
        {
            lock(players)
            {
                for(int i = 0; i < players.Count; ++i)
                {
                    Player curr_player = players[i];

                    if(curr_player.ClientId == client_id)
                    {
                        if (curr_player.ConnectedToRoom)
                        {
                            PlayerLeaveRoom(client_id, cause);
                        }

                        if (player_cluster != null)
                        {
                            player_cluster.PlayerDisconnected(curr_player);
                        }

                        players.RemoveAt(i);

                        Logger.ServerLogInfo("Player removed with id: " + client_id + " | Players count: " + players.Count);

                        break;
                    }
                }
            }
        }

        private Player GetPlayer(int client_id)
        {
            Player ret = null;

            for (int i = 0; i < players.Count; ++i)
            {
                if (players[i].ClientId == client_id)
                {
                    ret = players[i];

                    break;
                }
            }

            return ret;
        }

        public void PlayerCreateRoom(int client_id, string room_name, string room_id, object join_data, 
            Action on_succes, Action<ServerControllerError> on_fail)
        {
            Player player = GetPlayer(client_id);

            if (player != null)
            {
                Room room_test = GetRoom(room_id);

                if (room_test == null)
                {
                    Room room = CreateRoom(room_name, room_id);

                    if (room != null)
                    {
                        if (player.ConnectedToRoom)
                        {
                            PlayerLeaveRoom(player.ClientId, PlayerLeaveRoomCause.ROOM_CHANGE);
                        }

                        player.ConnectedToRoom = true;
                        player.RoomId = room.RoomId;

                        room.PlayerConnect(client_id, join_data,
                        delegate ()
                        {
                            if (on_succes != null)
                                on_succes.Invoke();
                        }, 
                        delegate(ServerControllerError error)
                        {
                            if (room.ConnectedPlayersCount == 0)
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
                            on_fail.Invoke(ServerControllerError.EMPTY);
                    }
                }
                else
                {
                    if (on_fail != null)
                        on_fail.Invoke(ServerControllerError.EMPTY);
                }
            }
            else
            {
                if (on_fail != null)
                    on_fail.Invoke(ServerControllerError.EMPTY);
            }
        }

        public void PlayerJoinRoom(int client_id, string room_id, object join_data, Action on_succes, Action<ServerControllerError> on_fail)
        {
            Player player = GetPlayer(client_id);

            if (player != null)
            {
                Room room = GetRoom(room_id);

                if (room != null)
                {
                    if (player.ConnectedToRoom)
                    {
                        PlayerLeaveRoom(player.ClientId, PlayerLeaveRoomCause.ROOM_CHANGE);
                    }

                    player.ConnectedToRoom = true;
                    player.RoomId = room.RoomId;

                    room.PlayerConnect(client_id, join_data,
                    delegate ()
                    {
                        if (on_succes != null)
                            on_succes.Invoke();
                    },
                    delegate (ServerControllerError error)
                    {
                        if (room.ConnectedPlayersCount == 0)
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
                        on_fail.Invoke(ServerControllerError.EMPTY);
                }
            }
            else
            {
                if (on_fail != null)
                    on_fail.Invoke(ServerControllerError.EMPTY);
            }
        }

        public void PlayerCreateJoinRoom(int client_id, string room_name, string room_id, object join_data, 
            Action on_succes, Action<ServerControllerError> on_fail)
        {
            Player player = GetPlayer(client_id);

            if (player != null)
            {
                Room room = GetRoom(room_id);

                if (room == null)
                {
                    room = CreateRoom(room_name, room_id);
                }

                if (room != null)
                {
                    if (player.ConnectedToRoom)
                    {
                        PlayerLeaveRoom(player.ClientId, PlayerLeaveRoomCause.ROOM_CHANGE);
                    }

                    player.ConnectedToRoom = true;
                    player.RoomId = room.RoomId;

                    room.PlayerConnect(client_id, join_data,
                    delegate ()
                    {
                        if (on_succes != null)
                            on_succes.Invoke();
                    },
                    delegate (ServerControllerError error)
                    {
                        if (room.ConnectedPlayersCount == 0)
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
                        on_fail.Invoke(ServerControllerError.EMPTY);
                }
            }
            else
            {
                if (on_fail != null)
                    on_fail.Invoke(ServerControllerError.EMPTY);
            }
        }

        public void PlayerLeaveRoom(int client_id, PlayerLeaveRoomCause cause)
        {
            Player player = GetPlayer(client_id);

            if(player != null)
            {
                if(player.ConnectedToRoom)
                {
                    Room room = GetRoom(player.RoomId);

                    if(room != null)
                    {
                        room.PlayerDisconnect(client_id);

                        player.ConnectedToRoom = false;
                        player.RoomId = "";

                        byte[] data = Parsers.ByteParser.ComposeObject(new DisconnectedFromRoomMessage(cause));

                        server.SendMessage(client_id, data);

                        if (room.ConnectedPlayersCount == 0)
                        {
                            RemoveRoom(room.RoomId);
                        }
                    }
                }
            }
        }

        public void ClientMessageReceived(int client_id, object message_obj)
        {
            Player player = GetPlayer(client_id);

            if (player != null)
            {
                if (player.ConnectedToRoom)
                {
                    Room room = GetRoom(player.RoomId);

                    if (room != null)
                    {
                        room.MessageReceived(client_id, message_obj);
                    }
                }
            }
        }
    }
}
