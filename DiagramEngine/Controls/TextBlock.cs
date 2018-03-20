using SynodeTechnologies.SkiaSharp.DiagramEngine.Core;
using SynodeTechnologies.SkiaSharp.DiagramEngine.Helpers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Controls
{
    public class TextBlock : Core.View
    {
        private static SKTypeface _defaultFont;
        public static SKTypeface DefaultFont { get => _defaultFont = _defaultFont ?? SKTypeface.FromFamilyName("Roboto"); }

        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(SKColor), typeof(TextBlock), SKColors.Black, propertyChanged: InvalidatePropertyChanged);
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(TextBlock), string.Empty, propertyChanged: InvalidatePropertyChanged);

        public static readonly BindableProperty FontProperty = BindableProperty.Create(nameof(Font), typeof(SKTypeface), typeof(TextBlock), null, propertyChanged: InvalidatePropertyChanged);
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(float), typeof(TextBlock), 10.0f, propertyChanged: InvalidatePropertyChanged);


        [TypeConverter(typeof(SKColorTypeConverter))]
        public SKColor Color
        {
            get
            {
                return (SKColor)this.GetValue(ColorProperty);
            }
            set
            {
                this.SetValue(ColorProperty, value);
            }
        }


        public SKTypeface Font
        {
            get
            {
                return (SKTypeface)this.GetValue(FontProperty);
            }
            set
            {
                this.SetValue(FontProperty, value);
            }
        }


        public float FontSize
        {
            get
            {
                return (float)this.GetValue(FontSizeProperty);
            }
            set
            {
                this.SetValue(FontSizeProperty, value);
            }
        }


        public string Text
        {
            get
            {
                return (string)this.GetValue(TextProperty);
            }
            set
            {
                this.SetValue(TextProperty, value);
            }
        }

        protected override SKSize MeasureContent(SKSize availableSize)
        {
            using (var paint = new SKPaint
            {
                IsAntialias = true,
                Color = Color,
                Typeface = Font ?? DefaultFont,
                TextSize = FontSize
            })
            {
                return new SKSize(paint.MeasureText(this.Text), FontSize);
            }
        }

        public override void Draw(SKCanvas canvas)
        {
            base.Draw(canvas);
            using (var paint = new SKPaint
            {
                IsAntialias = true,
                Color = Color,
                Typeface = Font ?? DefaultFont,
                TextSize = FontSize
            })
            {
                canvas.DrawText(Text, Padding.Left, Padding.Top + FontSize - paint.FontMetrics.Descent, paint);
            }
        }
    }
}
