using SynodeTechnologies.SkiaSharp.DiagramEngine.Core;
using SynodeTechnologies.SkiaSharp.DiagramEngine.Helpers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Linq;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Controls
{
    public class TextBlock : Core.View
    {
        public class Line
        {
            public string Value { get; set; }

            public float Width { get; set; }
        }

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
                var lines = SplitLines(this.Text, paint, availableSize.Width);

                return new SKSize(lines.Max(l => l.Width), lines.Length * FontSize);
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
                var lines = SplitLines(this.Text, paint, this._childrenBounds.Width);
                float offsetY = Padding.Top - paint.FontMetrics.Descent;
                foreach(var line in lines)
                {
                    offsetY += (FontSize );
                    canvas.DrawText(line.Value, Padding.Left, offsetY, paint);
                }
            }
        }


        private Line[] SplitLines(string text, SKPaint paint, float maxWidth)
        {
            var spaceWidth = paint.MeasureText(" ");
            var lines = text.Split('\n');

            return lines.SelectMany((line) =>
            {
                var result = new List<Line>();

                var words = line.Split(new[] { " " }, StringSplitOptions.None);

                var lineResult = new StringBuilder();
                float width = 0;
                foreach (var word in words)
                {
                    var wordWidth = paint.MeasureText(word);
                    var wordWithSpaceWidth = wordWidth + spaceWidth;
                    var wordWithSpace = word + " ";

                    if (width + wordWidth > maxWidth)
                    {
                        result.Add(new Line() { Value = lineResult.ToString(), Width = width });
                        lineResult = new StringBuilder(wordWithSpace);
                        width = wordWithSpaceWidth;
                    }
                    else
                    {
                        lineResult.Append(wordWithSpace);
                        width += wordWithSpaceWidth;
                    }
                }

                result.Add(new Line() { Value = lineResult.ToString(), Width = width });

                return result.ToArray();
            }).ToArray();
        }
    }
}
