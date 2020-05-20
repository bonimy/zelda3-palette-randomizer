// <copyright file="IndexRangeCollection.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections
{
    using System;
    using System.Collections.Generic;

    public class IndexRangeCollection : IndexCollection
    {
        public IndexRangeCollection(int startIndex, int count)
            : base(startIndex, startIndex + count - 1, count)
        {
        }

        public override int this[int i]
        {
            get
            {
                if (i < 0 || i >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(i));
                }

                return MinIndex + i;
            }
        }

        public override IndexCollection Move(int offset)
        {
            return new IndexRangeCollection(MinIndex + offset, Count);
        }

        public override bool Contains(int index)
        {
            return (index >= MinIndex) && (index <= MaxIndex);
        }

        public override IEnumerator<int> GetEnumerator()
        {
            for (var i = MinIndex; i <= MaxIndex; i++)
            {
                yield return i;
            }
        }
    }
}
