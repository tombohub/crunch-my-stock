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
        internal MultiplotTitle(string title) 
        {
            Text = title;
            FontSize = 24 * Scale/2;
        }
    }
}
