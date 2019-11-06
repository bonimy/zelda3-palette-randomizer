// <copyright file="ListSelection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ListSelection :
        IListSelection
    {
        public abstract int MinIndex
        {
            get;
        }

        public abstract int MaxIndex
        {
            get;
        }

        public abstract int Count
        {
            get;
        }

        public abstract int this[int index]
        {
            get;
        }

        public abstract ListSelection Move(int offset);

        public ListSelection Copy()
        {
            return Move(0);
        }

        public abstract bool ContainsIndex(int index);

        public IEnumerable<T> EnumerateValues<T>(IReadOnlyList<T> list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (MinIndex < 0 || MaxIndex >= list.Count)
            {
                throw new InvalidOperationException();
            }

            foreach (var index in this)
            {
                yield return list[index];
            }
        }

        public IEnumerable<(int index, T value)> EnumerateIndexValues<T>(
            IReadOnlyList<T> list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (MinIndex < 0 || MaxIndex >= list.Count)
            {
                throw new InvalidOperationException();
            }

            foreach (var index in this)
            {
                yield return (index, list[index]);
            }
        }

        public Dictionary<int, T> GetValueDictionary<T>(IReadOnlyList<T> list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var result = new Dictionary<int, T>(list.Count);
            foreach (var (index, value) in EnumerateIndexValues(list))
            {
                result.Add(index, value);
            }

            return result;
        }

        public abstract IEnumerator<int> GetEnumerator();

        IListSelection IListSelection.Move(int offset)
        {
            return Move(offset);
        }

        IListSelection IListSelection.Copy()
        {
            return Copy();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IListSelection IListSelection.ToByteSelection<T>(
            int startOffset,
            IByteDataConverter<T> converter)
        {
            return ToByteSelection(startOffset, converter);
        }

        public EnumerableIndexListSelection ToByteSelection<T>(
            int startOffset,
            IByteDataConverter<T> converter)
        {
            if (converter is null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            var sizeOfT = converter.SizeOfItem;
            return new EnumerableIndexListSelection(this.Select(GetOffset));

            int GetOffset(int index)
            {
                return (index * sizeOfT) + startOffset;
            }
        }
    }
}
