using ModelerClient.DiagramEngine.Core;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelerClient.DiagramEngine.Controls
{
    public class Oval : View
    {
        public override void Draw(SKCanvas canvas)
        {
            using (var paint = new SKPaint { IsAntialias = true })
            {
                var b = new SKRect(0, 0, this.boundsMinusMargin.Width, this.boundsMinusMargin.Height);
                if (drawBackground)
                {
                    paint.Color = BackgroundColor;
                    canvas.DrawOval(b, paint);
                }
                if (drawBorder)
                {
                    paint.Style = SKPaintStyle.Stroke;
                    paint.Color = BorderColor;
                    paint.StrokeWidth = BorderWidth;
                    canvas.DrawOval(b, paint);
                }
            }
        }
    }
}
