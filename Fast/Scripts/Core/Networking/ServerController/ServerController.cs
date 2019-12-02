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
        private Server server = null;

        private List<ServerModule> modules = new List<ServerModule>();

        private List<Player> players = new List<Player>();

        public ServerController(int server_port, string player_cluster_name)
        {
            Logger.ServerLogInfo("Starting server with port: " + server_port + " and player cluster: " + player_cluster_name);

            server = new Server(server_port);

            server.OnClientConnected.Subscribe(OnClientConnected);
            server.OnClientDisconnected.Subscribe(OnClientDisconnected);
            server.OnMessageReceived.Subscribe(OnMessageReceived);

            AddModule(new RoomsServerModule(this));

            for (int i = 0; i < modules.Count; ++i)
            {
                modules[i].Start();
            }
        }

        public void Start(int send_timeout_ms)
        {
            server.Start(send_timeout_ms);
        }

        public void Stop()
        {
            server.Stop();
        }

        public void Update()
        {
            server.ReadMessages();

            for (int i = 0; i < modules.Count; ++i)
            {
                modules[i].Update();
            }
        }

        private void AddModule(ServerModule module)
        {
            modules.Add(module);
        }

        private void OnClientConnected(int client_id)
        {
            int i = 0;
        }

        private void OnClientDisconnected(int client_id)
        {
            Player player = GetPlayer(client_id);

            if (player != null)
            {
                for (int i = 0; i < modules.Count; ++i)
                {
                    modules[i].OnPlayerDisconnected(player);
                }
            }

            RemovePlayer(client_id, PlayerLeaveRoomCause.CLIENT_DISCONNECTED);
        }

        private void OnMessageReceived(ServerMessage server_message)
        {
            ServerControllerMessage message = Parsers.ByteParser.ParseObject<ServerControllerMessage>(server_message.Data);

            switch (message.Type)
            {
                case ServerControllerMessageType.CREATE_PLAYER:
                    {
                        CreatePlayerMessage create_player_message = (CreatePlayerMessage)message;

                        Player new_player = CreatePlayer(server_message.ClientId, create_player_message.JoinData);

                        bool success = new_player != null;
                        byte[] data = Parsers.ByteParser.ComposeObject(new CreatePlayerResponseMessage(success));
                        server.SendMessage(server_message.ClientId, data);

                        for (int i = 0; i < modules.Count; ++i)
                        {
                            modules[i].OnPlayerConnected(new_player);
                        }

                        break;
                    }
            }

            Player player = GetPlayer(server_message.ClientId);

            if (player != null)
            {
                for (int i = 0; i < modules.Count; ++i)
                {
                    modules[i].OnMessageReceived(player, message);
                }
            }
        }

        public ReadOnlyCollection<Player> Players
        {
            get { return players.AsReadOnly(); }
        }

        public Player CreatePlayer(int client_id, object join_data)
        {
            Player ret = new Player(client_id, join_data);

            lock(players)
            {
                players.Add(ret);

                Logger.ServerLogInfo("Player created with id: " + client_id + " | Players count: " + players.Count);
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

        public void SendMessage(Player player, object message_obj)
        {
            byte[] message_data = Parsers.ByteParser.ComposeObject(message_obj);

            server.SendMessage(player.ClientId, message_data);
        }
    }
}
