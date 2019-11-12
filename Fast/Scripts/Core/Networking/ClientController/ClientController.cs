using System;
using System.Collections.Generic;

namespace Fast.Networking
{
    class ClientController
    {
        private Client client = null;

        private List<object> messages_to_read = new List<object>();

        private bool connected_to_room = false;
        private string connected_room_id = "";

        private Callback on_connected_to_server = new Callback();

        private Callback on_create_room_success = new Callback();
        private Callback<ServerControllerError> on_create_room_fail = new Callback<ServerControllerError>();

        private Callback on_join_room_success = new Callback();
        private Callback<ServerControllerError> on_join_room_fail = new Callback<ServerControllerError>();

        private Callback on_create_join_room_success = new Callback();
        private Callback<ServerControllerError> on_create_join_room_fail = new Callback<ServerControllerError>();

        private Callback on_disconnected_from_room = new Callback();

        public ClientController(string server_ip, int server_port)
        {
            client = new Client(server_ip, server_port);

            client.OnConnected.Subscribe(OnConnected);
            client.OnDisconnected.Subscribe(OnDisconnected);
            client.OnMessageReceived.Subscribe(OnMessageReceived);
        }

        public void Start(Action on_connected = null)
        {
            on_connected_to_server.UnSubscribeAll();
            on_connected_to_server.Subscribe(on_connected);

            client.Connect();
        }

        public void Update()
        {
            client.ReadMessages();
        }

        private void OnConnected()
        {
            on_connected_to_server.Invoke();

            Logger.ClientLogInfo("Connected to server");
        }

        private void OnDisconnected()
        {
            Logger.ClientLogInfo("Disconnected from server");
        }

        private void OnMessageReceived(byte[] message_data)
        {
            ServerControllerMessage message = Parsers.ByteParser.ParseObject<ServerControllerMessage>(message_data);

            switch (message.Type)
            {
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
                        on_disconnected_from_room.Invoke();

                        break;
                    }
            }
        }

        public void CreateRoom(string room_name, string room_id, Action on_success = null, Action<ServerControllerError> on_fail = null)
        {
            if (client.Connected)
            {
                on_create_room_success.UnSubscribeAll();
                on_create_room_success.Subscribe(on_success);

                on_create_room_fail.UnSubscribeAll();
                on_create_room_fail.Subscribe(on_fail);

                byte[] data = Parsers.ByteParser.ComposeObject(new CreateRoomMessage(room_name, room_id));

                client.SendMessage(data);
            }
        }

        public void JoinRoom(string room_id, Action on_success = null, Action<ServerControllerError> on_fail = null)
        {
            if (client.Connected)
            {
                on_join_room_success.UnSubscribeAll();
                on_join_room_success.Subscribe(on_success);

                on_join_room_fail.UnSubscribeAll();
                on_join_room_fail.Subscribe(on_fail);

                byte[] data = Parsers.ByteParser.ComposeObject(new JoinRoomMessage(room_id));

                client.SendMessage(data);
            }
        }

        public void CreateJoinRoom(string room_name, string room_id, Action on_success = null, Action<ServerControllerError> on_fail = null)
        {
            if (client.Connected)
            {
                on_create_join_room_success.UnSubscribeAll();
                on_create_join_room_success.Subscribe(on_success);

                on_create_join_room_fail.UnSubscribeAll();
                on_create_join_room_fail.Subscribe(on_fail);

                byte[] data = Parsers.ByteParser.ComposeObject(new CreateJoinRoomMessage(room_name, room_id));

                client.SendMessage(data);
            }
        }

        public void LeaveRoom()
        {
            if (client.Connected)
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

        public void SendMessage(object obj_to_send)
        {
            if (client.Connected)
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
