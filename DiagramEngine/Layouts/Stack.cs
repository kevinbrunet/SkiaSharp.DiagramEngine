using System;
using System.Collections.Generic;
using System.Text;
using ModelerClient.DiagramEngine.Abstracts;
using SkiaSharp;
using Xamarin.Forms;

namespace ModelerClient.DiagramEngine.Layouts
{
    public class Stack : Base
    {
        public override SKSize Measure(IList<IElement> elements, SKSize availableSize)
        {
            float width = 0.0f;
            float height = 0.0f;
            foreach (var element in elements)
            {
                element.Measure(availableSize);


                if (width < element.DesiredSize.Width)
                {
                    width = element.DesiredSize.Width;
                }

                height += element.DesiredSize.Height;
            }

            return new SKSize(width, height);
        }

        public override void Arrange(IList<IElement> elements, SKRect bounds)
        {
            SKRect childRect = new SKRect(bounds.Left, bounds.Top, bounds.Width, 0);
            foreach (var element in elements)
            {
                childRect.Bottom = childRect.Top+ element.DesiredSize.Height;
                element.Arrange(childRect);
                childRect.Top += childRect.Height;
            }
        }

        
    }
}
