// <copyright file="ByteDataConverter.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System;
    using System.Collections.Generic;

    public abstract class ByteDataConverter<T> :
        IByteDataConverter<T>
        where T : unmanaged
    {
        protected ByteDataConverter()
        {
        }

        public abstract int SizeOfItem
        {
            get;
        }

        public int GetOffset(int startOffset, int index)
        {
            return startOffset + (index * SizeOfItem);
        }

        public T GetItem(byte[] sourceArray)
        {
            return GetItem(sourceArray, 0);
        }

        public abstract T GetItem(
            byte[] sourceArray,
            int startOffset);

        public T[] GetItems(byte[] sourceArray)
        {
            if (sourceArray is null)
            {
                throw new ArgumentNullException(nameof(sourceArray));
            }

            return GetItems(sourceArray, 0, sourceArray.Length);
        }

        public abstract T[] GetItems(
            byte[] sourceArray,
            int startOffset,
            int size);

        public byte[] GetBytes(T item)
        {
            var result = new byte[SizeOfItem];
            GetBytes(item, result, 0);
            return result;
        }

        public abstract void GetBytes(
            T item,
            byte[] destinationArray,
            int startOffset);

        public byte[] GetBytes(IReadOnlyList<T> items)
        {
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var result = new byte[items.Count * SizeOfItem];
            GetBytes(items, result, 0);
            return result;
        }

        public abstract void GetBytes(
            IReadOnlyList<T> items,
            byte[] destinationArray,
            int startOffset);
    }
}
