// <copyright file="TriforceRoom.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using System;
    using Maseya.Helper.Collections;

    public class TriforceRoom : SelectionCollection
    {
        public TriforceRoom(PaletteSelectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(20, 0, 7);
            Unknown = builder.CreatePaletteSelection();

            builder.AddRow(21, 0, 7);
            Curtain = builder.CreatePaletteSelection();

            builder.AddRow(22, 0, 4);
            RoomFloor = builder.CreatePaletteSelection();

            builder.AddRow(22, 4, 3);
            CarpetFloor = builder.CreatePaletteSelection();

            builder.AddRow(23, 0, 7);
            TranslucentCurtain = builder.CreatePaletteSelection();

            builder.AddRow(24, 0, 5);
            Casket = builder.CreatePaletteSelection();

            builder.AddRow(24, 5, 2);
            CarpetSteps = builder.CreatePaletteSelection();
        }

        public IListSelection CarpetFloor
        {
            get;
        }

        public IListSelection RoomFloor
        {
            get;
        }

        public IListSelection Curtain
        {
            get;
        }

        public IListSelection CarpetSteps
        {
            get;
        }

        public IListSelection TranslucentCurtain
        {
            get;
        }

        public IListSelection Casket
        {
            get;
        }

        public IListSelection Unknown
        {
            get;
        }

        public override IListSelection[] AllSelections()
        {
            return new IListSelection[]
            {
                CarpetFloor,
                RoomFloor,
                Curtain,
                CarpetSteps,
                TranslucentCurtain,
                Casket,
                Unknown,
            };
        }
    }
}
