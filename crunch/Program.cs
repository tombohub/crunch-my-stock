using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Crunch.DataSource;
using Crunch.Core;
using Crunch.Database;
using Microsoft.Data.Analysis;
using Crunch.Database.Models;
using Crunch.UseCases;

namespace Crunch
{
    class Program
    {
        private static void Main(string[] args)
        {

            UseCase.ImportPrices();

        }

    }
}


