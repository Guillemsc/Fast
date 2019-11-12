using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PlayerClusterName : Attribute
    {
        private string name = "";

        public PlayerClusterName(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }
    }

    class PlayerCluster
    {
        private ServerController server = null;

        public void Init(ServerController server)
        {
            this.server = server;
        }

        public ReadOnlyCollection<Player> ConnectedPlayers
        {
            get { return server.Players; }
        }

        public void PlayerConnected(Player player)
        {

        }

        public void PlayerDisconnected(Player player)
        {

        }

        public virtual void OnPlayerConnected(Player player)
        {

        }

        public virtual void OnPlayerDisconnected(Player player)
        {

        }

    }
}
