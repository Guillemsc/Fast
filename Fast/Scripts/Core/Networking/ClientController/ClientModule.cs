using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public class ClientModule
    {
        private ClientController client_controller = null;

        public ClientModule(ClientController client_controller)
        {
            this.client_controller = client_controller;
        }

        public ClientController ClientController
        {
            get { return client_controller; }
        }

        public virtual void OnConnect()
        {

        }

        public virtual void OnDisconnect()
        {

        }

        public virtual void OnMessageReceived(ServerControllerMessage server_message)
        {

        }
    }
}
