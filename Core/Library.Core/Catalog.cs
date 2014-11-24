using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Library.Core.CatalogItems;
using Library.Core.LibraryList;

namespace Library.Core
{
    [Serializable]
    public sealed class Catalog
    {
        public LibraryList<Book> Books { get; set; }
        public LibraryList<Magazine> Magazines { get; set; }

        public Catalog()
        {
            Books = new LibraryList<Book>();
            Magazines = new LibraryList<Magazine>();
        }

        public LibraryList<CatalogItem> GetCatalogItems()
        {
            LibraryList<CatalogItem> items = new LibraryList<CatalogItem>(Books.ToArray<Book>());
            foreach (var magazine in Magazines)
            {
                items.Add(magazine);
            }

            return items;
        }
    }
}
