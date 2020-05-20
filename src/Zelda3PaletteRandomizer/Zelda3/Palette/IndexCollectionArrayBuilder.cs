// <copyright file="IndexCollectionArrayBuilder.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Zelda3.Palette
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Maseya.Helper.Collections;

    public class IndexCollectionArrayBuilder
    {
        public IndexCollectionArrayBuilder()
        {
            Builder = new Collection<IndexCollection>();
        }

        public Collection<IndexCollection> Builder
        {
            get;
        }

        public void AddCollectionsAsConcatenation(
            params IEnumerable<int>[] indexCollections)
        {
            var result = Enumerable.Empty<int>();
            foreach (var indexCollection in indexCollections)
            {
                result = result.Concat(indexCollection);
            }

            Builder.Add(new ListIndexCollection(result));
        }

        public IndexCollection[] Flush()
        {
            var result = Builder.ToArray();
            Builder.Clear();
            return result;
        }
    }
}
