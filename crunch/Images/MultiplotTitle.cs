using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Crunch.Images
{
    internal class MultiplotTitle : TextImageBase
    {
        internal MultiplotTitle(string title, float scale) 
        {
            Text = title;
            FontSize = 24 * scale/2;
        }
    }
}
