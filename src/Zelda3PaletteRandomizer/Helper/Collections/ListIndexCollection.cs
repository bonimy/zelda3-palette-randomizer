// <copyright file="ListIndexCollection.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ListIndexCollection : IndexCollection
    {
        public ListIndexCollection(IEnumerable<int> collection)
            : this(new Initializer(collection))
        {
        }

        private ListIndexCollection(Initializer initializer)
            : base(initializer.Min, initializer.Max, initializer.Count)
        {
            List = initializer.List;
        }

        private ReadOnlyCollection<int> List
        {
            get;
        }

        public override int this[int i]
        {
            get
            {
                return List[i];
            }
        }

        public override bool Contains(int index)
        {
            return List.Contains(index);
        }

        public override IEnumerator<int> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return List[i];
            }
        }

        public override IndexCollection Move(int offset)
        {
            return new ListIndexCollection(this.Select(i => i + offset));
        }

        private struct Initializer
        {
            public Initializer(IEnumerable<int> collection)
            {
                if (collection is null)
                {
                    throw new ArgumentNullException(nameof(collection));
                }

                List = new List<int>(collection).AsReadOnly();
                Count = List.Count;
                if (Count == 0)
                {
                    Max = Int32.MaxValue;
                    Min = Int32.MinValue;
                }
                else
                {
                    Max = List.Max();
                    Min = List.Min();
                }
            }

            public ReadOnlyCollection<int> List
            {
                get;
            }

            public int Count
            {
                get;
            }

            public int Min
            {
                get;
            }

            public int Max
            {
                get;
            }
        }
    }
}
