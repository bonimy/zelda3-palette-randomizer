// <copyright file="UndoEventArgs.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System;

    public class UndoEventArgs : EventArgs
    {
        public UndoEventArgs(IUndoElement undoElement)
        {
            UndoElement = undoElement
                ?? throw new ArgumentNullException(nameof(undoElement));
        }

        public IUndoElement UndoElement { get; }
    }
}
