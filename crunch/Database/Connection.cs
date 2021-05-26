using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;


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
    }
}
