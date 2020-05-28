// <copyright file="RandomColorFGenerator.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System;

    public class RandomColorFGenerator : IColorFGenerator
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

        public ColorF Next()
        {
            return ColorF.FromArgb(
                (float)Random.NextDouble(),
                (float)Random.NextDouble(),
                (float)Random.NextDouble());
        }
    }
}
