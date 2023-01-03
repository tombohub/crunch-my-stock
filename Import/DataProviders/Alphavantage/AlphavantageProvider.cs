using System.Globalization;
using System.Net;
using CrunchImport.DataProviders.Alphavantage.Responses;
using CsvHelper;

namespace CrunchImport.DataProviders.Alphavantage
{
    internal class AlphavantageProvider
    {
        private const string _apiKey = "***REMOVED***";
        private readonly WebClient _webClient = new WebClient();

        public List<ListingStatusResponse> GetListedSecurities()
        {
            string url = $"https://www.alphavantage.co/query?function=LISTING_STATUS&apikey={_apiKey}";
            Stream stream = _webClient.OpenRead(url);
            using StreamReader streamReader = new StreamReader(stream);
            using CsvReader csv = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<ListingStatusResponse>();
            return records.ToList();
        }
    }
}