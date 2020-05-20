// <copyright file="Kakariko.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Zelda3.Palette
{
    using System;
    using System.Collections.ObjectModel;
    using Maseya.Helper.Collections;

    public class Kakariko
    {
        public Kakariko(PaletteIndexCollectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(54, 1, 3);
            Steps1 = builder.Flush();

            builder.AddRow(54, 5, 2);
            Steps2 = builder.Flush();

            builder.AddRow(55, 5, 2);
            PinkBush = builder.Flush();

            builder.AddRow(56, 4, 2);
            BlueRoof = builder.Flush();

            AllIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    Steps1,
                    Steps2,
                    PinkBush,
                    BlueRoof,
                });
        }

        public IndexCollection Steps1
        {
            get;
        }

        public IndexCollection Steps2
        {
            get;
        }

        public IndexCollection PinkBush
        {
            get;
        }

        public IndexCollection BlueRoof
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> AllIndexCollections
        {
            get;
        }
    }
}
