using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Library.Core;
using Library.Core.LibraryList;
using Library.Core.CatalogItems;

using Library.FileManager;

using Library.SerializeManager;

namespace Library.Application
{
    public sealed class LibraryApplication
    {
        public delegate void LibraryEvent();
        
        private bool _isEnabledCRUD = false;
        private bool _isEnabledSave = false;

        public event LibraryEvent UpdateLibraryEvent;

        private string _fileName = string.Empty;
        private Catalog _catalog;
        private List<string> _commands = new List<string>();
        private Dictionary<int, Action> _commandsActions = new Dictionary<int, Action>();

        LibraryFileManager _fileManager = new LibraryFileManager(new XmlSerialization());

        public LibraryApplication()
        {
            LoadBaseMenuItems();
        }

        private void DisplayMenu(List<string> items, int selectedMenuItem)
        {
            Console.Clear();
            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine("{0} {1}", i == selectedMenuItem ? "    >\t" : "\t", items[i]);
            }

            DisplayHelp();
        }

        private void DisplayHelp() 
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 4);
            Console.WriteLine("Press <Enter> to select menu item.");
            Console.WriteLine("Press <Backspace> to return to previous submenu.");
        }

        private int SelectMenuItem(List<string> items)
        {
            Console.CursorVisible = false;
            int currentMenuItem = 0;
            while (true)
            {
                DisplayMenu(items, currentMenuItem);

                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow) 
                {
                    if (currentMenuItem > 0)
                        currentMenuItem--;
                }
                else if (key.Key == ConsoleKey.DownArrow) 
                {
                    if (currentMenuItem < items.Count - 1)
                        currentMenuItem++;
                }
                else if (key.Key == ConsoleKey.Enter) 
                {
                    return currentMenuItem;
                }
            }
        }

        public void Start() 
        {
            int currentMenuItem;
            while (true)
            {
                currentMenuItem = SelectMenuItem(_commands);
                PerformAction(currentMenuItem);
            }
        }

        private void PerformAction(int selectedMenuItem)
        {
            Console.Clear();
            _commandsActions[selectedMenuItem]();
            DisplayHelp();
        }

        private void LoadBaseMenuItems() 
        {
            _commands.Add("Load Catalog");
            _commandsActions.Add(0, LoadCatalog);

            _commands.Add("Create Catalog");
            _commandsActions.Add(1, CreateCatalog);
        }

        private void EnableCRUDMenuItems() 
        {
            if (!_isEnabledCRUD) 
            {
                _commands.Add("Add Book");
                _commands.Add("Edit Book");
                _commands.Add("Remove Book");
                _commands.Add("Find Book By Id");
                _commandsActions.Add(3, AddBook);
                _commandsActions.Add(4, EditBook);
                _commandsActions.Add(5, RemoveBook);
                _commandsActions.Add(6, FindBookById);

                _commands.Add("Add Magazine");
                _commands.Add("Edit Magazine");
                _commands.Add("Remove Magazine");
                _commands.Add("Find Magazine By Id");
                _commandsActions.Add(7, AddMagazine);
                _commandsActions.Add(8, EditMagazine);
                _commandsActions.Add(9, RemoveMagazine);
                _commandsActions.Add(10, FindMagazineById);

                _commands.Add("ShowItems");
                _commandsActions.Add(11, ShowItems);
            }
        }

        private void DisableCRUDMenuItems() 
        {
            if (_isEnabledCRUD)
            {
                _commands.Remove("Add Book");
                _commands.Remove("Edit Book");
                _commands.Remove("Remove Book");
                _commands.Remove("Find Book By Id");
                _commandsActions.Remove(3);
                _commandsActions.Remove(4);
                _commandsActions.Remove(5);
                _commandsActions.Remove(6);

                _commands.Remove("Add Magazine");
                _commands.Remove("Edit Magazine");
                _commands.Remove("Remove Magazine");
                _commands.Remove("Find Magazine By Id");
                _commandsActions.Remove(7);
                _commandsActions.Remove(8);
                _commandsActions.Remove(9);
                _commandsActions.Remove(10);

                _commands.Remove("ShowItems");
                _commandsActions.Remove(11);
            }
        }

        private void EnableSaveCatalog() 
        {
            if (!_isEnabledSave)
            {
                _commands.Add("Save Catalog");
                _commandsActions.Add(2, SaveCatalog);
            }
        }

        private void DisableSaveCatalog()
        {
            if (_isEnabledSave) 
            {
                _commands.Remove("Save Catalog");
                _commandsActions.Remove(2);
            }
        }

        private async void LoadCatalog() 
        {
            List<string> fullNamedFiles = _fileManager.GetAvaliableFiles();
            List<string> files = new List<string>();

            foreach (var file in fullNamedFiles) 
            {
                files.Add(file.Replace(Parameters.CatalogSerializedDirectoryPath, ""));
            }

            int currentMenuItem = SelectMenuItem(files);
            try
            {
                Console.SetCursorPosition(0, Console.WindowHeight - 2);
                _catalog = await _fileManager.DeserializeFileAsync(fullNamedFiles[currentMenuItem]);

                if (!_isEnabledCRUD && !_isEnabledSave)
                {
                    EnableSaveCatalog();
                    EnableCRUDMenuItems();

                    _isEnabledCRUD = true;
                    _isEnabledSave = true;
                }

                
                if (UpdateLibraryEvent == null) 
                {
                    UpdateLibraryEvent += UpdateCatalog; 
                }

                Console.WriteLine("!\tCatalog loaded");
            }
            catch (Exception e)
            {
                DisableCRUDMenuItems();
                DisableSaveCatalog();
                Console.SetCursorPosition(0, Console.WindowHeight - 2);
                Console.WriteLine("!\tLoad failed");
            }
        }

        private void CreateCatalog() 
        {
            _catalog = new Catalog();

            if (!_isEnabledCRUD && !_isEnabledSave)
            {
                EnableSaveCatalog();
                EnableCRUDMenuItems();
            }
        }

        private async void SaveCatalog() 
        {
            try
            {
                Console.Write("Input file name :\n> ");
                string path = Parameters.CatalogSerializedDirectoryPath + Console.ReadLine();

                Console.SetCursorPosition(0, Console.WindowHeight - 2);

                await _fileManager.SerializeFileAsync(_catalog, path);

                this._fileName = path;

                if (UpdateLibraryEvent == null)
                {
                    UpdateLibraryEvent += UpdateCatalog;
                }

                Console.WriteLine("!\tCatalog saved");
            }
            catch (Exception e) 
            {

                Console.SetCursorPosition(0, Console.WindowHeight - 2);
                Console.WriteLine("!\tCatalog save failed");
            }
        }

        private async void UpdateCatalog() 
        {
            try
            {
                Console.SetCursorPosition(0, Console.WindowHeight - 2);

                await _fileManager.SerializeFileAsync(_catalog, _fileName);

                Console.WriteLine("!\tCatalog updated");
            }
            catch (Exception e)
            {
                Console.SetCursorPosition(0, Console.WindowHeight - 2);
                Console.WriteLine("!\tCatalog update failed");
            }
        }

        private void AddBook() 
        {
            try
            {
                Console.Write("Input author name :\n> ");
                string author = Console.ReadLine();
                Console.Write("Input title :\n> ");
                string title = Console.ReadLine();
                Console.Write("Input year :\n> ");
                int year = int.Parse(Console.ReadLine());

                _catalog.Books.Add(new Book() { Author = author, Title = title, Year = year });
                
                if (UpdateLibraryEvent != null)
                    UpdateLibraryEvent();

                Console.WriteLine("!\tBook added");
            }
            catch (Exception e) 
            {
                Console.WriteLine("!\tBook adding failed");
            }
        }

        private void EditBook()
        {
            try
            {
                Console.Write("Input book id\n> ");
                int id = int.Parse(Console.ReadLine());
                if (!_catalog.Books.Constraints(id))
                    throw new IndexOutOfRangeException();
                Book tempBook = _catalog.Books.Find(id);

                Console.Write("Input author name :\n> ");
                string author = Console.ReadLine();
                if (author != string.Empty)
                    tempBook.Author = author;

                Console.Write("Input title :\n> ");
                string title = Console.ReadLine();
                if (title != string.Empty)
                    tempBook.Title = title;

                Console.Write("Input year :\n> ");
                string yearStr = Console.ReadLine();
                if (yearStr != string.Empty)
                {
                    int year = int.Parse(yearStr);
                    tempBook.Year = year;
                }

                if (UpdateLibraryEvent != null)
                    UpdateLibraryEvent();

                Console.WriteLine("!\tBook edited");
            }
            catch (Exception e)
            {
                Console.WriteLine("!\tBook editing failed");
            }
        }

        private void RemoveBook()
        {
            try
            {
                Console.Write("Input book id\n> ");
                int id = int.Parse(Console.ReadLine());

                if (!_catalog.Books.Constraints(id))
                    throw new IndexOutOfRangeException();

                _catalog.Books.Remove(id);

                if (UpdateLibraryEvent != null)
                    UpdateLibraryEvent();

                Console.WriteLine("!\tBook removed");
            }
            catch (Exception e)
            {
                Console.WriteLine("!\tBook removing failed");
            }
        }

        private void FindBookById()
        {
            try
            {
                Console.Write("Input book id\n> ");
                int id = int.Parse(Console.ReadLine());

                if (!_catalog.Books.Constraints(id))
                    throw new IndexOutOfRangeException();

                Console.WriteLine(_catalog.Books.Find(id));
                while (Console.ReadKey(true).Key != ConsoleKey.Backspace) ;
            }
            catch (Exception e)
            {
                Console.WriteLine("!\tBook aren't constraints");
            }
        }

        private void AddMagazine()
        {
            try
            {
                Console.Write("Input issue number :\n> ");
                int issueNumber = int.Parse(Console.ReadLine());
                Console.Write("Input title :\n> ");
                string title = Console.ReadLine();
                Console.Write("Input year :\n> ");
                int year = int.Parse(Console.ReadLine());

                _catalog.Magazines.Add(new Magazine() { IssueNumber = issueNumber, Title = title, Year = year });

                if (UpdateLibraryEvent != null)
                    UpdateLibraryEvent();

                Console.WriteLine("!\tBook added");
            }
            catch (Exception e)
            {
                Console.WriteLine("!\tMagazine adding failed");
            }
        }

        private void EditMagazine()
        {
            try
            {
                Console.Write("Input magazine id\n> ");
                int id = int.Parse(Console.ReadLine());

                if (!_catalog.Magazines.Constraints(id))
                    throw new IndexOutOfRangeException();

                Magazine tempMagazine = _catalog.Magazines.Find(id);

                Console.Write("Input issue number :\n> ");
                string issueStr = Console.ReadLine();
                if (issueStr != string.Empty)
                {
                    int issueNumber = int.Parse(issueStr);
                    tempMagazine.IssueNumber = issueNumber;
                }


                Console.Write("Input title :\n> ");
                string title = Console.ReadLine();
                if (title != string.Empty)
                    tempMagazine.Title = title;

                Console.Write("Input year :\n> ");
                string yearStr = Console.ReadLine();
                if (yearStr != string.Empty)
                {
                    int year = int.Parse(yearStr);
                    tempMagazine.Year = year;
                }

                if (UpdateLibraryEvent != null)
                    UpdateLibraryEvent();

                Console.WriteLine("!\tMagazine edited");
            }
            catch (Exception e)
            {
                Console.WriteLine("!\tMagazine editing failed");
            }
        }

        private void RemoveMagazine()
        {
            try
            {
                Console.Write("Input magazine id\n> ");
                int id = int.Parse(Console.ReadLine());

                if (!_catalog.Magazines.Constraints(id))
                    throw new IndexOutOfRangeException();

                _catalog.Magazines.Remove(id);
                
                if (UpdateLibraryEvent != null)
                    UpdateLibraryEvent();

                Console.WriteLine("!\tMagazine removed");
            }
            catch (Exception e)
            {
                Console.WriteLine("!\tMagazine removing failed");
            }
        }

        private void FindMagazineById()
        {
            try
            {
                Console.Write("Input Magazine id\n> ");
                int id = int.Parse(Console.ReadLine());

                if (!_catalog.Magazines.Constraints(id))
                    throw new IndexOutOfRangeException();

                Console.WriteLine(_catalog.Magazines.Find(id));
                while (Console.ReadKey(true).Key != ConsoleKey.Backspace) ;
            }
            catch (Exception e)
            {
                Console.WriteLine("!\tMagazine aren't constrainted");
            }
        }

        private void ShowItems() 
        {
            foreach (var item in _catalog.GetCatalogItems()) 
            {
                Console.WriteLine(item);
            }

            while (Console.ReadKey(true).Key != ConsoleKey.Backspace) ;
        }
    }
}
