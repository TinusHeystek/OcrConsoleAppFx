﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

        public void Run()
        {
            _processor = new Processor(true);
            Console.Clear();
            int i = 0;
            while (true)
            {
                if (_failureCount == -1 || (_failureCount > 50 && _failureCount%10 == 0) )
                {
                    _reference = FindBracketLocation();
                    if (_reference != null)
                    {
                        var height = Screen.PrimaryScreen.Bounds.Height;
                        var width = Screen.PrimaryScreen.Bounds.Width;
                        Console.WriteLine();
                        Console.WriteLine($"Height: {height}");
                        Console.WriteLine($"Width: {width}");
                        Console.WriteLine($"Reference Pixel is {_reference.Y} pixels from the TOP & {width - _reference.X} pixels from the RIGHT");
                        Console.WriteLine();
                    }
                }

                if (_reference != null)
                {
                    var byteArray = CapturePartOfScreen();

                    if (byteArray != null)
                    {
                        var result = _processor.ProcessImage(byteArray);

                        var results = result.Split(' ');

                        if (results.Any(r => !_rgx.IsMatch(r)))
                        {
                            _failureCount++;

                            // just for testing now -> TBR
                            Console.Write("\r{0}", result);
                        }
                        else {
                            Console.Write("\r{0}", result);
                            _failureCount = 0;
                        }                                      
                    }               
                }
                else 
                {
                    _failureCount++;
                    Console.Write("\rNo Coordinates found on screen ");
                }
                Thread.Sleep(100);
                i++;
            }
        }

        private byte[] CapturePartOfScreen()
        {
            try
            {
                Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                Graphics g = Graphics.FromImage(bm);
                g.CopyFromScreen(0, 0, 0, 0, bm.Size);


                System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(_reference.X - 274, _reference.Y, 284, 14);

                Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                using (Graphics g2 = Graphics.FromImage(target))
                {
                    g2.DrawImage(bm, new System.Drawing.Rectangle(0, 0, target.Width, target.Height),
                                        cropRect,
                                        GraphicsUnit.Pixel);


                }
                //target.Save(@"D:\Test123.bmp", ImageFormat.Bmp);

                return ImageToByte(target);
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
            return FindCoordinates(screenshot);
   
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
                //target.Save(@"D:\Test1234.bmp", ImageFormat.Bmp);

                return ImageToByte(target);
            }
            catch (Exception)
            {

                return null;
            }
        }

        private static Pixel FindCoordinates(byte[] byteArray)
        {
            Pixel bracketPixel = null;
            using (var image = SixLabors.ImageSharp.Image.Load(byteArray))
            {
                Parallel.For(1, image.Height, (y, yState) =>
                {
                    Parallel.For(1, image.Width - 1, (x, xState) =>
                    {
                        var hslColor = HSLColor.ColorOfPixel(image, x, y);

                        if (hslColor.Hue >= 58 
                        && hslColor.Hue <= 66
                        && image.Height - y > 12
                        && IsFullYellowPixel(image, x - 1, y)
                        && IsFullYellowPixel(image, x, y + 1)
                        && IsFullYellowPixel(image, x, y + 2)
                        && IsFullYellowPixel(image, x, y + 3)
                        && IsFullYellowPixel(image, x, y + 4)
                        && IsFullYellowPixel(image, x, y + 5)
                        && IsFullYellowPixel(image, x, y + 6)
                        && IsFullYellowPixel(image, x, y + 7)
                        && IsFullYellowPixel(image, x, y + 8)
                        && IsFullYellowPixel(image, x, y + 9)
                        && IsFullYellowPixel(image, x, y + 10)
                        && IsFullYellowPixel(image, x, y + 11)
                        && IsFullYellowPixel(image, x - 1, y + 11)
                        && !IsFullYellowPixel(image, x + 1, y + 11)
                        )
                        {
                            bracketPixel = new Pixel(x, y-1);
                            xState.Break();
                            yState.Break();
                        }
                    });
                });
            }
            return bracketPixel;
        }


        private static bool IsFullYellowPixel(Image<Rgba32> image, int x, int y)
        {
            var hsl = HSLColor.ColorOfPixel(image, x, y);
            return hsl.Hue >= 58 && hsl.Hue <= 66 && hsl.Luminosity > 0.7f;
        }
    }
}
