// <copyright file="TriforceRoom.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Zelda3.Palette
{
    using System;
    using System.Collections.ObjectModel;
    using Maseya.Helper.Collections;

    public class TriforceRoom
    {
        public TriforceRoom(PaletteIndexCollectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(20, 0, 7);
            Unknown = builder.Flush();

            builder.AddRow(21, 0, 7);
            Curtain = builder.Flush();

            builder.AddRow(22, 0, 4);
            RoomFloor = builder.Flush();

            builder.AddRow(22, 4, 3);
            CarpetFloor = builder.Flush();

            builder.AddRow(23, 0, 7);
            TranslucentCurtain = builder.Flush();

            builder.AddRow(24, 0, 5);
            Casket = builder.Flush();

            builder.AddRow(24, 5, 2);
            CarpetSteps = builder.Flush();

            AllIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    CarpetFloor,
                    RoomFloor,
                    Curtain,
                    CarpetSteps,
                    TranslucentCurtain,
                    Casket,
                    Unknown,
                });
        }

        public IndexCollection CarpetFloor
        {
            get;
        }

        public IndexCollection RoomFloor
        {
            get;
        }

        public IndexCollection Curtain
        {
            get;
        }

        public IndexCollection CarpetSteps
        {
            get;
        }

        public IndexCollection TranslucentCurtain
        {
            get;
        }

        public IndexCollection Casket
        {
            get;
        }

        public IndexCollection Unknown
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> AllIndexCollections
        {
            get;
        }
    }
}
