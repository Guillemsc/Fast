using System;
using System.Collections.Generic;

namespace Fast.Networking
{
    class ClientController
    {
        private Client client = null;

        private bool connected = false;

        private object join_data = null;

        private List<object> messages_to_read = new List<object>();

        private bool connected_to_room = false;
        private string connected_room_id = "";

        private Callback on_connect_to_server_success = new Callback();
        private Callback on_connect_to_server_fail = new Callback();

        private Callback on_create_room_success = new Callback();
        private Callback<ServerControllerError> on_create_room_fail = new Callback<ServerControllerError>();

        private Callback on_join_room_success = new Callback();
        private Callback<ServerControllerError> on_join_room_fail = new Callback<ServerControllerError>();

        private Callback on_create_join_room_success = new Callback();
        private Callback<ServerControllerError> on_create_join_room_fail = new Callback<ServerControllerError>();

        private Callback<PlayerLeaveRoomCause> on_disconnected_from_room = new Callback<PlayerLeaveRoomCause>();

        public ClientController(string server_ip, int server_port)
        {
            client = new Client(server_ip, server_port);

            client.OnConnected.Subscribe(OnConnected);
            client.OnDisconnected.Subscribe(OnDisconnected);
            client.OnMessageReceived.Subscribe(OnMessageReceived);
        }

        public void Connect(object join_data = null, Action on_connect_success = null, Action on_connect_fail = null)
        {
            on_connect_to_server_success.UnSubscribeAll();
            on_connect_to_server_success.Subscribe(on_connect_success);

            on_connect_to_server_fail.UnSubscribeAll();
            on_connect_to_server_fail.Subscribe(on_connect_fail);

            this.join_data = join_data;

            client.Connect();
        }

        public void Update()
        {
            client.ReadMessages();
        }

        private void OnConnected()
        {
            byte[] data = Parsers.ByteParser.ComposeObject(new CreatePlayerMessage(join_data));

            client.SendMessage(data);
        }

        private void OnDisconnected()
        {
            connected = false;

            Logger.ClientLogInfo("Disconnected from server");
        }

        private void OnMessageReceived(byte[] message_data)
        {
            ServerControllerMessage message = Parsers.ByteParser.ParseObject<ServerControllerMessage>(message_data);

            switch (message.Type)
            {
                case ServerControllerMessageType.CREATE_PLAYER_RESPONSE:
                    {
                        CreatePlayerResponseMessage response_message = (CreatePlayerResponseMessage)message;

                        if(response_message.Success)
                        {
                            connected = true;

                            on_connect_to_server_success.Invoke();
                        }
                        else
                        {
                            connected = false;

                            client.Disconnect();

                            on_connect_to_server_fail.Invoke();
                        }

                        break;
                    }

                case ServerControllerMessageType.DATA:
                    {
                        DataMessage data_message = (DataMessage)message;

                        messages_to_read.Add(data_message.MessageObj);

                        break;
                    }

                case ServerControllerMessageType.CREATE_ROOM_RESPONSE:
                    {
                        CreateRoomResponseMessage response_message = (CreateRoomResponseMessage)message;

                        if (response_message.Success)
                        {
                            connected_to_room = true;
                            connected_room_id = response_message.RoomId;

                            on_create_room_success.Invoke();
                        }
                        else
                        {
                            connected_to_room = false;
                            connected_room_id = "";

                            on_create_room_fail.Invoke(response_message.Error);
                        }

                        break;
                    }

                case ServerControllerMessageType.JOIN_ROOM_RESPONSE:
                    {
                        JoinRoomResponseMessage response_message = (JoinRoomResponseMessage)message;

                        if (response_message.Success)
                        {
                            connected_to_room = true;
                            connected_room_id = response_message.RoomId;

                            on_join_room_success.Invoke();
                        }
                        else
                        {
                            connected_to_room = false;
                            connected_room_id = "";

                            on_join_room_fail.Invoke(response_message.Error);
                        }

                        break;
                    }

                case ServerControllerMessageType.CREATE_JOIN_ROOM_RESPONSE:
                    {
                        CreateJoinRoomResponseMessage response_message = (CreateJoinRoomResponseMessage)message;

                        if (response_message.Success)
                        {
                            connected_to_room = true;
                            connected_room_id = response_message.RoomId;

                            on_create_join_room_success.Invoke();
                        }
                        else
                        {
                            connected_to_room = false;
                            connected_room_id = "";

                            on_create_join_room_fail.Invoke(response_message.Error);
                        }

                        break;
                    }

                case ServerControllerMessageType.DISCONNECTED_FROM_ROOM:
                    {
                        DisconnectedFromRoomMessage disconnected_message = (DisconnectedFromRoomMessage)message;

                        on_disconnected_from_room.Invoke(disconnected_message.PlayerLeaveRoomCause);

                        break;
                    }
            }
        }

        public void CreateRoom(string room_name, string room_id, object join_data = null, 
            Action on_success = null, Action<ServerControllerError> on_fail = null)
        {
            if (client.Connected && connected)
            {
                on_create_room_success.UnSubscribeAll();
                on_create_room_success.Subscribe(on_success);

                on_create_room_fail.UnSubscribeAll();
                on_create_room_fail.Subscribe(on_fail);

                byte[] data = Parsers.ByteParser.ComposeObject(new CreateRoomMessage(room_name, room_id, join_data));

                client.SendMessage(data);
            }
        }

        public void JoinRoom(string room_id, object join_data = null, Action on_success = null, Action<ServerControllerError> on_fail = null)
        {
            if (client.Connected && connected)
            {
                on_join_room_success.UnSubscribeAll();
                on_join_room_success.Subscribe(on_success);

                on_join_room_fail.UnSubscribeAll();
                on_join_room_fail.Subscribe(on_fail);

                byte[] data = Parsers.ByteParser.ComposeObject(new JoinRoomMessage(room_id, join_data));

                client.SendMessage(data);
            }
        }

        public void CreateJoinRoom(string room_name, string room_id, object join_data = null, Action on_success = null, Action<ServerControllerError> on_fail = null)
        {
            if (client.Connected && connected)
            {
                on_create_join_room_success.UnSubscribeAll();
                on_create_join_room_success.Subscribe(on_success);

                on_create_join_room_fail.UnSubscribeAll();
                on_create_join_room_fail.Subscribe(on_fail);

                byte[] data = Parsers.ByteParser.ComposeObject(new CreateJoinRoomMessage(room_name, room_id, join_data));

                client.SendMessage(data);
            }
        }

        public void LeaveRoom()
        {
            if (client.Connected && connected)
            {
                if (connected_to_room)
                {
                    byte[] data = Parsers.ByteParser.ComposeObject(new LeaveRoomMessage());

                    client.SendMessage(data);
                }
            }

            connected_to_room = false;
            connected_room_id = "";
        }

        public Callback<PlayerLeaveRoomCause> OnDisconnectedFromRoom
        {
            get { return on_disconnected_from_room; }
        }

        public void SendMessage(object obj_to_send)
        {
            if (client.Connected && connected)
            {
                if (connected_to_room)
                {
                    byte[] data = Parsers.ByteParser.ComposeObject(new DataMessage(obj_to_send));

                    client.SendMessage(data);
                }
            }
        }
    }
}
