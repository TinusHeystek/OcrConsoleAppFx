using SampleApp.AlternativePreProcessor;
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
using System.Text.RegularExpressions;
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
        //private HSLBounds _colorBounds;

        public Processor(bool isWindowMode)
        {
            _isWindowMode = isWindowMode;
            _appliedHMax = _isWindowMode ? _windowedHMax : _fullscreenHMax;
            //_colorBounds = new HSLBounds(55,_appliedHMax,)
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
                int h = 0;
                for (int i = 0; i < 30; i++)
                {
                    if (_isWindowMode && i > 3)
                    {
                        if (i == 4) j=1;
                        if ( i > 12 &&  !Char.IsDigit(text[0]) && !Char.IsDigit(text[1])) j = 2;
                    }

                    var rect = new Rectangle(img.Width - 23 - (9 * i) + j, 0, 9, 14);
                    if (rect.X < 0) continue;
                    var newImg = img.Clone();
                    newImg.Mutate(x => x.Crop(rect));
                    //newImg.SaveAsPng(Path.Combine(Environment.CurrentDirectory, $"test_{i}.png"));
                    //newImg = Image.Load(File.ReadAllBytes(@"D:\\7.bmp"));

                    var newChar = GetChar(newImg);
                    text = newChar + text;
                    //if (newChar.Equals("?") && _isWindowMode) j = 1;
                    //if (newChar.Equals("!") && _isWindowMode) h++;
                    //if (h == 2) j = 2;
                    
                }
                text = text.Replace("?", ".");
                text = text.Replace("!", ".");
                text = text.TrimStart('.');
                text = text.Replace(". ", "");
                
                text = text.Replace("..", " ");
                
                return text;
            }
        }

        private string GetChar(Image<Rgba32> image)
        {
            var matchedPixels = GetMatchingPixels(image, 30, 90);


            return _isWindowMode ? GetCharFromMatch2(matchedPixels, image) : GetCharFromMatch(matchedPixels, image);
        }

        private List<Pixel> GetMatchingPixels(Image<Rgba32> image, int hMin, int hMax)
        {
            ConcurrentBag<Pixel> matchingPixels = new ConcurrentBag<Pixel>();

            Parallel.For(0, image.Height, y =>
            {
                Parallel.For(0, image.Width, x =>
                {

                    var R = image[x, y].R;
                    var G = image[x, y].G;
                    var B = image[x, y].B;

                    var hslColor = HSLColor.ColorOfPixel(image, x, y);
                    //var hsbColor = HSBColor.ColorOfPixel(image, x, y);

                    if (hslColor.Hue >= hMin && hslColor.Hue <= hMax
                    && !(R < 100 || G < 100 || B < 70 )
                    && !(R > 230 && G > 230 && B > 230))
                   //if (hsbColor.Hue >= hMin && hsbColor.Hue <= hMax && hsbColor.Brightness > 0.5f /*&& hsbColor.Saturation > 5*/ )
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
           // if (IsMatch(matches, PixelDefinitions.char8) && IsNoMatch(matches, PixelDefinitions.char8Not)) return "8";
           // if (IsMatch(matches, PixelDefinitions.char3) && IsNoMatch(matches, PixelDefinitions.char3Not)) return "3";
            if (IsMatch(matches, PixelDefinitions.char3or8) && IsNoMatch(matches, PixelDefinitions.char3or8Not)) return Isit3Or8(image);                      
            if (IsMatch(matches, PixelDefinitions.char7) && IsNoMatch(matches, PixelDefinitions.char7Not)) return "7";
            if (IsMatch(matches, PixelDefinitions.comma) && IsNoMatch(matches, PixelDefinitions.commaNot)) return "."; // also returns dot here, todo= make OR 
            if (IsMatch(matches, PixelDefinitions.dot) && IsNoMatch(matches, PixelDefinitions.dotNot)) return "?";

            return ".";
        }

        private string GetCharFromMatch2(List<Pixel> matches, Image<Rgba32> image)
        {
            if (IsMatch(matches, PixelDefinitions.leftBracket) && IsNoMatch(matches, PixelDefinitions.leftBracketNot)) return "."; // No need to return [
            if (IsMatch(matches, PixelDefinitions.char1Windowed) && IsNoMatch(matches, PixelDefinitions.char1WindowedNot)) return "1";
            if (IsMatch(matches, PixelDefinitions.char4Windowed) && IsNoMatch(matches, PixelDefinitions.char4WindowedNot)) return "4";
            if ((IsMatch(matches, PixelDefinitions.char8Windowed) && IsNoMatch(matches, PixelDefinitions.char8WindowedNot)) 
                || (IsMatch(matches, PixelDefinitions.char3) && IsNoMatch(matches, PixelDefinitions.char3Not))) return Isit3Or8Windowed(image);
            if (IsMatch(matches, PixelDefinitions.char0Windowed) && IsNoMatch(matches, PixelDefinitions.char0WindowedNot)) return "0";
            if ((IsMatch(matches, PixelDefinitions.char6Windowed) && IsNoMatch(matches, PixelDefinitions.char6WindowedNot))
                ||( IsMatch(matches, PixelDefinitions.char5Windowed) && IsNoMatch(matches, PixelDefinitions.char5WindowedNot))) return Isit5Or6Windowed(image);
            if (IsMatch(matches, PixelDefinitions.char9Windowed) && IsNoMatch(matches, PixelDefinitions.char9WindowedNot)) return "9";
            if (IsMatch(matches, PixelDefinitions.char2) && IsNoMatch(matches, PixelDefinitions.char2WindowedNot)) return "2";
            if (IsMatch(matches, PixelDefinitions.char3or8) && IsNoMatch(matches, PixelDefinitions.char3or8Not)) return Isit3Or8(image);
            if (IsMatch(matches, PixelDefinitions.char7) && IsNoMatch(matches, PixelDefinitions.char7Not)) return "7";
            if (IsMatch(matches, PixelDefinitions.commaWindowed) && IsNoMatch(matches, PixelDefinitions.commaWindowedNot)) return "!"; // also returns dot here, todo= make OR 
            if (IsMatch(matches, PixelDefinitions.dot) && IsNoMatch(matches, PixelDefinitions.dotNot)) return "?";

            return ".";
        }

        private string Isit3Or8Windowed(Image<Rgba32> image)
        {
            var matches = GetMatchingPixels(image, 30, 90);

            var hsl32 = HSLColor.ColorOfPixel(image, 3, 2);
            var hsl23 = HSLColor.ColorOfPixel(image, 2, 3);

            if (!((Math.Abs(hsl32.Hue - hsl23.Hue) > 15) || (Math.Abs(hsl32.Luminosity - hsl23.Luminosity) > 0.4f))
                && IsMatch(matches, PixelDefinitions.char8Windowed)
                && IsNoMatch(matches, PixelDefinitions.char8WindowedNot)) return "8";

            return "3";
        }

        private string Isit5Or6Windowed(Image<Rgba32> image)
        {
            var matches = GetMatchingPixels(image, 30, 90);

            var hsl19 = HSLColor.ColorOfPixel(image, 2, 9);
            var hsl30 = HSLColor.ColorOfPixel(image, 2, 10);

            if (!((Math.Abs(hsl19.Hue - hsl30.Hue) > 15) || (Math.Abs(hsl19.Luminosity - hsl30.Luminosity) > 0.4f))
                && IsMatch(matches, PixelDefinitions.char6Windowed)
                && IsNoMatch(matches, PixelDefinitions.char6WindowedNot)) return "6";

            return "5";
        }

        private string Isit3Or8(Image<Rgba32> image)
        {
            var matches = GetMatchingPixels(image, 30, 90);

            var hsl29 = HSLColor.ColorOfPixel(image, 2, 9);
            var hsl30 = HSLColor.ColorOfPixel(image, 2, 10);

            if (!((Math.Abs(hsl29.Hue - hsl30.Hue) > 10) || (Math.Abs(hsl29.Luminosity - hsl30.Luminosity) > 0.3f)) 
                && IsMatch(matches, PixelDefinitions.char8) 
                && IsNoMatch(matches, PixelDefinitions.char8Not)) return "8";

            return "3";
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
