// <copyright file="PaletteIndexCollectionBuilder.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Zelda3.Palette
{
    using System.Collections.Generic;
    using System.Drawing;
    using Maseya.Helper;
    using Maseya.Helper.Collections;
    using Maseya.Snes;

    public class PaletteIndexCollectionBuilder
    {
        private static readonly ByteDataConverter<SnesColor> PaletteConverter
            = ByteDataConverter<SnesColor>.Default;

        public PaletteIndexCollectionBuilder(int offset, int rowSize)
                    : base()
        {
            Offset = offset;
            RowSize = rowSize;
            BaseBuilder = new IndexCollectionBuilder();
        }

        public int Offset
        {
            get;
        }

        public int RowSize
        {
            get;
        }

        private IndexCollectionBuilder BaseBuilder
        {
            get;
        }

        public void AddRow(int row, int index, int count)
        {
            BaseBuilder.AddIndexRange((row * RowSize) + index, count);
        }

        public void AddBox(int row, int index, Size size)
        {
            BaseBuilder.AddIndexBox(
                (row * RowSize) + index,
                size,
                RowSize);
        }

        public void AddFullRows(int row, int height)
        {
            AddBox(row, 0, RowSize, height);
        }

        public void AddBox(int row, int index, int width, int height)
        {
            BaseBuilder.AddIndexBox(
                (row * RowSize) + index,
                width,
                height,
                RowSize);
        }

        public void AddColumn(int row, int index, int height)
        {
            AddBox(row, index, 1, height);
        }

        public void Clear()
        {
            BaseBuilder.Clear();
        }

        public IEnumerable<int> ConvertBaseCollection()
        {
            return PaletteConverter.GetStartOffsetAtEachIndex(
                Offset,
                BaseBuilder.IndexCollection);
        }

        public ListIndexCollection Peek()
        {
            return new ListIndexCollection(ConvertBaseCollection());
        }

        public ListIndexCollection Flush()
        {
            var result = Peek();
            BaseBuilder.Clear();
            return result;
        }
    }
}
