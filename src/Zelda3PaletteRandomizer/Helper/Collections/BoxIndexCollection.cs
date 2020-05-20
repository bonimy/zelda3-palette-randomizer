// <copyright file="BoxIndexCollection.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections
{
    using System;
    using System.Drawing;

    public class BoxIndexCollection : IndexCollection
    {
        public BoxIndexCollection(int startIndex, Size boxSize, int rowWidth)
            : this(startIndex, boxSize.Width, boxSize.Height, rowWidth)
        {
        }

        public BoxIndexCollection(
            int startIndex,
            int boxWidth,
            int boxHeight,
            int rowWidth)
            : this(new Initializer(startIndex, boxWidth, boxHeight, rowWidth))
        {
        }

        private BoxIndexCollection(Initializer initializer)
            : base(initializer.Min, initializer.Max, initializer.Count)
        {
            BoxSize = initializer.Size;
            RowWidth = initializer.GridWidth;
        }

        public Size BoxSize
        {
            get;
        }

        public int BoxWidth
        {
            get
            {
                return BoxSize.Width;
            }
        }

        public int BoxHeight
        {
            get
            {
                return BoxSize.Height;
            }
        }

        public int RowWidth
        {
            get;
        }

        public override int this[int i]
        {
            get
            {
                if (i < 0 || i >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(i));
                }

                var x = i % BoxWidth;
                var y = i / BoxWidth;

                return MinIndex + x + (y * RowWidth);
            }
        }

        public override bool Contains(int index)
        {
            var i = index - MinIndex;
            var point = new Point(i % RowWidth, i / RowWidth);
            var bounds = new Rectangle(Point.Empty, BoxSize);
            return bounds.Contains(point);
        }

        public override IndexCollection Move(int offset)
        {
            return new BoxIndexCollection(
                MinIndex + offset,
                BoxSize,
                RowWidth);
        }

        private struct Initializer
        {
            public Initializer(
                int startIndex,
                int boxWidth,
                int boxHeight,
                int rowWidth)
            {
                if (rowWidth <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(rowWidth));
                }

                if (boxWidth < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(boxWidth));
                }

                if (boxHeight < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(boxHeight));
                }

                Min = startIndex;
                Max = Min + ((boxHeight - 1) * rowWidth) + boxWidth - 1;
                Count = boxHeight * boxWidth;

                Size = new Size(boxWidth, boxHeight);
                GridWidth = rowWidth;
            }

            public int Min
            {
                get;
            }

            public int Max
            {
                get;
            }

            public int Count
            {
                get;
            }

            public Size Size
            {
                get;
            }

            public int GridWidth
            {
                get;
            }
        }
    }
}
