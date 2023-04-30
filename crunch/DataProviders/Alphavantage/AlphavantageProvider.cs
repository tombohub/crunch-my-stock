using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Crunch.Core;
using Crunch.DataProviders.Alphavantage.Responses;
using CsvHelper;

namespace Crunch.DataProviders.Alphavantage
{
    internal class AlphavantageProvider
    {
        private const string _apiKey = "***REMOVED***";
        private readonly WebClient _webClient = new WebClient();

        /// <summary>
        ///  returns a list of active US stocks and ETFs.
        /// </summary>
        /// <returns></returns>
        public List<ListingStatusActiveResponse> ListingStatusActive()
        {
            string url = $"https://www.alphavantage.co/query?function=LISTING_STATUS&apikey={_apiKey}";
            var records = RecordsFromCsvUrl<ListingStatusActiveResponse>(url);
            return records;
        }

        /// <summary>
        /// Returns a list of delisted US stocks and ETFs
        /// </summary>
        /// <returns></returns>
        public List<ListingStatusDelistedResponse> ListingStatusDelisted()
        {
            string url = $"https://www.alphavantage.co/query?function=LISTING_STATUS&state=delisted&apikey={_apiKey}";
            var records = RecordsFromCsvUrl<ListingStatusDelistedResponse>(url);
            return records;
        }

        /// <summary>
        /// Deserializes csv data from url into list of objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        private List<T> RecordsFromCsvUrl<T>(string url)
        {
            Stream stream = _webClient.OpenRead(url);
            using StreamReader streamReader = new StreamReader(stream);
            using CsvReader csv = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<T>();
            return records.ToList();
        }

        /// <summary>
        /// Map response values for exchange to Exchange enum
        /// </summary>
        /// <param name="exchange"></param>
        /// <returns></returns>
        public Exchange MapExchange(string exchange)
        {
            var mapping = new Dictionary<string, Exchange>
            {
                {"NASDAQ", Exchange.Nasdaq},
                {"NYSE", Exchange.Nyse },
                {"NYSE ARCA", Exchange.NyseArca },
                {"NYSE MKT", Exchange.NyseAmerican },
                {"BATS", Exchange.Bats }
            };

            return mapping[exchange];
        }

        public SecurityType MapSecurityType(string securityType)
        {
            var mapping = new Dictionary<string, SecurityType>
            {
                {"Stock", SecurityType.Stock },
                {"ETF", SecurityType.Etf }
            };

            return mapping[securityType];
        }
    }
}