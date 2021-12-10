using System.Drawing;

namespace Crunch.Domain.Strategies
{
    interface IReport
    {
        public Bitmap Plot(int width, int height);
    }
}
