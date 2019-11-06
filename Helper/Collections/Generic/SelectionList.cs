// <copyright file="SelectionList.cs" company="Public Domain">
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

    public class SelectionList<T> :
        ISelectionList<T>,
        ISelectionList,
        IList,
        IReadOnlyList<T>
        where T : unmanaged
    {
        private const int DefaultCapacity = 4;
        private static readonly T[] EmptyArray = new T[0];
        private int _version;

        public SelectionList()
        {
            Items = EmptyArray;
        }

        public SelectionList(int capacity)
        {
            Items = capacity < 0
                ? throw new ArgumentOutOfRangeException(nameof(capacity))
                : capacity == 0
                ? EmptyArray
                : new T[capacity];
        }

        public SelectionList(IEnumerable<T> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (collection is ICollection<T> c)
            {
                if (c.Count == 0)
                {
                    Items = EmptyArray;
                }
                else
                {
                    Items = new T[c.Count];
                    c.CopyTo(Items, 0);
                    Count = c.Count;
                }
            }
            else
            {
                Count = 0;
                Items = EmptyArray;
                foreach (var item in collection)
                {
                    Add(item);
                }
            }
        }

        public event EventHandler ContentsModified;

        public int Capacity
        {
            get
            {
                return Items.Length;
            }

            set
            {
                if (value < Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (value == Count)
                {
                    return;
                }

                if (value > 0)
                {
                    var newItems = new T[value];
                    if (Count > 0)
                    {
                        Array.Copy(
                            sourceArray: Items,
                            destinationArray: newItems,
                            length: Count);
                    }

                    Items = newItems;
                }
                else
                {
                    Items = EmptyArray;
                }
            }
        }

        public int Count
        {
            get;
            private set;
        }

        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get;
        }

        private int Version
        {
            get
            {
                return _version;
            }

            set
            {
                _version = value;
                OnContentsModified(EventArgs.Empty);
            }
        }

        private T[] Items
        {
            get;
            set;
        }

        public T this[int index]
        {
            get
            {
                if ((uint)index >= (uint)Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return Items[index];
            }

            set
            {
                if ((uint)index >= (uint)Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                Items[index] = value;
                Version++;
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
            if (Count == Capacity)
            {
                EnsureCapacity(Count + 1);
            }

            Items[Count++] = item;
            Version++;
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
            if (Count > 0)
            {
                Array.Clear(Items, 0, Count);
                Count = 0;
            }

            Version++;
        }

        public void ClearSelection(IListSelection selection)
        {
            TransformSelection(selection, item => default);
        }

        public bool Contains(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            for (var i = 0; i < Count; i++)
            {
                if (comparer.Equals(Items[i], item))
                {
                    return true;
                }
            }

            return false;
        }

        bool IList.Contains(object value)
        {
            return IsCompatibleObject(value) ? Contains((T)value) : false;
        }

        public void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array != null && array.Rank != 1)
            {
                throw new ArgumentException();
            }

            try
            {
                Array.Copy(
                    sourceArray: Items,
                    sourceIndex: 0,
                    destinationArray: array,
                    destinationIndex: index,
                    length: Count);
            }
            catch (ArrayTypeMismatchException)
            {
                throw new ArgumentException();
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(array, 0, arrayIndex, Count);
        }

        public void CopyTo(T[] array, int index, int arrayIndex, int length)
        {
            Array.Copy(
                sourceArray: Items,
                sourceIndex: index,
                destinationArray: array,
                destinationIndex: arrayIndex,
                length: length);
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
            Array.Copy(
                sourceArray: array,
                sourceIndex: arrayIndex,
                destinationArray: Items,
                destinationIndex: index,
                length: length);

            Version++;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var version = Version;
            for (var i = 0; i < Count; i++)
            {
                if (Version != version)
                {
                    throw new InvalidOperationException();
                }

                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return Array.IndexOf(Items, item, 0, Count);
        }

        int IList.IndexOf(object value)
        {
            return IsCompatibleObject(value) ? IndexOf((T)value) : -1;
        }

        public int IndexOf(T item, int index)
        {
            if (index > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return Array.IndexOf(Items, item, index, Count - index);
        }

        public int IndexOf(T item, int index, int count)
        {
            if (index > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0 || index > Count - count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return Array.IndexOf(Items, item, index, count);
        }

        public void Insert(int index, T item)
        {
            if ((uint)index > (uint)Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (Count == Capacity)
            {
                EnsureCapacity(Count + 1);
            }

            if (index < Count)
            {
                Array.Copy(
                    sourceArray: Items,
                    sourceIndex: index,
                    destinationArray: Items,
                    destinationIndex: index + 1,
                    length: Count - index);
            }

            Items[index] = item;
            Count++;
            Version++;
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
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if ((uint)index > (uint)Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (collection is ICollection<T> c)
            {
                var count = c.Count;
                if (count == 0)
                {
                    return;
                }

                EnsureCapacity(Count + count);
                if (index < Count)
                {
                    Array.Copy(
                        sourceArray: Items,
                        sourceIndex: index,
                        destinationArray: Items,
                        destinationIndex: index + count,
                        length: Count - index);
                }

                if (this == c)
                {
                    Array.Copy(
                        sourceArray: Items,
                        sourceIndex: 0,
                        destinationArray: Items,
                        destinationIndex: index,
                        length: index);

                    Array.Copy(
                        sourceArray: Items,
                        sourceIndex: index + count,
                        destinationArray: Items,
                        destinationIndex: index + index,
                        length: Count - index);
                }
                else
                {
                    var itemsToInsert = new T[count];
                    c.CopyTo(itemsToInsert, 0);
                    itemsToInsert.CopyTo(Items, index);
                }

                Count += count;
            }
            else
            {
                foreach (var item in collection)
                {
                    Insert(index++, item);
                }
            }

            Version++;
        }

        void ISelectionList.InsertRange(int index, IEnumerable collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if ((uint)index > (uint)Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (collection is ICollection c)
            {
                var count = c.Count;
                if (count == 0)
                {
                    return;
                }

                EnsureCapacity(Count + count);
                if (index < Count)
                {
                    Array.Copy(
                        sourceArray: Items,
                        sourceIndex: index,
                        destinationArray: Items,
                        destinationIndex: index + count,
                        length: Count - index);
                }

                if (this == c)
                {
                    Array.Copy(
                        sourceArray: Items,
                        sourceIndex: 0,
                        destinationArray: Items,
                        destinationIndex: index,
                        length: index);

                    Array.Copy(
                        sourceArray: Items,
                        sourceIndex: index + count,
                        destinationArray: Items,
                        destinationIndex: index + index,
                        length: Count - index);
                }
                else
                {
                    var itemsToInsert = new T[count];
                    c.CopyTo(itemsToInsert, 0);
                    itemsToInsert.CopyTo(Items, index);
                }

                Count += count;
            }
            else
            {
                foreach (var item in collection)
                {
                    (this as IList).Insert(index++, item);
                }
            }

            Version++;
        }

        public void InsertSelection(IListSelectionData<T> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Selection.MaxIndex >= Count)
            {
                throw new ArgumentException();
            }

            if (Count + values.Count > Capacity)
            {
                EnsureCapacity(Count + values.Count);
            }

            var current = Count;
            for (var i = values.Count; --i >= 0;)
            {
                var freeIndex = values.Selection[i];
                while (--current != freeIndex)
                {
                    Items[current] = Items[current - (i + 1)];
                }

                Items[current] = values[freeIndex];
            }

            Version++;
        }

        void ISelectionList.InsertSelection(IListSelectionData values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Selection.MaxIndex >= Count)
            {
                throw new ArgumentException();
            }

            if (Count + values.Count > Capacity)
            {
                EnsureCapacity(Count + values.Count);
            }

            var current = Count;
            for (var i = values.Count; --i >= 0;)
            {
                var freeIndex = values.Selection[i];
                while (--current != freeIndex)
                {
                    Items[current] = Items[current - (i + 1)];
                }

                try
                {
                    Items[current] = (T)values[freeIndex];
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException();
                }
            }

            Version++;
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        void IList.Remove(object value)
        {
            if (IsCompatibleObject(value))
            {
                Remove((T)value);
            }
        }

        public void RemoveAt(int index)
        {
            if ((uint)index >= (uint)Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            Count--;
            if (index < Count)
            {
                Array.Copy(
                    sourceArray: Items,
                    sourceIndex: index + 1,
                    destinationArray: Items,
                    destinationIndex: index,
                    length: Count - index);
            }

            Items[Count] = default;
            Version++;
        }

        public void RemoveRange(int index, int count)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (index + count > Count)
            {
                throw new ArgumentException();
            }

            if (count == 0)
            {
                return;
            }

            Count -= count;
            if (index < Count)
            {
                Array.Copy(
                    sourceArray: Items,
                    sourceIndex: index + count,
                    destinationArray: Items,
                    destinationIndex: index,
                    length: Count - index);
            }

            Version++;
        }

        public void RemoveSelection(IListSelection selection)
        {
            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (selection.MaxIndex >= Count)
            {
                throw new ArgumentException();
            }

            var freeIndex = selection.MinIndex;
            var current = freeIndex + 1;
            while (current < Count)
            {
                while (current < Count && selection.ContainsIndex(current))
                {
                    current++;
                }

                if (current < Count)
                {
                    Items[freeIndex++] = Items[current++];
                }
            }

            Count -= freeIndex;
            Version++;
        }

        public void SetRange(int index, IEnumerable<T> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (collection is ICollection<T> c)
            {
                if (index + c.Count > Count)
                {
                    throw new ArgumentException();
                }

                c.CopyTo(Items, index);
            }
            else
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                foreach (var item in collection)
                {
                    if (index == Count)
                    {
                        throw new ArgumentException();
                    }

                    Items[index++] = item;
                }
            }

            Version++;
        }

        void ISelectionList.SetRange(int index, IEnumerable collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (collection is ICollection c)
            {
                if (index + c.Count > Count)
                {
                    throw new ArgumentException();
                }

                c.CopyTo(Items, index);
            }
            else
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                foreach (var item in collection)
                {
                    if (index == Count)
                    {
                        throw new ArgumentException();
                    }

                    try
                    {
                        Items[index++] = (T)item;
                    }
                    catch (InvalidCastException)
                    {
                        throw new ArgumentException();
                    }
                }
            }

            Version++;
        }

        public T[] ToArray()
        {
            var result = new T[Count];
            Array.Copy(Items, result, Count);
            return result;
        }

        public T[] ToArray(int index, int count)
        {
            if ((uint)index >= (uint)Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0 || index + count >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var result = new T[count];
            Array.Copy(
                sourceArray: Items,
                sourceIndex: index,
                destinationArray: result,
                destinationIndex: 0,
                length: count);

            return result;
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

            WriteSelection(values);
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

        public void WriteArrayData(Action<T[]> arrayCallback)
        {
            if (arrayCallback is null)
            {
                throw new ArgumentNullException(nameof(arrayCallback));
            }

            arrayCallback(Items);
            Version++;
        }

        public void WriteUnmanagedData(Action<IntPtr> arrayCallback)
        {
            if (arrayCallback is null)
            {
                throw new ArgumentNullException(nameof(arrayCallback));
            }

            unsafe
            {
                fixed (T* ptr = Items)
                {
                    arrayCallback((IntPtr)ptr);
                    Version++;
                }
            }
        }

        public void WriteSelection(IListSelectionData<T> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Selection.MaxIndex >= Count)
            {
                throw new ArgumentException();
            }

            foreach (var kvp in values)
            {
                Items[kvp.Key] = kvp.Value;
            }

            Version++;
        }

        void ISelectionList.WriteSelection(IListSelectionData values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Selection.MaxIndex >= Count)
            {
                throw new ArgumentException();
            }

            foreach (var index in values.Selection)
            {
                try
                {
                    Items[index] = (T)values[index];
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException();
                }
            }

            Version++;
        }

        protected virtual void OnContentsModified(EventArgs e)
        {
            ContentsModified?.Invoke(this, e);
        }

        private static bool IsCompatibleObject(object value)
        {
            return value is T;
        }

        private void EnsureCapacity(int capacity)
        {
            if (Capacity >= capacity)
            {
                return;
            }

            var newCapacity = Capacity == 0 ? DefaultCapacity : Capacity * 2;
            if ((uint)newCapacity > Int32.MaxValue)
            {
                newCapacity = Int32.MaxValue;
            }

            if (newCapacity < capacity)
            {
                newCapacity = capacity;
            }

            Capacity = newCapacity;
        }
    }
}
