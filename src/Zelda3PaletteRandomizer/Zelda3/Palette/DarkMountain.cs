// <copyright file="DarkMountain.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Zelda3.Palette
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Maseya.Helper.Collections;

    public class DarkMountain
    {
        public DarkMountain(
            PaletteIndexCollectionBuilder builder1,
            PaletteIndexCollectionBuilder builder2)
        {
            if (builder1 is null)
            {
                throw new ArgumentNullException(nameof(builder1));
            }

            if (builder2 is null)
            {
                throw new ArgumentNullException(nameof(builder2));
            }

            builder1.AddBox(15, 0, 4, 3);
            builder1.AddRow(15, 6, 1);
            builder1.AddRow(17, 4, 3);
            builder1.AddRow(85, 0, 4);
            builder1.AddRow(85, 6, 1);
            builder1.AddRow(89, 0, 4);
            builder2.AddBox(0, 0, 4, 2);
            builder2.AddRow(1, 6, 1);
            builder2.AddRow(2, 0, 1);
            builder2.AddRow(4, 0, 4);
            WallsAndAbyss = FlushCombined();

            builder1.AddRow(15, 4, 2);
            builder1.AddRow(18, 5, 2);
            builder1.AddRow(19, 5, 1);
            builder1.AddColumn(45, 5, 3);
            builder1.AddRow(84, 5, 1);
            builder1.AddRow(89, 5, 1);
            builder2.AddRow(4, 5, 1);
            Ground = FlushCombined();

            builder1.AddRow(16, 4, 1);
            builder1.AddRow(16, 6, 1);
            builder1.AddRow(47, 4, 1);
            builder1.AddRow(47, 6, 1);
            builder1.AddRow(85, 5, 1);
            builder1.AddRow(87, 0, 5);
            builder1.AddRow(89, 4, 1);
            builder1.AddRow(89, 6, 1);
            PinkClouds = builder1.Flush();

            builder1.AddRow(-10, 4, 1);
            builder1.AddRow(-10, 6, 1);
            builder1.AddRow(86, 4, 1);
            builder1.AddRow(86, 6, 1);
            BlackClouds = builder1.Flush();

            builder1.AddRow(86, 3, 1);
            Lava = builder1.Flush();

            builder1.AddRow(-10, 5, 1);
            builder1.AddRow(16, 5, 1);
            builder1.AddRow(86, 0, 3);
            builder1.AddRow(86, 5, 1);
            LavaGround = builder1.Flush();

            builder1.AddRow(18, 0, 5);
            RocksAndGems = builder1.Flush();

            builder1.AddRow(19, 0, 5);
            builder1.AddRow(19, 6, 1);
            PostsAndSteaks = builder1.Flush();

            builder2.AddRow(0, 4, 3);
            builder2.AddRow(1, 5, 1);
            builder2.AddRow(2, 1, 6);
            builder2.AddRow(3, 4, 3);
            builder2.AddRow(4, 4, 1);
            builder2.AddRow(4, 6, 1);
            Lightning = builder2.Flush();

            builder1.AddBox(45, 1, 4, 2);
            builder1.AddRow(47, 1, 3);
            TurtleRockHead = builder1.Flush();

            builder1.AddRow(84, 0, 5);
            builder1.AddRow(84, 6, 1);
            TurtleRockLegs = builder1.Flush();

            builder1.AddRow(88, 1, 1);
            builder1.AddRow(88, 6, 1);
            GanonsTowerHighlight = builder1.Flush();

            builder1.AddRow(88, 0, 1);
            builder1.AddRow(88, 2, 4);
            GanonsTowerPrimary = builder1.Flush();

            AllIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    Ground,
                    PinkClouds,
                    BlackClouds,
                    Lava,
                    LavaGround,
                    WallsAndAbyss,
                    RocksAndGems,
                    PostsAndSteaks,
                    TurtleRockHead,
                    TurtleRockLegs,
                    Lightning,
                    GanonsTowerPrimary,
                    GanonsTowerHighlight,
                });

            MiscIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    Ground,
                    PinkClouds,
                    Lava,
                    LavaGround,
                    Lightning,
                    RocksAndGems,
                    PostsAndSteaks,
                    TurtleRockHead,
                    TurtleRockLegs,
                    GanonsTowerHighlight,
                });

            IndexCollection FlushCombined()
            {
                var collection = builder1.ConvertBaseCollection().Concat(
                    builder2.ConvertBaseCollection());
                builder1.Clear();
                builder2.Clear();
                return new ListIndexCollection(collection);
            }
        }

        public IndexCollection Ground
        {
            get;
        }

        public IndexCollection PinkClouds
        {
            get;
        }

        public IndexCollection BlackClouds
        {
            get;
        }

        public IndexCollection Lava
        {
            get;
        }

        public IndexCollection LavaGround
        {
            get;
        }

        public IndexCollection WallsAndAbyss
        {
            get;
        }

        public IndexCollection RocksAndGems
        {
            get;
        }

        public IndexCollection PostsAndSteaks
        {
            get;
        }

        public IndexCollection TurtleRockHead
        {
            get;
        }

        public IndexCollection TurtleRockLegs
        {
            get;
        }

        public IndexCollection Lightning
        {
            get;
        }

        public IndexCollection GanonsTowerPrimary
        {
            get;
        }

        public IndexCollection GanonsTowerHighlight
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
