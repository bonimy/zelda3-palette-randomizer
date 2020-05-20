// <copyright file="IndexCollection.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    [System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    public abstract class IndexCollection : IReadOnlyList<int>
    {
        public static readonly IndexCollection Empty = new EmptyCollection();

        protected IndexCollection(int minIndex, int maxIndex, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (maxIndex < minIndex)
            {
                throw new ArgumentOutOfRangeException(nameof(maxIndex));
            }

            MinIndex = minIndex;
            MaxIndex = maxIndex;
            Count = count;
        }

        protected IndexCollection(IndexCollection copy)
        {
            if (copy is null)
            {
                throw new ArgumentNullException(nameof(copy));
            }

            MinIndex = copy.MinIndex;
            MaxIndex = copy.MaxIndex;
            Count = copy.Count;
        }

        public int MinIndex
        {
            get;
        }

        public int MaxIndex
        {
            get;
        }

        public int Count
        {
            get;
        }

        public abstract int this[int i]
        {
            get;
        }

        public abstract IndexCollection Move(int offset);

        public virtual bool Contains(int index)
        {
            for (var i = 0; i < Count; i++)
            {
                if (this[i] == index)
                {
                    return true;
                }
            }

            return false;
        }

        public virtual IEnumerator<int> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int[] ToArray()
        {
            var result = new int[Count];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = this[i];
            }

            return result;
        }

        public IndexRangeCollection[] ToRangeArray()
        {
            var result = new List<IndexRangeCollection>();
            using var en = GetEnumerator();
            if (en.MoveNext())
            {
                var startIndex = en.Current;
                var count = 1;
                while (en.MoveNext())
                {
                    if (en.Current != startIndex + count)
                    {
                        AddCurrent();
                        startIndex = en.Current;
                        count = 1;
                    }
                    else
                    {
                        count++;
                    }
                }

                AddCurrent();

                void AddCurrent()
                {
                    result.Add(new IndexRangeCollection(startIndex, count));
                }
            }

            return result.ToArray();
        }

        private class EmptyCollection : IndexCollection
        {
            public EmptyCollection()
                : base(Int32.MinValue, Int32.MaxValue, 0)
            {
            }

            public override int this[int i]
            {
                get
                {
                    throw new ArgumentOutOfRangeException(nameof(i));
                }
            }

            public override bool Contains(int index)
            {
                return false;
            }

            public override IEnumerator<int> GetEnumerator()
            {
                yield break;
            }

            public override IndexCollection Move(int offset)
            {
                return Empty;
            }
        }
    }
}
