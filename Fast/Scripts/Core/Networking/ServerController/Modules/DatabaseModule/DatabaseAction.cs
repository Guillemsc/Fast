using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Fast.Networking
{
    public class DatabaseAction
    {
        private string query_text = "";
        private Database.SQLQuery query = null;
        private bool requires_userID = false;

        public DatabaseAction(string query_text, bool requires_userID = true)
        {
            this.query_text = query_text;
            this.requires_userID = requires_userID;
        }
        
        public string QueryText
        {
            get { return query_text; }
            set { query_text = value; }
        }

        public Database.SQLQuery Query
        {
            get { return query; }
        }

        public bool RequiresUserID
        {
            get { return requires_userID; }
        }

        public void Execute(Database.SQLController connection, Dictionary<string, object> parameters, Action<DatabaseAction> on_success, Action on_fail)
        {
            query = new Database.SQLQuery(query_text);

            foreach(KeyValuePair<string, object> param in parameters)
            {
                query.AddParameter(param.Key, param.Value);
            }

            ExecuteInternal(connection, on_success, on_fail);
        }

        protected virtual void ExecuteInternal(Database.SQLController connection, Action<DatabaseAction> on_success, Action on_fail)
        {

        }
    }
}
