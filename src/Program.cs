using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using OcrConsoleAppFx.OcrReaders;
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

        private static List<string> _truth = new List<string>(File.ReadAllLines(@"TruthCoords.txt"));
        private static List<string> _notOKList = new List<string>();
        private static int _matchCounter = 0;
        private static int _imagesCount = Directory.GetFiles(ImageDirectory, "*.png", SearchOption.TopDirectoryOnly).Length;
        private static Stopwatch _sw;


        private static IronOcrProcessor _ironOcr = new IronOcrProcessor();
        private static TesseractProcessor _tesseractOcr = new TesseractProcessor();

        static void Main(string[] args)
        {
            Cleanup();


            _sw = Stopwatch.StartNew();
            for (int i = 0; i < _imagesCount; i++)
            {
                ProcessImage(i);
            }
            _sw.Stop();

            WriteResults();
        }

        private static void WriteResults()
        {
            string accuracy = ((decimal)_matchCounter / _imagesCount).ToString("0.00%");

            Console.WriteLine($"\nAverage Time for {_imagesCount} Images: {_sw.ElapsedMilliseconds / _imagesCount} ms");
            Console.WriteLine("Accuracy: " + accuracy);
            Console.WriteLine();
            foreach (string notOK in _notOKList)
            {
                Console.WriteLine(notOK);
            }
            Console.WriteLine();
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

            var wasChangeByAlternativePreProcessor = false;
            var processedByteArray = AlternativePreProcessor.PreprocessImage(byteArray, true, out wasChangeByAlternativePreProcessor);

            if (!wasChangeByAlternativePreProcessor)
            {
                processedByteArray = PreprocessImage(byteArray);
            }

            if (processedByteArray == null)
                return;

            File.WriteAllBytes(Path.Combine(ProcessDirectory, fileName), processedByteArray);
            
            var output = _tesseractOcr.ReadText(processedByteArray);

            // Uncomment to use the IronOCR processor
            // var output = _ironOcr.ReadText(processedByteArray);

            string results;
            try
            {
                //Extract the first 4 numbers
                MatchCollection matchList = Regex.Matches(output, "([0-9]{1,5})");
                var coords = matchList.Cast<Match>().Select(match => match.Value).ToList();
                results = $"{coords[0]} {coords[1].Substring(0,3)} {coords[2]} {coords[3].Substring(0, 3)}";
            }
            catch
            {
                results = output;
            }

            if (_truth[number].Equals(results))
            {
                results += " --> OK";
                _matchCounter++;
                // For debugging purposes
                //Console.WriteLine("OK");
            }
            else {
                // For debugging purposes
                //Console.WriteLine("X");
                _notOKList.Add($"{number}: Expected: {_truth[number]}, actual: {results}");
            }

            Console.WriteLine("OCR Output: " + number + " --> " + results);
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
