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
            TestFullscreen,
            TestWindowed,
            FarmingFullscreen,
            FarmingWindowed
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
                    new DemoRunner().Run(false, false);
                    break;
                case RunMode.FarmingFullscreen:
                    new DemoRunner().Run(true, false);
                    break;
                case RunMode.FarmingWindowed:
                    new DemoRunner().Run(true, true);
                    break;
                case RunMode.TestFullscreen:
                    new TestRunner().Run(false);
                    break;
                case RunMode.TestWindowed:
                    new TestRunner().Run(true);
                    break;
                default:
                    break;
            }
        }

        private static RunMode ModePrompt()
        {
            Console.Clear();
            Console.WriteLine("Press 0 to run Live Demo Mode \nPress 1 to run Fullscreen Farming mode \nPress 2 to run Windowed Farming mode \nPress 3 to run Fullscreen Test Mode \nPress 4 to run Windowed Test Mode ");
            char key = Console.ReadKey().KeyChar;

            switch (key)
            {
                case '0':
                    return RunMode.Demo;
                case '1':
                    return RunMode.FarmingFullscreen;
                case '2':
                    return RunMode.FarmingWindowed;
                case '3':
                    return RunMode.TestFullscreen;
                case '4':
                    return RunMode.TestWindowed;
                default:
                    ModePrompt();
                        break;
            }
            return RunMode.TestFullscreen;     
        }


    }
}
