// <copyright file="SingleIndexCollection.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections
{
    using System;
    using System.Collections.Generic;

    public class SingleIndexCollection : IndexCollection
    {
        public SingleIndexCollection(int value)
            : base(value, value, 1)
        {
            Value = value;
        }

        public int Value
        {
            get;
        }

        public override int this[int i]
        {
            get
            {
                if (i != 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(i));
                }

                return Value;
            }
        }

        public override bool Contains(int index)
        {
            return index == Value;
        }

        public override IndexCollection Move(int offset)
        {
            return new SingleIndexCollection(Value + offset);
        }

        public override IEnumerator<int> GetEnumerator()
        {
            yield return Value;
        }
    }
}
