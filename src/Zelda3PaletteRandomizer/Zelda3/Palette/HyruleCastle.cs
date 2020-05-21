// <copyright file="HyruleCastle.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Zelda3.Palette
{
    using System;
    using System.Collections.ObjectModel;
    using Maseya.Helper.Collections;

    public class HyruleCastle
    {
        public HyruleCastle(PaletteIndexCollectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(36, 1, 1);
            PlantRoot = builder.Flush();

            builder.AddRow(36, 2, 3);
            SmallPlant = builder.Flush();

            builder.AddRow(36, 5, 2);
            RoseBush = builder.Flush();

            builder.AddRow(37, 2, 2);
            Moat = builder.Flush();

            builder.AddRow(38, 1, 3);
            builder.AddRow(38, 6, 1);
            Walls = builder.Flush();

            AllIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    Walls,
                    Moat,
                    RoseBush,
                    SmallPlant,
                    PlantRoot,
                });

            MiscIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    Moat,
                    RoseBush,
                    SmallPlant,
                    PlantRoot,
                });
        }

        public IndexCollection Walls
        {
            get;
        }

        public IndexCollection Moat
        {
            get;
        }

        public IndexCollection RoseBush
        {
            get;
        }

        public IndexCollection SmallPlant
        {
            get;
        }

        public IndexCollection PlantRoot
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> AllIndexCollections
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> MiscIndexCollections
        {
            get;
        }
    }
}
