using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace Fast.Database
{
    public class SQLConnectObject
    {
        public string server = "";
        public string database = "";
        public string user = "";
        public string password = "";
    }

    public class SQLError
    {
        public string ErrorMessage { get; set; }
        public Exception ErrorException { get; set; }
    }

    public class SQLController
    {
        private SQLConnectObject connection_obj = null;

        private MySqlConnection connection = null;

        private string connection_string = "";

        private bool connected = false;

        public SQLController()
        {

        }

        public void Connect(SQLConnectObject connection_obj, Action on_success, Action<SQLError> on_fail)
        {
            this.connection_obj = connection_obj;

            if (connection != null)
            {
                connection.Dispose();
            }

            if (connection_obj != null)
            {
                connection_string = "server=" + connection_obj.server + "; user id=" + connection_obj.user + "; password=" +
                    "" + connection_obj.password + "; database=" + connection_obj.database + "; pooling=false;";

                connection = new MySqlConnection(connection_string);

                connection.OpenAsync().ContinueWith(delegate (Task task)
                {
                    if (task.IsCompleted)
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                        {
                            connected = true;

                            if (on_success != null)
                                on_success.Invoke();
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        if(task.IsFaulted)
                        {

                        }
                        else if(task.IsCanceled)
                        {

                        }
                    }

                });
            }
        }

        public void Disconect()
        {
            if (connection != null)
            {
                connection.Close();

                connection.Dispose();

                connection = null;

                connected = false;
            }
        }

        public void ExecuteGetQuery(string sql_query, Action<Data.GridData> on_success = null, Action<SQLError> on_fail = null)
        {
            if (connection != null)
            {
                if (connected)
                {
                    MySqlCommand adapter = new MySqlCommand(sql_query, connection);

                    adapter.ExecuteReaderAsync().ContinueWith(delegate (Task<DbDataReader> task)
                    {
                        string error_msg = "";
                        Exception exception = null;

                        bool has_errors = task.HasErrors(out error_msg, out exception);

                        if (!has_errors)
                        {
                            List<List<object>> query_data = new List<List<object>>();

                            if (task.Result.HasRows)
                            {
                                while (task.Result.Read())
                                {
                                    List<object> row = new List<object>(task.Result.FieldCount);

                                    int fields = task.Result.FieldCount;

                                    for (int i = 0; i < fields; ++i)
                                    {
                                        object curr_field = task.Result.GetValue(i);

                                        row.Add(curr_field);
                                    }

                                    query_data.Add(row);
                                }
                            }

                            task.Result.Close();

                            Data.GridData ret = new Data.GridData(query_data);

                            if (on_success != null)
                                on_success.Invoke(ret);
                        }
                        else
                        {
                            SQLError ret = new SQLError();
                            ret.ErrorMessage = error_msg;
                            ret.ErrorException = exception;

                            if (on_fail != null)
                                on_fail.Invoke(ret);
                        }
                    });
                }
            }
        }

        public void ExecutePostQuery(string sql_query, Action on_success = null, Action<SQLError> on_fail = null)
        {
            if (connection != null)
            {
                if (connected)
                {
                    MySqlCommand adapter = new MySqlCommand(sql_query, connection);

                    adapter.ExecuteNonQueryAsync().ContinueWith(delegate (Task task)
                    {
                        string error_msg = "";
                        Exception exception = null;

                        bool has_errors = task.HasErrors(out error_msg, out exception);

                        if (!has_errors)
                        {
                            if (on_success != null)
                                on_success.Invoke();
                        }
                        else 
                        {
                            SQLError ret = new SQLError();
                            ret.ErrorMessage = error_msg;
                            ret.ErrorException = exception;

                            if (on_fail != null)
                                on_fail.Invoke(ret);
                        }
                    });
                }
            }
        }
    }
}
