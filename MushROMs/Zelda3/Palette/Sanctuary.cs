// <copyright file="Sanctuary.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using System;
    using Maseya.Helper.Collections;

    public class Sanctuary : SelectionCollection
    {
        public Sanctuary(PaletteSelectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(33, 1, 3);
            Wall = builder.CreatePaletteSelection();

            builder.AddRow(33, 4, 1);
            Window = builder.CreatePaletteSelection();

            builder.AddRow(33, 5, 2);
            Roof = builder.CreatePaletteSelection();
        }

        public IListSelection Wall
        {
            get;
        }

        public IListSelection Roof
        {
            get;
        }

        public IListSelection Window
        {
            get;
        }

        public override IListSelection[] AllSelections()
        {
            return new IListSelection[]
            {
                Roof,
                Window,
            };
        }
    }
}
