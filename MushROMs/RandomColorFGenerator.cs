// <copyright file="RandomColorFGenerator.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs
{
    using System;
    using Maseya.Helper;

    public class RandomColorFGenerator
    {
        public RandomColorFGenerator()
        {
            Random = new Random();
        }

        public RandomColorFGenerator(int seed)
        {
            Random = new Random(seed);
        }

        private Random Random
        {
            get;
        }

        public ColorF NextColorF()
        {
            return ColorF.FromArgb(
                (float)Random.NextDouble(),
                (float)Random.NextDouble(),
                (float)Random.NextDouble());
        }

        public ColorF NextColorF(
            Func<float, float> hueTransform,
            Func<float, float> chromaTransform,
            Func<float, float> lumaTransform)
        {
            var hue = (float)Random.NextDouble();
            if (hueTransform != null)
            {
                hue = hueTransform(hue);
            }

            var chroma = (float)Random.NextDouble();
            if (chromaTransform != null)
            {
                chroma = chromaTransform(hue);
            }

            var luma = (float)Random.NextDouble();
            if (lumaTransform != null)
            {
                luma = lumaTransform(hue);
            }

            return ColorF.FromHcy(hue, chroma, luma);
        }
    }
}
