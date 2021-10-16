using System;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using Tesseract;

namespace OcrConsoleAppFx
{
    internal class Program
    {
        private const string ImageDirectory = @"Images"; 
        private const string ProcessDirectory = @"Images\Processed"; 
        
        static void Main(string[] args)
        {
            Cleanup();
            for (int i = 0; i < 103; i++)
            {
                ProcessImage(i);
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void Cleanup()
        {
            if (!Directory.Exists(ProcessDirectory))
                Directory.CreateDirectory(ProcessDirectory);
            else
            {
                var files = Directory.GetFiles(ProcessDirectory, "*", SearchOption.TopDirectoryOnly).ToList();
                files.ForEach(File.Delete);
            }
        }
 
        private static void ProcessImage(int number)
        {
            var fileName = $"Position{number}.png";
            var byteArray = File.ReadAllBytes(Path.Combine(ImageDirectory, fileName));

            var processedByteArray = AlternativePreProcessor.PreprocessImage(byteArray, false);
            //var processedByteArray = PreprocessImage(byteArray);
            if (processedByteArray == null)
                return;
            
            File.WriteAllBytes(Path.Combine(ProcessDirectory, fileName), processedByteArray);
            var output = ReadOcrText(processedByteArray);

            Console.WriteLine("OCR Output : " + number + " --> " + output );
        }

        private static string ReadOcrText(byte[] byteArray)
        {
            var output = "";
            try
            {
                var pix = Pix.LoadFromMemory(byteArray);
                using (var tesseractEngine =
                    new TesseractEngine($@"{AppDomain.CurrentDomain.BaseDirectory}\tessdata", "eng"))
                {
                    tesseractEngine.SetVariable("tessedit_char_whitelist", "0123456789,.[] ");
                    tesseractEngine.SetVariable("classify_bln_numeric_mode", "1");

                    var result = tesseractEngine.Process(pix, PageSegMode.SingleLine);
                    output = result.GetText()?.Trim();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return output;
        }

        private static byte[] PreprocessImage(byte[] byteArray)
        {
            try
            {
                using (var image = Image.Load(byteArray))
                {
                    var percentageWhite = 0;
                    image.Clone()
                        .Mutate(context => context
                            .Grayscale()
                            .GetBackgroundColor(0.8f, out percentageWhite)
                        );
                    
                    var size = new Size(image.Width, image.Height);
                    var largeSize = size.ResizeKeepAspect(image.Width * 3, image.Height * 3, true);
                    
                    image.Mutate(context => context
                        .Resize(largeSize)
                        .ProcessOnBackground(percentageWhite, size)
                    );
                    
                    return image.ToArray(PngFormat.Instance);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
