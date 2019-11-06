// <copyright file="PaletteSelectionData.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System;
    using Maseya.Helper;
    using Maseya.Helper.Collections;
    using Maseya.Helper.Collections.Generic;
    using Maseya.Helper.PixelFormat;

    public class PaletteSelectionData
    {
        public PaletteSelectionData(
            ListSelectionData<Color15BppBgr> data)
        {
            Data = data ??
                throw new ArgumentNullException(nameof(data));
        }

        public ListSelectionData<Color15BppBgr> Data
        {
            get;
        }

        public IListSelection Selection
        {
            get
            {
                return Data.Selection;
            }
        }

        public void InvertColors()
        {
            foreach (var index in Selection)
            {
                var color = Data[index];
                Data[index] = (Color15BppBgr)(color.Value ^ 0x7FFF);
            }
        }

        public void Transform(ColorTransformCallback transform)
        {
            if (transform is null)
            {
                throw new ArgumentNullException(nameof(transform));
            }

            foreach (var index in Selection)
            {
                var originalColor = (ColorF)Data[index];
                var newColor = transform(originalColor);
                Data[index] = (Color15BppBgr)ColorF.AlphaBlend(
                    newColor,
                    originalColor);
            }
        }

        public void Blend(ColorBlendCallback blend, ColorF blendColor)
        {
            if (blend is null)
            {
                throw new ArgumentNullException(nameof(blend));
            }

            Transform(x => blend(x, blendColor));
        }

        public void RotateHue(ColorF color)
        {
            Transform(x => x.RotateHue(color));
        }

        public void HueBlendRow(ColorF color)
        {
            Blend(ColorF.HueBlend, color);
        }
    }
}
