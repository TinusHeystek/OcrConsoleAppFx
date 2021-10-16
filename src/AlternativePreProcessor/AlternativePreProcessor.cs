using SixLabors.ImageSharp;
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
        private static readonly HSLBounds _colorBounds = new HSLBounds(50, 75, 0.2f, 1, 0, 1);
        private static readonly HSLBounds _backgroundBounds1 = new HSLBounds(0, 30, 0.0f, 1, 0, 1); 
        private static readonly HSLBounds _backgroundBounds2 = new HSLBounds(70, 77, 0.0f, 0.2f, 0, 1);
        private static readonly HSLBounds _backgroundBounds3 = new HSLBounds(70, 90, 0.0f, 0.13f, 0, 1);

        /// <summary>
        /// Execute the PreProcessing
        /// </summary>
        /// <param name="byteArray">Image as a byte array</param>
        /// <returns>PreProcessed byte array</returns>
        public static byte[] PreprocessImage(byte[] byteArray, bool doProcessBackground)
        {
            var format = Image.DetectFormat(byteArray);

            string[] imageUrls = new string[4];

            using (var image = Image.Load(byteArray))
            {
                if (_sizeMultiplier != 1)
                {
                    image.Mutate(i => i.Resize(new Size(image.Width * _sizeMultiplier, image.Height * _sizeMultiplier)));
                }

                using (var img = image.Clone())
                {
                    if (doProcessBackground)
                    {
                        var hsl = ProcessBackground(image);
                        if (hsl.IsInBounds(_backgroundBounds1) || hsl.IsInBounds(_backgroundBounds2) || hsl.IsInBounds(_backgroundBounds3))
                        {
                            return img.ToArray(format);
                        }
                    }

                    ReplaceBackgroundPixels(img);

                    return img.ToArray(format);
                }
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

        /// <summary>
        /// No in use yet, to be evaluated
        /// </summary>
        /// <param name="img"></param>
        private static void RemoveNoise(Image<Rgba32> img)
        {
            ConcurrentBag<Pixel> noisePixels = new ConcurrentBag<Pixel>();
            Parallel.For(0, img.Height, y =>
            {
                Parallel.For(0, img.Width, x =>
                {
                    if (!IsPixelBlack(img[x, y]) && GetNeighbourCount(img, x, y) > 2)
                    {
                        noisePixels.Add(new Pixel(x,y));
                    }
                });
            });

            Parallel.ForEach(noisePixels, pixel => {
                if (pixel == null) return;
                img[pixel.X, pixel.Y] = _blackPixel;
            });
        }

        private static int GetNeighbourCount(Image<Rgba32> img, int x, int y)
        {
            var count = 0;
            // left
            if (x > 0 && IsPixelBlack(img[x - 1, y])) count++;
            // right
            if (x < img.Width - 1 && IsPixelBlack(img[x + 1, y])) count++;
            // top
            if (y > 0 && IsPixelBlack(img[x, y - 1])) count++;
            // bottom
            if (y < img.Height - 1 && IsPixelBlack(img[x, y + 1])) count++;

            return count;
        }

        private static bool IsPixelBlack(Rgba32 pixel)
        {
            return pixel.ToHex().Equals("000000FF");
        }

        private class Pixel
        {
            public Pixel(int x, int y)
            {
                X = x;
                Y = y;
            }
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
