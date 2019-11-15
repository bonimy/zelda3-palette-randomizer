// <copyright file="Program.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs
{
    using System;
    using System.IO;
    using Maseya.Helper;
    using Maseya.MushROMs.Zelda3.Palette;

    public static class Program
    {
        public static void RandomizePalette(string input, string output)
        {
            var rom = File.ReadAllBytes(input);
            var manager = new PaletteManager(rom);
            var randomColorF = new RandomColorFGenerator();
            foreach (var selection in manager.Selections)
            {
                var blendColor = randomColorF.NextColorF();

                manager.Blend(
                    selection,
                    HcyAdditive,
                    blendColor);
            }

            File.WriteAllBytes(output, manager.GetRomData());

            ColorF HcyAdditive(ColorF x, ColorF y)
            {
                var hue = (y.Red * 0.95f) + 0.025f + x.Hue;
                var chroma = ((y.Green - 0.5f) * 1.0f * x.Chroma) + x.Chroma;
                var luma = ((y.Blue - 0.5f) * 0.5f * x.Luma) + x.Luma;
                var result = ColorF.FromHcy(
                    hue,
                    chroma,
                    luma);

                return result;
            }
        }

        private static int Main(string[] args)
        {
            string input, output;

            // Program opened with no input ROM.
            if (args.Length == 0)
            {
                // Let user know they can drag and drop ROM.
                PrintUsage();
                do
                {
                    Console.WriteLine("Input ROM path:");
                    input = Console.ReadLine();

                    // Exit if empty string given.
                    if (String.IsNullOrEmpty(input))
                    {
                        Console.WriteLine("Exiting...");
                        return 2;
                    }
                }
                while (!DiagnosticFileCheck(input));

                Console.WriteLine(
                    "Output ROM path (leave empty for same as input).");

                output = Console.ReadLine();
                if (String.IsNullOrEmpty(output))
                {
                    output = input;
                }
            }
            else if (args.Length == 1 && IsHelpCommand(args[0]))
            {
                PrintUsage();
                PrintExitCodes();
                return 1;
            }
            else
            {
                input = args[0];
                if (!DiagnosticFileCheck(input))
                {
                    return -1;
                }

                output = args.Length < 2
                    ? AppendToFileName(input, "-rand-pal")
                    : args[1];
            }

            try
            {
                RandomizePalette(input, output);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }

            if (args.Length == 0)
            {
                Console.WriteLine("Task completed successfully!");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }

            return 0;
        }

        private static bool DiagnosticFileCheck(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }

            Console.WriteLine($"Could not find file {path}");
            Console.WriteLine();
            return false;
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: program input.sfc [output.sfc]");
            Console.WriteLine("If output is not specified, then");
            Console.WriteLine("\"-rand-pal\" is appended to input file name");
            Console.WriteLine("to generate output file name.");
            Console.WriteLine();
        }

        private static void PrintExitCodes()
        {
            Console.WriteLine("Exit codes:");
            Console.WriteLine(" 0: Randomization applied successfully.");
            Console.WriteLine(" 1: Help requested with \"--help\" or \"-h\".");
            Console.WriteLine(" 2: No input given.");
            Console.WriteLine("-1: I/O error occurred.");
            Console.WriteLine("-2: Input ROM file was badly formatted.");
            Console.WriteLine();
        }

        private static string AppendToFileName(string input, string newName)
        {
            var dir = Path.GetDirectoryName(input);
            var name = Path.GetFileNameWithoutExtension(input);
            var ext = Path.GetExtension(input);
            return $"{dir}{Path.DirectorySeparatorChar}{name}{newName}{ext}";
        }

        private static bool IsHelpCommand(string arg)
        {
            return arg == "--help" || arg == "-h";
        }
    }
}
