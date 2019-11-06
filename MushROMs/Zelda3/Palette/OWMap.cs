// <copyright file="OWMap.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using System;
    using Maseya.Helper.Collections;

    public class OWMap : SelectionCollection
    {
        public OWMap(PaletteSelectionBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            World = new MapPaletteSelection(16);
            builder.AddFullRows(0, 8);
            World.LightWorld = builder.CreatePaletteSelection();
            World.CloneDarkWorld(8);

            ForestTrees = new MapPaletteSelection(16);
            builder.AddRow(0, 2, 2);
            builder.AddRow(6, 2, 4);
            ForestTrees.LightWorld = builder.CreatePaletteSelection();
            ForestTrees.CloneDarkWorld(8);

            DeathMountain = new MapPaletteSelection(16);
            builder.AddRow(0, 1, 1);
            builder.AddRow(0, 4, 1);
            builder.AddRow(1, 8, 1);
            builder.AddRow(3, 2, 4);
            DeathMountain.LightWorld = builder.CreatePaletteSelection();
            DeathMountain.CloneDarkWorld(8);

            HillAndDirt = new MapPaletteSelection(16);
            builder.AddRow(0, 6, 2);
            builder.AddRow(3, 8, 2);
            builder.AddRow(4, 1, 1);
            builder.AddRow(4, 4, 5);
            builder.AddRow(5, 6, 3);
            HillAndDirt.LightWorld = builder.CreatePaletteSelection();
            HillAndDirt.CloneDarkWorld(8);

            Clouds = new MapPaletteSelection(16);
            builder.AddRow(0, 8, 8);
            builder.AddRow(1, 4, 1);
            Clouds.LightWorld = builder.CreatePaletteSelection();
            Clouds.CloneDarkWorld(8);

            Hera = new MapPaletteSelection(16);
            builder.AddRow(1, 1, 4);
            Hera.LightWorld = builder.CreatePaletteSelection();
            Hera.CloneDarkWorld(8);

            IcePalace = new MapPaletteSelection(16);
            builder.AddRow(1, 9, 7);
            IcePalace.LightWorld = builder.CreatePaletteSelection();
            IcePalace.CloneDarkWorld(8);

            HyruleCastle = new MapPaletteSelection(16);
            builder.AddRow(2, 12, 4);
            HyruleCastle.LightWorld = builder.CreatePaletteSelection();
            HyruleCastle.CloneDarkWorld(8);

            Water = new MapPaletteSelection(16);
            builder.AddRow(2, 6, 2);
            builder.AddRow(2, 10, 1);
            builder.AddRow(3, 12, 2);
            builder.AddColumn(4, 9, 3);
            builder.AddRow(4, 10, 1);
            builder.AddRow(6, 9, 1);
            Water.LightWorld = builder.CreatePaletteSelection();
            Water.CloneDarkWorld(8);

            Grass = new MapPaletteSelection(16);
            builder.AddRow(2, 1, 1);
            builder.AddRow(2, 4, 2);
            builder.AddRow(2, 8, 1);
            builder.AddColumn(2, 11, 6);
            builder.AddRow(3, 6, 2);
            builder.AddRow(3, 10, 1);
            builder.AddRow(4, 2, 2);
            builder.AddRow(4, 15, 1);
            builder.AddRow(5, 2, 4);
            builder.AddRow(5, 10, 2);
            builder.AddRow(6, 1, 1);
            Grass.LightWorld = builder.CreatePaletteSelection();
            Grass.CloneDarkWorld(8);

            Sanctuary = new MapPaletteSelection(16);
            builder.AddRow(3, 1, 1);
            builder.AddRow(3, 14, 2);
            Sanctuary.LightWorld = builder.CreatePaletteSelection();
            Sanctuary.CloneDarkWorld(8);

            BridgeWalkways = new MapPaletteSelection(16);
            builder.AddRow(4, 12, 2);
            BridgeWalkways.LightWorld = builder.CreatePaletteSelection();
            BridgeWalkways.CloneDarkWorld(8);

            Houses = new MapPaletteSelection(16);
            builder.AddRow(5, 1, 1);
            builder.AddRow(5, 12, 4);
            Houses.LightWorld = builder.CreatePaletteSelection();
            Houses.CloneDarkWorld(8);

            TreeWood = new MapPaletteSelection(16);
            builder.AddRow(6, 6, 3);
            TreeWood.LightWorld = builder.CreatePaletteSelection();
            TreeWood.CloneDarkWorld(8);

            WoodBridges = new MapPaletteSelection(16);
            builder.AddRow(6, 12, 4);
            WoodBridges.LightWorld = builder.CreatePaletteSelection();
            WoodBridges.CloneDarkWorld(8);
        }

        public MapPaletteSelection World
        {
            get;
        }

        public MapPaletteSelection Water
        {
            get;
        }

        public MapPaletteSelection Grass
        {
            get;
        }

        public MapPaletteSelection HillAndDirt
        {
            get;
        }

        public MapPaletteSelection Clouds
        {
            get;
        }

        public MapPaletteSelection ForestTrees
        {
            get;
        }

        public MapPaletteSelection TreeWood
        {
            get;
        }

        public MapPaletteSelection HyruleCastle
        {
            get;
        }

        public MapPaletteSelection Sanctuary
        {
            get;
        }

        public MapPaletteSelection BridgeWalkways
        {
            get;
        }

        public MapPaletteSelection Houses
        {
            get;
        }

        public MapPaletteSelection WoodBridges
        {
            get;
        }

        public MapPaletteSelection DeathMountain
        {
            get;
        }

        public MapPaletteSelection Hera
        {
            get;
        }

        public MapPaletteSelection IcePalace
        {
            get;
        }

        public override IListSelection[] AllSelections()
        {
            return new IListSelection[]
            {
                Water.LightWorld,
                Water.DarkWorld,
                Grass.LightWorld,
                Grass.DarkWorld,
                HillAndDirt.LightWorld,
                HillAndDirt.DarkWorld,
                Clouds.LightWorld,
                Clouds.DarkWorld,
                ForestTrees.LightWorld,
                ForestTrees.DarkWorld,
                TreeWood.LightWorld,
                TreeWood.DarkWorld,
                HyruleCastle.LightWorld,
                HyruleCastle.DarkWorld,
                Sanctuary.LightWorld,
                Sanctuary.DarkWorld,
                BridgeWalkways.LightWorld,
                BridgeWalkways.DarkWorld,
                Houses.LightWorld,
                Houses.DarkWorld,
                WoodBridges.LightWorld,
                WoodBridges.DarkWorld,
                DeathMountain.LightWorld,
                DeathMountain.DarkWorld,
                Hera.LightWorld,
                Hera.DarkWorld,
                IcePalace.LightWorld,
                IcePalace.DarkWorld,
            };
        }
    }
}
