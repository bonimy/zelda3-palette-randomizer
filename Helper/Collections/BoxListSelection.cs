// <copyright file="BoxListSelection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class BoxListSelection : ListSelection
    {
        public BoxListSelection(int index, Size size, int gridWidth)
            : this(index, size.Width, size.Height, gridWidth)
        {
        }

        public BoxListSelection(
            int index,
            int width,
            int height,
            int gridWidth)
        {
            if (width < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }

            if (height < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            Size = new Size(width, height);
            MinIndex = index;
            Count = width * height;
            GridWidth = gridWidth;
        }

        public Size Size
        {
            get;
        }

        public override int Count
        {
            get;
        }

        public override int MinIndex
        {
            get;
        }

        public override int MaxIndex
        {
            get
            {
                return MinIndex + Count - 1;
            }
        }

        public int GridWidth
        {
            get;
        }

        public override int this[int index]
        {
            get
            {
                if (!ContainsIndex(index))
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return GetIndex(GetPoint(index));
            }
        }

        public override bool ContainsIndex(int index)
        {
            return new Rectangle(Point.Empty, Size).Contains(GetPoint(index));
        }

        public override ListSelection Move(int offset)
        {
            return new BoxListSelection(MinIndex + offset, Size, GridWidth);
        }

        public override IEnumerator<int> GetEnumerator()
        {
            for (var y = 0; y < Size.Height; y++)
            {
                var index = GetIndex(new Point(0, y));
                for (var x = 0; x < Size.Width; x++)
                {
                    yield return index++;
                }
            }
        }

        public int GetIndex(Point point)
        {
            return MinIndex + point.X + (point.Y * GridWidth);
        }

        public int GetIndex(int x, int y)
        {
            return MinIndex + x + (y * GridWidth);
        }

        public Point GetPoint(int index)
        {
            return new Point(
                (index - MinIndex) % GridWidth,
                (index - MinIndex) / GridWidth);
        }

        public (int x, int y) GetCoordinates(int index)
        {
            return (
                (index - MinIndex) % GridWidth,
                (index - MinIndex) / GridWidth);
        }
    }
}
