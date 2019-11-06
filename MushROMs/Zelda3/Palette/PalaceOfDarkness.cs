// <copyright file="PalaceOfDarkness.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using System;
    using Maseya.Helper.Collections;

    public class PalaceOfDarkness : SelectionCollection
    {
        public PalaceOfDarkness(PaletteSelectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(63, 0, 7);
            HedgeMaze = builder.CreatePaletteSelection();

            builder.AddRow(65, 1, 4);
            Entrance = builder.CreatePaletteSelection();
        }

        public IListSelection Entrance
        {
            get;
        }

        public IListSelection HedgeMaze
        {
            get;
        }

        public override IListSelection[] AllSelections()
        {
            return new IListSelection[]
            {
                Entrance,
                HedgeMaze,
            };
        }
    }
}
