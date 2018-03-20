using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SkiaSharp;
using System.Linq;
using System.Globalization;
using Xamarin.Forms.Internals;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Helpers
{
    public class SKColorTypeConverter : TypeConverter
    {
        /// <param name="value">The value to convert.</param>
        /// <summary>Creates a color from a valid color name.</summary>
        /// <returns>To be added.</returns>
        /// <remarks>To be added.</remarks>
        public override object ConvertFromInvariantString(string value)
        {
            if (value != null)
            {
                value = value.Trim();
                if (value.StartsWith("#", StringComparison.Ordinal))
                {
                    return FromHex(value);
                }
                if (value.StartsWith("rgba", StringComparison.OrdinalIgnoreCase))
                {
                    int op = value.IndexOf('(');
                    int cp = value.LastIndexOf(')');
                    if (op < 0 || cp < 0 || cp < op)
                    {
                        throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", new object[]
                        {
                            value,
                            typeof(SKColor)
                        }));
                    }
                    string[] quad = value.Substring(op + 1, cp - op - 1).Split(new char[]
                    {
                        ','
                    });
                    if (quad.Length != 4)
                    {
                        throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", new object[]
                        {
                            value,
                            typeof(SKColor)
                        }));
                    }
                    byte r = SKColorTypeConverter.ParseColorValue(quad[0], byte.MaxValue, true);
                    byte g = SKColorTypeConverter.ParseColorValue(quad[1], byte.MaxValue, true);
                    byte b = SKColorTypeConverter.ParseColorValue(quad[2], byte.MaxValue, true);
                    byte a = SKColorTypeConverter.ParseOpacity(quad[3]);
                    return new SKColor(r, g, b, a);
                }
                else if (value.StartsWith("rgb", StringComparison.OrdinalIgnoreCase))
                {
                    int op2 = value.IndexOf('(');
                    int cp2 = value.LastIndexOf(')');
                    if (op2 < 0 || cp2 < 0 || cp2 < op2)
                    {
                        throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", new object[]
                        {
                            value,
                            typeof(SKColor)
                        }));
                    }
                    string[] triplet = value.Substring(op2 + 1, cp2 - op2 - 1).Split(new char[]
                    {
                        ','
                    });
                    if (triplet.Length != 3)
                    {
                        throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", new object[]
                        {
                            value,
                            typeof(SKColor)
                        }));
                    }
                    byte r2 = SKColorTypeConverter.ParseColorValue(triplet[0], byte.MaxValue, true);
                    byte g2 = SKColorTypeConverter.ParseColorValue(triplet[1], byte.MaxValue, true);
                    byte b2 = SKColorTypeConverter.ParseColorValue(triplet[2], byte.MaxValue, true);
                    return new SKColor(r2, g2, b2);
                }
                else if (value.StartsWith("hsla", StringComparison.OrdinalIgnoreCase))
                {
                    int op3 = value.IndexOf('(');
                    int cp3 = value.LastIndexOf(')');
                    if (op3 < 0 || cp3 < 0 || cp3 < op3)
                    {
                        throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", new object[]
                        {
                            value,
                            typeof(Color)
                        }));
                    }
                    string[] quad2 = value.Substring(op3 + 1, cp3 - op3 - 1).Split(new char[]
                    {
                        ','
                    });
                    if (quad2.Length != 4)
                    {
                        throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", new object[]
                        {
                            value,
                            typeof(Color)
                        }));
                    }
                    float h = SKColorTypeConverter.ParseColorValue(quad2[0], 360, false);
                    float s = SKColorTypeConverter.ParseColorValue(quad2[1], 100, true);
                    float i = SKColorTypeConverter.ParseColorValue(quad2[2], 100, true);
                    byte a2 = SKColorTypeConverter.ParseOpacity(quad2[3]);
                    return SKColor.FromHsl(h, s, i, a2);
                }
                else if (value.StartsWith("hsl", StringComparison.OrdinalIgnoreCase))
                {
                    int op4 = value.IndexOf('(');
                    int cp4 = value.LastIndexOf(')');
                    if (op4 < 0 || cp4 < 0 || cp4 < op4)
                    {
                        throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", new object[]
                        {
                            value,
                            typeof(Color)
                        }));
                    }
                    string[] triplet2 = value.Substring(op4 + 1, cp4 - op4 - 1).Split(new char[]
                    {
                        ','
                    });
                    if (triplet2.Length != 3)
                    {
                        throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", new object[]
                        {
                            value,
                            typeof(Color)
                        }));
                    }
                    float h2 = SKColorTypeConverter.ParseColorValue(triplet2[0], 360, false);
                    float s2 = SKColorTypeConverter.ParseColorValue(triplet2[1], 100, true);
                    float j = SKColorTypeConverter.ParseColorValue(triplet2[2], 100, true);
                    return SKColor.FromHsl(h2, s2, j);
                }
                else
                {
                    string[] parts = value.Split('.');
                    if (parts.Length == 1 || (parts.Length == 2 && parts[0] == "Color"))
                    {
                        string color = parts[parts.Length - 1];
                        switch (color.ToLowerInvariant())
                        {
                            case "empty": return SKColor.Empty;
                            case "aliceblue": return SKColors.AliceBlue;
                            case "antiquewhite": return SKColors.AntiqueWhite;
                            case "aqua": return SKColors.Aqua;
                            case "aquamarine": return SKColors.Aquamarine;
                            case "azure": return SKColors.Azure;
                            case "beige": return SKColors.Beige;
                            case "bisque": return SKColors.Bisque;
                            case "black": return SKColors.Black;
                            case "blanchedalmond": return SKColors.BlanchedAlmond;
                            case "blue": return SKColors.Blue;
                            case "blueViolet": return SKColors.BlueViolet;
                            case "brown": return SKColors.Brown;
                            case "burlywood": return SKColors.BurlyWood;
                            case "cadetblue": return SKColors.CadetBlue;
                            case "chartreuse": return SKColors.Chartreuse;
                            case "chocolate": return SKColors.Chocolate;
                            case "coral": return SKColors.Coral;
                            case "cornflowerblue": return SKColors.CornflowerBlue;
                            case "cornsilk": return SKColors.Cornsilk;
                            case "crimson": return SKColors.Crimson;
                            case "cyan": return SKColors.Cyan;
                            case "darkblue": return SKColors.DarkBlue;
                            case "darkcyan": return SKColors.DarkCyan;
                            case "darkgoldenrod": return SKColors.DarkGoldenrod;
                            case "darkgray": return SKColors.DarkGray;
                            case "darkgreen": return SKColors.DarkGreen;
                            case "darkkhaki": return SKColors.DarkKhaki;
                            case "darkmagenta": return SKColors.DarkMagenta;
                            case "darkolivegreen": return SKColors.DarkOliveGreen;
                            case "darkorange": return SKColors.DarkOrange;
                            case "darkorchid": return SKColors.DarkOrchid;
                            case "darkred": return SKColors.DarkRed;
                            case "darksalmon": return SKColors.DarkSalmon;
                            case "darkseagreen": return SKColors.DarkSeaGreen;
                            case "darkslateblue": return SKColors.DarkSlateBlue;
                            case "darkslategray": return SKColors.DarkSlateGray;
                            case "darkturquoise": return SKColors.DarkTurquoise;
                            case "darkviolet": return SKColors.DarkViolet;
                            case "deeppink": return SKColors.DeepPink;
                            case "deepskyblue": return SKColors.DeepSkyBlue;
                            case "dimgray": return SKColors.DimGray;
                            case "dodgerblue": return SKColors.DodgerBlue;
                            case "firebrick": return SKColors.Firebrick;
                            case "floralwhite": return SKColors.FloralWhite;
                            case "forestgreen": return SKColors.ForestGreen;
                            case "fuchsia": return SKColors.Fuchsia;
                            case "gainsboro": return SKColors.Gainsboro;
                            case "ghostwhite": return SKColors.GhostWhite;
                            case "gold": return SKColors.Gold;
                            case "goldenrod": return SKColors.Goldenrod;
                            case "gray": return SKColors.Gray;
                            case "green": return SKColors.Green;
                            case "greenyellow": return SKColors.GreenYellow;
                            case "honeydew": return SKColors.Honeydew;
                            case "hotpink": return SKColors.HotPink;
                            case "indianred": return SKColors.IndianRed;
                            case "indigo": return SKColors.Indigo;
                            case "ivory": return SKColors.Ivory;
                            case "khaki": return SKColors.Khaki;
                            case "lavender": return SKColors.Lavender;
                            case "lavenderblush": return SKColors.LavenderBlush;
                            case "lawngreen": return SKColors.LawnGreen;
                            case "lemonchiffon": return SKColors.LemonChiffon;
                            case "lightblue": return SKColors.LightBlue;
                            case "lightcoral": return SKColors.LightCoral;
                            case "lightcyan": return SKColors.LightCyan;
                            case "lightgoldenrodyellow": return SKColors.LightGoldenrodYellow;
                            case "lightgray": return SKColors.LightGray;
                            case "lightgreen": return SKColors.LightGreen;
                            case "lightpink": return SKColors.LightPink;
                            case "lightsalmon": return SKColors.LightSalmon;
                            case "lightseagreen": return SKColors.LightSeaGreen;
                            case "lightskyblue": return SKColors.LightSkyBlue;
                            case "lightslategray": return SKColors.LightSlateGray;
                            case "lightsteelblue": return SKColors.LightSteelBlue;
                            case "lightyellow": return SKColors.LightYellow;
                            case "lime": return SKColors.Lime;
                            case "limegreen": return SKColors.LimeGreen;
                            case "linen": return SKColors.Linen;
                            case "magenta": return SKColors.Magenta;
                            case "maroon": return SKColors.Maroon;
                            case "mediumaquamarine": return SKColors.MediumAquamarine;
                            case "mediumblue": return SKColors.MediumBlue;
                            case "mediumorchid": return SKColors.MediumOrchid;
                            case "mediumpurple": return SKColors.MediumPurple;
                            case "mediumseagreen": return SKColors.MediumSeaGreen;
                            case "mediumslateblue": return SKColors.MediumSlateBlue;
                            case "mediumspringgreen": return SKColors.MediumSpringGreen;
                            case "mediumturquoise": return SKColors.MediumTurquoise;
                            case "mediumvioletred": return SKColors.MediumVioletRed;
                            case "midnightblue": return SKColors.MidnightBlue;
                            case "mintcream": return SKColors.MintCream;
                            case "mistyrose": return SKColors.MistyRose;
                            case "moccasin": return SKColors.Moccasin;
                            case "navajowhite": return SKColors.NavajoWhite;
                            case "navy": return SKColors.Navy;
                            case "oldlace": return SKColors.OldLace;
                            case "olive": return SKColors.Olive;
                            case "olivedrab": return SKColors.OliveDrab;
                            case "orange": return SKColors.Orange;
                            case "orangered": return SKColors.OrangeRed;
                            case "orchid": return SKColors.Orchid;
                            case "palegoldenrod": return SKColors.PaleGoldenrod;
                            case "palegreen": return SKColors.PaleGreen;
                            case "paleturquoise": return SKColors.PaleTurquoise;
                            case "palevioletred": return SKColors.PaleVioletRed;
                            case "papayawhip": return SKColors.PapayaWhip;
                            case "peachpuff": return SKColors.PeachPuff;
                            case "peru": return SKColors.Peru;
                            case "pink": return SKColors.Pink;
                            case "plum": return SKColors.Plum;
                            case "powderblue": return SKColors.PowderBlue;
                            case "purple": return SKColors.Purple;
                            case "red": return SKColors.Red;
                            case "rosybrown": return SKColors.RosyBrown;
                            case "royalblue": return SKColors.RoyalBlue;
                            case "saddlebrown": return SKColors.SaddleBrown;
                            case "salmon": return SKColors.Salmon;
                            case "sandybrown": return SKColors.SandyBrown;
                            case "seagreen": return SKColors.SeaGreen;
                            case "seashell": return SKColors.SeaShell;
                            case "sienna": return SKColors.Sienna;
                            case "silver": return SKColors.Silver;
                            case "skyblue": return SKColors.SkyBlue;
                            case "slateblue": return SKColors.SlateBlue;
                            case "slategray": return SKColors.SlateGray;
                            case "snow": return SKColors.Snow;
                            case "springgreen": return SKColors.SpringGreen;
                            case "steelblue": return SKColors.SteelBlue;
                            case "tan": return SKColors.Tan;
                            case "teal": return SKColors.Teal;
                            case "thistle": return SKColors.Thistle;
                            case "tomato": return SKColors.Tomato;
                            case "transparent": return SKColors.Transparent;
                            case "turquoise": return SKColors.Turquoise;
                            case "violet": return SKColors.Violet;
                            case "wheat": return SKColors.Wheat;
                            case "white": return SKColors.White;
                            case "whitesmoke": return SKColors.WhiteSmoke;
                            case "yellow": return SKColors.Yellow;
                            case "yellowgreen": return SKColors.YellowGreen;
                        }
                        var field = typeof(SKColors).GetFields().FirstOrDefault(fi => fi.IsStatic && string.Equals(fi.Name, color, StringComparison.OrdinalIgnoreCase));
                        if (field != null)
                            return (SKColor)field.GetValue(null);
                        var property = typeof(SKColors).GetProperties().FirstOrDefault(pi => string.Equals(pi.Name, color, StringComparison.OrdinalIgnoreCase) && pi.CanRead && pi.GetMethod.IsStatic);
                        if (property != null)
                            return (SKColor)property.GetValue(null, null);
                    }
                }
            }
            throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", new object[]
            {
            value,
            typeof(SKColor)
            }));
        }

        private static float ParseColorValue(string elem, int maxValue, bool acceptPercent)
        {
            elem = elem.Trim();
            if (elem.EndsWith("%", StringComparison.Ordinal) & acceptPercent)
            {
                maxValue = 100;
                elem = elem.Substring(0, elem.Length - 1);
            }
            return (float)int.Parse(elem, NumberStyles.Number, CultureInfo.InvariantCulture).Clamp(0, maxValue) / (float)maxValue;
        }

        private static byte ParseColorValue(string elem, byte maxValue, bool acceptPercent)
        {
            elem = elem.Trim();
            if (elem.EndsWith("%", StringComparison.Ordinal) & acceptPercent)
            {
                maxValue = 100;
                elem = elem.Substring(0, elem.Length - 1);
            }
            return Convert.ToByte(int.Parse(elem, NumberStyles.Number, CultureInfo.InvariantCulture).Clamp(0, maxValue) / (int)maxValue);
        }


        private static byte ParseOpacity(string elem)
        {
            return byte.Parse(elem, NumberStyles.Number, CultureInfo.InvariantCulture);
        }

        private static SKColor FromHex(string hex)
        {
            if (hex.Length < 3)
            {
                return SKColor.Empty;
            }
            int idx = (hex[0] == '#') ? 1 : 0;
            switch (hex.Length - idx)
            {
                case 3:
                    {
                        byte r = Convert.ToByte(ToHexD(hex[idx++]));
                        byte v = Convert.ToByte(ToHexD(hex[idx++]));
                        byte b = Convert.ToByte(ToHexD(hex[idx]));
                        return new SKColor(r, v, b);
                    }
                case 4:
                    {
                        byte a = Convert.ToByte(ToHexD(hex[idx++]));
                        byte r= Convert.ToByte(ToHexD(hex[idx++]));
                        byte v = Convert.ToByte(ToHexD(hex[idx++]));
                        byte b = Convert.ToByte(ToHexD(hex[idx]));
                        return new SKColor(r, v, b, a);
                    }
                case 6:
                    return new SKColor(Convert.ToByte((ToHex(hex[idx++]) << 4 | ToHex(hex[idx++]))), Convert.ToByte((ToHex(hex[idx++]) << 4 | ToHex(hex[idx++]))), Convert.ToByte((ToHex(hex[idx++]) << 4 | ToHex(hex[idx]))));
                case 8:
                    {
                        byte a = Convert.ToByte(ToHex(hex[idx++]) << 4 | ToHex(hex[idx++]));
                        return new SKColor(Convert.ToByte((ToHex(hex[idx++]) << 4 | ToHex(hex[idx++]))), Convert.ToByte((ToHex(hex[idx++]) << 4 | ToHex(hex[idx++]))), Convert.ToByte((ToHex(hex[idx++]) << 4 | ToHex(hex[idx]))), a);
                    }
            }
            return SKColor.Empty;
        }

        private static uint ToHexD(char c)
        {
            uint i = ToHex(c);
            return i << 4 | i;
        }

        private static uint ToHex(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return (uint)(c - '0');
            }
            ushort x = (ushort)(c | ' ');
            if (x >= 97 && x <= 102)
            {
                return (uint)(x - 97 + 10);
            }
            return 0u;
        }


    }
}
