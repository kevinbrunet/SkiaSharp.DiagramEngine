using System;
using System.Linq;
using ModelerClient.DiagramEngine.Abstracts;
using ModelerClient.DiagramEngine.Controls;
using SkiaSharp;
using Xamarin.Forms;

namespace ModelerClient.DiagramEngine.Core
{
    public class HierarchicalLayoutable : View, IHierarchicalLayoutable
    {
        public static readonly BindableProperty LayoutProperty = BindableProperty.Create(nameof(Layout), typeof(Abstracts.IHierarchicalLayout), typeof(HierarchicalLayoutable), null, propertyChanged: Visual.InvalidatePropertyChanged);

        public Abstracts.IHierarchicalLayout Layout
        {
            get
            {
                return (Abstracts.IHierarchicalLayout)GetValue(LayoutProperty);
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
                return Layout.Measure(this.Children.Cast<HierachicalNode>().ToList(), availableSize);
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
                Layout.Arrange(this.Children.Cast<HierachicalNode>().ToList(), childrenBounds);
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
                Layout.Render(this.Children.Cast<HierachicalNode>().ToList(), this._childrenBounds, canvas);
            }
            else
            {
                base.RenderChildren(canvas);
            }
        }
    }
}
