// <copyright file="IndexCollectionBuilder.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    public class IndexCollectionBuilder
    {
        public IndexCollectionBuilder()
        {
            IndexCollection = Enumerable.Empty<int>();
        }

        public IEnumerable<int> IndexCollection
        {
            get;
            private set;
        }

        public void Add(IEnumerable<int> indexes)
        {
            IndexCollection = IndexCollection.Concat(indexes);
        }

        public void AddIndexRange(int index, int count)
        {
            Add(new IndexRangeCollection(index, count));
        }

        public void AddIndexBox(int startIndex, Size boxSize, int rowWidth)
        {
            Add(new BoxIndexCollection(startIndex, boxSize, rowWidth));
        }

        public void AddIndexBox(
            int startIndex,
            int boxWidth,
            int boxHeight,
            int rowWidth)
        {
            AddIndexBox(startIndex, new Size(boxWidth, boxHeight), rowWidth);
        }

        public void AddIndex(int index)
        {
            Add(new int[1] { index });
        }

        public void Clear()
        {
            IndexCollection = Enumerable.Empty<int>();
        }

        public void AssertUnique()
        {
            var hash = new HashSet<int>();
            foreach (var index in IndexCollection)
            {
                if (hash.Contains(index))
                {
                    throw new InvalidOperationException();
                }

                hash.Add(index);
            }
        }
    }
}
