using SkiaSharp;

namespace ModelerClient.DiagramEngine.Helpers
{
    public static class SKMatrixHelper
    {
        public static SKMatrix Rotate(this SkiaSharp.SKMatrix m, float angle)
        {
            return m.Concat(SKMatrix.MakeRotation(angle));
        }

        public static SKMatrix RotateDegrees(this SkiaSharp.SKMatrix m, float andegrees)
        {
            return m.Concat(SKMatrix.MakeRotationDegrees(andegrees));
        }

        public static SKMatrix Scale(this SkiaSharp.SKMatrix m, float sx, float sy)
        {
            return m.Concat(SKMatrix.MakeScale(sx, sy));
        }

        public static SKMatrix Translate(this SkiaSharp.SKMatrix m, float dx, float dy)
        {
            return m.Concat(SKMatrix.MakeTranslation(dx, dy));
        }

        public static SKMatrix Invert(this SkiaSharp.SKMatrix m)
        {
            m.TryInvert(out var m2);
            return m2;
        }

        public static SKMatrix Concat(this SkiaSharp.SKMatrix m, SkiaSharp.SKMatrix m2)
        {
            var target = m;
            SkiaSharp.SKMatrix.PreConcat(ref target, m2);
            return target;
        }
    }
}
