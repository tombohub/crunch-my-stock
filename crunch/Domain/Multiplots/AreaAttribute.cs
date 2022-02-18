using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Domain.Multiplots
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AreaAttribute : Attribute
    {
        /// <summary>
        /// Strategy name
        /// </summary>
        public string Strategy { get; init; }

        /// <summary>
        /// Area name
        /// </summary>
        public string Name { get; init; }

    }
}
