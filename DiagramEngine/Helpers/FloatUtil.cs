using System;
using SkiaSharp;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Helpers
{
    public static class FloatUtil
    {
        public static bool AreClose(this float value1, float value2)
        {
            if (float.IsNaN(value1) && float.IsNaN(value2))
                return true;
            else if (float.IsPositiveInfinity(value1) && float.IsPositiveInfinity(value2))
                return true;
            else if (float.IsNegativeInfinity(value1) && float.IsNegativeInfinity(value2))
                return true;
            else
                return value1 - value2 <= float.Epsilon;

        }
        public static bool AreClose(SKSize availableSize, SKSize previousAvailableSize)
        {
            return AreClose(availableSize.Width, previousAvailableSize.Width) && AreClose(availableSize.Height, previousAvailableSize.Height);
        }

        internal static bool AreClose(SKRect rect1, SKRect rect2)
        {
            return AreClose(rect1.Location, rect2.Location) && AreClose(rect1.Size,rect2.Size);
        }

        private static bool AreClose(SKPoint location1, SKPoint location2)
        {
            return AreClose(location1.X, location2.X) && AreClose(location1.Y, location2.Y);
        }
    }
}