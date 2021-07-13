using System;
using System.Linq;
using System.Text.Json;
using Crunch.Core;
using MySql.Data.MySqlClient;


namespace Crunch
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.Write(PriceInterval.OneDay);
            DateTime k = new DateTime();
            LocalDataStoreSlot l = new LocalDataStoreSlot();



        }
    }

    
    class Go
    {
        public string ko;
        public string lo;

        public Go()
        {
            ko = "kosko";
            lo = "kosko";
        }

    }

    class Mo<T>
    {
        public T l;
        public Mo(T col)
        {
            l = col;
        }
    }
}
