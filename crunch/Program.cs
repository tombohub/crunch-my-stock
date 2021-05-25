using System;
using MySql.Data.MySqlClient;


namespace Crunch
{
    class Program
    {
        static void Main(string[] args)
        {
            var ko = new Crunch.Env.Variables();
            string Fmp = Env.Variables.FmpApiKey;

            Console.WriteLine($"dasd {Fmp}");


        }
    }

    class DateTimeUtils
    {

    }
}
