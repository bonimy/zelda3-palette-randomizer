// <copyright file="Kakariko.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using System;
    using Maseya.Helper.Collections;

    public class Kakariko : SelectionCollection
    {
        public Kakariko(PaletteSelectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(54, 1, 3);
            Steps1 = builder.CreatePaletteSelection();

            builder.AddRow(54, 5, 2);
            Steps2 = builder.CreatePaletteSelection();

            builder.AddRow(55, 5, 2);
            PinkBush = builder.CreatePaletteSelection();

            builder.AddRow(56, 4, 2);
            BlueRoof = builder.CreatePaletteSelection();
        }

        public IListSelection Steps1
        {
            get;
        }

        public IListSelection Steps2
        {
            get;
        }

        public IListSelection PinkBush
        {
            get;
        }

        public IListSelection BlueRoof
        {
            get;
        }

        public override IListSelection[] AllSelections()
        {
            return new IListSelection[]
            {
                Steps1,
                Steps2,
                PinkBush,
                BlueRoof,
            };
        }
    }
}
