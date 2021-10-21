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
    public class TestRunner
    {
        private Processor _processor = null;
        private static string _imageDirectory = @"Captures";
        private static string _processDirectory = @"Images\Processed";

        private static List<string> _truth;
        private static List<string> _notOKList = new List<string>();
        private static int _matchCounter = 0;
        private static int _imagesCount;
        private static Stopwatch _sw;
        private static readonly Pixel _reference = new Pixel(260,1);
        private static bool _isWindowMode;

        public void Run(bool isWindowMode)
        {
            _isWindowMode = isWindowMode;
            _imageDirectory = _isWindowMode ? @"WindowCaptures" : @"Captures";
            _processDirectory = _isWindowMode ? @"Images\WindowProcessed" :  @"Images\Processed";

            _imagesCount = Directory.GetFiles(_imageDirectory, "*.bmp", SearchOption.TopDirectoryOnly).Length;

            _truth = new List<string>(File.ReadAllLines( _isWindowMode ? @"WindowTruthCoords.txt" :  @"TruthCoords.txt"));

        _processor = new Processor(_isWindowMode);
            Cleanup();
            _sw = Stopwatch.StartNew();
            for (int i = 0; i < _imagesCount; i++)
            {
                ProcessImage(i);
            }
            _sw.Stop();

            WriteResults();
        }

        private void WriteResults()
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
            Program.Start();
        }

        private void Cleanup()
        {
            if (!Directory.Exists(_processDirectory))
                Directory.CreateDirectory(_processDirectory);
            else
            {
                var files = Directory.GetFiles(_processDirectory, "*", SearchOption.TopDirectoryOnly).ToList();
                files.ForEach(File.Delete);
            }
        }

        private void ProcessImage(int number)
        {
            var fileName = _isWindowMode ?  $"WindowCapture_{number}.bmp" : $"Capture_{number}.bmp";
            var byteArray = File.ReadAllBytes(Path.Combine(_imageDirectory, fileName));

            var results = _processor.ProcessImage(byteArray);

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
