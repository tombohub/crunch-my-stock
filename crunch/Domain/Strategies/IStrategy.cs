﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Domain.Strategies
{
    /// <summary>
    /// Abstract Factory. Responsible for creating specific objects for data source,
    /// repository, publishing templates
    /// </summary>
    interface IStrategy
    {
        /// <summary>
        /// Name of the strategy
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Print strategy name to the console
        /// </summary>
        void RunAnalytics(DateOnly date);

    }
}