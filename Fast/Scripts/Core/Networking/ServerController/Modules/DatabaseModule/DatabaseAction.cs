using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Fast.Networking
{
    public enum DatabaseActionExecuteType
    {
        POST,
        GET,
    }

    public class DatabaseAction
    {
        private Database.SQLQuery query = null;
        private DatabaseActionExecuteType execute_type = new DatabaseActionExecuteType();
        private bool requires_userID = false;

        public DatabaseAction(Database.SQLQuery query, DatabaseActionExecuteType execute_type, bool requires_userID = true)
        {
            this.query = query;
            this.execute_type = execute_type;
            this.requires_userID = requires_userID;
        }

        public Database.SQLQuery Query
        {
            get { return query; }
        }

        public bool RequiresUserID
        {
            get { return requires_userID; }
        }

        public DatabaseActionExecuteType ExecuteType
        {
            get { return execute_type; }
        }

        public void Execute(Database.SQLController connection, Dictionary<string, object> parameters, Action<DatabaseAction> on_success, Action on_fail)
        {
            foreach(KeyValuePair<string, object> param in parameters)
            {
                query.AddParameter(param.Key, param.Value);
            }

            ExecuteInternal(on_success, on_fail);
        }

        protected virtual void ExecuteInternal(Action<DatabaseAction> on_success, Action on_fail)
        {

        }
    }
}
