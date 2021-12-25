using MySql.Data.MySqlClient;
using Npgsql;
using System;


namespace Crunch.Database
{
    /// <summary>
    /// Providing the connection to MySql production database
    /// </summary>
    class DbConnections
    {
        /// <summary>
        /// MySql connection object
        /// </summary>
        public MySqlConnection MySqlConnection { get; set; }

        /// <summary>
        /// PostgreSQL connection object
        /// </summary>
        public NpgsqlConnection PsqlConnection { get; set; }

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

        /// <summary>
        /// Create PostgreSQL remote connection
        /// </summary>
        /// <returns></returns>
        public static NpgsqlConnection CreatePsqlConnection()
        {
            string dbUri = Configuration.PostgresSQLConnectionString;
            return new NpgsqlConnection(dbUri);
        }

        public void GetData()
        {
            MySqlConnection.Open();
            string sql = "SELECT * FROM `groups`";
            var cmd = new MySqlCommand(sql, MySqlConnection);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine(rdr[0] + "-----" + rdr[1]);
            }
            rdr.Close();
            MySqlConnection.Close();
        }

        public void Insert(string text)
        {
            MySqlConnection.Open();
            string sql = "insert into `test` values(@text)";
            var cmd = new MySqlCommand(sql, MySqlConnection);
            cmd.Parameters.AddWithValue("@text", text);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}
