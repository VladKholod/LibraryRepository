using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Library.Core;
using Library.Core.LibraryList;
using Library.Core.CatalogItems;

using Library.FileManager;

using Library.SerializeManager;

namespace Library.Application
{
    public sealed class Program
    {
        public static void Main()
        {
            #region xmlFile
            
            //LibraryFileManager fileManager = new LibraryFileManager(new XmlSerialization());
            //
            //LibraryList<Book> books = new LibraryList<Book>();
            //books.Add(new Book() { Author = "Richter", Year = 2012, Title = "CLR via C#" });
            //books.Add(new Book() { Author = "Mc Donald", Year = 2013, Title = "ASP.NET MVC" });
            //books.Add(new Book() { Author = "Troelsen", Year = 2013, Title = "Pro C#" });
            //books.Add(new Book() { Author = "Goodliffe", Year = 2014, Title = "Becoming a better programmer" });
            //books.Add(new Book() { Author = "Alex Davies", Year = 2012, Title = "Async in C# 5.0" });
            //
            //LibraryList<Magazine> magazines = new LibraryList<Magazine>();
            //magazines.Add(new Magazine() { IssueNumber = 1, Title = "X-Men", Year = 1000 });
            //magazines.Add(new Magazine() { IssueNumber = 10, Title = "XX-Men", Year = 2000 });
            //magazines.Add(new Magazine() { IssueNumber = 100, Title = "XXX-Men", Year = 3000 });
            //magazines.Add(new Magazine() { IssueNumber = 2, Title = "Y-Men", Year = 1100 });
            //magazines.Add(new Magazine() { IssueNumber = 20, Title = "YY-Men", Year = 2200 });
            //magazines.Add(new Magazine() { IssueNumber = 200, Title = "YYY-Men", Year = 3300 });
            //
            //Catalog catalog = new Catalog() { Books = books, Magazines = magazines };
            //
            //fileManager.SerializeFile(catalog, Parameters.CatalogSerializedDirectoryPath + @"\trueFile.xml");

            #endregion xmlFile
            LibraryApplication app = new LibraryApplication();

            app.Start();


        }
    }
}
