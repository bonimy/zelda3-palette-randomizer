// <copyright file="LightWorld.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using System;
    using System.Collections.Generic;
    using Maseya.Helper.Collections;

    public class LightWorld : SelectionCollection
    {
        public LightWorld(PaletteSelectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(-7, 4, 1);
            builder.AddRow(-7, 6, 1);
            builder.AddBox(0, 0, 4, 3);
            builder.AddColumn(0, 6, 2);
            builder.AddRow(2, 4, 3);
            builder.AddRow(42, 1, 1);
            builder.AddRow(42, 6, 1);
            builder.AddRow(44, 1, 3);
            builder.AddRow(44, 6, 1);
            builder.AddRow(57, 5, 2);
            builder.AddRow(59, 5, 1);
            HillAndDirt = builder.CreatePaletteSelection();

            builder.AddRow(0, 4, 2);
            builder.AddBox(3, 4, 3, 2);
            builder.AddRow(31, 5, 2);
            builder.AddRow(32, 4, 3);
            builder.AddRow(34, 3, 1);
            builder.AddRow(34, 5, 1);
            builder.AddRow(35, 4, 3);
            builder.AddRow(38, 5, 2);
            builder.AddBox(42, 4, 2, 3);
            builder.AddRow(53, 1, 2);
            builder.AddRow(53, 5, 1);
            builder.AddColumn(54, 4, 2);
            GrassShrubsAndTrees = builder.CreatePaletteSelection();

            builder.AddRow(1, 4, 2);
            builder.AddRow(32, 1, 3);
            builder.AddRow(34, 1, 2);
            builder.AddRow(55, 1, 2);
            builder.AddRow(58, 1, 7);
            builder.AddRow(59, 5, 2);
            DryGrassAndSand = builder.CreatePaletteSelection();

            builder.AddRow(-7, 1, 3);
            builder.AddRow(-7, 5, 1);
            builder.AddRow(42, 2, 2);
            builder.AddRow(43, 1, 1);
            builder.AddRow(43, 6, 1);
            Water = builder.CreatePaletteSelection();

            builder.AddRow(3, 0, 4);
            builder.AddRow(31, 0, 5);
            FlowersAndRocks = builder.CreatePaletteSelection();

            builder.AddRow(4, 1, 3);
            SignsPostsAndBushes = builder.CreatePaletteSelection();

            builder.AddRow(43, 2, 2);
            CaveCrest = builder.CreatePaletteSelection();

            builder.AddRow(30, 1, 6);
            builder.AddRow(56, 1, 3);
            builder.AddRow(56, 6, 1);
            House = builder.CreatePaletteSelection();

            builder.AddRow(34, 4, 1);
            builder.AddRow(34, 6, 1);
            RedTree = builder.CreatePaletteSelection();

            builder.AddRow(35, 1, 3);
            Tombstone = builder.CreatePaletteSelection();

            builder.AddRow(57, 1, 3);
            builder.AddRow(59, 1, 4);
            Palace = builder.CreatePaletteSelection();

            builder.AddRow(57, 4, 1);
            DesertPrayerFloor = builder.CreatePaletteSelection();

            HyruleCastle = new HyruleCastle(builder);
            Sanctuary = new Sanctuary(builder);
            Kakariko = new Kakariko(builder);
            DeathMountain = new DeathMountain(builder);
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

        public IListSelection HillAndDirt
        {
            get;
        }

        public IListSelection Water
        {
            get;
        }

        public IListSelection RedTree
        {
            get;
        }

        public IListSelection House
        {
            get;
        }

        public IListSelection Tombstone
        {
            get;
        }

        public IListSelection CaveCrest
        {
            get;
        }

        public IListSelection GrassShrubsAndTrees
        {
            get;
        }

        public IListSelection DryGrassAndSand
        {
            get;
        }

        public IListSelection FlowersAndRocks
        {
            get;
        }

        public IListSelection SignsPostsAndBushes
        {
            get;
        }

        public IListSelection Palace
        {
            get;
        }

        public IListSelection DesertPrayerFloor
        {
            get;
        }

        public override IListSelection[] AllSelections()
        {
            var result = new List<IListSelection>()
            {
                HillAndDirt,
                Water,
                RedTree,
                House,
                Tombstone,
                CaveCrest,
                GrassShrubsAndTrees,
                DryGrassAndSand,
                FlowersAndRocks,
                SignsPostsAndBushes,
                Palace,
                DesertPrayerFloor,
            };

            result.AddRange(HyruleCastle);
            result.AddRange(Sanctuary);
            result.AddRange(Kakariko);
            result.AddRange(DeathMountain);

            return result.ToArray();
        }
    }
}
