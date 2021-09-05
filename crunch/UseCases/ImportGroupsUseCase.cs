using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.DataSource;
using Crunch.Database;
using Crunch.Core;

namespace Crunch.UseCases
{
    static class ImportGroupsUseCase
    {
        /// <summary>
        /// Data source API instance
        /// </summary>
        private static readonly DataSourceAPI _dataSource = new DataSourceAPI();

        /// <summary>
        /// Database API instance
        /// </summary>
        private static readonly DatabaseAPI _database = new DatabaseAPI();

        public static void ImportGroups()
        {
            var groupsData = _dataSource.GetGroupsData();
            _database.SaveGroups(groupsData);
        }
    }

 }
