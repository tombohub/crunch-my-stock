using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Images
{
    internal class Description: TextImageBase
    {
        internal Description(string text)
        {
            Text = text;
            FontSize = 14 * Scale;
        }
    }
}
