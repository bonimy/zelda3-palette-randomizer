// <copyright file="PaletteEditor.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System;
    using System.Collections.Generic;
    using Maseya.Helper;
    using Maseya.Helper.Collections;

    public class PaletteEditor
    {
        public PaletteEditor(IndexCollection offsets, IReadOnlyList<byte> rom)
        {
            if (offsets is null)
            {
                throw new ArgumentNullException(nameof(offsets));
            }

            if (rom is null)
            {
                throw new ArgumentNullException(nameof(rom));
            }

            Items = new Dictionary<int, SnesColor>(offsets.Count);
            foreach (var offset in offsets)
            {
                if (offset > 0)
                {
                    var low = rom[offset + 0];
                    var high = rom[offset + 1];
                    Items.Add(offset, new SnesColor(low, high));
                }
                else
                {
                    var r = rom[0 - offset] & 0x1F;
                    var g = rom[1 - offset] & 0x1F;
                    var b = rom[4 - offset] & 0x1F;
                    Items.Add(offset, new SnesColor(r, g, b));
                }
            }
        }

        public Dictionary<int, SnesColor> Items
        {
            get;
        }

        public void Blend(ColorBlendCallback blend, ColorF blendColor)
        {
            if (blend is null)
            {
                throw new ArgumentNullException(nameof(blend));
            }

            foreach (var offset in new List<int>(Items.Keys))
            {
                Items[offset] = (SnesColor)blend(Items[offset], blendColor);
            }
        }

        public void BlendByColor(ColorTransformCallback getColor)
        {
            if (getColor is null)
            {
                throw new ArgumentNullException(nameof(getColor));
            }

            var colorGroupings = new Dictionary<SnesColor, List<int>>();
            foreach (var kvp in Items)
            {
                var offset = kvp.Key;
                var color = kvp.Value;
                if (colorGroupings.TryGetValue(color, out var list))
                {
                    list.Add(offset);
                }
                else
                {
                    colorGroupings[color] = new List<int>() { offset };
                }
            }

            foreach (var kvp in colorGroupings)
            {
                var color = (SnesColor)getColor(kvp.Key);
                foreach (var offset in kvp.Value)
                {
                    Items[offset] = color;
                }
            }
        }

        public void WriteToRom(IList<byte> rom)
        {
            if (rom is null)
            {
                throw new ArgumentNullException(nameof(rom));
            }

            foreach (var kvp in Items)
            {
                var offset = kvp.Key;
                var color = kvp.Value;
                if (offset >= 0)
                {
                    rom[offset + 0] = color.Low;
                    rom[offset + 1] = color.High;
                }
                else
                {
                    rom[0 - offset] = (byte)(0x20 | color.Red);
                    rom[1 - offset] =
                    rom[3 - offset] = (byte)(0x40 | color.Green);
                    rom[4 - offset] = (byte)(0x80 | color.Blue);
                }
            }
        }

        public IndexCollection CreateIndexCollection()
        {
            return new ListIndexCollection(Items.Keys);
        }
    }
}
