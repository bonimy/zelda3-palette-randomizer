// <copyright file="UndoRedoList.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections.Generic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class UndoRedoList<T> :
        ISelectionList<T>,
        ISelectionList,
        IList,
        IReadOnlyList<T>
        where T : unmanaged
    {
        public UndoRedoList()
        {
            History = new UndoFactory();
            BaseList = new SelectionList<T>();
            BaseList.ContentsModified += (s, e) => OnContentsModified(
                EventArgs.Empty);
        }

        public UndoRedoList(int capacity)
        {
            History = new UndoFactory();
            BaseList = new SelectionList<T>(capacity);
            BaseList.ContentsModified += (s, e) => OnContentsModified(
                EventArgs.Empty);
        }

        public UndoRedoList(IEnumerable<T> collection)
        {
            History = new UndoFactory();
            BaseList = new SelectionList<T>(collection);
            BaseList.ContentsModified += (s, e) => OnContentsModified(
                EventArgs.Empty);
        }

        public event EventHandler ContentsModified;

        public bool CanRedo
        {
            get
            {
                return History.CanRedo;
            }
        }

        public bool CanUndo
        {
            get
            {
                return History.CanUndo;
            }
        }

        public int Capacity
        {
            get
            {
                return BaseList.Capacity;
            }

            set
            {
                BaseList.Capacity = value;
            }
        }

        public int Count
        {
            get
            {
                return BaseList.Count;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                return (BaseList as IList).IsFixedSize;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return (BaseList as ICollection<T>).IsReadOnly;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return (BaseList as IList).IsReadOnly;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return (BaseList as ICollection).IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return (BaseList as ICollection).SyncRoot;
            }
        }

        private UndoFactory History
        {
            get;
        }

        private SelectionList<T> BaseList
        {
            get;
        }

        public T this[int index]
        {
            get
            {
                return BaseList[index];
            }

            set
            {
                var oldValue = BaseList[index];
                ModifyList(
                    list => list[index] = value,
                    list => list[index] = oldValue);
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                try
                {
                    this[index] = (T)value;
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException();
                }
            }
        }

        public void Add(T item)
        {
            Insert(Count, item);
        }

        int IList.Add(object value)
        {
            try
            {
                Add((T)value);
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException();
            }

            return Count - 1;
        }

        public void AddRange(IEnumerable<T> collection)
        {
            InsertRange(Count, collection);
        }

        void ISelectionList.AddRange(IEnumerable collection)
        {
            (this as ISelectionList).InsertRange(Count, collection);
        }

        public ReadOnlyCollection<T> AsReadOnly()
        {
            return new ReadOnlyCollection<T>(this);
        }

        public void Clear()
        {
            var items = new List<T>(BaseList);
            ModifyList(
                list => list.Clear(),
                list => list.AddRange(items));
        }

        public void ClearSelection(IListSelection selection)
        {
            TransformSelection(selection, item => default);
        }

        public bool Contains(T item)
        {
            return BaseList.Contains(item);
        }

        bool IList.Contains(object value)
        {
            return (BaseList as IList).Contains(value);
        }

        public void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            (BaseList as ICollection).CopyTo(array, index);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(array, 0, arrayIndex, Count);
        }

        public void CopyTo(T[] array, int index, int arrayIndex, int length)
        {
            BaseList.CopyTo(array, index, arrayIndex, length);
        }

        public void CopyFrom(T[] array, int index)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            CopyFrom(array, index, 0, array.Length);
        }

        public void CopyFrom(T[] array, int index, int arrayIndex, int length)
        {
            var copy = new T[length];
            Array.Copy(
                sourceArray: array,
                sourceIndex: arrayIndex,
                destinationArray: copy,
                destinationIndex: 0,
                length: length);

            var oldValues = BaseList.ToArray(index, length);
            ModifyList(
                list => CopyFrom(copy, index, arrayIndex, length),
                list => CopyFrom(oldValues, index, arrayIndex, length));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return BaseList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return BaseList.IndexOf(item);
        }

        int IList.IndexOf(object value)
        {
            return (BaseList as IList).IndexOf(value);
        }

        public int IndexOf(T item, int index)
        {
            return BaseList.IndexOf(item, index);
        }

        public int IndexOf(T item, int index, int count)
        {
            return BaseList.IndexOf(item, index, count);
        }

        public void Insert(int index, T item)
        {
            ModifyList(
                list => list.Insert(index, item),
                list => list.RemoveAt(index));
        }

        void IList.Insert(int index, object value)
        {
            try
            {
                Insert(index, (T)value);
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException();
            }
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            var items = new List<T>(collection);
            ModifyList(
                list => list.InsertRange(index, items),
                list => list.RemoveRange(index, items.Count));
        }

        void ISelectionList.InsertRange(int index, IEnumerable collection)
        {
            var items = new List<T>();
            (items as ISelectionList).AddRange(collection);
            ModifyList(
                list => list.InsertRange(index, items),
                list => list.RemoveRange(index, items.Count));
        }

        public void InsertSelection(IListSelectionData<T> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var selection = values.Selection.Copy();
            var data = values.Copy();
            ModifyList(
                list => list.InsertSelection(data),
                list => list.RemoveSelection(selection));
        }

        void ISelectionList.InsertSelection(IListSelectionData values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var selection = values.Selection.Copy();
            var data = values.Copy();
            ModifyList(
                list => (list as ISelectionList).InsertSelection(data),
                list => list.RemoveSelection(selection));
        }

        public void Redo()
        {
            History.Redo();
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index != -1)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        void IList.Remove(object value)
        {
            var index = (BaseList as IList).IndexOf(value);
            if (index != -1)
            {
                RemoveAt(index);
            }
        }

        public void RemoveAt(int index)
        {
            var item = BaseList[index];
            ModifyList(
                list => list.RemoveAt(index),
                list => list.Insert(index, item));
        }

        public void RemoveRange(int index, int count)
        {
            var items = BaseList.ToArray(index, count);
            ModifyList(
                list => list.RemoveRange(index, count),
                list => list.InsertRange(index, items));
        }

        public void RemoveSelection(IListSelection selection)
        {
            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            var copy = selection.Copy();
            var values = new ListSelectionData<T>(selection, this);
            ModifyList(
                list => list.RemoveSelection(selection),
                list => list.InsertSelection(values));
        }

        public void SetRange(int index, IEnumerable<T> collection)
        {
            var items = new List<T>(collection);
            var oldValues = BaseList.ToArray(index, items.Count);
            ModifyList(
                list => list.SetRange(index, items),
                list => list.SetRange(index, oldValues));
        }

        void ISelectionList.SetRange(int index, IEnumerable collection)
        {
            var items = new List<T>();
            (items as ISelectionList).AddRange(collection);
            var oldValues = BaseList.ToArray(index, items.Count);
            ModifyList(
                list => list.SetRange(index, items),
                list => list.SetRange(index, oldValues));
        }

        public T[] ToArray()
        {
            return BaseList.ToArray();
        }

        public T[] ToArray(int index, int count)
        {
            return BaseList.ToArray(index, count);
        }

        public void TransformSelection(
            IListSelection selection,
            Func<T, T> transformItem)
        {
            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (transformItem is null)
            {
                throw new ArgumentNullException(nameof(transformItem));
            }

            var values = new ListSelectionData<T>(selection);
            foreach (var index in selection)
            {
                values[index] = transformItem(this[index]);
            }

            var oldValues = new ListSelectionData<T>(selection, BaseList);
            ModifyList(
                list => list.WriteSelection(values),
                list => list.WriteSelection(oldValues));
        }

        void ISelectionList.TransformSelection(
            IListSelection selection,
            Func<object, object> transformItem)
        {
            TransformSelection(selection, TryTransformItem);

            T TryTransformItem(T item)
            {
                try
                {
                    return (T)transformItem(item);
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException();
                }
            }
        }

        public void Undo()
        {
            History.Undo();
        }

        public void WriteSelection(IListSelectionData<T> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var copy = values.Copy();
            var oldValues = new ListSelectionData<T>(copy.Selection, BaseList);
            ModifyList(
                list => list.WriteSelection(copy),
                list => list.WriteSelection(oldValues));
        }

        void ISelectionList.WriteSelection(IListSelectionData values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var copy = values.Copy();
            var oldValues = new ListSelectionData<T>(copy.Selection, BaseList);
            ModifyList(
                list => (list as ISelectionList).WriteSelection(copy),
                list => list.WriteSelection(oldValues));
        }

        protected virtual void OnContentsModified(EventArgs e)
        {
            ContentsModified?.Invoke(this, e);
        }

        private void ModifyList(
            Action<SelectionList<T>> redo,
            Action<SelectionList<T>> undo)
        {
            History.Add(Modify(undo), Modify(redo));
            redo(BaseList);

            Action Modify(Action<SelectionList<T>> action)
            {
                return () => action(BaseList);
            }
        }
    }
}
