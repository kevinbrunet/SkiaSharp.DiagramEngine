using ModelerClient.DiagramEngine.Helpers;
using SkiaSharp;
using Xamarin.Forms;

namespace ModelerClient.DiagramEngine.Controls
{
    public class Polygon : Core.View
    {
        public static readonly BindableProperty FillTypeProperty = BindableProperty.Create(nameof(FillType), typeof(SKPathFillType), typeof(Polygon), SKPathFillType.EvenOdd, propertyChanged: InvalidatePropertyChanged);
        public static readonly BindableProperty PathProperty = BindableProperty.Create(nameof(Path), typeof(SKPath), typeof(TextBlock), null, propertyChanged: InvalidatePropertyChanged);
        public static readonly BindableProperty AutoScaleProperty = BindableProperty.Create(nameof(AutoScale), typeof(bool), typeof(TextBlock), true, propertyChanged: InvalidatePropertyChanged);

        public SKPathFillType FillType
        {
            get => (SKPathFillType)GetValue(FillTypeProperty);
            set
            {
                SetValue(FillTypeProperty, value);
            }
        }

        [TypeConverter(typeof(SKPathTypeConverter))]
        public SKPath Path
        {
            get => (SKPath)GetValue(PathProperty);
            set
            {
                SetValue(PathProperty, value);
            }
        }

        public bool AutoScale
        {
            get => (bool)GetValue(AutoScaleProperty);
            set
            {
                SetValue(AutoScaleProperty, value);
            }
        }

        protected override SKSize MeasureContent(SKSize availableSize)
        {
            if (Path == null)
                return SKSize.Empty;
            else
            {
                var b = Path.Bounds;
                return new SKSize(b.Right,b.Bottom);
            }
        }

        public override void Draw(SKCanvas canvas)
        {
            SkiaSharp.SKMatrix scaleMatrix = SkiaSharp.SKMatrix.MakeIdentity();
            if (AutoScale)
            {
                var b = Path.Bounds;
                scaleMatrix = SkiaSharp.SKMatrix.MakeScale(this.boundsMinusMargin.Width / b.Right,this.boundsMinusMargin.Height / b.Bottom);
                var invert = scaleMatrix.Invert();
                canvas.Concat(ref scaleMatrix);
                scaleMatrix = invert;
            }
            using (var paint = new SKPaint { IsAntialias = true })
            {
                if (drawBackground)
                {
                    Path.FillType = FillType;
                    paint.Color = BackgroundColor;
                    canvas.DrawPath(Path, paint);
                }
                if (drawBorder)
                {
                    paint.Style = SKPaintStyle.Stroke;
                    paint.Color = BorderColor;
                    paint.StrokeWidth = BorderWidth;
                    Path.FillType = SKPathFillType.Winding;
                    canvas.DrawPath(Path, paint);
                }
            }
            if(AutoScale)
            {
                canvas.Concat(ref scaleMatrix);
            }
        }
    }
}
