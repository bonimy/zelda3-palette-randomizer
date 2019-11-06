// <copyright file="ISelectionList.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections.Generic
{
    using System;
    using System.Collections.Generic;

    public interface ISelectionList<T> : IList<T>
    {
        event EventHandler ContentsModified;

        void SetRange(int index, IEnumerable<T> collection);

        void AddRange(IEnumerable<T> collection);

        void InsertRange(int index, IEnumerable<T> collection);

        void RemoveRange(int index, int count);

        void WriteSelection(IListSelectionData<T> values);

        void TransformSelection(
            IListSelection selection,
            Func<T, T> transformItem);

        void InsertSelection(IListSelectionData<T> values);

        void RemoveSelection(IListSelection selection);
    }
}
