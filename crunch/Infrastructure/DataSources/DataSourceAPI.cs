using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crunch.UseCases;


namespace Crunch.Infrastructure.DataSources

{
    /// <summary>
    /// Class exposing the methods to communicate with Data Source module.
    /// </summary>
    class DataSourceAPI
    {
        /// <summary>
        /// Get Groups (Industries and Sectors) daily performance overview data
        /// </summary>
        /// <returns>object containing the Sectors and Industries data, Groups data</returns>
        public string GetGroupsData()
        {
            string groupsData = Finviz.GetGroupsData();
            return groupsData;
        }

    }

}
