using System;

namespace Crunch.Core.Multiplots
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