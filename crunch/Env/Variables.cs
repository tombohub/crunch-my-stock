/// <summary>
/// Namespace dealing with loading env variables from .env file
/// </summary>
namespace Crunch.Env
{
    /// <summary>
    /// Provides the variables from .env file
    /// </summary>
    class Variables
    {
        private const string _FmpApiKeyName = "FMP_API_KEY";
        private const string _DBName = "DB";
        private const string _DBDevName = "DB_DEV";

        /// <summary>
        /// Financial modelling prep API key
        /// </summary>
        public static string FmpApiKey
        {
            get { return GetVariable(_FmpApiKeyName); }
        }

        /// <summary>
        /// URI to connect to database
        /// </summary>
        public static string DatabaseURI
        {
            get { return GetVariable(_DBName); }
        }

        /// <summary>
        /// URI to connect to dev database
        /// </summary>
        public static string DatabaseDevURI
        {
            get { return GetVariable(_DBDevName); }
        }

        /// <summary>
        /// Get the variable value from .env file
        /// </summary>
        /// <param name="name">name of the variable</param>
        /// <returns>value of .env variable</returns>
        private static string GetVariable(string name)
        {
            DotNetEnv.Env.TraversePath().Load();
            string variable = DotNetEnv.Env.GetString(name);
            return variable;

        }


    }
}
