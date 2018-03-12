using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ModelerClient.DiagramEngine.Helpers
{
    public class SKThicknessTypeConverter : TypeConverter
    {
        public override object ConvertFromInvariantString(string value)
        {
            if (value != null)
            {
                value = value.Trim();
                if (value.Contains(","))
                { //Xaml
                    var thickness = value.Split(',');
                    switch (thickness.Length)
                    {
                        case 2:
                            if (float.TryParse(thickness[0], NumberStyles.Number, CultureInfo.InvariantCulture, out float h)
                                && float.TryParse(thickness[1], NumberStyles.Number, CultureInfo.InvariantCulture, out float v))
                                return new SKRect(0, 0, h, v);
                            break;
                        case 4:
                            if (float.TryParse(thickness[0], NumberStyles.Number, CultureInfo.InvariantCulture, out float l)
                                && float.TryParse(thickness[1], NumberStyles.Number, CultureInfo.InvariantCulture, out float t)
                                && float.TryParse(thickness[2], NumberStyles.Number, CultureInfo.InvariantCulture, out float r)
                                && float.TryParse(thickness[3], NumberStyles.Number, CultureInfo.InvariantCulture, out float b))
                                return new SKRect(l, t, l+r, t+b);
                            break;
                    }
                }
                else if (value.Contains(" "))
                { //CSS
                    var thickness = value.Split(' ');
                    switch (thickness.Length)
                    {
                        case 2:
                            if (float.TryParse(thickness[0], NumberStyles.Number, CultureInfo.InvariantCulture, out float v)
                                && float.TryParse(thickness[1], NumberStyles.Number, CultureInfo.InvariantCulture, out float h))
                                return new SKRect(0,0,h, v);
                            break;
                        case 3:
                            if (float.TryParse(thickness[0], NumberStyles.Number, CultureInfo.InvariantCulture, out float t)
                                && float.TryParse(thickness[1], NumberStyles.Number, CultureInfo.InvariantCulture, out h)
                                && float.TryParse(thickness[2], NumberStyles.Number, CultureInfo.InvariantCulture, out float b))
                                return new SKRect(h, t, h+h, t+b);
                            break;
                        case 4:
                            if (float.TryParse(thickness[0], NumberStyles.Number, CultureInfo.InvariantCulture, out t)
                                && float.TryParse(thickness[1], NumberStyles.Number, CultureInfo.InvariantCulture, out float r)
                                && float.TryParse(thickness[2], NumberStyles.Number, CultureInfo.InvariantCulture, out b)
                                && float.TryParse(thickness[3], NumberStyles.Number, CultureInfo.InvariantCulture, out float l))
                                return new SKRect(l, t, l+r, t+b);
                            break;
                    }
                }
                else
                { //single uniform thickness
                    if (float.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out float l))
                        return new SKRect(l,l,l+l,l+l);
                }
            }

            throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(SKRect)}");
        }
    }
}
