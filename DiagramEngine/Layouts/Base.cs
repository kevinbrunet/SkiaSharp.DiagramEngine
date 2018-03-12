using System;
using System.Collections.Generic;
using System.Text;
using ModelerClient.DiagramEngine.Abstracts;
using ModelerClient.DiagramEngine.Core;
using SkiaSharp;
using Xamarin.Forms;

namespace ModelerClient.DiagramEngine.Layouts
{
    public class Base : BindableObject, Abstracts.ILayout
    {
        public virtual void Arrange(IList<IElement> elements, SKRect bounds)
        {
            foreach(var elem in elements)
            {
                elem.Arrange(bounds);
            }
        }

        public virtual SKSize Measure(IList<IElement> elements, SKSize availableSize)
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

                if (height < element.DesiredSize.Height)
                {
                    height = element.DesiredSize.Height;
                }
            }

            return new SKSize(width, height);
        }

        public virtual void Render(IList<IElement> elements, SKRect bounds, SKCanvas canvas)
        {
            foreach (var elem in elements)
            {
                elem.Render(canvas);
            }
        }
    }
}
