using Crunch.Domain;
using Dapper;

namespace Crunch.Database
{
    internal class DailyPriceSetRepository
    {
        // TODO: unfinished method for inserting into prices_daily using Dapper
        public void SaveWithDapper(DailyPriceSet priceSet, PriceInterval interval)
        {
            string sql = @"insert into
                public.prices_daily (date, symbol, open, high, low, close, volume, interval)
                VALUES (@Date, @Symbol, @Open, @High, @Low, @Close, @Volume, @Interval)
                ON CONFLICT ON CONSTRAINT date_symbol_un
                DO UPDATE SET
                              open = @Open,
                              high = @High,
                              low = @Low,
                              close = @Close,
                              volume = @Volume;";
            using var conn = DbConnections.CreatePsqlConnection();
            foreach (var price in priceSet.Prices)
            {
                var parameters = new
                {
                    Date = price.Timestamp,
                    Symbol = priceSet.Symbol,
                    Open = price.Open,
                    High = price.High,
                    Low = price.Low,
                    Close = price.Close,
                    Volume = price.Volume,
                    Interval = priceSet.Interval.ToString()
                };

                conn.Execute(sql, parameters);
            }
        }

        //public DailyPriceSet Load(string symbol, PriceInterval interval, DateOnly start, DateOnly end)
        //{
        //    string intervalDb = Helpers.PriceIntervalToString(interval);

        //    var pricesDb = new List<PricesDaily>();
        //    using (_db = new stock_analyticsContext())
        //    {
        //        pricesDb = _db.PricesDailies
        //            .Where(price => price.Symbol == symbol)
        //            .Where(price => price.Interval == intervalDb)
        //            .Where(price => (price.Timestamp >= start) && (price.Timestamp <= end))
        //            .ToList();
        //    }

        //    Console.WriteLine(pricesDb);

        //    var priceSet = new List<PriceDaily>();
        //    foreach (var priceDb in pricesDb)
        //    {
        //        var price = new PriceDaily(
        //            Timestamp: priceDb.Timestamp,
        //            Open: priceDb.Open,
        //            High: priceDb.High,
        //            Low: priceDb.Low,
        //            Close: priceDb.Close,
        //            Volume: priceDb.Volume);
        //        priceSet.Add(price);
        //    }

        //    return new DailyPriceSet(
        //        Symbol: symbol,
        //        Interval: interval,
        //        Prices: priceSet);
        //}
    }
}