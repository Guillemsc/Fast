using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Fast.MySQL
{
    class MySQLTests
    {
        public MySQLTests()
        {

        }

        public void Start()
        {
            string server = "remotemysql.com";
            string database = "yrzo1rYYTH";
            string user = "yrzo1rYYTH";
            string password = "3FtD6vlDvi";


            //MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(connection_string);

            //connection.OpenAsync().ContinueWith(delegate (Task task)
            //{
            //    if (connection.State == System.Data.ConnectionState.Open)
            //    {
            //        int i = 0;
            //    }

            //});
        }
    }
}
