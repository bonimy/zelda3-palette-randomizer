// <copyright file="PaletteRandomizer.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.LttpPaletteRandomizer
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using Maseya.Helper;
    using Maseya.Snes;
    using Maseya.Zelda3.Palette;
    using static System.Math;

    public class PaletteRandomizer
    {
        public PaletteRandomizer(
            Options options,
            IColorFGenerator generator)
        {
            Options = options
                ?? throw new ArgumentNullException(nameof(options));
            Generator = generator
                ?? throw new ArgumentNullException(nameof(generator));
            Rom = File.ReadAllBytes(options.InputRomPath);
            PaletteOffsetCollections = new PaletteOffsetCollections();
            PaletteEditorsInternal = new Collection<PaletteEditor>();
            PaletteEditors = new ReadOnlyCollection<PaletteEditor>(
                PaletteEditorsInternal);
            RebuildCollections();
        }

        public Options Options
        {
            get;
        }

        public PaletteOffsetCollections PaletteOffsetCollections
        {
            get;
        }

        public ReadOnlyCollection<PaletteEditor> PaletteEditors
        {
            get;
        }

        private Collection<PaletteEditor> PaletteEditorsInternal
        {
            get;
        }

        private IColorFGenerator Generator
        {
            get;
        }

        private byte[] Rom
        {
            get;
        }

        public void RebuildCollections()
        {
            PaletteEditorsInternal.Clear();
            var offsetCollection = PaletteOffsetCollections.GetCollections(Options);
            foreach (var offsets in offsetCollection)
            {
                PaletteEditorsInternal.Add(new PaletteEditor(Rom, offsets));
            }
        }

        public void Randomize()
        {
            Randomize(Options.RandomizerMode);
        }

        public void Randomize(RandomizerMode randomizerMode)
        {
            switch (randomizerMode)
            {
            case RandomizerMode.None:
                break;

            case RandomizerMode.Default:
                MaseyaRandomize();
                break;

            case RandomizerMode.Grayscale:
                Grayscale();
                break;

            case RandomizerMode.Negative:
                Invert();
                break;

            case RandomizerMode.Puke:
                Puke();
                break;

            case RandomizerMode.Blackout:
                Blackout();
                break;

            default:
                throw new InvalidEnumArgumentException(
                    nameof(randomizerMode),
                    (int)randomizerMode,
                    typeof(RandomizerMode));
            }
        }

        public void MaseyaRandomize()
        {
            foreach (var paletteEditor in PaletteEditors)
            {
                paletteEditor.Blend(MaseyaBlend, Generator.Next());
            }
        }

        public void Puke()
        {
            foreach (var paletteEditor in PaletteEditors)
            {
                paletteEditor.BlendByColor(x => Generator.Next());
            }
        }

        public void Blackout()
        {
            foreach (var paletteEditor in PaletteEditors)
            {
                paletteEditor.Blend((x, y) => y, default);
            }
        }

        public void Grayscale()
        {
            var white = (ColorF)Color.White;
            foreach (var paletteEditor in PaletteEditors)
            {
                paletteEditor.Blend(ColorF.LumaGrayscale, white);
            }
        }

        public void Invert()
        {
            foreach (var paletteEditor in PaletteEditors)
            {
                paletteEditor.Blend((x, y) => x.Invert(), default);
            }
        }

        public byte[] GetUpdatedRomData()
        {
            var result = new byte[Rom.Length];
            Array.Copy(Rom, result, Rom.Length);
            foreach (var paletteEditor in PaletteEditors)
            {
                paletteEditor.WriteToRom(result);
            }

            return result;
        }

        public string CreateJsonOfChanges()
        {
            return PaletteJsonFormatter.CreateJson(Rom, GetUpdatedRomData());
        }

        public void WriteToOutput()
        {
            if (Options.OutputJson)
            {
                File.WriteAllText(Options.OutputPath, CreateJsonOfChanges());
            }
            else
            {
                File.WriteAllBytes(Options.OutputPath, GetUpdatedRomData());
            }
        }

        public string CreateJsonOfOffsets()
        {
            return PaletteJsonFormatter.CreateJson(
                PaletteOffsetCollections,
                Options);
        }

        public string CreateJsonOfPaletteData()
        {
            return PaletteJsonFormatter.CreateJson(PaletteEditors);
        }

        private static ColorF MaseyaBlend(ColorF x, ColorF y)
        {
            // Ensure at least a 2.5% change in hue.
            var hue = (y.Red * 0.95f) + 0.025f + x.Hue;

            var chromaShift = y.Green - 0.5f;
            var chroma = x.Chroma;
            if (chromaShift > 0)
            {
                // Put heavy limitations on oversaturating colors.
                chroma *= 1.0f + ((1.0f - chroma) * chromaShift * 0.5f);
            }
            else
            {
                // Put no limitation on desaturating colors. However, make it
                // more likely that only a little desaturation will occur.
                chroma *= (float)Pow(1 - Pow(chromaShift * 2, 2), 0.5);
            }

            var lumaShift = y.Blue - 0.5f;
            var luma = x.Luma;
            if (lumaShift > 0)
            {
                // Do not heavily brighten colors. However, if we removed a
                // lot of saturation, then we can allow for some brighter
                // colors.
                var chromaDiff = Max(x.Chroma - chroma, 0);
                luma *= 1.0f + ((1.0f - x.Luma) * lumaShift * (1.0f + chromaDiff));
            }
            else
            {
                // Do not let colors get too dark.
                luma *= 1.0f + (lumaShift / 2.0f);
            }

            var result = ColorF.FromHcy(
                hue,
                chroma,
                luma);

            return result;
        }
    }
}
