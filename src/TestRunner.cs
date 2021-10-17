using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OcrConsoleAppFx
{
    public static class TestRunner
    {
        private const string ImageDirectory = @"Captures";
        private const string ProcessDirectory = @"Images\Processed";

        private static List<string> _truth = new List<string>(File.ReadAllLines(@"TruthCoords.txt"));
        private static List<string> _notOKList = new List<string>();
        private static int _matchCounter = 0;
        private static int _imagesCount = Directory.GetFiles(ImageDirectory, "*.bmp", SearchOption.TopDirectoryOnly).Length;
        private static Stopwatch _sw;
        private static readonly Pixel _reference = new Pixel(260,1);

        public static void Run()
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

            Console.WriteLine($"\n Processed {_imagesCount} files");
            Console.WriteLine($"\n Average time per file: {_sw.ElapsedMilliseconds / _imagesCount } ms");
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
            var fileName = $"Capture_{number}.bmp";
            var byteArray = File.ReadAllBytes(Path.Combine(ImageDirectory, fileName));

            var results = Processor.ProcessImage(byteArray, _reference);

            if (_truth[number].Equals(results))
            {
                results += " --> OK";
                _matchCounter++;
            }
            else
            {
                // For debugging purposes
                //Console.WriteLine("X");
                _notOKList.Add($"{number}: Expected: {_truth[number]}, actual: {results}");
            }

            Console.WriteLine("OCR Output: " + number + " --> " + results);
        }


    }
}
