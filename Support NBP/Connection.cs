using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace SupportNBP
{
    class Connection
    {
        private MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;database=ipbank");

        public MySqlConnection GetConnection()
        {
            return connection;
        }

        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public void OpenConnection()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }
    }
}
