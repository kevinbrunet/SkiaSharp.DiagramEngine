using SynodeTechnologies.SkiaSharp.DiagramEngine.Abstracts;
using SynodeTechnologies.SkiaSharp.DiagramEngine.Controls;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Layouts
{
    public class HierarchicalBase : BindableObject, IHierarchicalLayout
    {

        public virtual SKSize Measure(IList<HierarchicalNode> elements, SKSize availableSize)
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

        public virtual void Arrange(IList<HierarchicalNode> elements, SKRect bounds)
        {
            foreach (var elem in elements)
            {
                elem.Arrange(bounds);
            }
        }


        public virtual void Render(IList<HierarchicalNode> elements, SKRect bounds,SKCanvas canvas)
        {
            foreach (var elem in elements)
            {
                elem.Render(canvas);
            }
        }
    }
}
