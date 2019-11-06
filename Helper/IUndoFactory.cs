// <copyright file="IUndoFactory.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System;

    /// <summary>
    /// Defines methods to support undo and redo.
    /// </summary>
    public interface IUndoFactory
    {
        event EventHandler UndoComplete;

        event EventHandler RedoComplete;

        event EventHandler<UndoEventArgs> UndoElementAdded;

        /// <summary>
        /// Gets a value indicating whether this instance of <see
        /// cref="IUndoFactory"/> can invoke <see cref="Undo"/>.
        /// </summary>
        bool CanUndo
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this instance of <see
        /// cref="IUndoFactory"/> can invoke <see cref="Redo"/>.
        /// </summary>
        bool CanRedo
        {
            get;
        }

        /// <summary>
        /// Undoes the last operation. If <see cref="CanUndo"/> is <see
        /// langword="false"/>, no action is taken.
        /// </summary>
        void Undo();

        /// <summary>
        /// Redoes the last operation that was undone. If <see cref="CanRedo"/>
        /// is <see langword="false"/>, no action is taken.
        /// </summary>
        void Redo();
    }
}
