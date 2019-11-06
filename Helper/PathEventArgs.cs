// <copyright file="PathEventArgs.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System;

    public class PathEventArgs : EventArgs
    {
        public PathEventArgs(string path)
        {
            Path = path;
        }

        public string Path
        {
            get;
            set;
        }
    }
}
