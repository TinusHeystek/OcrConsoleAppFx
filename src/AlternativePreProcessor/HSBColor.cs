using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrConsoleAppFx
{
    /// <summary>
    /// Hue, Saturation & Luminosity properties of a Color
    /// Currently only supports creation from RGB
    /// </summary>
    public class HSBColor
    {
        public float Hue;
        public float Saturation;
        public float Brightness;

        public HSBColor(float H, float S, float B)
        {
            Hue = H;
            Saturation = S;
            Brightness = B;
        }

        public static HSBColor FromRGB(Byte R, Byte G, Byte B)
        {
            // _NOTE #1: Even though we're dealing with a very small range of
            // numbers, the accuracy of all calculations is fairly important.
            // For this reason, I've opted to use double data types instead
            // of float, which gives us a little bit extra precision (recall
            // that precision is the number of significant digits with which
            // the result is expressed).

            float r = (R / 255f);
            float g = (G / 255f);
            float b = (B / 255f);

            var minValue = getMinimumValue(r, g, b);
            var maxValue = getMaximumValue(r, g, b);
            var delta = maxValue - minValue;

            double hue = 0;
            double saturation;
            var brightness = maxValue * 100;

            if (Math.Abs(maxValue - 0) < double.Epsilon || Math.Abs(delta - 0) < double.Epsilon)
            {
                hue = 0;
                saturation = 0;
            }
            else
            {
                // _NOTE #2: FXCop insists that we avoid testing for floating 
                // point equality (CA1902). Instead, we'll perform a series of
                // tests with the help of Double.Epsilon that will provide 
                // a more accurate equality evaluation.

                if (Math.Abs(minValue - 0) < double.Epsilon)
                {
                    saturation = 100;
                }
                else
                {
                    saturation = delta / maxValue * 100;
                }

                if (Math.Abs(r - maxValue) < double.Epsilon)
                {
                    hue = (g - b) / delta;
                }
                else if (Math.Abs(g - maxValue) < double.Epsilon)
                {
                    hue = 2 + (b - r) / delta;
                }
                else if (Math.Abs(b - maxValue) < double.Epsilon)
                {
                    hue = 4 + (r - g) / delta;
                }
            }

            hue *= 60;
            if (hue < 0)
            {
                hue += 360;
            }

            return new HSBColor(
                (float)hue,
                (float)saturation,
                (float)brightness);
        }

        /// <summary>
        /// Determines the minimum value of all of the numbers provided in the
        /// variable argument list.
        /// </summary>
        private static double getMinimumValue(
            params double[] values)
        {
            var minValue = values[0];

            if (values.Length >= 2)
            {
                for (var i = 1; i < values.Length; i++)
                {
                    var num = values[i];
                    minValue = Math.Min(minValue, num);
                }
            }

            return minValue;
        }

        /// <summary>
        /// Determines the maximum value of all of the numbers provided in the
        /// variable argument list.
        /// </summary>
        private static double getMaximumValue(
            params double[] values)
        {
            var maxValue = values[0];

            if (values.Length >= 2)
            {
                for (var i = 1; i < values.Length; i++)
                {
                    var num = values[i];
                    maxValue = Math.Max(maxValue, num);
                }
            }

            return maxValue;
        }

        public static HSBColor ColorOfPixel(Image<Rgba32> image, int x, int y)
        {
            var R = image[x, y].R;
            var G = image[x, y].G;
            var B = image[x, y].B;

            return HSBColor.FromRGB(R, G, B);
        }
    }
}
