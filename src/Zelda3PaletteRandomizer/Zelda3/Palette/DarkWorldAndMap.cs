// <copyright file="DarkWorldAndMap.cs" company="Public Domain">
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

    public class DarkWorldAndMap
    {
        public DarkWorldAndMap(DarkWorld darkWorld, OverworldMap darkWorldMap)
        {
            if (darkWorld is null)
            {
                throw new ArgumentNullException(nameof(darkWorld));
            }

            if (darkWorldMap is null)
            {
                throw new ArgumentNullException(nameof(darkWorldMap));
            }

            GrassShrubsAndTrees = AddCollectionsAsConcatenation(
                darkWorld.GrassShrubsAndTrees,
                darkWorldMap.Grass,
                darkWorldMap.Sanctuary);

            FlowersAndRocks = AddCollectionsAsConcatenation(
                darkWorld.FlowersAndRocks,
                darkWorldMap.Flowers);

            TreeWood = AddCollectionsAsConcatenation(
                darkWorld.TreeWood,
                darkWorldMap.TreeWood);

            Water = AddCollectionsAsConcatenation(
                darkWorld.Water,
                darkWorldMap.Water);

            HillsAndDirt = AddCollectionsAsConcatenation(
                darkWorld.HillsAndDirt,
                darkWorldMap.HillsAndDirt);

            Clouds = AddCollectionsAsConcatenation(
                darkWorld.DarkMountain.BlackClouds,
                darkWorldMap.Clouds);

            HyruleCastleWalls = AddCollectionsAsConcatenation(
                darkWorld.Pyramid,
                darkWorldMap.HyruleCastle);

            Houses = AddCollectionsAsConcatenation(
                darkWorld.Houses,
                darkWorldMap.Houses);

            DeathMountain = AddCollectionsAsConcatenation(
                darkWorld.DarkMountain.WallsAndAbyss,
                darkWorldMap.DeathMountain);

            GanonsTower = AddCollectionsAsConcatenation(
                darkWorld.DarkMountain.GanonsTowerPrimary,
                darkWorldMap.TowerOfHera);

            IcePalace = AddCollectionsAsConcatenation(
                darkWorld.IcePalaceEntrance,
                darkWorldMap.IcePalace);

            AllIndexCollections = new ReadOnlyCollection<IndexCollection>(
                new IndexCollection[]
                {
                    GrassShrubsAndTrees,
                    FlowersAndRocks,
                    TreeWood,
                    Water,
                    HillsAndDirt,
                    Clouds,
                    HyruleCastleWalls,
                    Houses,
                    DeathMountain,
                    GanonsTower,
                    IcePalace,
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

        public IndexCollection TreeWood
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

        public IndexCollection Houses
        {
            get;
        }

        public IndexCollection DeathMountain
        {
            get;
        }

        public IndexCollection GanonsTower
        {
            get;
        }

        public IndexCollection IcePalace
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
