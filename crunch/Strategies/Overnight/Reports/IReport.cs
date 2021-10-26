using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Crunch.Strategies.Overnight.Reports
{
    interface IReport
    {
        public Bitmap Plot(int width, int height);
    }
}
