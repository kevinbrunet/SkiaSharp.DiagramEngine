using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ModelerClient.DiagramEngine.Helpers
{
    public class SKPathTypeConverter : TypeConverter
    {
        public override object ConvertFromInvariantString(string value)
        {
            return SKPath.ParseSvgPathData(value);
        }
    }
}
