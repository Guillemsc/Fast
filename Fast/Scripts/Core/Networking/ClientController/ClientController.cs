using System;
using System.Collections.Generic;

namespace Fast.Networking
{
    class ClientController
    {
        private Client client = null;

        private List<ClientModule> modules = new List<ClientModule>();

        private bool connected = false;

        private object join_data = null;

        private Callback on_connect_to_server_success = new Callback();
        private Callback on_connect_to_server_fail = new Callback();

        private RoomsClientModule rooms_module = null;

        public ClientController(string server_ip, int server_port)
        {
            client = new Client(server_ip, server_port);

            client.OnConnected.Subscribe(OnConnected);
            client.OnDisconnected.Subscribe(OnDisconnected);
            client.OnMessageReceived.Subscribe(OnMessageReceived);

            rooms_module = (RoomsClientModule)AddModule(new RoomsClientModule(this));
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

        private ClientModule AddModule(ClientModule module)
        {
            modules.Add(module);

            return module;
        }

        private void OnConnected()
        {
            byte[] message_data = Parsers.ByteParser.ComposeObject(new CreatePlayerMessage(join_data));

            client.SendMessage(message_data);
        }

        private void OnDisconnected()
        {
            connected = false;

            for (int i = 0; i < modules.Count; ++i)
            {
                modules[i].OnDisconnect();
            }
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

                            for (int i = 0; i < modules.Count; ++i)
                            {
                                modules[i].OnConnect();
                            }

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
            }

            for(int i = 0; i < modules.Count; ++i)
            {
                modules[i].OnMessageReceived(message);
            }
        }

        public void SendMessage(object message_obj)
        {
            if (client.Connected && connected)
            {
                byte[] message_data = Parsers.ByteParser.ComposeObject(message_obj);

                client.SendMessage(message_data);
            }
        }

        public RoomsClientModule MRooms
        {
            get { return rooms_module; }
        }
    }
}
