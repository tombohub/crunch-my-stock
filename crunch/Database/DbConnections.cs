using System;
using System.Data;
using Dapper;
using Npgsql;

namespace Crunch.Database
{
    /// <summary>
    /// Providing the connection to database
    /// </summary>
    public class DbConnections
    {
        /// <summary>
        /// Create PostgreSQL remote connection customized for use with Dapper
        /// </summary>
        /// <returns></returns>
        public static NpgsqlConnection CreatePsqlConnection()
        {
            string dbUri = Configuration.PostgresSQLConnectionString;
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
            return new NpgsqlConnection(dbUri);
        }
    }

    internal class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
    {
        /// <summary>
        /// Parse the database 'date' type to C# DateOnly.
        /// Reason is because Dapper and ADO.NET converts the 'date' type to
        /// DateTime.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override DateOnly Parse(object value)
        {
            var dateTime = (DateTime)value;
            return DateOnly.Parse(dateTime.ToShortDateString());
        }

        public override void SetValue(IDbDataParameter parameter, DateOnly value)
        {
            parameter.Value = value;
        }
    }
}