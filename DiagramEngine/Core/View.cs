using SynodeTechnologies.SkiaSharp.DiagramEngine.Helpers;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Core
{
    public class View : Element
    {
        public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(SKColor), typeof(View), SKColors.Transparent, propertyChanged: BackgroundPropertyChanged);
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(SKColor), typeof(View), SKColors.Transparent, propertyChanged: BorderPropertyChanged);
        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(float), typeof(View), 0.0f, propertyChanged: BorderPropertyChanged);
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(SKPoint), typeof(View), SKPoint.Empty, propertyChanged: BorderPropertyChanged);

        public static void BackgroundPropertyChanged(BindableObject bo, object oldValue, object newValue)
        {
            ((View)bo).Invalidate();
            ((View)bo).drawBackground = true;
        }
        public static void BorderPropertyChanged(BindableObject bo, object oldValue, object newValue)
        {
            ((View)bo).Invalidate();
            ((View)bo).drawBorder = true;
        }

        #region Properties

        [TypeConverter(typeof(SKColorTypeConverter))]
        public SKColor BackgroundColor
        {
            get
            {
                return (SKColor)GetValue(BackgroundColorProperty);
            }
            set
            {
                SetValue(BackgroundColorProperty, value);
            }
        }

        [TypeConverter(typeof(SKColorTypeConverter))]
        public SKColor BorderColor
        {
            get
            {
                return (SKColor)GetValue(BorderColorProperty);
            }
            set
            {
                SetValue(BorderColorProperty, value);
            }
        }

        public float BorderWidth
        {
            get
            {
                return (float)GetValue(BorderWidthProperty);
            }
            set
            {
                SetValue(BorderWidthProperty, value);
            }
        }

        public SKPoint CornerRadius
        {
            get
            {
                return (SKPoint)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }

        #endregion

        protected bool drawBackground;
        protected bool drawBorder;
        public override void Draw(SKCanvas canvas)
        {
            using (var paint = new SKPaint { IsAntialias = true })
            {
                var b = new SKRect(0, 0, this.boundsMinusMargin.Width, this.boundsMinusMargin.Height);
                if (drawBackground)
                {
                    paint.Color = BackgroundColor;
                    canvas.DrawRoundRect(b, CornerRadius.X, CornerRadius.Y, paint);
                }
                if (drawBorder)
                {
                    paint.Style = SKPaintStyle.Stroke;
                    paint.Color = BorderColor;
                    paint.StrokeWidth = BorderWidth;
                    canvas.DrawRoundRect(new SKRect(0, 0, b.Width, b.Height), CornerRadius.X, CornerRadius.Y, paint);
                }
            }
        }


        
    }
}
