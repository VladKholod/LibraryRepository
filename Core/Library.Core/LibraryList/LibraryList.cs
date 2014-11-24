using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Library.Core.CatalogItems;

namespace Library.Core.LibraryList
{
    [Serializable]
    public sealed class LibraryList<T> : ILibraryList<T>, IEnumerable<T>, IEnumerable where T : CatalogItem
    {
        private T[] _array;
        private int _capacity = 0;
        private int _count = 0;

        #region Constructors
        public LibraryList()
            : this(10)
        {
        }

        public LibraryList(int capacity)
        {
            this._array = new T[capacity];

            this._capacity = capacity;
        }

        public LibraryList(T[] array)
        {
            this._array = array;

            this._capacity = array.Length;
            this._count = array.Length;
        }
        #endregion Constructors

        #region Implementations

        #region IEnumerable
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                yield return _array[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion IEnumerable

        #region ILibraryList

        public int Count
        {
            get { return _count; }
        }

        public T this[int index]
        {
            get
            {
                if (IsValidIndex(index))
                    return _array[index];

                throw new IndexOutOfRangeException();
            }
            set
            {
                if (IsValidIndex(index))
                    _array[index] = value;

                throw new IndexOutOfRangeException();
            }
        }

        public void Add(T item)
        {
            if (!Constraints(item.Id))
            {
                if (_count == _capacity)
                    Resize();

                this._array[_count++] = item;
            }
        }

        public bool Constraints(int id)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_array[i].Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        public int IndexOf(int id)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_array[i].Id == id)
                    return i;
            }

            return -1;
        }

        public void Remove(int id)
        {
            if (Constraints(id))
            {
                int index = IndexOf(id);
                if ( index == _count - 1)
                    _count--;
                else
                    _array[index] = _array[--_count];
            }
        }

        public void RemoveAt(int index)
        {
            if (!IsValidIndex(index))
                throw new IndexOutOfRangeException();

            Remove(_array[index].Id);
        }

        public List<T> ToList()
        {
            return this._array.ToList<T>();
        }

        public T Find(int id)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_array[i].Id == id)
                    return _array[i];
            }

            return null;
        }

        public void Sort()
        {
            for (int i = 0; i < _count; i++)
            {
                for (int j = 0; j < _count; j++)
                {
                    if (_array[i].CompareTo(_array[j]) == -1)
                    {
                        T temp = _array[i];
                        _array[i] = _array[j];
                        _array[j] = temp;
                    }
                }
            }
        }

        #endregion ILibraryList

        #endregion implementations

        private void Resize()
        {
            int newCapacity = Count * 2 + 1;
            T[] tempArray = new T[newCapacity];

            for (int i = 0; i < Count; i++)
            {
                tempArray[i] = _array[i];
            }

            this._array = tempArray;
            this._capacity = newCapacity;
        }

        private bool IsValidIndex(int index)
        {
            return (index >= 0 && index < _count);
        }

        public void LeftShift(int index)
        {
            if (index != _count - 1)
            {
                for (int i = index; i < _count; i++)
                {
                    if (i == _capacity)
                        break;

                    _array[i] = _array[i + 1];
                }
            }

            _count--;
        }
    }
}