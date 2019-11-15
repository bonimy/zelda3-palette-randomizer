// <copyright file="DarkMountain.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using System;
    using System.Linq;
    using Maseya.Helper.Collections;

    public class DarkMountain : SelectionCollection
    {
        public DarkMountain(PaletteSelectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var builder2 = new PaletteSelectionBuilder(0x3F97B, 7);

            builder.AddBox(15, 0, 4, 3);
            builder.AddRow(15, 6, 1);
            builder.AddRow(17, 4, 3);
            builder.AddRow(19, 6, 1);
            builder.AddRow(85, 0, 4);
            builder.AddRow(85, 6, 1);
            builder.AddRow(89, 0, 4);
            var list = builder.CurrentPaletteIndexes();
            builder.Clear();
            builder2.AddBox(0, 0, 4, 2);
            builder2.AddRow(1, 6, 1);
            builder2.AddRow(2, 0, 1);
            builder2.AddRow(4, 0, 4);
            list.AddRange(builder2.CurrentPaletteIndexes());
            builder2.Clear();
            WallsAndAbyss = new EnumerableIndexListSelection(list);

            builder.AddRow(15, 4, 2);
            builder.AddBox(18, 5, 2, 2);
            builder.AddColumn(45, 5, 3);
            builder.AddRow(84, 5, 1);
            builder.AddColumn(88, 5, 2);
            list = builder.CurrentPaletteIndexes();
            builder2.AddRow(4, 5, 1);
            list.AddRange(builder2.CurrentPaletteIndexes());
            builder2.Clear();

            // TODO: Figure out what other palettes exist at this address.
            list.Add(0x3F9BD);
            Ground = builder.CreatePaletteSelection();

            builder.AddRow(16, 4, 1);
            builder.AddRow(16, 6, 1);
            builder.AddRow(47, 4, 1);
            builder.AddRow(47, 6, 1);
            builder.AddRow(85, 5, 1);
            builder.AddRow(87, 0, 5);
            builder.AddRow(89, 4, 1);
            builder.AddRow(89, 6, 1);
            PinkClouds = builder.CreatePaletteSelection();

            builder.AddRow(-10, 4, 1);
            builder.AddRow(-10, 6, 1);
            builder.AddRow(86, 4, 1);
            builder.AddRow(86, 6, 1);
            BlackClouds = builder.CreatePaletteSelection();

            builder.AddRow(86, 3, 1);
            Lava = builder.CreatePaletteSelection();

            builder.AddRow(-10, 5, 1);
            builder.AddRow(16, 5, 1);
            builder.AddRow(86, 0, 3);
            builder.AddRow(86, 5, 1);
            LavaGround = builder.CreatePaletteSelection();

            builder.AddRow(18, 0, 5);
            ShiniesAndRocks = builder.CreatePaletteSelection();

            builder.AddRow(19, 0, 5);
            builder.AddRow(19, 6, 1);
            PostsAndSteaks = builder.CreatePaletteSelection();

            builder2.AddRow(0, 4, 3);
            builder2.AddRow(1, 5, 1);
            builder2.AddRow(2, 1, 6);
            builder2.AddRow(3, 4, 3);
            builder2.AddRow(4, 4, 1);
            builder2.AddRow(4, 6, 1);
            Lightning = builder2.CreatePaletteSelection();

            builder.AddBox(45, 1, 4, 2);
            builder.AddRow(47, 1, 3);
            TurtleRockHead = builder.CreatePaletteSelection();

            builder.AddRow(84, 0, 5);
            builder.AddRow(84, 6, 1);
            TurtleRockLegs = builder.CreatePaletteSelection();

            builder.AddRow(88, 1, 1);
            builder.AddRow(88, 6, 1);
            GanonsTowerHighlight = builder.CreatePaletteSelection();

            builder.AddRow(88, 0, 1);
            builder.AddRow(88, 2, 4);
            GanonsTowerPrimary = builder.CreatePaletteSelection();
        }

        public IListSelection Ground
        {
            get;
        }

        public IListSelection PinkClouds
        {
            get;
        }

        public IListSelection BlackClouds
        {
            get;
        }

        public IListSelection Lava
        {
            get;
        }

        public IListSelection LavaGround
        {
            get;
        }

        public IListSelection WallsAndAbyss
        {
            get;
        }

        public IListSelection ShiniesAndRocks
        {
            get;
        }

        public IListSelection PostsAndSteaks
        {
            get;
        }

        public IListSelection TurtleRockHead
        {
            get;
        }

        public IListSelection TurtleRockLegs
        {
            get;
        }

        public IListSelection Lightning
        {
            get;
        }

        public IListSelection GanonsTowerPrimary
        {
            get;
        }

        public IListSelection GanonsTowerHighlight
        {
            get;
        }

        public override IListSelection[] AllSelections()
        {
            return new IListSelection[]
            {
                Ground,
                PinkClouds,
                Lava,
                LavaGround,
                Lightning,
                ShiniesAndRocks,
                PostsAndSteaks,
                TurtleRockHead,
                TurtleRockLegs,
                GanonsTowerHighlight,
            };
        }
    }
}
