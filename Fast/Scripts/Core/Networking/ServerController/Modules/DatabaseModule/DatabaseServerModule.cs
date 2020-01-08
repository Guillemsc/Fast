using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    class DatabaseServerModule : ServerModule
    {
        private Dictionary<DatabaseActionTypes, DatabaseAction> actions = new Dictionary<DatabaseActionTypes, DatabaseAction>();
        private Fast.Database.SQLController sql_controller = null;

        private bool allow_execute = false;

        public DatabaseServerModule(ServerController serverController) : base(serverController)
        {
            sql_controller = new Database.SQLController();
        }

        public override void Start()
        {
            AddActions();

            sql_controller.Connect(ServerController.SQLInfo, 
            delegate()
            {
                allow_execute = true;

                Logger.ServerLogInfo(ToString() + "Successfully connected to the Database!");
            }
            , delegate(Database.SQLError error)
            {
                Logger.ServerLogError(ToString() + "Error connectiong to SQL Database: " + error.ErrorMessage);
            });
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

        public override void OnMessageReceived(Player player, ServerControllerMessage server_message)
        {
            switch (server_message.Type)
            {
                case ServerControllerMessageType.DATABASE_REQUEST:
                    {
                        DatabaseRequestMessage msg = (DatabaseRequestMessage)server_message;
                        ExecuteQuery(player, msg.Type, msg.Parameters);

                        break;
                    }
                default:
                    break;
            }
        }

        public void ExecuteQuery(Player player, DatabaseActionTypes type, Dictionary<string, object> parameters)
        {
            Task.Factory.StartNew(() => OnExecuteQuery(player, type, parameters)).
                ContinueWith(delegate (Task execute_task)
                {
                    string error_msg = "";
                    Exception exception = null;

                    bool has_errors = execute_task.HasErrors(out error_msg, out exception);

                    if (has_errors)
                    {
                        if (exception != null)
                        {
                            Logger.ServerLogError(ToString() + "OnExecuteQuery(): " + execute_task.Exception.Message);
                        }
                        else
                        {
                            Logger.ServerLogError(ToString() + "OnExecuteQuery(): " + "Task has errors");
                        }
                    }
                });
        }
        
        private void OnExecuteQuery(Player player, DatabaseActionTypes type, Dictionary<string,object> parameters)
        {
            if (actions[type].RequiresUserID)
            {
                parameters.Add("@userid", player.DatabaseID);
            }

            actions[type].Execute(sql_controller, parameters);
        }

        public override string ToString()
        {
            return "Database Module: ";
        }
    }
}
