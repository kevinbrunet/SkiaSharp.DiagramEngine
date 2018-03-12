using System;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using ModelerClient.DiagramEngine.Core;
using ModelerClient.DiagramEngine.Abstracts;

namespace ModelerClient.DiagramEngine.Controls
{
    [ContentProperty("Children")]
    public class Canvas : SKCanvasView, ILayoutable
    {
        public static readonly BindableProperty TouchListenerProperty = BindableProperty.Create(nameof(TouchListener), typeof(ITouchListener), typeof(Canvas),  null, BindingMode.OneWay, null, null, null, null, null);

        protected readonly Core.Layoutable surface = new Core.Layoutable();

        public ITouchListener TouchListener
        {
            get
            {
                return (ITouchListener)this.GetValue(TouchListenerProperty);
            }
            set
            {
                this.SetValue(TouchListenerProperty, value);
            }
        }

        public Canvas()
        {
            this.EnableTouchEvents = true;
            surface.TransformationPivot = SKPoint.Empty;
            surface.AttachParent(this);
        }

        public ElementsCollection Children { get => surface.Children; }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            isInvalidated = false;
            e.Surface.Canvas.Clear(SKColors.White);
            base.OnPaintSurface(e);
            Measure(new SKSize(float.PositiveInfinity, float.PositiveInfinity));
            Arrange(new SKRect(0, 0,base.CanvasSize.Width, base.CanvasSize.Height));
            Render(e.Surface.Canvas);
        }

       


        protected override void OnTouch(SKTouchEventArgs e)
        {
            if(TouchListener != null)
            {
                TouchListener.OnTouch(surface,e);
            }
            if(e.Handled==false)
            {
                e.Handled = true;
            }
        }

       
        SKColor IView.BackgroundColor { get => surface.BackgroundColor; set => surface.BackgroundColor = value; }
        public SKColor BorderColor { get => surface.BorderColor; set => surface.BorderColor = value; }
        public float BorderWidth { get => surface.BorderWidth; set => surface.BorderWidth = value; }
        public SKPoint CornerRadius { get => surface.CornerRadius; set => surface.CornerRadius = value; }
        IElement IElement.Parent { get => null; set {}}
        public SKRect Padding { get => surface.Margin; set => surface.Margin = value; }
        SKRect IVisual.Margin { get => SKRect.Empty; set {} }
        public SkiaSharp.SKMatrix? Transformation { get => surface.Transformation; set => surface.Transformation = value; }
        public SKPoint TransformationPivot { get => surface.TransformationPivot; set => surface.TransformationPivot = value; }
        public bool Visibility { get => surface.Visibility; set => surface.Visibility = value; }

        public SKSize DesiredSize => surface.DesiredSize;

        public bool IsMeasureValid => surface.IsMeasureValid;

        public bool MeasureInProgress => surface.MeasureInProgress;

        public bool IsArrangeValid => surface.IsArrangeValid;

        public bool IsTransformationValid => surface.IsTransformationValid;

        public bool IsRenderValid => surface.IsRenderValid;

        float IVisual.X { get => 0; set { } }
        float IVisual.Y { get => 0; set { } }
        float IVisual.Width { get => (float)this.Width; set => this.WidthRequest = (double)value; }
        float IVisual.Height { get => (float)this.Height; set => this.HeightRequest = (double)value; }

        public SKMatrix VisualTransform => surface.VisualTransform;

        SKRect IVisual.Bounds => new SKRect(0, 0, base.CanvasSize.Width, base.CanvasSize.Height);

        Abstracts.ILayout ILayoutable.Layout { get => surface.Layout; set => surface.Layout = value; }

        private bool isInvalidated = false;
        public void Invalidate()
        {
            if (!isInvalidated)
            {
                this.InvalidateSurface();
                isInvalidated = true;
            }
        }

        public void OnChildDesiredSizeChanged(Core.Element child)
        {
            this.Invalidate();
        }

        public virtual IElement GetElementAtPoint(SKPoint point)
        {
            return surface.GetElementAtPoint(point);
        }

        public virtual IElement GetElementAtPoint(SKPoint point, Func<IElement, bool> predicate)
        {
            return surface.GetElementAtPoint(point, predicate);
        }

        public virtual IElement GetElementAtPoint(SKPoint point, Func<IElement, bool> predicate, SkiaSharp.SKMatrix transformationsStack)
        {
            return surface.GetElementAtPoint(point, predicate);
        }

        void IVisual.InvalidateMeasure()
        {
            surface.InvalidateMeasure();
        }

        public virtual void Measure(SKSize availableSize)
        {
            surface.Measure(availableSize);
        }

        public void InvalidateArrange()
        {
            surface.InvalidateArrange();
        }

        public virtual void Arrange(SKRect finalRect)
        {
            surface.Arrange(finalRect);
        }

        public virtual void Render(SKCanvas canvas)
        {
            surface.Render(canvas);
        }

        public virtual void Draw(SKCanvas canvas)
        {
            surface.Draw(canvas);
        }

        public bool IsPointInside(SKPoint point, SkiaSharp.SKMatrix transfromStack)
        {
            return surface.IsPointInside(point, transfromStack);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindableObject.SetInheritedBindingContext(surface, this.BindingContext);
            if(TouchListener != null)
                BindableObject.SetInheritedBindingContext((BindableObject)TouchListener, this.BindingContext);
        }

        public void AttachParent(IElement parent)
        {

        }      
    }
}
