// <copyright file="Overworld.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using System;
    using System.Collections.Generic;
    using Maseya.Helper.Collections;

    public class Overworld : SelectionCollection
    {
        public Overworld(PaletteSelectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddFullRows(0, (6 * 5) + (20 * 3));
            FullOverworld = builder.CreatePaletteSelection();

            LightWorld = new LightWorld(builder);
            DarkWorld = new DarkWorld(builder);
            TriforceRoom = new TriforceRoom(builder);
        }

        public IListSelection FullOverworld
        {
            get;
        }

        public LightWorld LightWorld
        {
            get;
        }

        public DarkWorld DarkWorld
        {
            get;
        }

        public TriforceRoom TriforceRoom
        {
            get;
        }

        public override IListSelection[] AllSelections()
        {
            var result = new List<IListSelection>();
            result.AddRange(LightWorld);
            result.AddRange(DarkWorld);
            result.AddRange(TriforceRoom);
            return result.ToArray();
        }
    }
}
