// <copyright file="Swamp.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using System;
    using Maseya.Helper.Collections;

    public class Swamp : SelectionCollection
    {
        public Swamp(PaletteSelectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(70, 2, 3);
            VinesPrimary = builder.CreatePaletteSelection();

            builder.AddRow(70, 5, 2);
            VinesHighlight = builder.CreatePaletteSelection();
        }

        public IListSelection VinesPrimary
        {
            get;
        }

        public IListSelection VinesHighlight
        {
            get;
        }

        public override IListSelection[] AllSelections()
        {
            return new IListSelection[]
            {
                VinesPrimary,
                VinesHighlight,
            };
        }
    }
}
