using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Domain.Multiplots;
using Crunch.Images;

namespace Crunch.Strategies.Overnight.AreaContents
{
    internal class Title : IAreaContent
    {
        private string _titleText { get; init; }
        
        public Title(DateOnly date)
        {
            _titleText = $"Overnight strategy {date}";
        }

        public Bitmap RenderImage(int width, int height)
        {
            var titleRender = new MultiplotTitle(_titleText);
            return titleRender.RenderImage(width, height);
        }

    }
}
