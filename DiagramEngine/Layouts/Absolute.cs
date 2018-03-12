using System;
using System.Collections.Generic;
using System.Text;
using ModelerClient.DiagramEngine.Abstracts;
using ModelerClient.DiagramEngine.Core;
using SkiaSharp;

namespace ModelerClient.DiagramEngine.Layouts
{
    public class Absolute : Base
    {
        public override SKSize Measure(IList<IElement> elements, SKSize availableSize)
        {
            float width = 0.0f;
            float height = 0.0f;
            foreach (var element in elements)
            {
                element.Measure(availableSize);



                if (width < (element.DesiredSize.Width + element.X))
                {
                    width = (element.DesiredSize.Width + element.X);
                }

                if (height < (element.DesiredSize.Height + element.Y))
                {
                    height = (element.DesiredSize.Height + element.Y);
                }
            }

            return new SKSize(width, height);
        }
        public override void Arrange(IList<IElement> elements, SKRect bounds)
        {
            foreach (var element in elements)
            {
                element.Arrange(new SKRect(element.X, element.Y, element.X + element.DesiredSize.Width, element.Y + element.DesiredSize.Height));
            }
        }
    }
}
