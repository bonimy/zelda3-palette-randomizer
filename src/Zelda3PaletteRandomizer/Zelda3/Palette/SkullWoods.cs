// <copyright file="SkullWoods.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Zelda3.Palette
{
    using System;
    using System.Collections.ObjectModel;
    using Maseya.Helper.Collections;

    public class SkullWoods
    {
        public SkullWoods(PaletteIndexCollectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(67, 0, 4);
            builder.AddRow(67, 5, 1);
            Skull = builder.Flush();

            builder.AddRow(67, 4, 1);
            builder.AddRow(67, 6, 1);
            builder.AddRow(68, 0, 7);
            Trees = builder.Flush();

            AllIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    Trees,
                    Skull,
                });
        }

        public IndexCollection Trees
        {
            get;
        }

        public IndexCollection Skull
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> AllIndexCollections
        {
            get;
        }
    }
}
