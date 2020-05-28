// <copyright file="PaletteJsonFormatter.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using Helper;
    using Maseya.Snes;
    using Maseya.Zelda3.Palette;
    using static Maseya.Helper.StringHelper;

    internal static class PaletteJsonFormatter
    {
        private static readonly Regex OffsetFormat = new Regex(
            @"(\[(-?[0-9]+,?)+],?)",
            RegexOptions.Compiled);

        private static readonly Regex RgbFormat = new Regex(
            @"(\[[0-9,\.]+],?)",
            RegexOptions.Compiled);

        public static string CreateJson(byte[] original, byte[] modifed)
        {
            var result = GetDiff(original, modifed);
            return JsonSerializer.Serialize(
                result,
                new JsonSerializerOptions() { WriteIndented = true });
        }

        public static string CreateJson(
            PaletteOffsetCollections collections,
            Options options)
        {
            var offsets = new List<int[]>();
            foreach (var array in collections.GetCollections(options))
            {
                offsets.Add(array.ToArray());
            }

            var result = JsonSerializer.Serialize(offsets);
            var sb = new StringBuilder();
            sb.Append('[');
            sb.Append(Environment.NewLine);
            foreach (var match in OffsetFormat.Matches(result))
            {
                sb.Append("  ");
                sb.Append(match);
                sb.Append(Environment.NewLine);
            }

            sb.Append(']');
            return sb.ToString();
        }

        public static string CreateJson(List<float[]> colors)
        {
            var result = JsonSerializer.Serialize(
                colors,
                new JsonSerializerOptions() { WriteIndented = false });

            var sb = new StringBuilder();
            sb.Append('[');
            sb.Append(Environment.NewLine);
            foreach (var match in RgbFormat.Matches(result))
            {
                sb.Append("  ");
                sb.Append(match);
                sb.Append(Environment.NewLine);
            }

            sb.Append(']');
            return sb.ToString();
        }

        public static string CreateJson(IEnumerable<PaletteEditor> collections)
        {
            var offsets = new Dictionary<string, Dictionary<string, int[]>>()
            {
                { "oam", new Dictionary<string, int[]>() },
                { "raw", new Dictionary<string, int[]>() },
            };
            foreach (var editor in collections)
            {
                foreach (var kvp in editor.Items)
                {
                    var index = kvp.Key;
                    var color = (SnesColor)kvp.Value;
                    if (index >= 0)
                    {
                        offsets["raw"][GetString(index)] = new int[]
                        {
                            color.Low,
                            color.High
                        };
                    }
                    else
                    {
                        offsets["oam"][GetString(-index)] = new int[]
                        {
                            color.Red | 0x20,
                            color.Green | 0x40,
                            0,
                            color.Green | 0x40,
                            color.Blue | 0x80
                        };
                    }
                }
            }

            return JsonSerializer.Serialize(
                offsets,
                new JsonSerializerOptions() { WriteIndented = false });
        }

        private static List<Dictionary<string, List<int>>> GetDiff(
            byte[] original,
            byte[] modified)
        {
            var result = new List<Dictionary<string, List<int>>>();
            var relax = new Queue<int>();
            for (var i = 0; i < original.Length; i++)
            {
                if (original[i] == modified[i])
                {
                    continue;
                }

                var offset = i;
                var changes = new List<int>() { modified[i] };
                while (++i < original.Length)
                {
                    if (original[i] != modified[i])
                    {
                        changes.AddRange(relax);
                        relax.Clear();
                        changes.Add(modified[i]);
                    }
                    else if (relax.Count < 4)
                    {
                        // We use the relax queue to add changes even across
                        // small regions that have matching bytes. This
                        // produces less entries and gives us a smaller final
                        // file size.
                        relax.Enqueue(modified[i]);
                    }
                    else
                    {
                        relax.Clear();
                        break;
                    }
                }

                var entry = new Dictionary<string, List<int>>()
                {
                    { StringHelper.GetString(offset), changes },
                };
                result.Add(entry);
            }

            return result;
        }
    }
}
