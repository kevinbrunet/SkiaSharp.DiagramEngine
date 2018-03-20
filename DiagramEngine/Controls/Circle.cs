using SynodeTechnologies.SkiaSharp.DiagramEngine.Core;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Controls
{
    public class Circle : View
    {
        protected override void ArrangeCore(SKRect finalRect)
        {
            base.ArrangeCore(finalRect);
        }
        public override void Draw(SKCanvas canvas)
        {
            using (var paint = new SKPaint { IsAntialias = true })
            {
                var size = Math.Min(boundsMinusMargin.Width, boundsMinusMargin.Height);
                var offsetTop = (boundsMinusMargin.Height - size) / 2.0f;
                var offsetLeft = (boundsMinusMargin.Width - size) / 2.0f;
                var b = new SKRect(offsetLeft, offsetTop, offsetLeft+ size, offsetTop+size);
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
