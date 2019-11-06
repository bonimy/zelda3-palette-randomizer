// <copyright file="UndoElement.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System;

    public class UndoElement : IUndoElement
    {
        public UndoElement(Action undo, Action redo)
        {
            UndoCallback = undo
                ?? throw new ArgumentNullException(nameof(undo));

            RedoCallback = redo
                ?? throw new ArgumentNullException(nameof(redo));
        }

        private Action UndoCallback { get; }

        private Action RedoCallback { get; }

        public void Undo()
        {
            UndoCallback();
        }

        public void Redo()
        {
            RedoCallback();
        }
    }
}
