// <copyright file="HashListIndexCollection.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections
{
    using System.Collections.Generic;
    using System.Linq;

    public class HashListIndexCollection : ListIndexCollection
    {
        public HashListIndexCollection(IEnumerable<int> collection)
            : base(collection)
        {
            HashSet = new HashSet<int>(this);
        }

        private HashSet<int> HashSet
        {
            get;
        }

        public override bool Contains(int index)
        {
            return HashSet.Contains(index);
        }

        public override IndexCollection Move(int offset)
        {
            return new HashListIndexCollection(this.Select(i => i + offset));
        }
    }
}
