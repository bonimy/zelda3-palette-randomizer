// <copyright file="ByteDataConverter.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using static ThrowHelper;

    public abstract class ByteDataConverter<T>
        where T : unmanaged
    {
        public static readonly ByteDataConverter<T> Default =
            new DefaultConverter();

        protected ByteDataConverter(int sizeOfItem)
        {
            if (sizeOfItem <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sizeOfItem));
            }

            SizeOfItem = sizeOfItem;
        }

        public int SizeOfItem
        {
            get;
        }

        public int GetOffset(int startOffset, int index)
        {
            return startOffset + (index * SizeOfItem);
        }

        public IEnumerable<int> GetStartOffsetAtEachIndex(
            int startOffset,
            IEnumerable<int> indexes)
        {
            if (indexes is null)
            {
                throw new ArgumentNullException(nameof(indexes));
            }

            foreach (var index in indexes)
            {
                yield return GetOffset(startOffset, index);
            }
        }

        public IEnumerable<int> GetOffsetRanges(
            int startOffset,
            IEnumerable<int> indexes)
        {
            if (indexes is null)
            {
                throw new ArgumentNullException(nameof(indexes));
            }

            foreach (var index in indexes)
            {
                var offset = GetOffset(startOffset, index);
                for (var i = 0; i < SizeOfItem; i++)
                {
                    yield return offset + i;
                }
            }
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

        private sealed class DefaultConverter : ByteDataConverter<T>
        {
            public DefaultConverter()
                : base(Marshal.SizeOf<T>())
            {
            }

            public unsafe override T GetItem(byte[] sourceArray, int startOffset)
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
                    throw IndexBoundsArgumentException(nameof(startOffset));
                }

                fixed (byte* source = &sourceArray[startOffset])
                {
                    return GetItemInternal(source);
                }
            }

            public unsafe override T[] GetItems(
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

                if (size < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(size));
                }

                if (startOffset + size > sourceArray.Length)
                {
                    throw IndexBoundsArgumentException(nameof(startOffset));
                }

                var result = new T[size / SizeOfItem];
                fixed (byte* source = &sourceArray[startOffset])
                {
                    for (var i = 0; i < result.Length; i++)
                    {
                        result[i] = GetItemInternal(source + (i * SizeOfItem));
                    }
                }

                return result;
            }

            public unsafe override void GetBytes(
                T item,
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
                    throw IndexBoundsArgumentException(nameof(startOffset));
                }

                fixed (byte* dest = &destinationArray[startOffset])
                {
                    GetBytesInternal(item, dest);
                }
            }

            public unsafe override void GetBytes(
                IReadOnlyList<T> items,
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
                    throw IndexBoundsArgumentException(nameof(startOffset));
                }

                fixed (byte* dest = &destinationArray[startOffset])
                {
                    for (var i = 0; i < items.Count; i++)
                    {
                        GetBytesInternal(
                            items[i],
                            dest + (i * SizeOfItem));
                    }
                }
            }

            private unsafe T GetItemInternal(
                byte* source)
            {
                T result = default;
                Buffer.MemoryCopy(
                    source,
                    &result,
                    SizeOfItem,
                    SizeOfItem);
                return result;
            }

            private unsafe void GetBytesInternal(
                T item,
                byte* dest)
            {
                Buffer.MemoryCopy(
                    &item,
                    dest,
                    SizeOfItem,
                    SizeOfItem);
            }
        }
    }
}
