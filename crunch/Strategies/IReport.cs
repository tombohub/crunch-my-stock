using System.Drawing;

namespace Crunch.Strategies
{
    interface IReport
    {
        public Bitmap Plot(int width, int height);
    }
}
