// <copyright file="PaletteManager.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using System;
    using Maseya.Helper;
    using Maseya.Helper.Collections;
    using Maseya.Helper.Collections.Generic;
    using Maseya.Helper.PixelFormat;
    using Maseya.Snes;

    public class PaletteManager
    {
        public PaletteManager(byte[] rom)
        {
            if (rom is null)
            {
                throw new ArgumentNullException(nameof(rom));
            }

            if (rom.Length < 0x10_0000)
            {
                throw new InvalidOperationException(
                    "Expected a headerless ROM at least 1MB in size.");
            }

            Rom = new byte[rom.Length];
            Array.Copy(rom, Rom, rom.Length);

            Selections = new GamePaletteSelections();
        }

        public GamePaletteSelections Selections
        {
            get;
        }

        private byte[] Rom
        {
            get;
        }

        public void Blend(
            IListSelection selection,
            ColorBlendCallback blend,
            ColorF blendColor)
        {
            var palette = GetPalette(selection);
            palette.Blend(blend, blendColor);
            WritePalette(palette.Data);
        }

        public PaletteSelectionData GetPalette(
            IListSelection selection)
        {
            var data = new ListSelectionData<Color15BppBgr>(
                selection,
                Rom,
                PaletteConverter.Default);

            return new PaletteSelectionData(data);
        }

        public void WritePalette(ListSelectionData<Color15BppBgr> palette)
        {
            if (palette is null)
            {
                throw new ArgumentNullException(nameof(palette));
            }

            palette.WriteToBytes(Rom);
        }

        public byte[] GetRomData()
        {
            var result = new byte[Rom.Length];
            Array.Copy(Rom, result, Rom.Length);
            return result;
        }
    }
}
