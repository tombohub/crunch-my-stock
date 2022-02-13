using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Crunch.Images
{
    internal class SingleMetric : TextImageBase
    {
        internal SingleMetric(string text) 
        {
            Text = text;
            FontSize = 18 * Scale / 2;
        }
    }
}
