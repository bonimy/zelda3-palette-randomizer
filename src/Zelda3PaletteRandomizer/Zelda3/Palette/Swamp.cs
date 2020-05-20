// <copyright file="Swamp.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Zelda3.Palette
{
    using System;
    using System.Collections.ObjectModel;
    using Maseya.Helper.Collections;

    public class Swamp
    {
        public Swamp(PaletteIndexCollectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddRow(70, 2, 3);
            VinesPrimary = builder.Flush();

            builder.AddRow(70, 5, 2);
            VinesHighlight = builder.Flush();

            AllIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    VinesPrimary,
                    VinesHighlight,
                });
        }

        public IndexCollection VinesPrimary
        {
            get;
        }

        public IndexCollection VinesHighlight
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> AllIndexCollections
        {
            get;
        }
    }
}
