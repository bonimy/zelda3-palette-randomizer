// <copyright file="LightWorldAndMap.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Zelda3.Palette
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Maseya.Helper.Collections;

    public class LightWorldAndMap
    {
        public LightWorldAndMap(LightWorld lightWorld, OverworldMap lightWorldMap)
        {
            if (lightWorld is null)
            {
                throw new ArgumentNullException(nameof(lightWorld));
            }

            if (lightWorldMap is null)
            {
                throw new ArgumentNullException(nameof(lightWorldMap));
            }

            GrassShrubsAndTrees = AddCollectionsAsConcatenation(
                lightWorld.GrassShrubsAndTrees,
                lightWorldMap.Grass);

            FlowersAndRocks = AddCollectionsAsConcatenation(
                lightWorld.FlowersAndRocks,
                lightWorldMap.Flowers);

            Water = AddCollectionsAsConcatenation(
                lightWorld.Water,
                lightWorldMap.Water);

            HillsAndDirt = AddCollectionsAsConcatenation(
                lightWorld.HillsAndDirt,
                lightWorldMap.HillsAndDirt,
                lightWorldMap.TreeWood);

            Clouds = AddCollectionsAsConcatenation(
                lightWorld.DeathMountain.Clouds,
                lightWorldMap.Clouds);

            HyruleCastleWalls = AddCollectionsAsConcatenation(
                lightWorld.HyruleCastle.Walls,
                lightWorldMap.HyruleCastle);

            Sanctuary = AddCollectionsAsConcatenation(
                lightWorld.Sanctuary.Wall,
                lightWorldMap.Sanctuary);

            Houses = AddCollectionsAsConcatenation(
                lightWorld.Houses,
                lightWorldMap.Houses);

            DeathMountain = AddCollectionsAsConcatenation(
                lightWorld.DeathMountain.WallsAndAbyss,
                lightWorldMap.DeathMountain);

            TowerOfHera = AddCollectionsAsConcatenation(
                lightWorld.DeathMountain.HeraBricks,
                lightWorldMap.TowerOfHera);

            AllIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    GrassShrubsAndTrees,
                    FlowersAndRocks,
                    Water,
                    HillsAndDirt,
                    Clouds,
                    HyruleCastleWalls,
                    Sanctuary,
                    Houses,
                    DeathMountain,
                    TowerOfHera,
                });
        }

        public IndexCollection GrassShrubsAndTrees
        {
            get;
        }

        public IndexCollection FlowersAndRocks
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

        public IndexCollection Clouds
        {
            get;
        }

        public IndexCollection HyruleCastleWalls
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

        public ReadOnlyCollection<IndexCollection> AllIndexCollections
        {
            get;
        }

        private static ListIndexCollection AddCollectionsAsConcatenation(
            params IEnumerable<int>[] indexCollections)
        {
            var result = Enumerable.Empty<int>();
            foreach (var indexCollection in indexCollections)
            {
                result = result.Concat(indexCollection);
            }

            return new ListIndexCollection(result);
        }
    }
}
