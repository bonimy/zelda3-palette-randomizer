// <copyright file="PaletteRandomizer.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.LttpPaletteRandomizer
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using Maseya.Helper;
    using Maseya.Snes;
    using Maseya.Zelda3.Palette;

    public class PaletteRandomizer
    {
        public PaletteRandomizer(Options options)
        {
            Options = options
                ?? throw new ArgumentNullException(nameof(options));
            Rom = File.ReadAllBytes(options.InputRomPath);
            PaletteOffsetCollections = new PaletteOffsetCollections();
            Random = options.Seed == -1
                ? new RandomColorFGenerator()
                : new RandomColorFGenerator(options.Seed);
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

        private RandomColorFGenerator Random
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
                PaletteEditorsInternal.Add(new PaletteEditor(offsets, Rom));
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
            }
        }

        public void MaseyaRandomize()
        {
            foreach (var paletteEditor in PaletteEditors)
            {
                paletteEditor.Blend(MaseyaBlend, Random.NextColorF());
            }
        }

        public void Puke()
        {
            foreach (var paletteEditor in PaletteEditors)
            {
                paletteEditor.BlendByColor(x => Random.NextColorF());
            }
        }

        public void Blackout()
        {
            foreach (var paletteEditor in PaletteEditors)
            {
                paletteEditor.BlendByColor(x => default);
            }
        }

        public void Grayscale()
        {
            foreach (var paletteEditor in PaletteEditors)
            {
                paletteEditor.BlendByColor(x => x.LumaGrayscale());
            }
        }

        public void Invert()
        {
            foreach (var paletteEditor in PaletteEditors)
            {
                paletteEditor.BlendByColor(x => x.Invert());
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

        private static ColorF MaseyaBlend(ColorF x, ColorF y)
        {
            // Ensure at least a 5% change in hue.
            var hue = (y.Red * 0.95f) + 0.025f + x.Hue;

            var chromaShift = y.Green - 0.5f;
            var chroma = x.Chroma;
            if (chromaShift > 0)
            {
                // Put heavy limitations on oversaturating colors.
                chroma *= 1.0f + ((1.0f - x.Chroma) * chromaShift * 0.5f);
            }
            else
            {
                // Put no limitation on desaturating colors.
                chroma *= 0.5f + chromaShift;
            }

            var lumaShift = y.Blue - 0.5f;
            var luma = x.Luma;
            if (lumaShift > 0)
            {
                // Do not heavily brighten colors. However, if we removed a
                // lot of saturation, then we can allow for some brighter
                // colors.
                var chromaDiff = Math.Max(chroma - x.Chroma, 0);
                luma *= 1.0f + ((1.0f - x.Luma) * lumaShift * (1.0f + chromaDiff));
            }
            else
            {
                // Do not let colors get too dark.
                luma *= 1.0f + ((0.5f + lumaShift) / 1.0f);
            }

            var result = ColorF.FromHcy(
                hue,
                chroma,
                luma);

            return result;
        }
    }
}
