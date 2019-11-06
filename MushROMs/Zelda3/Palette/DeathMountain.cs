// <copyright file="DeathMountain.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using System;
    using Maseya.Helper.Collections;

    public class DeathMountain : SelectionCollection
    {
        public DeathMountain(PaletteSelectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddBox(10, 0, 4, 3);
            builder.AddRow(10, 6, 1);
            builder.AddRow(12, 4, 3);
            builder.AddRow(14, 6, 1);
            builder.AddRow(49, 5, 2);
            builder.AddBox(51, 1, 3, 2);
            builder.AddColumn(51, 6, 2);
            WallsAndAbyss = builder.CreatePaletteSelection();

            builder.AddRow(10, 4, 2);
            builder.AddBox(13, 5, 2, 2);
            builder.AddRow(51, 4, 2);
            Ground = builder.CreatePaletteSelection();

            builder.AddRow(11, 4, 3);
            builder.AddRow(50, 4, 1);
            builder.AddRow(50, 6, 1);
            Clouds = builder.CreatePaletteSelection();

            builder.AddRow(13, 0, 5);
            ShiniesAndRocks = builder.CreatePaletteSelection();

            builder.AddRow(14, 0, 5);
            builder.AddRow(14, 6, 1);
            PostsAndSteaks = builder.CreatePaletteSelection();

            builder.AddRow(49, 1, 3);
            HeraStone = builder.CreatePaletteSelection();

            builder.AddRow(49, 4, 1);
            HeraBricks = builder.CreatePaletteSelection();
        }

        public IListSelection WallsAndAbyss
        {
            get;
        }

        public IListSelection Ground
        {
            get;
        }

        public IListSelection Clouds
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

        public IListSelection HeraBricks
        {
            get;
        }

        public IListSelection HeraStone
        {
            get;
        }

        public override IListSelection[] AllSelections()
        {
            return new IListSelection[]
            {
                ShiniesAndRocks,
                PostsAndSteaks,
                HeraStone,
            };
        }
    }
}
