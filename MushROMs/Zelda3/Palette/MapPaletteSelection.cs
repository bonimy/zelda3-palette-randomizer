// <copyright file="MapPaletteSelection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs.Zelda3.Palette
{
    using Maseya.Helper.Collections;
    using Maseya.Snes;

    public class MapPaletteSelection
    {
        public MapPaletteSelection(int rowSize)
        {
            RowSize = rowSize;
        }

        public MapPaletteSelection(
            int rowSize,
            IListSelection lightWorld,
            IListSelection darkWorld)
            : this(rowSize)
        {
            LightWorld = lightWorld;
            DarkWorld = darkWorld;
        }

        public int RowSize
        {
            get;
        }

        public IListSelection LightWorld
        {
            get;
            set;
        }

        public IListSelection DarkWorld
        {
            get;
            set;
        }

        public void CloneDarkWorld(int rowOffset)
        {
            var sizeOfT = PaletteConverter.Default.SizeOfItem;
            DarkWorld = LightWorld.Move(rowOffset * RowSize * sizeOfT);
        }
    }
}
