namespace Crunch
{
    /// <summary>
    /// Holding the important configuration variables like api keys 
    /// and connection strings
    /// </summary>
    internal static class Configuration
    {
        /// <summary>
        /// Api key for FMP data source
        /// </summary>
        public static string FmpApiKey = "***REMOVED***";

        /// <summary>
        /// Connection string for production database
        /// </summary>
        public static string DbConnectionString = "server=***REMOVED***;user=***REMOVED***;database***REMOVED***;port=3306;password=***REMOVED***";
    }
}
