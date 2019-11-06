// <copyright file="MathHelper.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
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
        /// <param name="left">
        /// The first value to compare.
        /// </param>
        /// <param name="right">
        /// The second value to compare.
        /// </param>
        /// <param name="tolerance">
        /// The threshold to consider when comparing <paramref name=" left"/>
        /// to <paramref name="right"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref
        /// name="right"/> are equal within <paramref name=" tolerance"/>.
        /// </returns>
        /// <example>
        /// Floating point arithmetic can quickly lead to rounding errors where
        /// the last few digits are not the precise value the user could have
        /// been expecting. The example below demonstrates how simple, repeated
        /// additions can lead results that are not exactly equal with the
        /// expected value. However, using <see cref="NearlyEquals(Single,
        /// Single, Single)"/>, it can be determined whether the values are
        /// close to within a given range.
        /// <code language="csharp" title="Floating point errors" source="../../examples/Helper.Examples/MathHelper/NearlyEqualsDefault.cs"/>
        /// </example>
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
        /// <param name="left">
        /// The first value to compare.
        /// </param>
        /// <param name="right">
        /// The second value to compare.
        /// </param>
        /// <param name="tolerance">
        /// The threshold to consider when comparing <paramref name=" left"/>
        /// to <paramref name="right"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref
        /// name="right"/> are less than or equal to within <paramref
        /// name="tolerance"/>.
        /// </returns>
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
        /// <param name="left">
        /// The first value to compare.
        /// </param>
        /// <param name="right">
        /// The second value to compare.
        /// </param>
        /// <param name="tolerance">
        /// The threshold to consider when comparing <paramref name=" left"/>
        /// to <paramref name="right"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref
        /// name="right"/> are greater than or equal to within <paramref
        /// name="tolerance"/>.
        /// </returns>
        public static bool GreaterThanOrNearlyEqualTo(
            float left,
            float right,
            float tolerance = DefaultTolerance)
        {
            return left > right || NearlyEquals(left, right, tolerance);
        }

        /// <summary>
        /// Snap a value to some limit if it is equal to that limit within some
        /// threshold.
        /// </summary>
        /// <param name="value">
        /// The value to consider snapping to some limit.
        /// </param>
        /// <param name="limit">
        /// The limit to snap to.
        /// </param>
        /// <param name="tolerance">
        /// The threshold to consider when comparing <paramref name=" value"/>
        /// to <paramref name="limit"/>.
        /// </param>
        /// <returns>
        /// <paramref name="limit"/> if <paramref name="value"/> is equal to
        /// <paramref name="limit"/> within a threshold of <paramref
        /// name="tolerance"/>. Otherwise, <paramref name=" value"/>.
        /// </returns>
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
        /// <param name="value">
        /// The value to clamp.
        /// </param>
        /// <param name="min">
        /// The minimum boundary to keep <paramref name="value"/> in.
        /// </param>
        /// <param name="max">
        /// The maximum boundary to keep <paramref name="value"/> in.
        /// </param>
        /// <param name="tolerance">
        /// The threshold to consider when comparing <paramref name=" value"/>
        /// to <paramref name="min"/> and <paramref name=" max"/>.
        /// </param>
        /// <returns>
        /// <see cref="Single.NaN"/><paramref name="max"/> is less than
        /// <paramref name="min"/>. Otherwise <paramref name="min"/> if
        /// <paramref name="value"/> is less than or equal to <paramref
        /// name="min"/> within <paramref name="tolerance"/>. Otherwise
        /// <paramref name="max"/> if <paramref name="value"/> is greater than
        /// or equal to <paramref name="max"/> within <paramref
        /// name="tolerance"/>. Otherwise <paramref name=" value"/>.
        /// </returns>
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
        /// <param name="value1">
        /// The first value to compare.
        /// </param>
        /// <param name="value2">
        /// The second value to compare.
        /// </param>
        /// <param name="values">
        /// An optional collection of extra values to compare.
        /// </param>
        /// <returns>
        /// <paramref name="value1"/>, <paramref name="value2"/>, or one of the
        /// optional <paramref name="values"/> parameters, whichever is
        /// largest. If any of these values are equal to <see
        /// cref="Single.NaN"/>, then <see cref="Single.NaN"/> is returned.
        /// </returns>
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
        /// <param name="value1">
        /// The first value to compare.
        /// </param>
        /// <param name="value2">
        /// The second value to compare.
        /// </param>
        /// <param name="values">
        /// An optional collection of extra values to compare.
        /// </param>
        /// <returns>
        /// <paramref name="value1"/>, <paramref name="value2"/>, or one of the
        /// optional <paramref name="values"/> parameters, whichever is
        /// smallest. If any of these values are equal to <see
        /// cref="Single.NaN"/>, then <see cref="Single.NaN"/> is returned.
        /// </returns>
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
    }
}
