using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public class Client
    {
        private Telepathy.Client client = new Telepathy.Client();

        private string ip = "";
        private int port = 0;

        private List<object> messages_to_read = new List<object>();

        private bool connected = false;

        private Callback on_connected = new Callback();
        private Callback on_disconnected = new Callback();

        private bool connected_to_room = false;
        private string connected_room_id = "";

        private Callback on_create_room_success = new Callback();
        private Callback on_create_room_fail = new Callback();

        private Callback on_join_room_success = new Callback();
        private Callback on_join_room_fail = new Callback();

        private Callback on_create_join_room_success = new Callback();
        private Callback on_create_join_room_fail = new Callback();

        private Callback on_disconnected_from_room = new Callback();

        public Client(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public void Connect()
        {
            client.Connect(ip, port);
        }

        public void Disconnect()
        {
            client.Disconnect();
        }

        public bool Connected
        {
            get { return connected; }
        }

        public string IP
        {
            get { return ip; }
        }

        public int Port
        {
            get { return port; }
        }

        public Callback OnConnected
        {
            get { return on_connected; }
        }

        public Callback OnDisconnected
        {
            get { return on_disconnected; }
        }

        public void ReadMessages()
        {
            List<object> ret = new List<object>();

            Telepathy.Message msg;
            while (client.GetNextMessage(out msg))
            {
                switch (msg.eventType)
                {
                    case Telepathy.EventType.Connected:
                        {
                            connected = true;

                            on_connected.Invoke();

                            break;
                        }

                    case Telepathy.EventType.Disconnected:
                        {
                            connected = false;

                            on_disconnected.Invoke();

                            break;
                        }

                    case Telepathy.EventType.Data:
                        {
                            Message message = Parsers.ByteParser.ParseObject<Message>(msg.data);

                            switch(message.Type)
                            {
                                case MessageType.DATA:
                                    {
                                        DataMessage data_message = (DataMessage)message;

                                        messages_to_read.Add(data_message.MessageObj);

                                        break;
                                    }

                                case MessageType.CREATE_ROOM_RESPONSE:
                                    {
                                        CreateRoomResponseMessage response_message = (CreateRoomResponseMessage)message;

                                        if(response_message.Success)
                                        {
                                            connected_to_room = true;
                                            connected_room_id = response_message.RoomId;

                                            on_create_room_success.Invoke();
                                        }
                                        else
                                        {
                                            connected_to_room = false;
                                            connected_room_id = "";
                                        }

                                        break;
                                    }

                                case MessageType.JOIN_ROOM_RESPONSE:
                                    {
                                        JoinRoomResponseMessage response_message = (JoinRoomResponseMessage)message;

                                        if(response_message.Success)
                                        {
                                        }
                                        else
                                        {

                                        }

                                        break;
                                    }

                                case MessageType.CREATE_JOIN_ROOM_RESPONSE:
                                    {
                                        CreateJoinRoomResponseMessage response_message = (CreateJoinRoomResponseMessage)message;

                                        if(response_message.Success)
                                        {

                                        }
                                        else
                                        {

                                        }

                                        break;
                                    }

                                case MessageType.DISCONNECTED_FROM_ROOM:
                                    {
                                        on_disconnected_from_room.Invoke();

                                        break;
                                    }
                            }

                            break;
                        }
                }
            }
        }

        public List<object> PopMessages()
        {
            List<object> ret = new List<object>(messages_to_read);

            messages_to_read.Clear();

            return ret;
        }

        public void CreateRoom(string room_name, string room_id, Action on_success = null, Action<NetworkError> on_fail = null)
        {
            if (connected)
            {
                on_create_room_success.UnSubscribeAll();
                on_create_room_success.Subscribe(on_success);

                byte[] data = Parsers.ByteParser.ComposeObject(new CreateRoomMessage(room_name, room_id));

                client.Send(data);
            }
        }

        public void JoinRoom(string room_id, Action on_success = null, Action<NetworkError> on_fail = null)
        {
            if (connected)
            {
                byte[] data = Parsers.ByteParser.ComposeObject(new JoinRoomMessage(room_id));

                client.Send(data);
            }
        }

        public void CreateJoinRoom(string room_name, string room_id, Action on_success = null, Action<NetworkError> on_fail = null)
        {
            if (connected)
            {
                byte[] data = Parsers.ByteParser.ComposeObject(new CreateJoinRoomMessage(room_name, room_id));

                client.Send(data);
            }
        }

        public void LeaveRoom()
        {
            if (connected)
            {
                if (connected_to_room)
                {
                    byte[] data = Parsers.ByteParser.ComposeObject(new LeaveRoomMessage());

                    client.Send(data);
                }
            }

            connected_to_room = false;
            connected_room_id = "";
        }

        public void SendMessage(object obj_to_send)
        {
            if (connected)
            {
                if (connected_to_room)
                {
                    byte[] data = Parsers.ByteParser.ComposeObject(new DataMessage(obj_to_send));

                    client.Send(data);
                }
            }
        }
    }
}
