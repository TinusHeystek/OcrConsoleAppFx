using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        private static List<string> truth = new List<string>(File.ReadAllLines(@"TruthCoords.txt"));
        private static int matchCounter = 0;

        static void Main(string[] args)
        {
            int ImagesCount = Directory.GetFiles(ImageDirectory, "*.png", SearchOption.TopDirectoryOnly).Length;
            Cleanup();


            var sw = Stopwatch.StartNew();
            for (int i = 0; i < ImagesCount; i++)
            {
                ProcessImage(i);
            }
            sw.Stop();

            string accuracy = ((decimal)matchCounter / ImagesCount).ToString("0.00%");

            Console.WriteLine($"\nAverage Time for {ImagesCount} Images: {sw.ElapsedMilliseconds / ImagesCount} ms");
            Console.WriteLine("Accuracy: " + accuracy);
            Console.WriteLine("\nPress any key to exit");
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

            var processedByteArray = AlternativePreProcessor.PreprocessImage(byteArray);
            //var processedByteArray = PreprocessImage(byteArray);
            if (processedByteArray == null)
                return;

            File.WriteAllBytes(Path.Combine(ProcessDirectory, fileName), processedByteArray);
            var output = ReadOcrText(processedByteArray);

            string results;
            try
            {
                //Extract the first 4 numbers
                MatchCollection matchList = Regex.Matches(output, "([0-9]{1,5})");
                var coords = matchList.Cast<Match>().Select(match => match.Value).ToList();
                results = $"{coords[0]} {coords[1]} {coords[2]} {coords[3]}";
            }
            catch
            {
                results = output;
            }

            if (truth[number].Equals(results))
            {
                results += " --> OK";
                matchCounter++;
            }

            Console.WriteLine("OCR Output: " + number + " --> " + results);
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
