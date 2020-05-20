// <copyright file="ExtraPalettes.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Zelda3.Palette
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Maseya.Helper;
    using Maseya.Helper.Collections;
    using Maseya.Snes;

    public class ExtraPalettes
    {
        private static readonly ByteDataConverter<SnesColor> PaletteConverter
            = ByteDataConverter<SnesColor>.Default;

        public ExtraPalettes()
        {
            // TODO: Ice golems
            LightWorldSprites = ReadOnlyIndexCollectionPerRow(0xDD218, 4, 15);
            DarkWorldSprites = ReadOnlyIndexCollectionPerRow(0xDD290, 4, 15);
            Clothes = ReadOnlyIndexCollectionPerRow(0xDD308, 5, 15);
            Enemies2 = ReadOnlyIndexCollectionPerRow(0xDD39E, 12, 7);
            OtherSprites = ReadOnlyIndexCollectionPerRow(0xDD446, 11, 7);
            var enemies1 = new List<IndexCollection>(
                IndexCollectionPerRow(0xDD4E0, 24, 7));
            Sword = ReadOnlyIndexCollectionPerRow(0xDD630, 4, 3);
            Shield = ReadOnlyIndexCollectionPerRow(0xDD648, 3, 4);
            HUD = ReadOnlyIndexCollectionPerRow(0xDD660, 2 * 8, 4);
            PalaceMapSprite = ReadOnlyIndexCollectionPerRow(0xDD70A, 3, 7);

            // Mix Ice Golems with Ice Palace
            var dungeons = IndexCollectionPerRow(0xDD734, 20, 6 * 15);
            var icePalace = new List<int>(dungeons[4]);
            icePalace.AddRange(enemies1[12]);
            dungeons[4] = new ListIndexCollection(icePalace);
            enemies1[12] = IndexCollection.Empty;
            DungeonMainBG = new ReadOnlyCollection<IndexCollection>(dungeons);
            Enemies1 = new ReadOnlyCollection<IndexCollection>(enemies1);

            var builder = new PaletteIndexCollectionBuilder(0xDE544, 16);
            builder.AddFullRows(0, 6);
            DungeonMap = builder.Flush();

            OverworldAuxiliary3 = ReadOnlyIndexCollectionPerRow(0xDE604, 14, 7);
            MainOverworldArea = ReadOnlyIndexCollectionPerRow(0xDE6C8, 6 * 5, 7);
            OverworldAuxiliary = ReadOnlyIndexCollectionPerRow(0xDE86C, 20 * 3, 7);
        }

        public IndexCollection DungeonMap
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> Enemies1
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> Enemies2
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> LightWorldSprites
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> DarkWorldSprites
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> Clothes
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> OtherSprites
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> Sword
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> Shield
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> HUD
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> PalaceMapSprite
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> DungeonMainBG
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> OverworldAuxiliary3
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> MainOverworldArea
        {
            get;
        }

        public ReadOnlyCollection<IndexCollection> OverworldAuxiliary
        {
            get;
        }

        public IndexCollection[] GetSpritePalettes(
            bool sprites,
            bool advancedSprites)
        {
            var result = new List<IndexCollection>();
            if (sprites)
            {
                result.AddRange(Enemies1);
                result.AddRange(Enemies2);
                result.AddRange(OtherSprites);
            }

            if (advancedSprites)
            {
                result.AddRange(LightWorldSprites);
                result.AddRange(DarkWorldSprites);
            }

            return result.ToArray();
        }

        public IndexCollection[] GetPlayerPalettes(
            bool includeClothes,
            bool includeSword,
            bool includeShield)
        {
            var result = new List<IndexCollection>();
            if (includeClothes)
            {
                foreach (var indexCollection in Clothes)
                {
                    // Remove the last color, which is used for water.
                    result.Add(new IndexRangeCollection(
                        indexCollection.MinIndex,
                        indexCollection.Count - 1));
                }
            }

            if (includeSword)
            {
                result.AddRange(Sword);
            }

            if (includeShield)
            {
                result.AddRange(Shield);
            }

            return result.ToArray();
        }

        public IndexCollection[] GetOverlayPalettes(
            bool includeDungeonMap,
            bool includeDungeonMapSprites,
            bool includeHud)
        {
            var result = new List<IndexCollection>();
            if (includeDungeonMap)
            {
                result.Add(DungeonMap);
            }

            if (includeDungeonMapSprites)
            {
                result.AddRange(PalaceMapSprite);
            }

            if (includeHud)
            {
                result.AddRange(HUD);
            }

            return result.ToArray();
        }

        public IndexCollection[] GetWorldPalettes(
            bool includeOverworld,
            bool includeDungeon)
        {
            var result = new List<IndexCollection>();
            if (includeOverworld)
            {
                result.AddRange(MainOverworldArea);
                result.AddRange(OverworldAuxiliary);
                result.AddRange(OverworldAuxiliary3);
            }

            if (includeDungeon)
            {
                result.AddRange(DungeonMainBG);
            }

            return result.ToArray();
        }

        private static IndexCollection[] IndexCollectionPerRow(
            int startOffset,
            int rows,
            int rowSize)
        {
            var result = new ListIndexCollection[rows];
            for (var i = 0; i < rows; i++)
            {
                var baseSelection = new IndexRangeCollection(
                    rowSize * i,
                    rowSize);

                var en = PaletteConverter.GetStartOffsetAtEachIndex(
                    startOffset,
                    baseSelection);
                result[i] = new ListIndexCollection(en);
            }

            return result;
        }

        private static ReadOnlyCollection<IndexCollection>
            ReadOnlyIndexCollectionPerRow(int startOffset, int rows, int rowSize)
        {
            var result = new ListIndexCollection[rows];
            for (var i = 0; i < rows; i++)
            {
                var baseSelection = new IndexRangeCollection(
                    rowSize * i,
                    rowSize);

                var en = PaletteConverter.GetStartOffsetAtEachIndex(
                    startOffset,
                    baseSelection);
                result[i] = new ListIndexCollection(en);
            }

            return new ReadOnlyCollection<IndexCollection>(result);
        }
    }
}
