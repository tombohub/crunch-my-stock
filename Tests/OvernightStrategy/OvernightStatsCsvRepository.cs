using Crunch.Domain;
using Crunch.Strategies.Overnight;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CrunchTests.OvernightStrategy
{
    /// <summary>
    /// Overight stats repository base on csv file exported from database
    /// </summary>
    class OvernightStatsCsvRepository
    {
        /// <summary>
        /// Create OvernightStats Entity from data in csv file
        /// </summary>
        /// <returns></returns>
        public static OvernightStats GetOvernightStats()
        {
            var reader = new StreamReader("OvernightStrategy/CsvData/OvernightStatsWeek37.csv");
            var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            var records = csv.GetRecords<OvernightStatsCsvModel>().ToList();

            //HACK: lot of code duplicated from DB repository

            List<OvernightStatsCsvModel> overnightStats = records
                .Where(s => s.Strategy == "overnight")
                .ToList();
            List<OvernightStatsCsvModel> benchmarkStats = records
                .Where(s => s.Strategy == "benchmark")
                .ToList();
            var statsCsv = overnightStats
                .Join(benchmarkStats, overnight => overnight.Symbol, benchmark => benchmark.Symbol,
                    (overnight, benchmark) => new
                    {
                        Symbol = overnight.Symbol,
                        SecurityType = overnight.SecurityType,
                        OvernightRoi = overnight.Roi,
                        BenchmarkRoi = benchmark.Roi
                    })
                .ToList();

            var stats = new List<SingleSymbolStats>();
            foreach (var statCsv in statsCsv)
            {
                SecurityType securityType = statCsv.SecurityType switch
                {   // hack: values are from database
                    "stocks" => SecurityType.Stock,
                    "etfs" => SecurityType.Etf,
                    _ => throw new NotImplementedException()
                };

                stats.Add(new SingleSymbolStats
                (
                    Symbol: statCsv.Symbol,
                    SecurityType: securityType,
                    OvernightRoi: statCsv.OvernightRoi,
                    BenchmarkRoi: statCsv.BenchmarkRoi
                ));
            }

            var overnightStatsEntity = new OvernightStats(stats);
            return overnightStatsEntity;
        }
    }
}
