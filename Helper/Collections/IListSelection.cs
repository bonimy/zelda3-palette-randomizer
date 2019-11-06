// <copyright file="IListSelection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections
{
    using System.Collections.Generic;

    public interface IListSelection : IReadOnlyList<int>
    {
        int MinIndex
        {
            get;
        }

        int MaxIndex
        {
            get;
        }

        bool ContainsIndex(int index);

        IListSelection Copy();

        IListSelection Move(int offset);

        IListSelection ToByteSelection<T>(
            int startOffset,
            IByteDataConverter<T> converter);
    }
}
