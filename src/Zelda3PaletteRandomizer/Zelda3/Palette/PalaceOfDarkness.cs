// <copyright file="PalaceOfDarkness.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Zelda3.Palette
{
    using System;
    using System.Collections.ObjectModel;
    using Maseya.Helper.Collections;

    public class PalaceOfDarkness
    {
        public PalaceOfDarkness(PaletteIndexCollectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(63, 0, 7);
            HedgeMaze = builder.Flush();

            builder.AddRow(65, 1, 4);
            Entrance = builder.Flush();

            AllIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    Entrance,
                    HedgeMaze,
                });
        }

        public IndexCollection Entrance
        {
            get;
        }

        public IndexCollection HedgeMaze
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> AllIndexCollections
        {
            get;
        }
    }
}
