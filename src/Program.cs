using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace OcrConsoleAppFx
{
    internal class Program
    {
        private enum RunMode
        {
            Demo,
            Test,
            Farming
        }

        static void Main(string[] args)
        {
            Start();
        }

        public static void Start()
        {
            RunMode mode = ModePrompt();
            Console.WriteLine();

            switch (mode)
            {
                case RunMode.Demo:
                    new DemoRunner().Run(false);
                    break;
                case RunMode.Farming:
                    new DemoRunner().Run(true);
                    break;
                case RunMode.Test:
                    new TestRunner().Run();
                    break;
                default:
                    break;
            }
        }

        private static RunMode ModePrompt()
        {
            Console.Clear();
            Console.WriteLine("Press 0 to run Live Demo Mode \nPress 1 to run Test Mode \nPress 2 to run Farming mode");
            char key = Console.ReadKey().KeyChar;

            switch (key)
            {
                case '0':
                    return RunMode.Demo;
                case '1':
                    return RunMode.Test;
                case '2':
                    return RunMode.Farming;
                default:
                    ModePrompt();
                        break;
            }
            return RunMode.Test;     
        }


    }
}
