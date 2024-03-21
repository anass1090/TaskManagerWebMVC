using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TaskManager.DAL.Connection
{
    public class DataAccess
    {
        private readonly string ConnectionString = "Server=localhost;Port=3306;Database=taskmanager;Uid=root;Pwd=;\r\n";
        public MySqlConnection Connection { get; private set; }

        public DataAccess()
        {
            Connection = new MySqlConnection(ConnectionString);
        }

        public void OpenConnection()
        {
            Connection.Open();
        }

        public void CloseConnection()
        {
            Connection.Close();
        }
    }
}
