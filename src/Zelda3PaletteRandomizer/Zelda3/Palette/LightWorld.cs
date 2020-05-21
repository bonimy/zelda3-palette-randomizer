// <copyright file="LightWorld.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Zelda3.Palette
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Maseya.Helper.Collections;

    public class LightWorld
    {
        public LightWorld(
            PaletteIndexCollectionBuilder objectBuilder,
            PaletteIndexCollectionBuilder spriteBuilder,
            PaletteIndexCollectionBuilder lightWorldSprites)
        {
            if (objectBuilder is null)
            {
                throw new ArgumentNullException(nameof(objectBuilder));
            }

            if (spriteBuilder is null)
            {
                throw new ArgumentNullException(nameof(spriteBuilder));
            }

            if (lightWorldSprites is null)
            {
                throw new ArgumentNullException(nameof(lightWorldSprites));
            }

            objectBuilder.AddRow(-7, 0, 1);
            objectBuilder.AddRow(-7, 4, 1);
            objectBuilder.AddRow(-7, 6, 1);
            objectBuilder.AddBox(0, 0, 4, 3);
            objectBuilder.AddColumn(0, 6, 2);
            objectBuilder.AddRow(2, 4, 3);
            objectBuilder.AddRow(30, 0, 1);
            objectBuilder.AddColumn(33, 0, 5);
            objectBuilder.AddRow(42, 0, 2);
            objectBuilder.AddRow(42, 6, 1);
            objectBuilder.AddRow(43, 0, 1);
            objectBuilder.AddRow(44, 0, 4);
            objectBuilder.AddRow(44, 6, 1);
            objectBuilder.AddRow(53, 3, 1);
            objectBuilder.AddColumn(55, 0, 5);
            objectBuilder.AddRow(57, 5, 2);
            HillsAndDirt = objectBuilder.Flush();

            objectBuilder.AddRow(0, 4, 2);
            objectBuilder.AddBox(3, 4, 3, 2);
            objectBuilder.AddBox(25, 0, 7, 5);
            objectBuilder.AddRow(31, 5, 2);
            objectBuilder.AddRow(32, 0, 1);
            objectBuilder.AddRow(32, 4, 3);
            objectBuilder.AddRow(34, 3, 1);
            objectBuilder.AddRow(34, 5, 1);
            objectBuilder.AddRow(35, 4, 3);
            objectBuilder.AddRow(38, 4, 2);
            objectBuilder.AddBox(42, 4, 2, 3);
            objectBuilder.AddRow(53, 0, 3);
            objectBuilder.AddRow(53, 5, 1);
            objectBuilder.AddRow(54, 0, 1);
            objectBuilder.AddColumn(54, 4, 2);
            objectBuilder.AddRow(60, 6, 1);
            objectBuilder.AddRow(62, 0, 1);
            objectBuilder.AddRow(62, 2, 5);
            objectBuilder.AddRow(61, 0, 7);
            var list = new List<int>(objectBuilder.Flush())
            {
                -0x1199C,
                -0x17F7E,
                -0x5FEE7,
                0x5FEA9,
                0x67FC6,
                0x67FE1,
                0x67FE6,
            };
            spriteBuilder.AddRow(6, 2, 2);
            spriteBuilder.AddRow(-11, 1, 1);
            spriteBuilder.AddRow(-11, 4, 3);
            list.AddRange(spriteBuilder.Flush());
            lightWorldSprites.AddRow(0, 2, 3);
            list.AddRange(lightWorldSprites.Flush());
            GrassShrubsAndTrees = new ListIndexCollection(list);
            list.Clear();

            objectBuilder.AddRow(1, 4, 2);
            objectBuilder.AddRow(32, 1, 3);
            objectBuilder.AddRow(34, 1, 2);
            objectBuilder.AddRow(55, 1, 2);
            objectBuilder.AddRow(58, 1, 6);
            objectBuilder.AddRow(59, 5, 2);
            objectBuilder.AddRow(62, 1, 1);
            list.AddRange(objectBuilder.Flush());
            spriteBuilder.AddRow(7, 2, 2);
            list.AddRange(spriteBuilder.Flush());
            DryGrassAndSand = new ListIndexCollection(list);
            list.Clear();

            objectBuilder.AddRow(-7, 1, 3);
            objectBuilder.AddRow(-7, 5, 1);
            objectBuilder.AddRow(42, 2, 2);
            objectBuilder.AddRow(43, 1, 1);
            objectBuilder.AddRow(43, 6, 1);
            objectBuilder.AddRow(60, 0, 6);
            list.AddRange(objectBuilder.Flush());
            lightWorldSprites.AddRow(1, 0, 4);
            lightWorldSprites.AddRow(1, 5, 2);
            lightWorldSprites.AddColumn(8, 14, 4);
            list.AddRange(lightWorldSprites.Flush());
            Water = new ListIndexCollection(list);
            list.Clear();

            objectBuilder.AddRow(3, 0, 4);
            objectBuilder.AddRow(31, 0, 5);
            list.AddRange(objectBuilder.Flush());
            spriteBuilder.AddBox(6, 0, 2, 2);
            spriteBuilder.AddBox(6, 5, 2, 2);
            list.AddRange(spriteBuilder.Flush());
            FlowersAndRocks = new ListIndexCollection(list);
            list.Clear();

            objectBuilder.AddRow(4, 0, 4);
            list.AddRange(objectBuilder.Flush());
            spriteBuilder.AddColumn(6, 4, 2);
            list.AddRange(spriteBuilder.Flush());
            SignsAndPosts = new ListIndexCollection(list);
            list.Clear();

            objectBuilder.AddRow(43, 2, 2);
            CaveCrest = objectBuilder.Flush();

            objectBuilder.AddRow(30, 1, 6);
            objectBuilder.AddRow(56, 1, 3);
            objectBuilder.AddRow(56, 6, 1);
            Houses = objectBuilder.Flush();

            objectBuilder.AddRow(34, 4, 1);
            objectBuilder.AddRow(34, 6, 1);
            RedTree = objectBuilder.Flush();

            objectBuilder.AddRow(35, 1, 3);
            Tombstone = objectBuilder.Flush();

            objectBuilder.AddRow(57, 1, 3);
            objectBuilder.AddRow(59, 1, 4);
            Palace = objectBuilder.Flush();

            objectBuilder.AddRow(57, 4, 1);
            DesertPrayerFloor = objectBuilder.Flush();

            objectBuilder.AddRow(-9, 1, 3);
            WarpTile = objectBuilder.Flush();

            HyruleCastle = new HyruleCastle(objectBuilder);
            Sanctuary = new Sanctuary(objectBuilder);
            Kakariko = new Kakariko(objectBuilder);
            DeathMountain = new DeathMountain(objectBuilder);

            AllIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    GrassShrubsAndTrees,
                    HillsAndDirt,
                    Water,
                    RedTree,
                    Houses,
                    Tombstone,
                    CaveCrest,
                    DryGrassAndSand,
                    FlowersAndRocks,
                    SignsAndPosts,
                    Palace,
                    DesertPrayerFloor,
                    WarpTile,
                });

            MiscIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    RedTree,
                    Tombstone,
                    CaveCrest,
                    DryGrassAndSand,
                    SignsAndPosts,
                    Palace,
                    DesertPrayerFloor,
                    WarpTile,
                });
        }

        public IndexCollection HillsAndDirt
        {
            get;
        }

        public IndexCollection Water
        {
            get;
        }

        public IndexCollection RedTree
        {
            get;
        }

        public IndexCollection Houses
        {
            get;
        }

        public IndexCollection Tombstone
        {
            get;
        }

        public IndexCollection CaveCrest
        {
            get;
        }

        public IndexCollection GrassShrubsAndTrees
        {
            get;
        }

        public IndexCollection DryGrassAndSand
        {
            get;
        }

        public IndexCollection FlowersAndRocks
        {
            get;
        }

        public IndexCollection SignsAndPosts
        {
            get;
        }

        public IndexCollection Palace
        {
            get;
        }

        public IndexCollection DesertPrayerFloor
        {
            get;
        }

        public IndexCollection WarpTile
        {
            get;
        }

        public HyruleCastle HyruleCastle
        {
            get;
        }

        public Sanctuary Sanctuary
        {
            get;
        }

        public Kakariko Kakariko
        {
            get;
        }

        public DeathMountain DeathMountain
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
