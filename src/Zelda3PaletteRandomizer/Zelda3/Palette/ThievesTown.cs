// <copyright file="ThievesTown.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Zelda3.Palette
{
    using System;
    using System.Collections.ObjectModel;
    using Maseya.Helper.Collections;

    public class ThievesTown
    {
        public ThievesTown(PaletteIndexCollectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(75, 0, 4);
            HedgeFence = builder.Flush();

            builder.AddRow(75, 5, 2);
            Walkway = builder.Flush();

            builder.AddRow(77, 0, 7);
            GrassRoof = builder.Flush();

            AllIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    Walkway,
                    GrassRoof,
                    HedgeFence,
                });
        }

        public IndexCollection Walkway
        {
            get;
        }

        public IndexCollection GrassRoof
        {
            get;
        }

        public IndexCollection HedgeFence
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> AllIndexCollections
        {
            get;
        }
    }
}
