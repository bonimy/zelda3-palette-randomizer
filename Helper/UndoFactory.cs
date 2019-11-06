// <copyright file="UndoFactory.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements a linear undo and redo history.
    /// </summary>
    public class UndoFactory : IUndoFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UndoFactory"/> class.
        /// </summary>
        public UndoFactory()
        {
            History = new List<IUndoElement>();
        }

        public event EventHandler UndoComplete;

        public event EventHandler RedoComplete;

        public event EventHandler<UndoEventArgs> UndoElementAdded;

        /// <summary>
        /// Gets the total number of undo actions in this <see cref="
        /// UndoFactory"/>.
        /// </summary>
        public int Count
        {
            get
            {
                return History.Count;
            }
        }

        /// <summary>
        /// Gets the number of remaining undo operations that can still be
        /// performed.
        /// </summary>
        /// <remarks>
        /// Invoking <see cref="Undo"/> decreased this value by one until it is
        /// zero. Invoking <see cref="Redo"/> increased this value by one until
        /// it is equal to <see cref="Count"/>.
        /// </remarks>
        public int Index
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance of <see
        /// cref="UndoFactory"/> can invoke <see cref="Undo"/>.
        /// </summary>
        public bool CanUndo
        {
            get
            {
                return Index > 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance of <see
        /// cref="UndoFactory"/> can invoke <see cref="Redo"/>.
        /// </summary>
        public bool CanRedo
        {
            get
            {
                return Index < Count;
            }
        }

        private List<IUndoElement> History
        {
            get;
        }

        /// <summary>
        /// Adds a new undo and its redo to the history at the current state.
        /// </summary>
        /// <param name="undo">
        /// The <see cref="Action"/> to invoke when undoing an operation.
        /// </param>
        /// <param name="redo">
        /// The <see cref="Action"/> to invoke when redoing an operation.
        /// </param>
        /// <remarks>
        /// <paramref name="redo"/> and <paramref name="undo"/> must be
        /// invertible functions such that when one is applied after the other,
        /// no change has been made. For example, if <paramref name="undo"/> is
        /// <c>x++</c>, then <paramref name="redo"/> must be <c>x--</c>.
        /// <para/>
        /// If the undo history is not up to date ( <see cref="Index"/> is less
        /// than <see cref="Count"/>), all undo actions between <see
        /// cref="Index"/> and <see cref="Count"/> are lost and the tip of the
        /// undo list is set to <paramref name="undo"/> and <paramref
        /// name="redo"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="undo"/> -or- <paramref name="redo"/> is <see
        /// langword="null"/>.
        /// </exception>
        public void Add(Action undo, Action redo)
        {
            if (undo is null)
            {
                throw new ArgumentNullException(nameof(undo));
            }

            if (redo is null)
            {
                throw new ArgumentNullException(nameof(redo));
            }

            Add(new UndoElement(undo, redo));
        }

        /// <summary>
        /// Adds a new undo and its redo to the history at the current state.
        /// </summary>
        /// <param name="undoElement">
        /// The <see cref="IUndoElement"/> to add to the history.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="undoElement"/> is <see langword="null"/>.
        /// </exception>
        public void Add(IUndoElement undoElement)
        {
            if (undoElement is null)
            {
                throw new ArgumentNullException(nameof(undoElement));
            }

            if (Index < Count)
            {
                History.RemoveRange(Index, Count - Index);
            }

            History.Add(undoElement);
            Index++;
            OnUndoElementAdded(new UndoEventArgs(undoElement));
        }

        /// <summary>
        /// Undoes the last operation. If <see cref="CanUndo"/> is <see
        /// langword="false"/>, no action is taken.
        /// </summary>
        public void Undo()
        {
            if (!CanUndo)
            {
                return;
            }

            History[--Index].Undo();
            OnUndoComplete(EventArgs.Empty);
        }

        /// <summary>
        /// Redoes the last operation that was undone. If <see cref="CanRedo"/>
        /// is <see langword="false"/>, no action is taken.
        /// </summary>
        public void Redo()
        {
            if (!CanRedo)
            {
                return;
            }

            History[Index++].Redo();
            OnRedoComplete(EventArgs.Empty);
        }

        protected virtual void OnUndoComplete(EventArgs e)
        {
            UndoComplete?.Invoke(this, e);
        }

        protected virtual void OnRedoComplete(EventArgs e)
        {
            RedoComplete?.Invoke(this, e);
        }

        protected virtual void OnUndoElementAdded(UndoEventArgs e)
        {
            UndoElementAdded?.Invoke(this, e);
        }
    }
}
