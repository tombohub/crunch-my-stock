using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.CLI.Controllers;

namespace Crunch.CLI.Controllers
{
    /// <summary>
    /// Interface for command line interface controllers
    /// </summary>
    internal interface ICliController
    {
        public void RunUseCase();
    }
}
