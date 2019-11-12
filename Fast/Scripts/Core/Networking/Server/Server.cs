using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    /// <summary>
    /// This server abstraction now only holds TCP connections with Telepathy, 
    /// but in the future could also hold other types of conneciton
    /// </summary>
    public class Server 
    {
        private Telepathy.Server server = new Telepathy.Server();

        private int port = 0;

        private bool started = false;

        private Callback<int> on_client_connected = new Callback<int>();
        private Callback<int> on_client_disconnected = new Callback<int>();
        private Callback<ServerMessage> on_message_received = new Callback<ServerMessage> ();

        public Server(int port)
        {
            this.port = port;
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

        public Callback<int> OnClientConnected
        {
            get { return on_client_connected; }
        }

        public Callback<int> OnClientDisconnected
        {
            get { return on_client_disconnected; }
        }

        public Callback<ServerMessage> OnMessageReceived
        {
            get { return on_message_received; }
        }

        public void SendMessage(int client_id, byte[] data)
        {
            if (started)
            {
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
                            on_client_connected.Invoke(msg.connectionId);
                        }
                        break;

                    case Telepathy.EventType.Disconnected:
                        {                            
                            on_client_disconnected.Invoke(msg.connectionId);
                        }
                        break;

                    case Telepathy.EventType.Data:
                        {
                            ServerMessage message = new ServerMessage(msg.connectionId, msg.data);

                            on_message_received.Invoke(message);
                        }
                        break;
                }
            }
        }
    }
}
