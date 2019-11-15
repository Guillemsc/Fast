using System;

namespace Fast.Networking
{
    public class ServerModule
    {
        private ServerController server_controller = null;

        public ServerModule(ServerController server_controller)
        {
            this.server_controller = server_controller;
        }

        public ServerController ServerController
        {
            get { return server_controller; }
        }

        public virtual void Start()
        {

        }

        public virtual void OnPlayerConnected(Player player)
        {

        }

        public virtual void OnPlayerDisconnected(Player player)
        {

        }

        public virtual void OnMessageReceived(Player player, ServerControllerMessage server_message)
        {

        }
    }
}
