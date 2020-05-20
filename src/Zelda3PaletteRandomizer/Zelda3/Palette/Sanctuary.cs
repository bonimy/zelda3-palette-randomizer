// <copyright file="Sanctuary.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Zelda3.Palette
{
    using System;
    using System.Collections.ObjectModel;
    using Maseya.Helper.Collections;

    public class Sanctuary
    {
        public Sanctuary(PaletteIndexCollectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(33, 1, 3);
            Wall = builder.Flush();

            builder.AddRow(33, 4, 1);
            Window = builder.Flush();

            builder.AddRow(33, 5, 2);
            Roof = builder.Flush();

            AllIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    Wall,
                    Roof,
                    Window,
                });

            MiscIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    Roof,
                    Window,
                });
        }

        public IndexCollection Wall
        {
            get;
        }

        public IndexCollection Roof
        {
            get;
        }

        public IndexCollection Window
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> AllIndexCollections
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> MiscIndexCollections
        {
            get;
        }
    }
}
