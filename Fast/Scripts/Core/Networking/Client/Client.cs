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

        private bool connected = false;

        private Callback on_connected = new Callback();
        private Callback on_disconnected = new Callback();
        private Callback<byte[]> on_message_received = new Callback<byte[]>();

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

        public Callback<byte[]> OnMessageReceived
        {
            get { return on_message_received; }
        }

        public void SendMessage(byte[] data)
        {
            if (connected)
            {
                client.Send(data);
            }
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
                            on_message_received.Invoke(msg.data);

                            break;
                        }
                }
            }
        }
    }
}
