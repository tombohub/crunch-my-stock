namespace Crunch
{
    /// <summary>
    /// Holding the important configuration variables like api keys 
    /// and connection strings
    /// </summary>
    internal class Configuration
    {
        /// <summary>
        /// Api key for FMP data source
        /// </summary>
        public const string FmpApiKey = "***REMOVED***";

        /// <summary>
        /// Connection string for production database
        /// </summary>
        public const string DbConnectionString = "server=***REMOVED***;user=***REMOVED***;database***REMOVED***;port=3306;password=***REMOVED***";

        /// <summary>
        /// Local MySql Connection string
        /// </summary>
        public const string DbLocalConnectionString = "server=localhost;user=***REMOVED***;database***REMOVED***;port=3306;password=***REMOVED***";

        /// <summary>
        /// PostgreSQL database connection string
        /// </summary>
        public const string PostgresSQLConnectionString = "Server=***REMOVED***;Port=5432;Database***REMOVED***;Userid=***REMOVED***;Password=***REMOVED***;";
    }
}
