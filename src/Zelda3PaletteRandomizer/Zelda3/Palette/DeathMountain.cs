// <copyright file="DeathMountain.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Zelda3.Palette
{
    using System;
    using System.Collections.ObjectModel;
    using Maseya.Helper.Collections;

    public class DeathMountain
    {
        public DeathMountain(PaletteIndexCollectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(-9, 0, 1);
            builder.AddBox(10, 0, 4, 3);
            builder.AddRow(10, 6, 1);
            builder.AddRow(12, 4, 3);
            builder.AddRow(49, 5, 2);
            builder.AddBox(51, 0, 4, 2);
            builder.AddColumn(51, 6, 2);
            WallsAndAbyss = builder.Flush();

            builder.AddRow(10, 4, 2);
            builder.AddBox(13, 4, 2, 2);
            builder.AddRow(51, 4, 2);
            Ground = builder.Flush();

            builder.AddRow(-9, 4, 1);
            builder.AddRow(-9, 6, 1);
            builder.AddRow(11, 4, 3);
            builder.AddRow(50, 4, 1);
            builder.AddRow(50, 6, 1);
            builder.AddRow(52, 4, 2);
            builder.AddRow(53, 4, 1);
            builder.AddRow(53, 6, 1);
            Clouds = builder.Flush();

            builder.AddRow(50, 0, 4);
            Bridge = builder.Flush();

            builder.AddRow(13, 0, 4);
            RocksAndGems = builder.Flush();

            builder.AddRow(14, 0, 4);
            builder.AddRow(14, 6, 1);
            PostsAndSteaks = builder.Flush();

            builder.AddRow(49, 0, 4);
            HeraStone = builder.Flush();

            builder.AddRow(49, 4, 1);
            HeraBricks = builder.Flush();

            AllIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    WallsAndAbyss,
                    Ground,
                    Clouds,
                    RocksAndGems,
                    PostsAndSteaks,
                    Bridge,
                    HeraBricks,
                    HeraStone,
                });

            MiscIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    Ground,
                    RocksAndGems,
                    PostsAndSteaks,
                    Bridge,
                    HeraStone,
                });
        }

        public IndexCollection WallsAndAbyss
        {
            get;
        }

        public IndexCollection Ground
        {
            get;
        }

        public IndexCollection Clouds
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

        public IndexCollection Bridge
        {
            get;
        }

        public IndexCollection HeraBricks
        {
            get;
        }

        public IndexCollection HeraStone
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
