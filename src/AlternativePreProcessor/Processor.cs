using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OcrConsoleAppFx
{
    /// <summary>
    /// PreProcessor which retains all pixels within the HSL bounds
    /// All other pixels are converted to a black pixel, used for contrast
    /// </summary>
    public class Processor
    {
        private bool _isWindowMode = false;
        private readonly int _appliedHMax = 65;
        private readonly int _fullscreenHMax = 65;
        private readonly int _windowedHMax = 80;

        public Processor(bool isWindowMode)
        {
            _isWindowMode = isWindowMode;

            _appliedHMax = _isWindowMode ? _windowedHMax : _fullscreenHMax;
        }

        /// <summary>
        /// Execute the Processing
        /// </summary>
        /// <param name="byteArray">Image as a byte array</param>
        /// <returns>PreProcessed byte array</returns>
        public string ProcessImage(byte[] byteArray)
        {
            using (var img = Image.Load(byteArray))
            {
                string text = string.Empty;
                int j = 0;
                for (int i = 0; i < 30; i++)
                {                 
                    var rect = new Rectangle(img.Width - 23 - (9 * i) + j, 0, 9, 14);
                    if (rect.X < 0) continue;
                    var newImg = img.Clone();
                    newImg.Mutate(x => x.Crop(rect));
                    newImg.SaveAsPng(Path.Combine(Environment.CurrentDirectory, $"_{i}.png"));

                    var newChar = GetChar(newImg);
                    text = newChar + text;
                    if (newChar.Equals("?") && _isWindowMode) j++;
                    
                }
                text = text.Replace("?", ".");
                text = text.TrimStart('.');
                text = text.Replace(". ", "");
                
                text = text.Replace("..", " ");
                
                return text;
            }
        }

        private string GetChar(Image<Rgba32> image)
        {
            var matchedPixels = GetMatchingPixels(image, 55, _appliedHMax);

            return GetCharFromMatch(matchedPixels, image);
        }

        private List<Pixel> GetMatchingPixels(Image<Rgba32> image, int hMin, int hMax)
        {
            ConcurrentBag<Pixel> matchingPixels = new ConcurrentBag<Pixel>();

            Parallel.For(0, image.Height, y =>
            {
                Parallel.For(0, image.Width, x =>
                {

                    var hslColor = HSLColor.ColorOfPixel(image, x, y);

                    if (hslColor.Hue >= hMin && hslColor.Hue <= hMax && hslColor.Luminosity > 0.3f)
                    {
                        matchingPixels.Add(new Pixel(x, y));
                    }

                });
            });
            return matchingPixels.ToList();
        }


        private string GetCharFromMatch(List<Pixel> matches, Image<Rgba32> image)
        {
            if (IsMatch(matches, PixelDefinitions.leftBracket) && IsNoMatch(matches, PixelDefinitions.leftBracketNot)) return "."; // No need to return [
            if (IsMatch(matches, PixelDefinitions.char1) && IsNoMatch(matches, PixelDefinitions.char1Not)) return "1";
            if (IsMatch(matches, PixelDefinitions.char4) && IsNoMatch(matches, PixelDefinitions.char4Not)) return "4";
            if (IsMatch(matches, PixelDefinitions.char0) && IsNoMatch(matches, PixelDefinitions.char0Not)) return "0";
            if (IsMatch(matches, PixelDefinitions.char5) && IsNoMatch(matches, PixelDefinitions.char5Not)) return "5";
            if (IsMatch(matches, PixelDefinitions.char6) && IsNoMatch(matches, PixelDefinitions.char6Not)) return "6";
            if (IsMatch(matches, PixelDefinitions.char9) && IsNoMatch(matches, PixelDefinitions.char9Not)) return "9";
            if (IsMatch(matches, PixelDefinitions.char2) && IsNoMatch(matches, PixelDefinitions.char2Not)) return "2";
            if (IsMatch(matches, PixelDefinitions.char3or8) && IsNoMatch(matches, PixelDefinitions.char3or8Not)) return Isit3Or8(image);                      
            if (IsMatch(matches, PixelDefinitions.char7) && IsNoMatch(matches, PixelDefinitions.char7Not)) return "7";
            if (IsMatch(matches, PixelDefinitions.comma) && IsNoMatch(matches, PixelDefinitions.commaNot)) return "."; // also returns dot here, todo= make OR 
            if (IsMatch(matches, PixelDefinitions.dot) && IsNoMatch(matches, PixelDefinitions.dotNot)) return "?";

            return ".";
        }

        

        private string Isit3Or8(Image<Rgba32> image)
        {
            var matches = GetMatchingPixels(image, 45, 75);

            var hsl29 = HSLColor.ColorOfPixel(image, 2, 9);
            var hsl30 = HSLColor.ColorOfPixel(image, 2, 10);

            if ((Math.Abs(hsl29.Hue - hsl30.Hue) > 10) || (Math.Abs(hsl29.Luminosity - hsl30.Luminosity) > 0.3f)) return "3";

            return "8";
        }

        private bool IsMatch(List<Pixel> matches, List<Pixel> reference)
        {
            var intersectCount = matches.Intersect(reference).Count();
            return intersectCount == reference.Count();
        }

        private bool IsNoMatch(List<Pixel> matches, List<Pixel> reference)
        {
            var intersectCount = matches.Intersect(reference).Count();
            return intersectCount == 0;
        }
    }
}
