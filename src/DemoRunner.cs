using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OcrConsoleAppFx
{
    public static class DemoRunner
    {
        private static Pixel _reference = null;
        public static void Run()
        {
            Console.Clear();
            int i = 0;
            while (true)
            {
                if (i % 1000 == 0)
                {
                    _reference = FindBracketLocation();
                }

                if (_reference != null)
                {
                    var byteArray = CapturePartOfScreen();

                    if (byteArray != null)
                    {
                        var result = Processor.ProcessImage(byteArray, _reference);
                        Console.Write("\r{0}", result);
                    }               
                }
                else 
                {
                        Console.Write("\rNo Coordinates found on screen ");
                }
                Thread.Sleep(10);
                i++;
            }
        }

        private static byte[] CapturePartOfScreen()
        {
            try
            {
                Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                Graphics g = Graphics.FromImage(bm);
                g.CopyFromScreen(0, 0, 0, 0, bm.Size);


                System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(_reference.X - 260, _reference.Y, 270, 14);

                Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                using (Graphics g2 = Graphics.FromImage(target))
                {
                    g2.DrawImage(bm, new System.Drawing.Rectangle(0, 0, target.Width, target.Height),
                                        cropRect,
                                        GraphicsUnit.Pixel);


                }
                target.Save(@"D:\Test123.bmp", ImageFormat.Bmp);

                return ImageToByte(target);
            }
            catch (Exception)
            {

                return null;
            }

        }

        public static byte[] ImageToByte(Bitmap img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        private static Pixel FindBracketLocation()
        {
            var screenshot = TopBarScreenShot();
            return FindCoordinates(screenshot);
   
        }

        private static byte[] TopBarScreenShot()
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

                        if (hslColor.Hue == 60
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
            return hsl.Hue == 60 && hsl.Luminosity > 0.8f;
        }
    }
}
