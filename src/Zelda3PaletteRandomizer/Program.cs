// <copyright file="Program.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.LttpPaletteRandomizer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using CommandLine;
    using Maseya.Properties;

    public static class Program
    {
        private static readonly Regex YesRegex = new Regex(
            @"y(es)?|t(rue)?",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex NoRegex = new Regex(
            @"n(o)?|f(alse)?",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static void RandomizePalette(Options options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (String.IsNullOrEmpty(options.OutputPath))
            {
                options.OutputPath = options.OutputJson
                    ? Path.ChangeExtension(
                        options.InputRomPath,
                        ".json")
                    : AppendToFileName(
                        options.InputRomPath,
                        Resources.DefaultOutputNameSuffix);
            }

            var randomizer = new PaletteRandomizer(options);
            randomizer.Randomize();
            randomizer.WriteToOutput();
        }

        public static int Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var configPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "args.config");
            var options = FindDefaultOptions(configPath);
            if (args.Length == 0)
            {
                CreateOptionsFromInput(options);
            }
            else
            {
                CreateOptionsFromArgs(options, args);
            }

            try
            {
                RandomizePalette(options);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }

            // Print message and wait for termination if application was
            // started with no args. Meant for users who started the app from
            // desktop and may not be used to command line interfaces.
            if (args.Length == 0)
            {
                Console.WriteLine(Resources.StatusSuccessfulTermination);
                Console.ReadKey();
            }

            return 0;
        }

        public static Options CreateOptionsFromInput(Options options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Console.WriteLine(Resources.PromptForInputPath);
            options.InputRomPath = Console.ReadLine();
            if (String.IsNullOrEmpty(options.InputRomPath))
            {
                // Exit if empty string given.
                Console.WriteLine(Resources.StatusNoInputGiven);
                Environment.Exit(1);
            }

            Console.WriteLine(Resources.PromptForOutputPath);
            options.OutputPath = Console.ReadLine();
            if (String.IsNullOrEmpty(options.OutputPath))
            {
                options.OutputPath = options.InputRomPath;
            }

            options.OutputJson = String.Equals(
                Path.GetExtension(options.OutputPath),
                ".json",
                StringComparison.CurrentCultureIgnoreCase);
            options.RandomizeOverworld = ParseInputBool(
                Resources.PromptRandomizeOverworld,
                true);
            options.RandomizeDungeon = ParseInputBool(
                Resources.PromptRandomizeDungeon,
                true);
            options.RandomizeLinkSpritePalette = ParseInputBool(
                Resources.PromptRandomizeLinkSprite,
                false);
            options.RandomizeSwordPalette = ParseInputBool(
                Resources.PromptRandomizeSword,
                false);
            options.RandomizeShieldPalette = ParseInputBool(
                Resources.PromptRandomizeShield,
                false);
            options.RandomizeHudPalettes = ParseInputBool(
                Resources.PromptRandomizeHud,
                false);
            options.Seed = ParseInputInt32(Resources.PromptSeed, -1);
            options.RandomizerMode = ParseInputMode(
                Resources.PromptRandomizerMode,
                RandomizerMode.Default);

            return options;
        }

        public static Options FindDefaultOptions(string path)
        {
            var args = new List<string>();
            if (File.Exists(path))
            {
                args.AddRange(File.ReadAllLines(path));
            }
            else
            {
                var array = Resources.FallbackConfig.Split(
                    new string[] { "\n", "\r\n" },
                    StringSplitOptions.RemoveEmptyEntries);
                File.WriteAllLines(path, array);
                args.AddRange(array);
            }

            args.Add(String.Empty);
            return CreateOptionsFromArgs(new Options(), args);
        }

        private static Options CreateOptionsFromArgs(
            Options options,
            IEnumerable<string> args)
        {
            using var parser = new Parser(conf =>
            {
                conf.CaseInsensitiveEnumValues = true;
                conf.CaseSensitive = false;
                conf.IgnoreUnknownArguments = true;
                conf.ParsingCulture = CultureInfo.CurrentUICulture;
            });
            parser.ParseArguments(() => options, args)
                .WithParsed(op => options = op)
                .WithNotParsed(errors =>
                {
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error);
                    }

                    Environment.Exit(1);
                });

            return options;
        }

        private static int ParseInputInt32(string prompt, int fallback)
        {
            _loop:
            Console.WriteLine(prompt);
            var input = Console.ReadLine();
            if (String.IsNullOrEmpty(input))
            {
                return fallback;
            }

            if (Int32.TryParse(input, out var result))
            {
                return result;
            }

            if (input.StartsWith("0x", StringComparison.CurrentCulture) &&
                Int32.TryParse(
                    input.Substring(2),
                    NumberStyles.HexNumber,
                    CultureInfo.CurrentCulture,
                    out result))
            {
                return result;
            }

            Console.WriteLine(Resources.StatusInvalidInput);
            goto _loop;
        }

        private static bool ParseInputBool(string prompt, bool fallback)
        {
            _loop:
            Console.WriteLine(prompt);
            var input = Console.ReadLine();
            if (String.IsNullOrEmpty(input))
            {
                return fallback;
            }

            if (YesRegex.IsMatch(input))
            {
                return true;
            }

            if (NoRegex.IsMatch(input))
            {
                return false;
            }

            Console.WriteLine(Resources.StatusInvalidInput);
            goto _loop;
        }

        private static RandomizerMode ParseInputMode(
            string prompt,
            RandomizerMode fallback)
        {
            _loop:
            Console.WriteLine(prompt);
            var input = Console.ReadLine();
            if (String.IsNullOrEmpty(input))
            {
                return fallback;
            }

            if (Enum.TryParse(input, true, out RandomizerMode result))
            {
                return result;
            }

            Console.WriteLine(Resources.StatusInvalidInput);
            goto _loop;
        }

        private static string AppendToFileName(
            string baseFileName,
            string textToAppend)
        {
            var newFileName = new StringBuilder();
            newFileName.Append(Path.GetFileNameWithoutExtension(baseFileName));
            newFileName.Append(textToAppend);
            newFileName.Append(Path.GetExtension(baseFileName));
            return Path.Combine(
                Path.GetDirectoryName(baseFileName),
                newFileName.ToString());
        }
    }
}
