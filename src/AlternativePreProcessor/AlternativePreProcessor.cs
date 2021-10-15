using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
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

        /// <summary>
        /// Execute the PreProcessing
        /// </summary>
        /// <param name="byteArray">Image as a byte array</param>
        /// <returns>PreProcessed byte array</returns>
        public static byte[] PreprocessImage(byte[] byteArray)
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
                    Parallel.For(0, img.Height, y =>
                    {
                        Parallel.For(0, img.Width, x =>
                        {
                            var R = img[x, y].R;
                            var G = img[x, y].G;
                            var B = img[x, y].B;

                            var hslColor = HSLColor.FromRGB(R, G, B);

                            if (!hslColor.IsInBounds(_colorBounds))
                            {
                                img[x, y] = _blackPixel;
                            }
                        });
                    });
                    return img.ToArray(format);
                }             
            }
        }
    }
}
