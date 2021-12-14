using MySql.Data.MySqlClient;
using System;


namespace Crunch.Database
{
    /// <summary>
    /// Providing the connection to MySql production database
    /// </summary>
    class MySqlDatabase
    {
        public MySqlConnection Conn { get; set; }

        public MySqlDatabase()
        {
            string dbUri = Configuration.DbConnectionString;
            Conn = new MySqlConnection(dbUri);
        }

        /// <summary>
        /// Create MySql connection object
        /// </summary>
        /// <returns></returns>
        public static MySqlConnection GetConnection()
        {
            string dbUri = Configuration.DbConnectionString;
            return new MySqlConnection(dbUri);
        }

        /// <summary>
        /// Create local MySql database connection object
        /// </summary>
        /// <returns></returns>
        public static MySqlConnection GetLocalConnection()
        {
            string dbUri = Configuration.DbLocalConnectionString;
            return new MySqlConnection(dbUri);
        }

        public void GetData()
        {
            Conn.Open();
            string sql = "SELECT * FROM `groups`";
            var cmd = new MySqlCommand(sql, Conn);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine(rdr[0] + "-----" + rdr[1]);
            }
            rdr.Close();
            Conn.Close();
        }

        public void Insert(string text)
        {
            Conn.Open();
            string sql = "insert into `test` values(@text)";
            var cmd = new MySqlCommand(sql, Conn);
            cmd.Parameters.AddWithValue("@text", text);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}
