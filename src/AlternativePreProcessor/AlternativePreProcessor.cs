using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrConsoleAppFx
{
    /// <summary>
    /// PreProcessor which retains all pixels within the HSL bounds
    /// All other pixels are converted to a black pixel, used for contrast
    /// </summary>
    public class AlternativePreProcessor
    {
        /// <summary>
        /// You can play around with these settings to finetune the PreProcessing
        /// </summary>
        private static readonly int _sizeMultiplier = 3;
        private static readonly Rgba32 _blackPixel = Rgba32.ParseHex("000");
        private static readonly HSLBounds _colorBounds = new HSLBounds(50, 75, 0.18f, 1, 0, 1);
        private static readonly HSLBounds _backgroundBounds1 = new HSLBounds(0, 15, 0.0f, 1, 0, 1); 
        private static readonly HSLBounds _backgroundBounds2 = new HSLBounds(70, 80, 0.0f, 0.15f, 0.75f, 1);
        private static readonly HSLBounds _backgroundBounds3 = new HSLBounds(80, 90, 0.0f, 0.13f, 0.75f, 1);

        /// <summary>
        /// Execute the PreProcessing
        /// </summary>
        /// <param name="byteArray">Image as a byte array</param>
        /// <returns>PreProcessed byte array</returns>
        public static byte[] PreprocessImage(byte[] byteArray, bool doProcessBackground, out bool wasChanged)
        {
            using (var img = Image.Load(byteArray))
            {
                if (_sizeMultiplier != 1)
                {
                    img.Mutate(i => i.Resize(new Size(img.Width * _sizeMultiplier, img.Height * _sizeMultiplier)));
                }

                if (doProcessBackground)
                {
                    var hsl = ProcessBackground(img);
                    if (hsl.IsInBounds(_backgroundBounds1) || hsl.IsInBounds(_backgroundBounds2) || hsl.IsInBounds(_backgroundBounds3))
                    {
                        wasChanged = false;
                        return img.ToArray(PngFormat.Instance);
                    }
                }

                ReplaceBackgroundPixels(img);
                wasChanged = true;

                return img.ToArray(PngFormat.Instance);
            }
        }

        private static void ReplaceBackgroundPixels(Image<Rgba32> image)
        {
            Parallel.For(0, image.Height, y =>
            {
                Parallel.For(0, image.Width, x =>
                {
                    var R = image[x, y].R;
                    var G = image[x, y].G;
                    var B = image[x, y].B;

                    var hslColor = HSLColor.FromRGB(R, G, B);

                    var pixel = image[x, y];

                    if (!hslColor.IsInBounds(_colorBounds))
                    {
                        image[x, y] = _blackPixel;
                    }
                });
            });
        }

        private static HSLColor ProcessBackground(Image<Rgba32> image)
        {
            Span<Rgba32> pixelRowSpan = image.GetPixelRowSpan(image.Height - 2);
            float HCount = 0.0f;
            float SCount = 0.0f;
            float LCount = 0.0f;

            for (int i = 0; i < pixelRowSpan.Length; i++)
            {
                var p = pixelRowSpan[i];

                var hsl = HSLColor.FromRGB(p.R, p.G, p.B);
                HCount += hsl.Hue;
                SCount += hsl.Saturation;
                LCount += hsl.Luminosity;
            }

            HCount /= pixelRowSpan.Length;
            SCount /= pixelRowSpan.Length;
            LCount /= pixelRowSpan.Length;

            return new HSLColor(HCount, SCount, LCount);
        }

    }
}
