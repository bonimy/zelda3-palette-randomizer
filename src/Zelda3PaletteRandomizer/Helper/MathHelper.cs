// <copyright file="MathHelper.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System;

    /// <summary>
    /// Provides static methods and constants for simple math functions not
    /// provided in <see cref="Math"/> or extending them.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Represents the default minimum threshold where <see cref="
        /// NearlyEquals(Single, Single, Single)"/> would return <see
        /// langword="false"/>.
        /// </summary>
        public const float DefaultTolerance = 1e-7f;

        /// <summary>
        /// Returns an equality comparison of two variables within some
        /// threshold.
        /// </summary>
        public static bool NearlyEquals(
            float left,
            float right,
            float tolerance = DefaultTolerance)
        {
            return Math.Abs(left - right) <= tolerance;
        }

        /// <summary>
        /// Returns a less than or equal to comparison of two variables within
        /// some threshold.
        /// </summary>
        public static bool LessThanOrNearlyEqualTo(
            float left,
            float right,
            float tolerance = DefaultTolerance)
        {
            return left < right || NearlyEquals(left, right, tolerance);
        }

        /// <summary>
        /// Returns a greater than or equal to comparison of two variables
        /// within some threshold.
        /// </summary>
        public static bool GreaterThanOrNearlyEqualTo(
            float left,
            float right,
            float tolerance = DefaultTolerance)
        {
            return left > right || NearlyEquals(left, right, tolerance);
        }

        /// <summary>
        /// Snap a value to some limit if it is equal to that limit within
        /// some threshold.
        /// </summary>
        public static float SnapToLimit(
            float value,
            float limit,
            float tolerance = DefaultTolerance)
        {
            return NearlyEquals(value, limit, tolerance) ? limit : value;
        }

        /// <summary>
        /// Clamp a value to be within a min and max value.
        /// </summary>
        public static float Clamp(
            float value,
            float min,
            float max,
            float tolerance = DefaultTolerance)
        {
            // If max is less than min, then we have a null range.
            if (max < min)
            {
                return Single.NaN;
            }

            // Clamp left
            if (LessThanOrNearlyEqualTo(value, min, tolerance))
            {
                return min;
            }

            // Clamp right
            if (GreaterThanOrNearlyEqualTo(value, max, tolerance))
            {
                return max;
            }

            // Return original value
            return value;
        }

        /// <summary>
        /// Returns the largest value among a collection of single-precision
        /// floating-point values.
        /// </summary>
        public static float Max(
            float value1,
            float value2,
            params float[] values)
        {
            var max = Math.Max(value1, value2);
            if (values != null)
            {
                foreach (var value in values)
                {
                    max = Math.Max(max, value);
                }
            }

            return max;
        }

        /// <summary>
        /// Returns the smallest value among a collection of single-precision
        /// floating-point values.
        /// </summary>
        public static float Min(
            float value1,
            float value2,
            params float[] values)
        {
            var min = Math.Min(value1, value2);
            if (values != null)
            {
                foreach (var value in values)
                {
                    min = Math.Min(min, value);
                }
            }

            return min;
        }

        public static byte ReverseBits(byte value)
        {
            // Swap left 4 bits with right 4 bits.
            value = (byte)(((value & 0xF0) >> 4) | ((value & 0x0F) << 4));

            // Swap all adjacent pairs of bits.
            value = (byte)(((value & 0xCC) >> 2) | ((value & 0x33) << 2));

            // Swap all adjacent single bits.
            value = (byte)(((value & 0xAA) >> 1) | ((value & 0x55) << 1));
            return value;
        }

        public static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        public static float DegreesToRadians(float degrees)
        {
            return (float)DegreesToRadians((double)degrees);
        }

        public static double RadiansToDegrees(double radians)
        {
            return radians * 180.0 / Math.PI;
        }

        public static float RadiansToDegrees(float radians)
        {
            return (float)RadiansToDegrees((double)radians);
        }
    }
}
