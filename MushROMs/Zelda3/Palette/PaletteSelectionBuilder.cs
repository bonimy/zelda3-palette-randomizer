// <copyright file="PaletteSelectionBuilder.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Maseya.Helper.Collections;
    using Maseya.Snes;

    public class PaletteSelectionBuilder : SelectionBuilder
    {
        public PaletteSelectionBuilder(int offset, int rowSize)
            : base()
        {
            Offset = offset;
            RowSize = rowSize;
        }

        public int Offset
        {
            get;
        }

        public int RowSize
        {
            get;
        }

        public void AddRow(int row, int index, int count)
        {
            AddLinearSelection((row * RowSize) + index, count);
        }

        public void AddBox(int row, int index, Size size)
        {
            AddBoxSelection(
                (row * RowSize) + index,
                size,
                RowSize);
        }

        public void AddFullRows(
            int row,
            int height)
        {
            AddBox(row, 0, RowSize, height);
        }

        public void AddBox(
            int row,
            int index,
            int width,
            int height)
        {
            AddBoxSelection(
                (row * RowSize) + index,
                width,
                height,
                RowSize);
        }

        public void AddColumn(int row, int index, int height)
        {
            AddBoxSelection(
                (row * RowSize) + index,
                new Size(1, height),
                RowSize);
        }

        public EnumerableIndexListSelection CreatePaletteSelection()
        {
            return CreateSelection().ToByteSelection(
                Offset,
                PaletteConverter.Default);
        }

        public List<int> CurrentPaletteIndexes()
        {
            return new List<int>(Current().Select(GetOffset));

            int GetOffset(int index)
            {
                return PaletteConverter.Default.GetOffset(Offset, index);
            }
        }
    }
}
