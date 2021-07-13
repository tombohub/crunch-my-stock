using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.UseCases;


namespace Crunch.DataSource

{
    /// <summary>
    /// Class exposing the methods to communicate with Data Source module.
    /// </summary>
    class Api : IDataSourceAPI
    {
        /// <summary>
        /// Get Groups (Industries and Sectors) daily performance overview data
        /// </summary>
        /// <returns>object containing the Sectors and Industries data, Groups data</returns>
        public GroupsData GetGroupsData()
        {
            return new GroupsData();

        }

    }

}
