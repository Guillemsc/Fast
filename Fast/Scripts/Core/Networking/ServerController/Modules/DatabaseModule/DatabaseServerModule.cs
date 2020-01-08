using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    class DatabaseServerModule : ServerModule
    {
        Dictionary<DatabaseActionTypes, DatabaseAction> actions;
        private Fast.Database.SQLController sql_controller;

        bool allow_execute = false;

        public DatabaseServerModule(ServerController serverController) : base(serverController)
        {
            sql_controller = new Database.SQLController();
        }

        public override void Start()
        {
            AddActions();
            sql_controller.Connect(ServerController.SQLInfo, OnConnectionSuccess, OnConnectionFail);
        }

        private void AddActions()
        {
            //Fill with actions to add
        }

        public void AddAction(DatabaseAction action)
        {
            lock (actions)
            {
                actions.Add(action.ActionType, action);
            }
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

        public override void OnMessageReceived(Player player, ServerControllerMessage server_message)
        {
            switch (server_message.Type)
            {
                case ServerControllerMessageType.DATABASE_REQUEST:
                    DatabaseRequestMessage msg = (DatabaseRequestMessage)server_message;
                    ExecuteQuery(player, msg.Type, msg.Parameters);
                    break;
                default:
                    break;
            }
        }

        public void ExecuteQuery(Player player, DatabaseActionTypes type, Dictionary<string, object> parameters)
        {
            Task.Factory.StartNew(() => OnExecuteQuery(player, type, parameters)).
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
        
        private void OnExecuteQuery(Player player, DatabaseActionTypes type, Dictionary<string,object> parameters)
        {
            if (actions[type].RequiresUserID)
                parameters.Add("@userid", player.DatabaseID);
            actions[type].Execute(sql_controller, parameters);
        }

        public override string ToString()
        {
            return "Database Module: ";
        }
    }
}
