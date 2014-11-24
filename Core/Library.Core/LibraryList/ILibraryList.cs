using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.LibraryList
{
    public interface ILibraryList<T> : IEnumerable<T>
    {
        int Count { get; }
        T this[int index] { get; set; }
        void Add(T item);
        bool Constraints(int id);
        int IndexOf(int id);
        void Remove(int id);
        void RemoveAt(int index);
        List<T> ToList();
        T Find(int id);
        void Sort();
    }
}

