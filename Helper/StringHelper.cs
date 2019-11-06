// <copyright file="StringHelper.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Provides static methods for formatting strings while automatically
    /// specifying which <see cref="IFormatProvider"/> to use.
    /// </summary>
    /// <remarks>
    /// This class is designed to resolve the Visual Studio rule that format
    /// strings should specify <see cref="IFormatProvider"/> when calling <see
    /// cref="String.Format(String, Object[])"/> and <see
    /// cref="IFormattable.ToString(String, IFormatProvider)"/>.
    /// </remarks>
    public static class StringHelper
    {
        /// <summary>
        /// Gets <see cref="CultureInfo.CurrentCulture"/>.
        /// </summary>
        private static CultureInfo CurrentCulture
        {
            get
            {
                return CultureInfo.CurrentCulture;
            }
        }

        /// <summary>
        /// Gets <see cref="CultureInfo.CurrentUICulture"/>.
        /// </summary>
        private static CultureInfo CurrentUICulture
        {
            get
            {
                return CultureInfo.CurrentUICulture;
            }
        }

        /// <summary>
        /// Calls <see cref="String.Format(IFormatProvider, String,
        /// Object[])"/> using <see cref="CultureInfo.CurrentCulture"/> as the
        /// <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="format">
        /// A composite format string.
        /// </param>
        /// <param name="args">
        /// An array that contains zero or more objects to format.
        /// </param>
        /// <returns>
        /// A copy of <paramref name="format"/> in which the format items have
        /// been replaced by the string representation of the corresponding
        /// objects in <paramref name="args"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="format"/> or <paramref name="args"/> is <see
        /// langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="format"/> is invalid.
        /// <para/>
        /// <para/>
        /// -or-
        /// <para/>
        /// The index of a format item is less than zero, or greater than or
        /// equal to the length of the <paramref name="args"/> array.
        /// </exception>
        /// <seealso cref="String.Format(IFormatProvider, String, Object[])"/>
        public static string GetString(string format, params object[] args)
        {
            return String.Format(CurrentCulture, format, args);
        }

        /// <summary>
        /// Formats the value of an instance of <see cref="IFormattable"/>
        /// using <see cref="CultureInfo. CurrentCulture"/> as its <see
        /// cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="value">
        /// The instance to format.
        /// </param>
        /// <param name="format">
        /// The format to use.
        /// <para/>
        /// <para/>
        /// -or-
        /// <para/>
        /// A null reference (Nothing in Visual Basic) to use the default
        /// format defined for the type of the <see cref="IFormattable"/>
        /// implementation.
        /// </param>
        /// <returns>
        /// The string representation of <paramref name="value"/> in the
        /// specified format.
        /// </returns>
        /// <seealso cref="IFormattable.ToString(String, IFormatProvider)"/>
        public static string GetString(
            IFormattable value,
            string format = null)
        {
            return value?.ToString(format, CurrentCulture);
        }

        /// <summary>
        /// Calls <see cref="String.Format(IFormatProvider, String,
        /// Object[])"/> using <see cref="CultureInfo. CurrentUICulture"/> as
        /// the <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="format">
        /// A composite format string.
        /// </param>
        /// <param name="args">
        /// An array that contains zero or more objects to format.
        /// </param>
        /// <returns>
        /// A copy of <paramref name="format"/> in which the format items have
        /// been replaced by the string representation of the corresponding
        /// objects in <paramref name="args"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="format"/> or <paramref name="args"/> is <see
        /// langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="format"/> is invalid.
        /// <para/>
        /// <para/>
        /// -or-
        /// <para/>
        /// The index of a format item is less than zero, or greater than or
        /// equal to the length of the <paramref name="args"/> array.
        /// </exception>
        /// <seealso cref="String.Format(IFormatProvider, String, Object[])"/>
        public static string GetUIString(string format, params object[] args)
        {
            return String.Format(CurrentUICulture, format, args);
        }

        /// <summary>
        /// Formats the value of an instance of <see cref="IFormattable"/>
        /// using <see cref="CultureInfo. CurrentUICulture"/> as its <see
        /// cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="value">
        /// The instance to format.
        /// </param>
        /// <param name="format">
        /// The format to use.
        /// <para/>
        /// <para/>
        /// -or-
        /// <para/>
        /// A null reference (Nothing in Visual Basic) to use the default
        /// format defined for the type of the <see cref="IFormattable"/>
        /// implementation.
        /// </param>
        /// <returns>
        /// The string representation of <paramref name="value"/> in the
        /// specified format.
        /// </returns>
        /// <seealso cref="IFormattable.ToString(String, IFormatProvider)"/>
        public static string GetUIString(
            IFormattable value,
            string format = null)
        {
            return value?.ToString(format, CurrentUICulture);
        }
    }
}
