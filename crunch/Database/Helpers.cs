using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Crunch.Database
{
    /// <summary>
    /// General methods for performing various tasks
    /// </summary>
    internal class Helpers
    {
        /// <summary>
        /// Truncate the prices table. 
        /// </summary>
        /// <remarks>Reasong for truncating is because many times inserted prices
        /// are with same symbol and date, which is unique key. In that case MySql creates error</remarks>
        public static void TruncatePricesTable()
        {
            try
            {
                var db = new stock_analyticsContext();
                db.Database.ExecuteSqlRaw("TRUNCATE TABLE prices");
                db.Dispose();
                Console.WriteLine("Prices table truncated");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static List<string> GetSecuritySymbols()
        {
            var db = new stock_analyticsContext();
            var symbols = db.Securities.Select(s => s.Symbol).ToList();
            return symbols;
        }
    }
}
