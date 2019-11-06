// <copyright file="StringFuncComparer.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System;
    using static System.IO.Path;

    public class StringFuncComparer : StringComparer
    {
        public static readonly StringFuncComparer WindowsPathComparer =
            new StringFuncComparer(GetFullPath, OrdinalIgnoreCase);

        public static readonly StringFuncComparer UnixPathComparer =
            new StringFuncComparer(GetFullPath, Ordinal);

        public static readonly StringFuncComparer WindowsExtensionComparer =
            new StringFuncComparer(GetExtension, OrdinalIgnoreCase);

        public static readonly StringFuncComparer UnixExtensionComparer =
            new StringFuncComparer(GetExtension, Ordinal);

        public StringFuncComparer(Func<string, string> stringFunc)
            : this(stringFunc, CurrentCulture)
        {
        }

        public StringFuncComparer(
            Func<string, string> stringFunc,
            StringComparer baseComparer)
        {
            StringFunc = stringFunc
                ?? throw new ArgumentNullException(nameof(stringFunc));

            BaseComparer = baseComparer
                ?? throw new ArgumentNullException(nameof(baseComparer));
        }

        public StringComparer BaseComparer
        {
            get;
        }

        public Func<string, string> StringFunc
        {
            get;
        }

        public sealed override int Compare(string x, string y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (BaseComparer.Equals(x, y))
            {
                return 0;
            }

            var funcX = x != null ? StringFunc(x) : null;
            var funcY = y != null ? StringFunc(y) : null;
            return BaseComparer.Compare(funcX, funcY);
        }

        public sealed override bool Equals(string x, string y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (BaseComparer.Equals(x, y))
            {
                return true;
            }

            var funcX = x != null ? StringFunc(x) : null;
            var funcY = y != null ? StringFunc(y) : null;
            return BaseComparer.Equals(funcX, funcY);
        }

        public sealed override int GetHashCode(string obj)
        {
            var funcObj = StringFunc(obj);
            return BaseComparer.GetHashCode(funcObj);
        }
    }
}
