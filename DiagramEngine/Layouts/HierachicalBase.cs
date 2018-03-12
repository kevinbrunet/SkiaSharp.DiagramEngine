using ModelerClient.DiagramEngine.Abstracts;
using ModelerClient.DiagramEngine.Controls;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelerClient.DiagramEngine.Layouts
{
    public class HierachicalBase : IHierarchicalLayout
    {

        public virtual SKSize Measure(IList<HierachicalNode> elements, SKSize availableSize)
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

        public virtual void Arrange(IList<HierachicalNode> elements, SKRect bounds)
        {
            foreach (var elem in elements)
            {
                elem.Arrange(bounds);
            }
        }


        public virtual void Render(IList<HierachicalNode> elements, SKRect bounds,SKCanvas canvas)
        {
            foreach (var elem in elements)
            {
                elem.Render(canvas);
            }
        }
    }
}
