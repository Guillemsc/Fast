using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Collections.ObjectModel;

namespace Fast.Database
{
    public class SQLQuery
    {
        private string query = "";

        private Dictionary<string, object> parameters = new Dictionary<string, object>();

        public SQLQuery(string query)
        {
            this.query = query;
        }

        public void AddParameter(string key, object value)
        {
            parameters.Add(key, value);
        }

        public string Query
        {
            get { return query; }
        }

        public Dictionary<string, object> Parameters
        {
            get { return parameters; }
        }
    }
}
