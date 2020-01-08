using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    class DatabaseServerModule : ServerModule
    {
        private Fast.Database.SQLController sql_controller;

        bool allow_execute = false;

        public DatabaseServerModule(ServerController serverController) : base(serverController)
        {
            sql_controller = new Database.SQLController();
        }

        public override void Start()
        {
            sql_controller.Connect(ServerController.SQLInfo, OnConnectionSuccess, OnConnectionFail);
        }

        private void OnConnectionSuccess()
        {
            allow_execute = true;
            Logger.ServerLogInfo(ToString() + "Successfully connected to the Database!");
        }

        private void OnConnectionFail(Database.SQLError error)
        {
            Logger.ServerLogError(ToString() + "Error connectiong to SQL Database");
        }

        public void ExecuteQuery(Player player, DatabaseAction action, Dictionary<string, object> parameters)
        {
            Task.Factory.StartNew(() => OnExecuteQuery(player, action, parameters)).
                ContinueWith(delegate (Task execute_task)
                {
                    if (execute_task.IsFaulted || execute_task.IsCanceled)
                    {
                        if (execute_task.Exception != null)
                        {
                            Logger.ServerLogError(ToString() + "OnExecuteQuery(): " + execute_task.Exception);
                        }
                        else
                        {
                            Logger.ServerLogError(ToString() + "OnExecuteQuery(): " + "Task faulted or cancelled");
                        }
                    }
                });
        }
        
        private void OnExecuteQuery(Player player, DatabaseAction action, Dictionary<string,object> parameters)
        {
            if (action.RequiresUserID)
            {
                parameters.Add("@userid", player.DatabaseID);
            }
            action.Execute(sql_controller, parameters);
        }

        public override string ToString()
        {
            return "Database Module: ";
        }
    }
}
