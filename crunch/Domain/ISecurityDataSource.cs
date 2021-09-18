using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Domain
{
    /// <summary>
    /// Methods for retreiving newest securities from data source
    /// </summary>
    interface ISecurityDataSource
    {
        /// <summary>
        /// Get the current list of securities available on market.
        /// </summary>
        /// <returns>List of securities</returns>
        public List<Security> GetSecurities();
    }
}
