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
            Test
        }

        static void Main(string[] args)
        {
            RunMode mode = ModePrompt();
            Console.WriteLine();

            switch (mode)
            {
                case RunMode.Demo:
                    new DemoRunner().Run();
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
            Console.WriteLine("Press 0 to run LiveDemo, Press 1 to run tests\n");
            char key = Console.ReadKey().KeyChar;

            switch (key)
            {
                case '0':
                    return RunMode.Demo;
                case '1':
                    return RunMode.Test;
                default:
                    ModePrompt();
                        break;
            }
            return RunMode.Test;     
        }


    }
}
