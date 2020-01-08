using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    class DatabaseServerModule : ServerModule
    {
        private Fast.Database.SQLController sql_controller = null;

        private bool allow_execute = false;

        public DatabaseServerModule(ServerController serverController) : base(serverController)
        {
            sql_controller = new Database.SQLController();
        }

        public override void Start()
        {
            sql_controller.Connect(ServerController.SQLInfo,
            delegate ()
            {
                allow_execute = true;

                Logger.ServerLogInfo(ToString() + "Successfully connected to the Database!");
            }
            , delegate (Database.SQLError error)
            {
                Logger.ServerLogError(ToString() + "Error connectiong to SQL Database: " + error.ErrorMessage);
            });
        }

        public void ExecuteQuery(Player player, DatabaseAction action, Dictionary<string, object> parameters, Action<DatabaseAction> on_success, Action on_fail)
        {
            if (action.RequiresUserID)
            {
                parameters.Add("@userid", player.DatabaseID);
            }

            Task.Factory.StartNew(() => action.Execute(sql_controller, parameters, on_success, on_fail)).
                ContinueWith(delegate (Task execute_task)
                {
                    string error_msg = "";
                    Exception exception = null;

                    bool has_errors = execute_task.HasErrors(out error_msg, out exception);

                    if (has_errors)
                    {
                        Logger.ServerLogError(ToString() + "OnExecuteQuery(): " + error_msg);

                        if(on_fail != null)
                            on_fail.Invoke();
                    }
                });
        }

        public override string ToString()
        {
            return "Database Module: ";
        }
    }
}
