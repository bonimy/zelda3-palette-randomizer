// <copyright file="IByteDataConverter.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System.Collections.Generic;

    public interface IByteDataConverter<T>
    {
        int SizeOfItem
        {
            get;
        }

        int GetOffset(int startOffset, int index);

        T GetItem(byte[] sourceArray);

        T GetItem(byte[] sourceArray, int startOffset);

        T[] GetItems(byte[] sourceArray);

        T[] GetItems(
            byte[] sourceArray,
            int startOffset,
            int size);

        byte[] GetBytes(T item);

        byte[] GetBytes(IReadOnlyList<T> items);

        void GetBytes(
            T item,
            byte[] destinationArray,
            int startOffset);

        void GetBytes(
            IReadOnlyList<T> items,
            byte[] destinationArray,
            int startOffset);
    }
}
