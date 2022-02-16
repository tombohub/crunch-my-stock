using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Domain.Multiplots
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AreaNameAttribute : Attribute
    {
        public string Name { get; init; }
        public AreaNameAttribute(string name)
        {
            Name = name;
        }
    }
}
