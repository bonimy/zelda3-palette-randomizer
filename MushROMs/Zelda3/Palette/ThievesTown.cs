// <copyright file="ThievesTown.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using System;
    using Maseya.Helper.Collections;

    public class ThievesTown : SelectionCollection
    {
        public ThievesTown(PaletteSelectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(75, 0, 4);
            HedgeFence = builder.CreatePaletteSelection();

            builder.AddRow(75, 5, 2);
            Walkway = builder.CreatePaletteSelection();

            builder.AddRow(77, 0, 7);
            GrassRoof = builder.CreatePaletteSelection();
        }

        public IListSelection Walkway
        {
            get;
        }

        public IListSelection GrassRoof
        {
            get;
        }

        public IListSelection HedgeFence
        {
            get;
        }

        public override IListSelection[] AllSelections()
        {
            return new IListSelection[]
            {
                Walkway,
                GrassRoof,
                HedgeFence,
            };
        }
    }
}
