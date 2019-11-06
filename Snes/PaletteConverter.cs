// <copyright file="PaletteConverter.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System;
    using System.Collections.Generic;
    using Maseya.Helper;
    using Maseya.Helper.PixelFormat;

    public class PaletteConverter : ByteDataConverter<Color15BppBgr>
    {
        public static readonly PaletteConverter Default =
            new PaletteConverter();

        private PaletteConverter()
            : base()
        {
        }

        public override int SizeOfItem
        {
            get
            {
                return Color15BppBgr.SizeOf;
            }
        }

        public override Color15BppBgr GetItem(
            byte[] sourceArray,
            int startOffset)
        {
            if (sourceArray is null)
            {
                throw new ArgumentNullException(nameof(sourceArray));
            }

            if (startOffset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startOffset));
            }

            if (startOffset + SizeOfItem > sourceArray.Length)
            {
                throw new ArgumentException();
            }

            return new Color15BppBgr(
                sourceArray[startOffset],
                sourceArray[startOffset + 1]);
        }

        public override Color15BppBgr[] GetItems(
            byte[] sourceArray,
            int startOffset,
            int size)
        {
            if (sourceArray is null)
            {
                throw new ArgumentNullException(nameof(sourceArray));
            }

            if (startOffset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startOffset));
            }

            if (startOffset + size > sourceArray.Length)
            {
                throw new ArgumentException();
            }

            var result = new Color15BppBgr[size / SizeOfItem];
            for (var i = 0; i < result.Length; i++)
            {
                var index = startOffset + (i * SizeOfItem);
                result[i] = new Color15BppBgr(
                    sourceArray[index],
                    sourceArray[index + 1]);
            }

            return result;
        }

        public override void GetBytes(
            Color15BppBgr item,
            byte[] destinationArray,
            int startOffset)
        {
            if (destinationArray is null)
            {
                throw new ArgumentNullException(nameof(destinationArray));
            }

            if (startOffset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startOffset));
            }

            if (startOffset + SizeOfItem > destinationArray.Length)
            {
                throw new ArgumentException();
            }

            destinationArray[startOffset] = item.Low;
            destinationArray[startOffset + 1] = item.High;
        }

        public override void GetBytes(
            IReadOnlyList<Color15BppBgr> items,
            byte[] destinationArray,
            int startOffset)
        {
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (startOffset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startOffset));
            }

            var size = items.Count * SizeOfItem;
            if (startOffset + size > destinationArray.Length)
            {
                throw new ArgumentException();
            }

            for (var i = 0; i < items.Count; i++)
            {
                var index = startOffset + (i * SizeOfItem);
                destinationArray[index] = items[i].Low;
                destinationArray[index + 1] = items[i].High;
            }
        }
    }
}
