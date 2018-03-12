using System;
using ModelerClient.DiagramEngine.Abstracts;
using SkiaSharp;
using Xamarin.Forms;

namespace ModelerClient.DiagramEngine.Core
{
    public class Layoutable : View, ILayoutable
    {
        public static readonly BindableProperty LayoutProperty = BindableProperty.Create(nameof(Layout), typeof(Abstracts.ILayout), typeof(Layoutable), null, propertyChanged: Visual.InvalidatePropertyChanged);

        public Abstracts.ILayout Layout
        {
            get
            {
                return (Abstracts.ILayout)GetValue(LayoutProperty);
            }
            set
            {
                SetValue(LayoutProperty, value);
            }
        }

        protected override SKSize MeasureChildren(SKSize availableSize)
        {
            if (Layout != null)
            {
                return Layout.Measure(this.Children, availableSize);
            }
            else
            {
                return base.MeasureChildren(availableSize);
            }
        }

        protected override void ArrangeChildren(SKRect childrenBounds)
        {
            if (Layout != null)
            {
                Layout.Arrange(this.Children, childrenBounds);
            }
            else
            {
                base.ArrangeChildren(childrenBounds);
            }
        }

        protected override void RenderChildren(SKCanvas canvas)
        {
            if (Layout != null)
            {
                Layout.Render(this.Children, this._childrenBounds, canvas);
            }
            else
            {
                base.RenderChildren(canvas);
            }
        }
    }
}
