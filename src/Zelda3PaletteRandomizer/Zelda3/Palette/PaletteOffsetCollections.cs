// <copyright file="PaletteOffsetCollections.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Zelda3.Palette
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Maseya.Helper.Collections;

    public class PaletteOffsetCollections
    {
        public PaletteOffsetCollections()
        {
            ExtraPalettes = new ExtraPalettes();

            var objectBuilder = new PaletteIndexCollectionBuilder(0xDE6C8, 7);
            var spriteBuilder = new PaletteIndexCollectionBuilder(0xDD446, 7);
            LightWorld = new LightWorld(
                objectBuilder,
                spriteBuilder,
                new PaletteIndexCollectionBuilder(0xDD218, 15));
            DarkWorld = new DarkWorld(
                objectBuilder,
                spriteBuilder,
                new PaletteIndexCollectionBuilder(0xDD290, 15));
            TriforceRoom = new TriforceRoom(objectBuilder);

            LightWorldMap = new OverworldMap(
                new PaletteIndexCollectionBuilder(0x55B39, 16));
            DarkWorldMap = new OverworldMap(
                new PaletteIndexCollectionBuilder(0x55C39, 16));

            LightWorldAndMap = new LightWorldAndMap(LightWorld, LightWorldMap);
            DarkWorldAndMap = new DarkWorldAndMap(DarkWorld, DarkWorldMap);
        }

        public LightWorld LightWorld
        {
            get;
        }

        public DarkWorld DarkWorld
        {
            get;
        }

        public TriforceRoom TriforceRoom
        {
            get;
        }

        public OverworldMap LightWorldMap
        {
            get;
        }

        public OverworldMap DarkWorldMap
        {
            get;
        }

        public LightWorldAndMap LightWorldAndMap
        {
            get;
        }

        public DarkWorldAndMap DarkWorldAndMap
        {
            get;
        }

        public ExtraPalettes ExtraPalettes
        {
            get;
        }

        public IndexCollection[] GetCollections(Options options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var result = new UniqueList();

            if (options.RandomizeDungeon)
            {
                result.AddRange(TriforceRoom.AllIndexCollections);
                result.AddRange(ExtraPalettes.DungeonMainBG);
            }

            result.AddRange(ExtraPalettes.GetOverlayPalettes(
                options.RandomizeDungeon,
                options.RandomizeDungeon,
                options.RandomizeHudPalettes));
            result.AddRange(ExtraPalettes.GetPlayerPalettes(
                options.RandomizeLinkSpritePalette,
                options.RandomizeSwordPalette,
                options.RandomizeShieldPalette));

            if (options.RandomizeOverworld)
            {
                result.AddRange(LightWorldAndMap.AllIndexCollections);
                result.AddRange(DarkWorldAndMap.AllIndexCollections);

                result.AddRange(LightWorld.MiscIndexCollections);
                result.AddRange(LightWorld.HyruleCastle.MiscIndexCollections);
                result.AddRange(LightWorld.Sanctuary.MiscIndexCollections);
                result.AddRange(LightWorld.Kakariko.AllIndexCollections);
                result.AddRange(LightWorld.DeathMountain.MiscIndexCollections);
                result.AddRange(DarkWorld.MiscIndexCollections);
                result.AddRange(DarkWorld.PalaceOfDarkness.AllIndexCollections);
                result.AddRange(DarkWorld.SkullWoods.AllIndexCollections);
                result.AddRange(DarkWorld.ThievesTown.AllIndexCollections);
                result.AddRange(DarkWorld.Swamp.AllIndexCollections);
                result.AddRange(DarkWorld.DarkMountain.MiscIndexCollections);
            }

            if (options.RandomizeSpritePalettes ||
                options.RandomizeAdvancedSpritePalettes)
            {
                Console.Error.WriteLine(
                    "Randomizing sprite palettes is currently not supported.");
            }

            return result.ToArray();
        }

        private class UniqueList : Collection<IndexCollection>
        {
            public UniqueList()
                : base()
            {
                Hash = new HashSet<int>();
            }

            private HashSet<int> Hash
            {
                get;
            }

            public void AddRange(IEnumerable<IndexCollection> items)
            {
                foreach (var item in items)
                {
                    Add(item);
                }
            }

            public IndexCollection[] ToArray()
            {
                var result = new IndexCollection[Count];
                CopyTo(result, 0);
                return result;
            }

            protected override void InsertItem(int index, IndexCollection item)
            {
                AddCollection(item);
                base.InsertItem(index, item);
            }

            protected override void RemoveItem(int index)
            {
                ClearCollection(index);
                base.RemoveItem(index);
            }

            protected override void ClearItems()
            {
                Hash.Clear();
                base.ClearItems();
            }

            protected override void SetItem(int index, IndexCollection item)
            {
                ClearCollection(index);
                AddCollection(item);
                base.SetItem(index, item);
            }

            private void AddCollection(IndexCollection item)
            {
                foreach (var x in item)
                {
                    if (Hash.Contains(x))
                    {
                        throw new InvalidOperationException();
                    }

                    Hash.Add(x);
                }
            }

            private void ClearCollection(int index)
            {
                foreach (var x in Items[index])
                {
                    Hash.Remove(x);
                }
            }
        }
    }
}
