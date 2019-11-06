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
    using static Maseya.Helper.MathHelper;

    public static class Program
    {
        public static void RandomizePalette(string input, string output)
        {
            var rom = File.ReadAllBytes(input);
            var manager = new PaletteManager(rom);
            var randomColorF = new RandomColorFGenerator();
            var grassColor = randomColorF.NextColorF();
            manager.Blend(
                manager.Selections.DefaultBGColor[0],
                HcyAdditive,
                grassColor);

            var lightWorld = manager.Selections.Overworld.LightWorld;
            foreach (var selection in lightWorld)
            {
                var blendColor = selection == lightWorld.GrassShrubsAndTrees
                    ? grassColor
                    : randomColorF.NextColorF();

                manager.Blend(
                    selection,
                    HcyAdditive,
                    blendColor);
            }

            grassColor = randomColorF.NextColorF();
            manager.Blend(
                manager.Selections.DefaultBGColor[1],
                HcyAdditive,
                grassColor);

            var darkWorld = manager.Selections.Overworld.DarkWorld;
            foreach (var selection in darkWorld)
            {
                var blendColor = selection == darkWorld.GrassShrubsAndTrees
                    ? grassColor
                    : randomColorF.NextColorF();

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
            if (args.Length == 0)
            {
                PrintUsage();
                return 2;
            }

            if (args.Length == 1 && (args[0] == "--help" || args[0] == "-h"))
            {
                PrintUsage();
                Console.WriteLine();
                PrintExitCodes();
                return 1;
            }

            var input = args[0];
            var output = args.Length < 2
                ? AppendToFileName(input, "-rand-pal")
                : args[1];

            try
            {
                RandomizePalette(input, output);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }

            return 0;
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: program input.sfc [output.sfc]");
            Console.WriteLine("If output is not specified, then \"-rand-" +
                "pal\" is appended to input file name to generate " +
                "output file name.");
        }

        private static void PrintExitCodes()
        {
            Console.WriteLine("Exit codes:");
            Console.WriteLine(" 0: Randomization applied successfully.");
            Console.WriteLine(" 1: Help requested with \"--help\" or \"-h\".");
            Console.WriteLine(" 2: No input given.");
            Console.WriteLine("-1: I/O error occurred.");
            Console.WriteLine("-2: Input ROM file was badly formatted.");
        }

        private static string AppendToFileName(string input, string newName)
        {
            var dir = Path.GetDirectoryName(input);
            var name = Path.GetFileNameWithoutExtension(input);
            var ext = Path.GetExtension(input);
            return $"{dir}{Path.DirectorySeparatorChar}{name}{newName}{ext}";
        }
    }
}
