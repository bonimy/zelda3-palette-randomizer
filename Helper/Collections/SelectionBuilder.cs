// <copyright file="SelectionBuilder.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    public class SelectionBuilder
    {
        public SelectionBuilder()
        {
            Collection = Enumerable.Empty<int>();
        }

        private IEnumerable<int> Collection
        {
            get;
            set;
        }

        public void Add(IListSelection selection)
        {
            Collection = Collection.Union(selection);
        }

        public void AddLinearSelection(int index, int count)
        {
            Add(new LinearListSelection(index, count));
        }

        public void AddBoxSelection(int index, Size size, int gridWidth)
        {
            Add(new BoxListSelection(index, size, gridWidth));
        }

        public void AddBoxSelection(
            int index,
            int width,
            int height,
            int gridWidth)
        {
            Add(new BoxListSelection(index, width, height, gridWidth));
        }

        public void AddIndex(int index)
        {
            Collection.Union(new int[] { index });
        }

        public void Remove(IListSelection selection)
        {
            Collection.Except(selection);
        }

        public void Clear()
        {
            Collection = Enumerable.Empty<int>();
        }

        public EnumerableIndexListSelection CreateSelection()
        {
            return CreateSelection(true);
        }

        public EnumerableIndexListSelection CreateSelection(bool clear)
        {
            var result = new EnumerableIndexListSelection(Collection);
            if (clear)
            {
                Clear();
            }

            return result;
        }

        public List<int> Current()
        {
            return new List<int>(Collection);
        }
    }
}
