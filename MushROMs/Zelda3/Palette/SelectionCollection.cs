// <copyright file="SelectionCollection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using System.Collections;
    using System.Collections.Generic;
    using Maseya.Helper.Collections;

    public abstract class SelectionCollection : IEnumerable<IListSelection>
    {
        public abstract IListSelection[] AllSelections();

        public IEnumerator<IListSelection> GetEnumerator()
        {
            var selections = AllSelections();
            for (var i = 0; i < selections.Length; i++)
            {
                yield return selections[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
