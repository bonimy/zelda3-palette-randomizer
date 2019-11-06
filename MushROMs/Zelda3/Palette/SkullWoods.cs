// <copyright file="SkullWoods.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using System;
    using Maseya.Helper.Collections;

    public class SkullWoods : SelectionCollection
    {
        public SkullWoods(PaletteSelectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(67, 0, 4);
            builder.AddRow(67, 5, 1);
            Skull = builder.CreatePaletteSelection();

            builder.AddRow(67, 4, 1);
            builder.AddRow(67, 6, 1);
            builder.AddRow(68, 0, 7);
            Trees = builder.CreatePaletteSelection();
        }

        public IListSelection Trees
        {
            get;
        }

        public IListSelection Skull
        {
            get;
        }

        public override IListSelection[] AllSelections()
        {
            return new IListSelection[]
            {
                Trees,
                Skull,
            };
        }
    }
}
