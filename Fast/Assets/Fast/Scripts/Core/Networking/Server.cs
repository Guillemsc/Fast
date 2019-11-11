using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public class Server
    {
        private Telepathy.Server server = new Telepathy.Server();

        private int port = 0;

        private bool started = false;

        private Callback<int> on_client_connected = new Callback<int>();
        private Callback<int> on_client_disconnected = new Callback<int>();

        private ServerController server_controller = null;

        public Server(int port, string player_cluster_name = "")
        {
            this.port = port;
            this.server_controller = new ServerController(this, player_cluster_name);
        }

        public void Start()
        {
            server.Start(port);

            started = true;
        }

        public void Stop()
        {
            started = false;

            server.Stop();
        }

        public bool Started
        {
            get { return started; }
        }

        public int Port
        {
            get { return port; }
        }

        public ServerController ServerController
        {
            get { return server_controller; }
        }

        public Callback<int> OnClientConnected
        {
            get { return on_client_connected; }
        }

        public Callback<int> OnClientDisconnected
        {
            get { return on_client_disconnected; }
        }

        public void SendMessage(int client_id, object obj_to_send)
        {
            if (started)
            {
                byte[] data = Parsers.ByteParser.ComposeObject(new DataMessage(obj_to_send));

                server.Send(client_id, data);
            }
        }

        public void ReadMessages()
        {
            List<object> ret = new List<object>();

            Telepathy.Message msg;
            while (server.GetNextMessage(out msg))
            {
                switch (msg.eventType)
                {
                    case Telepathy.EventType.Connected:
                        {                            
                            server_controller.CreatePlayer(msg.connectionId);

                            on_client_connected.Invoke(msg.connectionId);

                            Logger.ServerLogInfo("Client connected with id: " + msg.connectionId);
                        }
                        break;

                    case Telepathy.EventType.Disconnected:
                        {                            
                            server_controller.RemovePlayer(msg.connectionId);

                            on_client_disconnected.Invoke(msg.connectionId);

                            Logger.ServerLogInfo("Client disconnected with id: " + msg.connectionId);
                        }
                        break;

                    case Telepathy.EventType.Data:
                        {
                            Message message = Parsers.ByteParser.ParseObject<Message>(msg.data);

                            ManageDataMessage(msg.connectionId, message);
                        }
                        break;
                }
            }
        }

        private void ManageDataMessage(int client_id, Message message)
        {
            switch (message.Type)
            {
                case MessageType.DATA:
                    {
                        DataMessage data_message = (DataMessage)message;
                        
                        server_controller.ClientMessageReceived(client_id, data_message.MessageObj);
                        
                        break;
                    }

                case MessageType.CREATE_ROOM:
                    {
                        CreateRoomMessage create_room_message = (CreateRoomMessage)message;

                        bool success = false;
                        
                        success = server_controller.PlayerCreateRoom(client_id, create_room_message.RoomName, 
                            create_room_message.RoomId);
                        
                        byte[] data = Parsers.ByteParser.ComposeObject(new CreateRoomResponseMessage(success, create_room_message.RoomId));

                        server.Send(client_id, data);

                        break;
                    }

                case MessageType.JOIN_ROOM:
                    {
                        JoinRoomMessage join_room_message = (JoinRoomMessage)message;

                        bool success = false;
                        
                        success = server_controller.PlayerJoinRoom(client_id, join_room_message.RoomId);
                        
                        byte[] data = Parsers.ByteParser.ComposeObject(new JoinRoomResponseMessage(success, join_room_message.RoomId));

                        server.Send(client_id, data);

                        break;
                    }

                case MessageType.CREATE_JOIN_ROOM:
                    {
                        CreateJoinRoomMessage create_join_room_message = (CreateJoinRoomMessage)message;

                        bool success = false;
                        
                        success = server_controller.PlayerCreateJoinRoom(client_id, create_join_room_message.RoomName, 
                            create_join_room_message.RoomId);
                        
                        byte[] data = Parsers.ByteParser.ComposeObject(new CreateJoinRoomResponseMessage(success, create_join_room_message.RoomId));

                        server.Send(client_id, data);

                        break;
                    }

                case MessageType.LEAVE_ROOM:
                    {                        
                        server_controller.PlayerLeaveRoom(client_id);
                        
                        break;
                    }
            }
        }

        public void SendDisconnectedFromRoomMessage(int client_id)
        {
            byte[] data = Parsers.ByteParser.ComposeObject(new DisconnectedFromRoomMessage());

            server.Send(client_id, data);
        }
    }
}
