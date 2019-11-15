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

        public void PlayerConnected(Player player, object join_data)
        {
            //Task.Factory.StartNew(() => OnPlayerConnected(player, join_data)).
            //ContinueWith(delegate (Task player_connected_task)
            //{
            //    if(player_connected_task.IsCanceled || player_connected_task.IsFaulted)
            //    {
            //        if (player_connected_task.Exception != null)
            //        {
            //            Logger.ServerLogError("Player Cluster:" + " OnPlayerConnected(): " + player_connected_task.Exception);

            //            server.OnServerException(player_connected_task.Exception);
            //        }
            //        else
            //        {
            //            Logger.ServerLogError("Player Cluster:" + " OnPlayerConnected(): " + "Task faulted or cancelled");
            //        }
            //    }
            //});
        }

        public void PlayerDisconnected(Player player)
        {
            //Task.Factory.StartNew(() => OnPlayerDisconnected(player)).
            //ContinueWith(delegate (Task player_disconnected_task)
            //{
            //    if (player_disconnected_task.IsCanceled || player_disconnected_task.IsFaulted)
            //    {
            //        if (player_disconnected_task.Exception != null)
            //        {
            //            Logger.ServerLogError("Player Cluster:" + " OnPlayerDisconnected(): " + player_disconnected_task.Exception);

            //            server.OnServerException(player_disconnected_task.Exception);
            //        }
            //        else
            //        {
            //            Logger.ServerLogError("Player Cluster:" + " OnPlayerDisconnected(): " + "Task faulted or cancelled");
            //        }
            //    }
            //});
        }

        public virtual void OnPlayerConnected(Player player, object join_data)
        {

        }

        public virtual void OnPlayerDisconnected(Player player)
        {

        }

        public virtual void OnServerException(Exception exception)
        {

        }
    }
}
