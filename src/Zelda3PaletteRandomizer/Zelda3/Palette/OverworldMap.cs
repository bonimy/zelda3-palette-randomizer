// <copyright file="OverworldMap.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Zelda3.Palette
{
    using System;
    using Maseya.Helper.Collections;

    public class OverworldMap
    {
        public OverworldMap(PaletteIndexCollectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddFullRows(0, 8);
            Everything = builder.Flush();

            builder.AddRow(1, 9, 1);
            builder.AddRow(2, 6, 2);
            builder.AddRow(2, 10, 1);
            builder.AddRow(3, 12, 2);
            builder.AddColumn(4, 9, 3);
            builder.AddRow(4, 10, 1);
            Water = builder.Flush();

            builder.AddRow(0, 6, 2);
            builder.AddRow(2, 2, 1);
            builder.AddRow(3, 8, 2);
            builder.AddRow(4, 1, 1);
            builder.AddRow(4, 4, 5);
            builder.AddRow(5, 6, 3);
            builder.AddRow(6, 12, 4);
            HillsAndDirt = builder.Flush();

            builder.AddRow(0, 2, 2);
            builder.AddRow(6, 2, 4);
            builder.AddRow(2, 1, 1);
            builder.AddRow(2, 4, 2);
            builder.AddRow(2, 8, 1);
            builder.AddColumn(2, 11, 6);
            builder.AddRow(3, 6, 2);
            builder.AddRow(3, 10, 1);
            builder.AddRow(4, 2, 2);
            builder.AddRow(4, 15, 1);
            builder.AddRow(5, 2, 3);
            builder.AddRow(5, 10, 1);
            builder.AddRow(6, 1, 1);
            Grass = builder.Flush();

            builder.AddRow(4, 12, 3);
            builder.AddRow(5, 5, 1);
            Flowers = builder.Flush();

            builder.AddRow(6, 6, 3);
            TreeWood = builder.Flush();

            builder.AddRow(0, 8, 8);
            builder.AddRow(1, 4, 1);
            Clouds = builder.Flush();

            builder.AddRow(2, 12, 4);
            HyruleCastle = builder.Flush();

            builder.AddRow(3, 1, 1);
            builder.AddRow(3, 14, 2);
            Sanctuary = builder.Flush();

            builder.AddRow(5, 1, 1);
            builder.AddRow(5, 12, 4);
            Houses = builder.Flush();

            builder.AddRow(0, 1, 1);
            builder.AddRow(0, 4, 1);
            builder.AddRow(1, 8, 1);
            builder.AddRow(3, 2, 4);
            DeathMountain = builder.Flush();

            builder.AddRow(1, 1, 1);
            builder.AddRow(1, 5, 3);
            TowerOfHera = builder.Flush();

            builder.AddRow(1, 10, 6);
            IcePalace = builder.Flush();
        }

        public IndexCollection BridgeWalkways
        {
            get;
        }

        public IndexCollection Everything
        {
            get;
        }

        public IndexCollection Water
        {
            get;
        }

        public IndexCollection HillsAndDirt
        {
            get;
        }

        public IndexCollection Grass
        {
            get;
        }

        public IndexCollection Flowers
        {
            get;
        }

        public IndexCollection TreeWood
        {
            get;
        }

        public IndexCollection Clouds
        {
            get;
        }

        public IndexCollection HyruleCastle
        {
            get;
        }

        public IndexCollection Sanctuary
        {
            get;
        }

        public IndexCollection Houses
        {
            get;
        }

        public IndexCollection DeathMountain
        {
            get;
        }

        public IndexCollection TowerOfHera
        {
            get;
        }

        public IndexCollection IcePalace
        {
            get;
        }
    }
}
