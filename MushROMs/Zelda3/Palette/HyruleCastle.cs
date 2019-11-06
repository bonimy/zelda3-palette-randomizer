// <copyright file="HyruleCastle.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using System;
    using Maseya.Helper.Collections;

    public class HyruleCastle : SelectionCollection
    {
        public HyruleCastle(PaletteSelectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(36, 1, 1);
            PlantRoot = builder.CreatePaletteSelection();

            builder.AddRow(36, 2, 3);
            SmallPlant = builder.CreatePaletteSelection();

            builder.AddRow(36, 5, 2);
            RoseBush = builder.CreatePaletteSelection();

            builder.AddRow(37, 2, 2);
            Moat = builder.CreatePaletteSelection();

            builder.AddRow(38, 0, 4);
            builder.AddRow(38, 6, 1);
            Walls = builder.CreatePaletteSelection();
        }

        public IListSelection Walls
        {
            get;
        }

        public IListSelection Moat
        {
            get;
        }

        public IListSelection RoseBush
        {
            get;
        }

        public IListSelection SmallPlant
        {
            get;
        }

        public IListSelection PlantRoot
        {
            get;
        }

        public override IListSelection[] AllSelections()
        {
            return new IListSelection[]
            {
                Walls,
                Moat,
                RoseBush,
                SmallPlant,
                PlantRoot,
            };
        }
    }
}
