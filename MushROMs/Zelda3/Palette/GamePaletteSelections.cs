// <copyright file="GamePaletteSelections.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using System;
    using Maseya.Helper.Collections;
    using Maseya.Snes;

    public class GamePaletteSelections : SelectionCollection
    {
        public GamePaletteSelections()
        {
            DefaultBGColor = new SingleItemListSelection[]
            {
                new SingleItemListSelection(0x5FEA9),
                new SingleItemListSelection(0x5FEB3),
            };

            Sword = LinearPaletteSelections(0xDD630, 4, 3);
            Shield = LinearPaletteSelections(0xDD648, 3, 4);
            Clothes = LinearPaletteSelections(0xDD308, 5, 15);
            LightWorldSprites = LinearPaletteSelections(0xDD218, 4, 15);
            DarkWorldSprites = LinearPaletteSelections(0xDD290, 4, 15);
            HUD = LinearPaletteSelections(0xDD660, 2 * 8, 4);
            PalaceMapSprite = LinearPaletteSelections(0xDD70A, 3, 7);
            DungeonMainBG = LinearPaletteSelections(0xDD734, 20 * 6, 15);
            OverworldAuxiliary3 = LinearPaletteSelections(0xDE604, 14, 7);
            MainOverworldArea = LinearPaletteSelections(0xDE6C8, 6 * 5, 7);
            OverworldAuxiliary = LinearPaletteSelections(0xDE86C, 20 * 3, 7);
            Enemies1 = LinearPaletteSelections(0xDD4E0, 24, 7);
            Enemies2 = LinearPaletteSelections(0xDD39E, 12, 7);
            OtherSprites = LinearPaletteSelections(0xDD446, 11, 7);

            var builder = new PaletteSelectionBuilder(0xDE544, 16);
            builder.AddFullRows(0, 6);
            DungeonMap = builder.CreatePaletteSelection();

            Overworld = new Overworld(new PaletteSelectionBuilder(0xDE6C8, 7));
            OWMap = new OWMap(new PaletteSelectionBuilder(0x55B39, 16));
        }

        public Overworld Overworld
        {
            get;
        }

        public OWMap OWMap
        {
            get;
        }

        public IListSelection[] Enemies1
        {
            get;
        }

        public IListSelection[] Enemies2
        {
            get;
        }

        public IListSelection DungeonMap
        {
            get;
        }

        public SingleItemListSelection[] DefaultBGColor
        {
            get;
        }

        public IListSelection[] LightWorldSprites
        {
            get;
        }

        public IListSelection[] DarkWorldSprites
        {
            get;
        }

        public IListSelection[] Clothes
        {
            get;
        }

        public IListSelection[] OtherSprites
        {
            get;
        }

        public IListSelection[] Sword
        {
            get;
        }

        public IListSelection[] Shield
        {
            get;
        }

        public IListSelection[] HUD
        {
            get;
        }

        public IListSelection[] PalaceMapSprite
        {
            get;
        }

        public IListSelection[] DungeonMainBG
        {
            get;
        }

        public IListSelection[] OverworldAuxiliary3
        {
            get;
        }

        public IListSelection[] MainOverworldArea
        {
            get;
        }

        public IListSelection[] OverworldAuxiliary
        {
            get;
        }

        public override IListSelection[] AllSelections()
        {
            throw new NotImplementedException();
        }

        private IListSelection[] LinearPaletteSelections(
            int startOffset,
            int rows,
            int rowSize)
        {
            var result = new IListSelection[rows];
            for (var i = 0; i < rows; i++)
            {
                var baseSelection = new LinearListSelection(
                    rowSize * i,
                    rowSize);

                result[i] = baseSelection.ToByteSelection(
                    startOffset,
                    PaletteConverter.Default);
            }

            return result;
        }
    }
}
