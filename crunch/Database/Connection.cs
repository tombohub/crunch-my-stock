using Crunch.Domain.OhlcPrice;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Crunch.Database
{
    class Connection
    {
        private readonly MySqlConnection conn;

        public Connection()
        {
            string dbUri = Env.Variables.DatabaseURI;
            conn = new MySqlConnection(dbUri);
        }

        public void GetData()
        {
            conn.Open();
            string sql = "SELECT * FROM `groups`";
            var cmd = new MySqlCommand(sql, conn);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine(rdr[0] + "-----" + rdr[1]);
            }
            rdr.Close();
            conn.Close();
        }

        public void SavePrices(List<Price> prices)
        {
            string sql = "insert into ";
            var cmd = new MySqlCommand(sql, conn);
            conn.Close();
        }

        public void Insert(string text)
        {
            conn.Open();
            string sql = "insert into `test` values(@text)";
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@text", text);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}
