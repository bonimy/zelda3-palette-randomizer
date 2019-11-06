// <copyright file="DarkWorld.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using System;
    using System.Collections.Generic;
    using Maseya.Helper.Collections;

    public class DarkWorld : SelectionCollection
    {
        public DarkWorld(PaletteSelectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(-8, 4, 1);
            builder.AddRow(-8, 6, 1);
            builder.AddBox(5, 0, 4, 3);
            builder.AddColumn(5, 6, 2);
            builder.AddRow(7, 4, 3);
            builder.AddRow(72, 1, 1);
            builder.AddRow(72, 6, 1);
            builder.AddRow(74, 1, 2);
            builder.AddRow(74, 6, 1);
            HillAndDirt = builder.CreatePaletteSelection();

            builder.AddRow(5, 4, 2);
            builder.AddBox(8, 4, 3, 2);
            builder.AddBox(72, 4, 2, 3);
            builder.AddRow(79, 5, 2);
            GrassShrubsAndTrees = builder.CreatePaletteSelection();

            builder.AddRow(6, 4, 2);
            builder.AddRow(65, 5, 2);
            DryGrassAndSand = builder.CreatePaletteSelection();

            builder.AddRow(8, 1, 3);
            FlowersAndRocks = builder.CreatePaletteSelection();

            builder.AddRow(9, 1, 3);
            SignsPostsAndBushes = builder.CreatePaletteSelection();

            builder.AddRow(81, 0, 7);
            PyramidBG = builder.CreatePaletteSelection();

            builder.AddRow(83, 0, 7);
            Pyramid = builder.CreatePaletteSelection();

            builder.AddRow(-8, 0, 4);
            builder.AddRow(-8, 0, 5);
            builder.AddRow(69, 5, 1);
            builder.AddRow(70, 1, 1);
            builder.AddRow(71, 1, 2);
            builder.AddRow(71, 6, 1);
            builder.AddRow(72, 2, 2);
            builder.AddRow(73, 1, 1);
            builder.AddRow(73, 6, 1);
            Water = builder.CreatePaletteSelection();

            builder.AddRow(69, 0, 4);
            builder.AddRow(69, 6, 1);
            IcePalaceEntrance = builder.CreatePaletteSelection();

            builder.AddRow(78, 1, 3);
            HouseStructure = builder.CreatePaletteSelection();

            builder.AddRow(78, 4, 3);
            HouseRoof = builder.CreatePaletteSelection();

            builder.AddRow(64, 1, 3);
            builder.AddRow(76, 1, 3);
            builder.AddRow(80, 1, 3);
            TreeWood = builder.CreatePaletteSelection();

            builder.AddRow(80, 0, 1);
            builder.AddRow(80, 4, 3);
            Trees = builder.CreatePaletteSelection();

            builder.AddRow(64, 4, 3);
            YellowTrees = builder.CreatePaletteSelection();

            builder.AddRow(76, 4, 3);
            PinkTrees = builder.CreatePaletteSelection();

            builder.AddRow(82, 4, 3);
            PurpleTrees = builder.CreatePaletteSelection();

            PalaceOfDarkness = new PalaceOfDarkness(builder);
            SkullWoods = new SkullWoods(builder);
            ThievesTown = new ThievesTown(builder);
            Swamp = new Swamp(builder);
            DarkMountain = new DarkMountain(builder);
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

        public IListSelection YellowTrees
        {
            get;
        }

        public IListSelection SignsPostsAndBushes
        {
            get;
        }

        public IListSelection FlowersAndRocks
        {
            get;
        }

        public IListSelection DryGrassAndSand
        {
            get;
        }

        public IListSelection HillAndDirt
        {
            get;
        }

        public IListSelection GrassShrubsAndTrees
        {
            get;
        }

        public IListSelection TreeWood
        {
            get;
        }

        public IListSelection IcePalaceEntrance
        {
            get;
        }

        public IListSelection Water
        {
            get;
        }

        public IListSelection PurpleTrees
        {
            get;
        }

        public IListSelection Pyramid
        {
            get;
        }

        public IListSelection PyramidBG
        {
            get;
        }

        public IListSelection PinkTrees
        {
            get;
        }

        public IListSelection HouseStructure
        {
            get;
        }

        public IListSelection HouseRoof
        {
            get;
        }

        public IListSelection Trees
        {
            get;
        }

        public override IListSelection[] AllSelections()
        {
            var result = new List<IListSelection>()
            {
                YellowTrees,
                SignsPostsAndBushes,
                FlowersAndRocks,
                DryGrassAndSand,
                HillAndDirt,
                GrassShrubsAndTrees,
                TreeWood,
                IcePalaceEntrance,
                Water,
                PurpleTrees,
                Pyramid,
                PyramidBG,
                PinkTrees,
                HouseRoof,
                HouseRoof,
                Trees,
            };

            result.AddRange(PalaceOfDarkness);
            result.AddRange(SkullWoods);
            result.AddRange(ThievesTown);
            result.AddRange(Swamp);
            result.AddRange(DarkMountain);

            return result.ToArray();
        }
    }
}
