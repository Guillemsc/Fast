using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Fast.Networking
{
    public enum DatabaseActionExecutetype
    {
        DBA_Post,
        DBA_Get,
    }

    public abstract class DatabaseAction
    {
        private DatabaseActionTypes type;
        private Database.SQLQuery query;
        private DatabaseActionExecutetype execute_type;
        private bool requires_userID;

        public DatabaseAction(DatabaseActionTypes type, DatabaseActionExecutetype execute_type, Database.SQLQuery query, bool requires_userID = true)
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

        public abstract void Execute(Database.SQLController connection, Dictionary<string, object> parameters);
    }
}
