using ModelerClient.DiagramEngine.Helpers;
using SkiaSharp;

namespace ModelerClient.DiagramEngine.Controls
{
    public class Adorner : Core.Layoutable
    {

        public override bool IsPointInside(SKPoint point, SkiaSharp.SKMatrix transfromStack)
        {
            return base.IsPointInside(point, transfromStack.Translate(-this.Parent.Padding.Left, -this.Parent.Padding.Top));
        }

        protected override SKSize MeasureOverride(SKSize availableSize)
        {
            return SKSize.Empty;
        }

        protected override void ArrangeCore(SKRect finalRect)
        {
            base.ArrangeCore(new SKRect(-this.Parent.Padding.Left, -this.Parent.Padding.Top, -this.Parent.Padding.Left + this.Parent.Bounds.Width - this.Parent.Margin.Right, -this.Parent.Padding.Top + this.Parent.Bounds.Height - this.Parent.Margin.Bottom));
        }


    }
}
