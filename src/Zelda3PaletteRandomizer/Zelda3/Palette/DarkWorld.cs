// <copyright file="DarkWorld.cs" company="Public Domain">
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

    public class DarkWorld
    {
        public DarkWorld(
            PaletteIndexCollectionBuilder objectBuilder,
            PaletteIndexCollectionBuilder spriteBuilder,
            PaletteIndexCollectionBuilder darkWorldSprites)
        {
            if (objectBuilder is null)
            {
                throw new ArgumentNullException(nameof(objectBuilder));
            }

            if (spriteBuilder is null)
            {
                throw new ArgumentNullException(nameof(spriteBuilder));
            }

            if (darkWorldSprites is null)
            {
                throw new ArgumentNullException(nameof(darkWorldSprites));
            }

            objectBuilder.AddRow(-8, 4, 1);
            objectBuilder.AddBox(5, 0, 4, 3);
            objectBuilder.AddColumn(5, 6, 2);
            objectBuilder.AddRow(7, 4, 3);
            objectBuilder.AddRow(65, 0, 1);
            objectBuilder.AddColumn(70, 0, 6);
            objectBuilder.AddRow(72, 1, 1);
            objectBuilder.AddRow(72, 6, 1);
            objectBuilder.AddRow(74, 1, 3);
            objectBuilder.AddRow(74, 6, 1);
            objectBuilder.AddColumn(77, 0, 2);
            objectBuilder.AddColumn(82, 0, 1);
            HillsAndDirt = objectBuilder.Flush();

            objectBuilder.AddRow(5, 4, 2);
            objectBuilder.AddBox(8, 4, 3, 2);
            objectBuilder.AddRow(9, 0, 1);
            objectBuilder.AddRow(9, 3, 1);
            objectBuilder.AddBox(72, 4, 2, 3);
            objectBuilder.AddRow(79, 5, 2);
            var list = new List<int>(objectBuilder.Flush()) { 0x5FEB3 };
            spriteBuilder.AddRow(8, 2, 2);
            list.AddRange(spriteBuilder.Flush());
            darkWorldSprites.AddRow(0, 2, 3);
            list.AddRange(darkWorldSprites.Flush());
            GrassShrubsAndTrees = new ListIndexCollection(list);
            list.Clear();

            objectBuilder.AddRow(6, 4, 2);
            objectBuilder.AddRow(65, 5, 2);
            objectBuilder.AddRow(71, 3, 3);
            list.AddRange(objectBuilder.Flush());
            spriteBuilder.AddRow(9, 2, 2);
            list.AddRange(spriteBuilder.Flush());
            DryGrassAndSand = new ListIndexCollection(list);
            list.Clear();

            objectBuilder.AddRow(-8, 0, 4);
            objectBuilder.AddRow(-8, 5, 1);
            objectBuilder.AddRow(69, 5, 1);
            objectBuilder.AddRow(70, 1, 1);
            objectBuilder.AddRow(71, 1, 2);
            objectBuilder.AddRow(71, 6, 1);
            objectBuilder.AddRow(72, 2, 2);
            objectBuilder.AddRow(73, 1, 1);
            objectBuilder.AddRow(73, 6, 1);
            list.AddRange(objectBuilder.Flush());
            darkWorldSprites.AddRow(1, 0, 4);
            darkWorldSprites.AddRow(1, 5, 2);
            list.AddRange(darkWorldSprites.Flush());
            Water = new ListIndexCollection(list);
            list.Clear();

            objectBuilder.AddRow(8, 0, 4);
            objectBuilder.AddRow(79, 0, 5);
            list.AddRange(objectBuilder.Flush());
            spriteBuilder.AddBox(8, 0, 2, 2);
            spriteBuilder.AddBox(8, 5, 2, 2);
            list.AddRange(spriteBuilder.Flush());
            FlowersAndRocks = new ListIndexCollection(list);
            list.Clear();

            objectBuilder.AddRow(9, 1, 2);
            list.AddRange(objectBuilder.Flush());
            spriteBuilder.AddRow(8, 4, 1);
            list.AddRange(spriteBuilder.Flush());
            SignsPostsAndBushes = new ListIndexCollection(list);
            list.Clear();

            objectBuilder.AddRow(81, 0, 7);
            PyramidBG = objectBuilder.Flush();

            objectBuilder.AddRow(83, 0, 7);
            Pyramid = objectBuilder.Flush();

            objectBuilder.AddRow(69, 0, 5);
            objectBuilder.AddRow(69, 6, 1);
            IcePalaceEntrance = objectBuilder.Flush();

            objectBuilder.AddRow(78, 1, 3);
            HouseStructure = objectBuilder.Flush();

            objectBuilder.AddRow(78, 4, 3);
            Houses = objectBuilder.Flush();

            objectBuilder.AddRow(64, 0, 4);
            objectBuilder.AddRow(76, 0, 4);
            objectBuilder.AddRow(80, 0, 4);
            list.AddRange(objectBuilder.Flush());
            spriteBuilder.AddRow(-9, 1, 1);
            spriteBuilder.AddRow(-9, 4, 3);
            list.AddRange(spriteBuilder.Flush());
            TreeWood = new ListIndexCollection(list);
            list.Clear();

            objectBuilder.AddRow(80, 4, 3);
            Trees = objectBuilder.Flush();

            objectBuilder.AddRow(64, 4, 3);
            YellowTrees = objectBuilder.Flush();

            objectBuilder.AddRow(76, 4, 3);
            PinkTrees = objectBuilder.Flush();

            objectBuilder.AddRow(82, 4, 3);
            PurpleTrees = objectBuilder.Flush();

            PalaceOfDarkness = new PalaceOfDarkness(objectBuilder);
            SkullWoods = new SkullWoods(objectBuilder);
            ThievesTown = new ThievesTown(objectBuilder);
            Swamp = new Swamp(objectBuilder);
            DarkMountain = new DarkMountain(
                objectBuilder,
                new PaletteIndexCollectionBuilder(0x3F97B, 7));

            AllIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    YellowTrees,
                    SignsPostsAndBushes,
                    FlowersAndRocks,
                    DryGrassAndSand,
                    HillsAndDirt,
                    GrassShrubsAndTrees,
                    TreeWood,
                    IcePalaceEntrance,
                    Water,
                    PurpleTrees,
                    Pyramid,
                    PyramidBG,
                    PinkTrees,
                    HouseStructure,
                    Houses,
                    Trees,
                });

            MiscIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    YellowTrees,
                    SignsPostsAndBushes,
                    DryGrassAndSand,
                    PurpleTrees,
                    PyramidBG,
                    PinkTrees,
                    HouseStructure,
                    Trees,
                });
        }

        public IndexCollection YellowTrees
        {
            get;
        }

        public IndexCollection SignsPostsAndBushes
        {
            get;
        }

        public IndexCollection FlowersAndRocks
        {
            get;
        }

        public IndexCollection DryGrassAndSand
        {
            get;
        }

        public IndexCollection HillsAndDirt
        {
            get;
        }

        public IndexCollection GrassShrubsAndTrees
        {
            get;
        }

        public IndexCollection TreeWood
        {
            get;
        }

        public IndexCollection IcePalaceEntrance
        {
            get;
        }

        public IndexCollection Water
        {
            get;
        }

        public IndexCollection PurpleTrees
        {
            get;
        }

        public IndexCollection Pyramid
        {
            get;
        }

        public IndexCollection PyramidBG
        {
            get;
        }

        public IndexCollection PinkTrees
        {
            get;
        }

        public IndexCollection HouseStructure
        {
            get;
        }

        public IndexCollection Houses
        {
            get;
        }

        public IndexCollection Trees
        {
            get;
        }

        public PalaceOfDarkness PalaceOfDarkness
        {
            get;
        }

        public SkullWoods SkullWoods
        {
            get;
        }

        public ThievesTown ThievesTown
        {
            get;
        }

        public Swamp Swamp
        {
            get;
        }

        public DarkMountain DarkMountain
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
