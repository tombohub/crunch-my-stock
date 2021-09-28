using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Crunch.DataSources
{
    /// <summary>
    /// FinViz data source class.
    /// </summary>
    static class Finviz
    {
        private static readonly string _baseUrl = "https://us-central1-stock-analytics-310810.cloudfunctions.net/";
        private static readonly WebClient _client = new();


        /// <summary>
        /// Get groups (sectors and industries) data. Daily performance per sector and industry.
        /// </summary>
        /// <returns>JSON data as list of json objects [{"key":"value"..},..]</returns>
        public static string GetGroupsData()
        {
            string getGroupsUrl = _baseUrl + "get_groups?date=2021-07-05";
            string result = _client.DownloadString(getGroupsUrl);
            return result;
        }


    }
}
