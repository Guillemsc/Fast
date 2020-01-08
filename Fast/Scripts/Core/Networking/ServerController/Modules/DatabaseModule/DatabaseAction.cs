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
        DBA_Post,
        DBA_Get,
    }

    public class DatabaseAction
    {
        private DatabaseActionTypes type = new DatabaseActionTypes();
        private Database.SQLQuery query = null;
        private DatabaseActionExecuteType execute_type = new DatabaseActionExecuteType();
        private bool requires_userID = false;

        public DatabaseAction(DatabaseActionTypes type, DatabaseActionExecuteType execute_type, Database.SQLQuery query, bool requires_userID = true)
        {
            this.query = query;
            this.type = type;
            this.execute_type = execute_type;
            this.requires_userID = requires_userID;
        }

        public Database.SQLQuery Query
        {
            get { return query; }
        }

        public DatabaseActionTypes ActionType
        {
            get { return type; }
        }

        public bool RequiresUserID
        {
            get { return requires_userID; }
        }

        public DatabaseActionExecuteType ExecuteType
        {
            get { return execute_type; }
        }

        public void Execute(Database.SQLController connection, Dictionary<string, object> parameters)
        {
            foreach(KeyValuePair<string,object> param in parameters)
            {
                query.AddParameter(param.Key, param.Value);
            }

            ExecuteInternal();
        }

        protected virtual void ExecuteInternal()
        {

        }
    }
}
