using System;
using System.Linq;
using MySql.Data.MySqlClient;


namespace Crunch
{
    class Program
    {
        static void Main(string[] args)
        {
            var lo = new Database.Groups();
            lo.Select();

           
        }
    }

    class DateTimeUtils
    {

    }
}
