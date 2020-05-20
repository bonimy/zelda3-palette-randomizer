// <copyright file="Options.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya
{
    using CommandLine;

    public class Options
    {
        public Options()
        {
            OutputJson = false;
            RandomizeDungeon = true;
            RandomizeOverworld = true;
            RandomizerMode = RandomizerMode.Default;
            Seed = -1;
        }

        [Value(0, Required = true, HelpText = "Input ROM path")]
        public string InputRomPath
        {
            get;
            set;
        }

        [Value(1, Required = false, HelpText = "Path to result output")]
        public string OutputPath
        {
            get;
            set;
        }

        [Option('j', "use-json", HelpText = "Output results as JSON file.")]
        public bool OutputJson
        {
            get;
            set;
        }

        [Option('J', "no-use-json", HelpText = "Do not output results as JSON file.")]
        public bool NoOutputJson
        {
            get
            {
                return !OutputJson;
            }

            set
            {
                OutputJson = !value;
            }
        }

        [Option('w', "overworld", HelpText = "Randomize overworld palettes")]
        public bool RandomizeOverworld
        {
            get;
            set;
        }

        [Option('W', "no-overworld", HelpText = "Do not randomize overworld palettes")]
        public bool NoRandomizeOverworld
        {
            get
            {
                return !RandomizeOverworld;
            }

            set
            {
                RandomizeOverworld = !value;
            }
        }

        [Option('d', "dungeon", HelpText = "Randomize dungeon palettes")]
        public bool RandomizeDungeon
        {
            get;
            set;
        }

        [Option('D', "no-dungeon", HelpText = "Do not randomize dungeon palettes")]
        public bool NoRandomizeDungeon
        {
            get
            {
                return !RandomizeDungeon;
            }

            set
            {
                RandomizeDungeon = !value;
            }
        }

        [Option('l', "link-sprite", HelpText = "Randomize link sprite palette")]
        public bool RandomizeLinkSpritePalette
        {
            get;
            set;
        }

        [Option(
            'L',
            "no-link-sprite",
            HelpText = "Do not randomize link sprite palette")]
        public bool NoRandomizeLinkSpritePalette
        {
            get
            {
                return !RandomizeLinkSpritePalette;
            }

            set
            {
                RandomizeLinkSpritePalette = !value;
            }
        }

        [Option("sword", HelpText = "Randomize sword palettes")]
        public bool RandomizeSwordPalette
        {
            get;
            set;
        }

        [Option("no-sword", HelpText = "Do not randomize sword palettes")]
        public bool NoRandomizeSwordPalette
        {
            get
            {
                return !RandomizeSwordPalette;
            }

            set
            {
                RandomizeSwordPalette = !value;
            }
        }

        [Option("shield", HelpText = "Randomize shield palettes")]
        public bool RandomizeShieldPalette
        {
            get;
            set;
        }

        [Option("no-shield", HelpText = "Do not randomize shield palettes")]
        public bool NoRandomizeShieldPalette
        {
            get
            {
                return !RandomizeShieldPalette;
            }

            set
            {
                RandomizeShieldPalette = !value;
            }
        }

        [Option(
            "mode",
            HelpText = "randomizer mode (none, default, puke, negative, grayscale")]
        public RandomizerMode RandomizerMode
        {
            get;
            set;
        }

        [Option("sprite", HelpText = "Randomize sprite palettes")]
        public bool RandomizeSpritePalettes
        {
            get;
            set;
        }

        [Option("no-sprite", HelpText = "Do not randomize sprite palettes")]
        public bool NoRandomizeSpritePalettes
        {
            get
            {
                return !RandomizeSpritePalettes;
            }

            set
            {
                RandomizeSpritePalettes = !value;
            }
        }

        [Option("sprite2", HelpText = "Randomize advanced sprite palettes")]
        public bool RandomizeAdvancedSpritePalettes
        {
            get;
            set;
        }

        [Option("no-sprite2", HelpText = "Do not randomize advanced sprite palettes")]
        public bool NoRandomizeAdvancedSpritePalettes
        {
            get
            {
                return !RandomizeAdvancedSpritePalettes;
            }

            set
            {
                RandomizeAdvancedSpritePalettes = !value;
            }
        }

        [Option("hud", HelpText = "Randomize HUD palettes")]
        public bool RandomizeHudPalettes
        {
            get;
            set;
        }

        [Option("no-hud", HelpText = "Do not randomize HUD palettes")]
        public bool NoRandomizeHudPalettes
        {
            get
            {
                return !RandomizeHudPalettes;
            }

            set
            {
                RandomizeHudPalettes = !value;
            }
        }

        [Option("seed", HelpText = "Use specific seed for random generator")]
        public int Seed
        {
            get;
            set;
        }
    }
}
