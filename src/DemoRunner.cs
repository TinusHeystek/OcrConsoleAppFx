using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OcrConsoleAppFx
{
    public class DemoRunner
    {
        private static Pixel _reference = null;
        private Processor _processor = null;
        private readonly Regex _rgx = new Regex(@"^[0-9]{1,5}(?:.[0-9]{3})?$");
        private int _failureCount = -1;
        private bool _isFarming = false;

        public void Run(bool isFarming, bool isWindowed)
        {
            _isFarming = isFarming;
            _processor = new Processor(isWindowed);
            Console.Clear();
            int i = 0;
            while (true)
            {
                if (_failureCount == -1 || (_failureCount > 30 && _failureCount%10 == 0) )
                {
                    _reference = FindBracketLocation();
                    if (_reference != null)
                    {
                        var height = Screen.PrimaryScreen.Bounds.Height;
                        var width = Screen.PrimaryScreen.Bounds.Width;
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine($"Reference: [{_reference.Y},{_reference.X}]");
                    }
                }

                if (_reference != null)
                {

                    var bitmap = CapturePartOfScreen();

                    var byteArray = ImageToByte(bitmap);

                    if (byteArray != null)
                    {                       
                        var result = _processor.ProcessImageNew(byteArray);

                        var results = result.Split(' ');

                        if (results.Any(r => !_rgx.IsMatch(r)))
                        {
                            _failureCount++;

                            // just for testing now -> TBR
                            Console.SetCursorPosition(0, 6);
                            Console.WriteLine("Bad: {0}", result);
                            
                            if(_isFarming) bitmap.Save($"D:\\Capture_{i}.bmp", ImageFormat.Bmp);

                        }
                        else
                        {
                            Console.SetCursorPosition(0, 4);
                            Console.WriteLine("Good: {0}", result);
                            _failureCount = 0;
                        }
                    }
                    else 
                    {
                        _failureCount++;
                    }
                }
                else 
                {
                    _failureCount++;
                    Console.SetCursorPosition(0, 2);
                    Console.WriteLine("No Coordinates found!");
                }
                Thread.Sleep(100);
                i++;
            }
        }

        private Bitmap CapturePartOfScreen()
        {
            try
            {
                Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                Graphics g = Graphics.FromImage(bm);
                g.CopyFromScreen(0, 0, 0, 0, bm.Size);


                var maxWidth = Math.Min(Screen.PrimaryScreen.Bounds.Width - _reference.X, 284);

                System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(_reference.X, _reference.Y,maxWidth,14 );

                Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                using (Graphics g2 = Graphics.FromImage(target))
                {
                    g2.DrawImage(bm, new System.Drawing.Rectangle(0, 0, target.Width, target.Height),
                                        cropRect,
                                        GraphicsUnit.Pixel);


                }

                return target;
            }
            catch (Exception)
            {

                return null;
            }

        }

        public byte[] ImageToByte(Bitmap img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        private Pixel FindBracketLocation()
        {
            var screenshot = TopBarScreenShot();
            return FindCoordinates2(screenshot);
   
        }

        private byte[] TopBarScreenShot()
        {
            try
            {
                Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                Graphics g = Graphics.FromImage(bm);
                g.CopyFromScreen(0, 0, 0, 0, bm.Size);


                System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(0, 0, Screen.PrimaryScreen.Bounds.Width, 100);

                Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                using (Graphics g2 = Graphics.FromImage(target))
                {
                    g2.DrawImage(bm, new System.Drawing.Rectangle(0, 0, target.Width, target.Height),
                                        cropRect,
                                        GraphicsUnit.Pixel);


                }
                target.Save(@"D:\Test1234.bmp", ImageFormat.Bmp);

                return ImageToByte(target);
            }
            catch (Exception)
            {

                return null;
            }
        }

        //private static Pixel FindCoordinates(byte[] byteArray)
        //{
        //    if (byteArray == null) return null;
        //    Pixel bracketPixel = null;

        //    //byteArray = File.ReadAllBytes(@"E:\Test12345.bmp");

        //    using (var image = SixLabors.ImageSharp.Image.Load(byteArray))
        //    {
        //        Parallel.For(1, image.Height, (y, yState) =>
        //        {
        //            Parallel.For(1, image.Width - 1, (x, xState) =>
        //            {
        //                var hslColor = HSLColor.ColorOfPixel(image, x, y);

        //                if (hslColor.Hue >= 58 
        //                && hslColor.Hue <= 66
        //                && image.Height - y > 12
        //                && IsFullYellowPixel(image, x - 1, y)
        //                && IsFullYellowPixel(image, x, y + 1)
        //                && IsFullYellowPixel(image, x, y + 2)
        //                && IsFullYellowPixel(image, x, y + 3)
        //                && IsFullYellowPixel(image, x, y + 4)
        //                && IsFullYellowPixel(image, x, y + 5)
        //                && IsFullYellowPixel(image, x, y + 6)
        //                && IsFullYellowPixel(image, x, y + 7)
        //                && IsFullYellowPixel(image, x, y + 8)
        //                && IsFullYellowPixel(image, x, y + 9)
        //                && IsFullYellowPixel(image, x, y + 10)
        //                && IsFullYellowPixel(image, x, y + 11)
        //                && IsFullYellowPixel(image, x - 1, y + 11)
        //                && !IsFullYellowPixel(image, x +1, y + 12)
        //                //&& !IsFullYellowPixel(image, x + 1, y + 11)
        //                )
        //                {
        //                    bracketPixel = new Pixel(x, y-1);
        //                    xState.Break();
        //                    yState.Break();
        //                }
        //            });
        //        });
        //    }
        //    return bracketPixel;
        //}


        private static Pixel FindCoordinates2(byte[] byteArray)
        {
            if (byteArray == null) return null;
            Pixel startPosition = null;

            //byteArray = File.ReadAllBytes(@"E:\Test12345.bmp");

            using (var image = SixLabors.ImageSharp.Image.Load(byteArray))
            {
                Parallel.For(1, image.Height - 12, (y, yState) =>
                {
                    Parallel.For(1, image.Width - 300, (x, xState) =>
                    {
                        var hslColor = HSLColor.ColorOfPixel(image, x, y+1);

                    if (hslColor.Hue >= 58
                    && hslColor.Hue <= 66
                    && PixelDefinitions.Position.All( p => IsFullYellowPixel(image, p, x, y))
                    && PixelDefinitions.PositionNot.All( p => IsDarkPixel(image, p, x, y)))
                        {
                            startPosition = new Pixel(x+89, y-1);
                            xState.Break();
                            yState.Break();
                        }
                    });
                });
            }
            return startPosition;
        }



        private static bool IsFullYellowPixel(Image<Rgba32> image, Pixel p, int x, int y)
        {
            var hsl = HSLColor.ColorOfPixel(image, p.X + x, p.Y + y);
            return hsl.Hue >= 50 && hsl.Hue <= 66 && hsl.Luminosity > 0.5f;
        }

        private static bool IsDarkPixel(Image<Rgba32> image, Pixel p, int x, int y)
        {
            var R = image[x + p.X, y + p.Y].R;
            var G = image[x + p.X, y + p.Y].G;
            var B = image[x + p.X, y + p.Y].B;

            return R < 100 && G < 100 && B < 100;
        }
    }
}
