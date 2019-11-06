// <copyright file="ISelectionList.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections
{
    using System;
    using System.Collections;

    public interface ISelectionList : IList
    {
        event EventHandler ContentsModified;

        void SetRange(int index, IEnumerable collection);

        void AddRange(IEnumerable collection);

        void InsertRange(int index, IEnumerable collection);

        void RemoveRange(int index, int count);

        void WriteSelection(IListSelectionData values);

        void TransformSelection(
            IListSelection selection,
            Func<object, object> transformItem);

        void InsertSelection(IListSelectionData values);

        void RemoveSelection(IListSelection selection);
    }
}
