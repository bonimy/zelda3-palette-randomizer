// <copyright file="PaletteJsonFormatter.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya
{
    using System.Collections.Generic;
    using System.Text.Json;
    using Helper;

    internal static class PaletteJsonFormatter
    {
        public static string CreateJson(byte[] original, byte[] modifed)
        {
            var result = GetDiff(original, modifed);
            return JsonSerializer.Serialize(
                result,
                new JsonSerializerOptions() { WriteIndented = true });
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
