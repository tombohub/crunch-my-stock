using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Domain.Strategies
{
    /// <summary>
    /// Data Source is responsible for getting specific data for each strategy.
    /// </summary>
    /// <typeparam name="T">Data model</typeparam>
    interface IDataSource
    {
        public void
            DownloadData();
    }
}
