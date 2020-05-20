// <copyright file="ThrowHelper.cs" company="Public Domain">
//     Copyright (c) 2020 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System;
    using Maseya.Properties;
    using static StringHelper;

    /// <summary>
    /// Provides static methods for creating instances of classes derived from
    /// <see cref="Exception"/> with culture-formatted messages regarding
    /// given exceptional circumstances.
    /// </summary>
    public static class ThrowHelper
    {
        private static string ErrorEmptyColletion
        {
            get
            {
                return Resources.ErrorEmptyCollection;
            }
        }

        private static string ErrorIndexBounds
        {
            get
            {
                return Resources.ErrorIndexBounds;
            }
        }

        private static string ErrorCollectionBounds
        {
            get
            {
                return Resources.ErrorCollectionBounds;
            }
        }

        private static string ErrorOffsetBounds
        {
            get
            {
                return Resources.ErrorOffsetBounds;
            }
        }

        private static string ErrorCollectionMustBeUniqueValues
        {
            get
            {
                return Resources.ErrorCollectionMustBeUniqueValues;
            }
        }

        private static string ErrorNullInCollection
        {
            get
            {
                return Resources.ErrorNullInCollection;
            }
        }

        public static ArgumentException NullInCollection(string paramName)
        {
            return new ArgumentException(ErrorNullInCollection, paramName);
        }

        /// <summary>
        /// Create an instance of <see cref=" ArgumentOutOfRangeException"/>
        /// with a message specifying that the argument value must be greater
        /// than some given value.
        /// </summary>
        /// <param name="paramName">
        /// The name of the parameter that caused the exception.
        /// </param>
        /// <param name="actualValue">
        /// The value of the argument that causes this exception.
        /// </param>
        /// <param name="valid">
        /// The value that <paramref name="actualValue"/> must be greater than
        /// for it to not throw <see cref=" ArgumentOutOfRangeException"/>.
        /// </param>
        /// <returns>
        /// The result of <see cref="ArgumentOutOfRangeException(String,
        /// Object, String)"/> where its <c>message</c> parameter is
        /// culture-aware and specifies that <paramref name="paramName"/> has
        /// value <paramref name="actualValue"/> but must be greater than
        /// <paramref name="valid"/>.
        /// </returns>
        public static ArgumentOutOfRangeException ValueNotGreaterThan(
            string paramName,
            object actualValue,
            object valid = default)
        {
            var message = ErrorValueNotGreaterThan(
                paramName,
                actualValue,
                valid);

            return new ArgumentOutOfRangeException(
                paramName,
                actualValue,
                message);
        }

        /// <summary>
        /// Create an instance of <see cref=" ArgumentOutOfRangeException"/>
        /// with a message specifying that the argument value must be greater
        /// than or equal to some given value.
        /// </summary>
        /// <param name="paramName">
        /// The name of the parameter that caused the exception.
        /// </param>
        /// <param name="actualValue">
        /// The value of the argument that causes this exception.
        /// </param>
        /// <param name="valid">
        /// The value that <paramref name="actualValue"/> must be greater than
        /// or equal to for it to not throw <see cref="
        /// ArgumentOutOfRangeException"/>.
        /// </param>
        /// <returns>
        /// The result of <see cref="ArgumentOutOfRangeException(String,
        /// Object, String)"/> where its <c>message</c> parameter is
        /// culture-aware and specifies that <paramref name="paramName"/> has
        /// value <paramref name="actualValue"/> but must be greater than or
        /// equal to <paramref name="valid"/>.
        /// </returns>
        public static ArgumentOutOfRangeException ValueNotGreaterThanEqualTo(
            string paramName,
            object actualValue,
            object valid = default)
        {
            var message = ErrorValueNotGreaterThanOrEqualTo(
                paramName,
                actualValue,
                valid);

            return new ArgumentOutOfRangeException(
                paramName,
                actualValue,
                message);
        }

        /// <summary>
        /// Create an instance of <see cref=" ArgumentOutOfRangeException"/>
        /// with a message specifying that the argument value must be less
        /// than some given value.
        /// </summary>
        /// <param name="paramName">
        /// The name of the parameter that caused the exception.
        /// </param>
        /// <param name="actualValue">
        /// The value of the argument that causes this exception.
        /// </param>
        /// <param name="valid">
        /// The value that <paramref name="actualValue"/> must be less than
        /// for it to not throw <see cref=" ArgumentOutOfRangeException"/>.
        /// </param>
        /// <returns>
        /// The result of <see cref="ArgumentOutOfRangeException(String,
        /// Object, String)"/> where its <c>message</c> parameter is
        /// culture-aware and specifies that <paramref name="paramName"/> has
        /// value <paramref name="actualValue"/> but must be less than
        /// <paramref name="valid"/>.
        /// </returns>
        public static ArgumentOutOfRangeException ValueNotLessThan(
            string paramName,
            object actualValue,
            object valid = default)
        {
            var message = ErrorValueNotLessThan(
                paramName,
                actualValue,
                valid);

            return new ArgumentOutOfRangeException(
                paramName,
                actualValue,
                message);
        }

        /// <summary>
        /// Create an instance of <see cref=" ArgumentOutOfRangeException"/>
        /// with a message specifying that the argument value must be less
        /// than or equal to some given value.
        /// </summary>
        /// <param name="paramName">
        /// The name of the parameter that caused the exception.
        /// </param>
        /// <param name="actualValue">
        /// The value of the argument that causes this exception.
        /// </param>
        /// <param name="valid">
        /// The value that <paramref name="actualValue"/> must be less than or
        /// equal to for it to not throw <see cref="
        /// ArgumentOutOfRangeException"/>.
        /// </param>
        /// <returns>
        /// The result of <see cref="ArgumentOutOfRangeException(String,
        /// Object, String)"/> where its <c>message</c> parameter is
        /// culture-aware and specifies that <paramref name="paramName"/> has
        /// value <paramref name="actualValue"/> but must be less than or
        /// equal to <paramref name="valid"/>.
        /// </returns>
        public static ArgumentOutOfRangeException ValueNotLessThanEqualTo(
            string paramName,
            object actualValue,
            object valid = default)
        {
            var message = ErrorValueNotLessThanOrEqualTo(
                paramName,
                actualValue,
                valid);

            return new ArgumentOutOfRangeException(
                paramName,
                actualValue,
                message);
        }

        /// <summary>
        /// Create an instance of <see cref=" ArgumentOutOfRangeException"/>
        /// with a message specifying that the argument value must be inside
        /// the bounds of a given array.
        /// </summary>
        /// <param name="paramName">
        /// The name of the parameter that caused the exception.
        /// </param>
        /// <param name="actualValue">
        /// The value of the argument that causes this exception.
        /// </param>
        /// <param name="arraySize">
        /// The size of the array that <paramref name="actualValue"/> was
        /// compared against.
        /// </param>
        /// <returns>
        /// The result of <see cref="ArgumentOutOfRangeException(String,
        /// Object, String)"/> where its <c>message</c> parameter is
        /// culture-aware and specifies that <paramref name="paramName"/> has
        /// value <paramref name="actualValue"/> but must be greater than or
        /// equal to zero and less than <paramref name="arraySize"/>.
        /// </returns>
        public static ArgumentOutOfRangeException ValueNotInArrayBounds(
            string paramName,
            int actualValue,
            int arraySize)
        {
            var message = ErrorValueNotInArrayBounds(
                paramName,
                actualValue,
                arraySize);

            return new ArgumentOutOfRangeException(
                paramName,
                actualValue,
                message);
        }

        /// <summary>
        /// Create an instance of <see cref="ArgumentException"/> with a
        /// message specifying that the argument value is NaN where it was not
        /// expected.
        /// </summary>
        /// <param name="paramName">
        /// The name of the parameter that caused the exception.
        /// </param>
        /// <returns>
        /// The result of <see cref="ArgumentException(String, String)"/>
        /// where its <c>message</c> parameter is culture-aware and specifies
        /// that <paramref name="paramName"/> is NaN where it should not be.
        /// </returns>
        public static ArgumentException UnexpectedNaNArgumentExeption(
            string paramName)
        {
            var message = ErrorValueIsNaN(paramName);
            return new ArgumentException(message, paramName);
        }

        /// <summary>
        /// Create an instance of <see cref="ArgumentException"/> with a
        /// message specifying that the argument value is infinity where it
        /// was not expected.
        /// </summary>
        /// <param name="paramName">
        /// The name of the parameter that caused the exception.
        /// </param>
        /// <returns>
        /// The result of <see cref="ArgumentException(String, String)"/>
        /// where its <c>message</c> parameter is culture-aware and specifies
        /// that <paramref name="paramName"/> is infinity where it should not
        /// be.
        /// </returns>
        public static ArgumentException UnexpectedInfinityArgumentException(
            string paramName)
        {
            var message = ErrorValueIsInfinity(paramName);
            return new ArgumentException(message, paramName);
        }

        public static ArgumentException EmptyCollectionArgumentException(
            string paramName)
        {
            var message = ErrorEmptyColletion;
            return new ArgumentException(message, paramName);
        }

        public static ArgumentException IndexBoundsArgumentException(
            string paramName)
        {
            var message = ErrorIndexBounds;
            return new ArgumentException(message, paramName);
        }

        public static ArgumentException CollectionBoundsArgumentException(
            string paramName)
        {
            var message = ErrorCollectionBounds;
            return new ArgumentException(message, paramName);
        }

        public static ArgumentException InvalidOffsetArgumentException()
        {
            var message = ErrorOffsetBounds;
            return new ArgumentException(message);
        }

        public static ArgumentException CollectionMustBeOnlyUniqueValues(
            string paramName)
        {
            var message = ErrorCollectionMustBeUniqueValues;
            return new ArgumentException(message, paramName);
        }

        public static InvalidOperationException ObjectTypeException(
            Type actualType,
            Type expectedType)
        {
            var message = ErrorObjectType(actualType, expectedType);
            return new InvalidOperationException(message);
        }

        /// <summary>
        /// Formats an error message stating that a parameter's value is not
        /// greater than a valid specified value.
        /// </summary>
        /// <param name="paramName">
        /// The name of the parameter that caused the error.
        /// </param>
        /// <param name="actualValue">
        /// The value of the argument that causes this error.
        /// </param>
        /// <param name="valid">
        /// The value that <paramref name="actualValue"/> must be greater than
        /// for it to not cause an error.
        /// </param>
        /// <returns>
        /// A culture-aware message specifying that <paramref
        /// name="paramName"/> has value <paramref name="actualValue"/> but
        /// must be greater than <paramref name="valid"/>.
        /// </returns>
        private static string ErrorValueNotGreaterThan(
            string paramName,
            object actualValue,
            object valid)
        {
            return GetString(
                Resources.ErrorValueNotGreaterThan,
                paramName,
                actualValue,
                valid ?? 0);
        }

        /// <summary>
        /// Formats an error message stating that a parameter's value is not
        /// greater than or equal to a valid specified value.
        /// </summary>
        /// <param name="paramName">
        /// The name of the parameter that caused the error.
        /// </param>
        /// <param name="actualValue">
        /// The value of the argument that causes this error.
        /// </param>
        /// <param name="valid">
        /// The value that <paramref name="actualValue"/> must be greater than
        /// or equal to for it to not cause an error.
        /// </param>
        /// <returns>
        /// A culture-aware message specifying that <paramref
        /// name="paramName"/> has value <paramref name="actualValue"/> but
        /// must be greater than or equal to <paramref name="valid"/>.
        /// </returns>
        private static string ErrorValueNotGreaterThanOrEqualTo(
            string paramName,
            object actualValue,
            object valid)
        {
            return GetString(
                Resources.ErrorValueNotGreaterThanOrEqualTo,
                paramName,
                actualValue,
                valid ?? 0);
        }

        /// <summary>
        /// Formats an error message stating that a parameter's value is not
        /// less than a valid specified value.
        /// </summary>
        /// <param name="paramName">
        /// The name of the parameter that caused the error.
        /// </param>
        /// <param name="actualValue">
        /// The value of the argument that causes this error.
        /// </param>
        /// <param name="valid">
        /// The value that <paramref name="actualValue"/> must be less than
        /// for it to not cause an error.
        /// </param>
        /// <returns>
        /// A culture-aware message specifying that <paramref
        /// name="paramName"/> has value <paramref name="actualValue"/> but
        /// must be less than <paramref name="valid"/>.
        /// </returns>
        private static string ErrorValueNotLessThan(
            string paramName,
            object actualValue,
            object valid)
        {
            return GetString(
                Resources.ErrorValueNotLessThan,
                paramName,
                actualValue,
                valid ?? 0);
        }

        /// <summary>
        /// Formats an error message stating that a parameter's value is not
        /// less than or equal to a valid specified value.
        /// </summary>
        /// <param name="paramName">
        /// The name of the parameter that caused the error.
        /// </param>
        /// <param name="actualValue">
        /// The value of the argument that causes this error.
        /// </param>
        /// <param name="valid">
        /// The value that <paramref name="actualValue"/> must be less than or
        /// equal to for it to not cause an error.
        /// </param>
        /// <returns>
        /// A culture-aware message specifying that <paramref
        /// name="paramName"/> has value <paramref name="actualValue"/> but
        /// must be less than or equal to <paramref name="valid"/>.
        /// </returns>
        private static string ErrorValueNotLessThanOrEqualTo(
            string paramName,
            object actualValue,
            object valid)
        {
            return GetString(
                Resources.ErrorValueNotLessThanOrEqualTo,
                paramName,
                actualValue,
                valid ?? 0);
        }

        /// <summary>
        /// Formats an error message stating that a parameter's value must be
        /// inside the bounds of a given array.
        /// </summary>
        /// <param name="paramName">
        /// The name of the parameter that caused the error.
        /// </param>
        /// <param name="actualValue">
        /// The value of the argument that causes this error.
        /// </param>
        /// <param name="arraySize">
        /// The size of the array that <paramref name="actualValue"/> was
        /// compared against.
        /// </param>
        /// <returns>
        /// A culture-aware message specifying that <paramref
        /// name="paramName"/> has value <paramref name="actualValue"/> but
        /// must be greater than or equal to zero and less than <paramref
        /// name="arraySize"/>.
        /// </returns>
        private static string ErrorValueNotInArrayBounds(
            string paramName,
            int actualValue,
            int arraySize)
        {
            return GetString(
                Resources.ErrorValueNotInArrayBounds,
                paramName,
                actualValue,
                arraySize);
        }

        /// <summary>
        /// Formats an error message stating that a parameter's value is NaN
        /// where it was not expected.
        /// </summary>
        /// <param name="paramName">
        /// The name of the parameter that caused this error.
        /// </param>
        /// <returns>
        /// A culture-aware message specifying that <paramref
        /// name="paramName"/> is NaN where it should not be.
        /// </returns>
        private static string ErrorValueIsNaN(string paramName)
        {
            return GetString(Resources.ErrorValueIsNaN, paramName);
        }

        /// <summary>
        /// Formats an error message stating that a parameter's value is
        /// infinity where it was not expected.
        /// </summary>
        /// <param name="paramName">
        /// The name of the parameter that caused this error.
        /// </param>
        /// <returns>
        /// A culture-aware message specifying that <paramref
        /// name="paramName"/> is infinity where it should not be.
        /// </returns>
        private static string ErrorValueIsInfinity(string paramName)
        {
            return GetString(Resources.ErrorValueIsInfinity, paramName);
        }

        private static string ErrorObjectType(
            Type actualType,
            Type expectedType)
        {
            return GetString(
                Resources.ErrorObjectType,
                actualType,
                expectedType);
        }
    }
}
