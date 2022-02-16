using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Crunch.Domain.Multiplots
{
    /// <summary>
    /// Represents content of single multiplot area
    /// </summary>
    internal interface IAreaContent
    {
        Bitmap RenderImage(int width, int height);

    }
}
