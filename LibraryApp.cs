using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LexiconC_Slutuppgift_SmartBook;

static class LibraryApp
{

    static Library library = new Library();

    static public void LoadLibraryFromFile()
    {
        library.LoadLibraryFromFile();
    }

    static private bool clearConsole = true;

    static public void MainMenu()
    {
        ClearConsole();
        while (true)
        {
            Console.WriteLine("Please navigate through the menu by pressing the number of your choice"
                + "\n1. Add Book"
                + "\n2. Remove Book"
                + "\n3. List All Books"
                + "\n4. Search"
                + "\n5. Save Library To File"
                + "\n6. Load Library From File"
                + "\n0. Exit the application");
            char input = ' '; 
            input = Console.ReadKey(intercept: true).KeyChar;
            ClearConsole();
            Console.WriteLine($"you pressed {input}");
            switch (input)
            {
                case '1':
                    AddBook();
                    break;
                case '2':
                    RemoveBookByTitleOrISBN();
                    break;
                case '3':
                    ListAllBooks();
                    break;
                case '4':
                    SearchInLibrary();
                    break;
                case '5':
                    SaveLibraryToFile();
                    break;
                case '6':
                    LoadLibraryFromFile();
                    break;
                case '7':
                    Settings();
                    break;
                case '0':
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Please enter some valid input (0, 1, 2, 3, 4, 5, 6, 7)");
                    break;
            }
        }
    }

    private static void SaveLibraryToFile()
    {
        library.SaveLibraryToFile();
    }

    private static void ListAllBooks()
    {
        library.ListAllBooks();
    }

    private static void ClearConsole()
    {
        if (clearConsole) Console.Clear();
    }

    private static void Settings()
    {
        throw new NotImplementedException();
    }

    private static void SearchInLibrary()
    {
        ClearConsole();
        Console.WriteLine("Navigate by pressing digits"
            + "\n1. Search by Title"
            + "\n2. Search by Author"
            + "\n0. Return to main menu"
            );
        bool validSelection = false;

        while (!validSelection)
        {
            char input = Console.ReadKey(intercept: true).KeyChar;
            switch (input)
            {
                case '1':
                    Book book = SelectBook(b => b.Title);
                    Console.WriteLine(book.ToString());
                    BookMenu(book);
                    validSelection = true;
                    break;
                case '2':
                    book = SelectBook(b => b.Author.ToString());
                    Console.WriteLine(book.ToString());
                    BookMenu(book);
                    validSelection = true;
                    break;
                case '0':
                    validSelection = true;
                    break;
                default:
                    Console.WriteLine("Please enter a valid input");
                    break;
            }
        }
    }

    private static void BookMenu(Book book)
    {
        bool validInput = false;
        Console.WriteLine($"Options for {book.Title}:"
            + "\n1. Update Availability"
            + "\n2. Remove Book"
            + "\n0. Return To Main Menu");
        while (!validInput)
        {
            Console.WriteLine("Navigate by pressing digits");

            char input = ' ';
            input = Console.ReadKey(intercept: true).KeyChar;
            switch (input)
            {

                case '1':
                    UpdateAvailability(book);
                    validInput = true;
                    break;
                case '2':
                    RemoveBookConfirmation(book);
                    validInput = true;
                    break;
                case '0':
                    validInput = true;
                    break;
                default:
                    Console.WriteLine("Invalid Input, try again");
                    break;

            }


        }

    }

    private static void UpdateAvailability(Book book)
    {
        Console.WriteLine($"{book.Title} is currently {book.AvailableAsString}");
        Console.WriteLine($"Would you like to {(book.Available ? "check it out" : "return it")}");
        if(ChooseYesOrNo())book.CheckBookInAndOut();
    }

    private static bool ChooseYesOrNo()
    {
        Console.WriteLine($"(Y)es/(N)o:");
        while (true)
        {
            string input = Console.ReadKey(intercept: true).KeyChar.ToString().ToUpper();
            switch (input)
            {
                case "Y":
                    return true;
                    break;
                case "N":
                    return false;
                    break;
                default:
                    Console.WriteLine("press Y for 'Yes' or N for 'No' to make decission.");
                    break;
            }
        }
    }

    private static void ChooseYesOrNo(Book book, bool secure)
    {
        bool validInput = false;
        if (secure)
        {
            while (!validInput)
            {

                string input = Console.ReadLine();
                switch (input)
                {
                    case "Yes":
                        validInput = true;
                        break;
                    case "No":
                        validInput = true;
                        break;
                    default:
                        Console.WriteLine("input the text 'Yes' or 'No' to make decision.");
                        break;

                }
            }
        }
        else { ChooseYesOrNo(); }

    }

    private static void RemoveBookByTitleOrISBN()
    {
        ClearConsole();
        Console.WriteLine("Navigate by pressing digits"
            + "\n1. Remove by Title"
            + "\n2. Remove by ISBN"
            + "\n0. Return to main menu"
            );
        bool validSelection = false;
        while (!validSelection)
        {
            char input = Console.ReadKey(intercept: true).KeyChar;
            switch (input)
            {
                case '1':
                    RemoveBookConfirmation(SelectBook(b => b.Title));
                    validSelection = true;
                    break;
                case '2':
                    RemoveBookConfirmation(SelectBook(b => b.ISBN));
                    validSelection = true;
                    break;
                case '0':
                    validSelection = true;
                    break;
                default:
                    Console.WriteLine("Please enter a valid input");
                    break;
            }
        }

    }

    private static Book SelectBook(Expression<Func<Book, string>> selectorExpression)
    {
        var selector = selectorExpression.Compile();
        string propertyName = GetPropertyName(selectorExpression);
        string inputString = "";
        int selection = 0;
        while (true)
        {
            Console.Clear();

            Console.WriteLine($"Type the {propertyName} of the book you want to select or use the arrows to go trough list.");
            Console.WriteLine(inputString);

            IEnumerable<Book> bookSelection = library.Collection
                .Where(book => selector(book)
                .StartsWith(inputString, StringComparison.OrdinalIgnoreCase));

            foreach (var (index, book) in bookSelection.Index())
            {
                Console.WriteLine($"{(index == selection ? "> " : "")}{selector(book)}");

            }

            ConsoleKeyInfo inputKey = Console.ReadKey(intercept: true);
            switch (inputKey.Key)
            {
                case ConsoleKey.Enter:
                    return bookSelection.ElementAt(selection);
                case ConsoleKey.Escape:
                    MainMenu();
                    break;
                case ConsoleKey.DownArrow:
                    if (selection < bookSelection.Count() - 1)
                        selection++;
                    break;
                case ConsoleKey.UpArrow:
                    if (selection > 0)
                        selection--;
                    break;
                default:
                    inputString += inputKey.KeyChar;
                    break;
            }

        }
    }

    private static Author SelectOrAddAuthor()
    {
        string inputString = "";
        int selection = 0;
        while (true)
        {
            Console.Clear();

            Console.WriteLine($"Type the name of the author you want to select or use the arrows to go trough list.");
            Console.WriteLine(inputString);


            IEnumerable<Author> authorSelection = library.Authors
                .Where(author => author.ToString()
                .StartsWith(inputString, StringComparison.OrdinalIgnoreCase));

            //if(authorSelection.Count() == 0) selection = 0;
            if (selection > authorSelection.Count() - 1) selection = authorSelection.Count() - 1;
            Console.WriteLine($"{(-1 == selection ? "> " : "")}Add Author");

            foreach (var (index, author) in authorSelection.Index())
            {
                Console.WriteLine($"{(index == selection ? "> " : "")}{author.ToString()}");

            }


            ConsoleKeyInfo inputKey = Console.ReadKey(intercept: true);
            if (inputKey.Key == ConsoleKey.Enter)
            {
                if (selection == -1) return AddAuthor();
                return authorSelection.ElementAt(selection);
            }
            else if (inputKey.Key == ConsoleKey.Backspace)
            {
                inputString = inputString.Remove(inputString.Length - 1);
            }
            else if (inputKey.Key == ConsoleKey.Escape)
            {
                MainMenu();
            }
            else if (inputKey.Key == ConsoleKey.DownArrow)
            {
                if (selection < authorSelection.Count() - 1)
                    selection++;
            }
            else if (inputKey.Key == ConsoleKey.UpArrow)
            {
                if (selection > -1)
                    selection--;
            }
            else
            {

                char inputChar = inputKey.KeyChar;
                inputString += inputChar;
            }

        }
    }

    private static string GetPropertyName(Expression<Func<Book, string>> expression)
    {
        switch (expression.Body)
        {
            case MemberExpression memberExpr:
                return memberExpr.Member.Name;

            case MethodCallExpression methodCallExpr:
                if (methodCallExpr.Object is MemberExpression objMember)
                {
                    return objMember.Member.Name;
                }
                break;
        }

        throw new InvalidOperationException("Unsupported expression type for property name extraction.");
    }

    private static void RemoveBookConfirmation(Book book)
    {

        ClearConsole();
        Console.WriteLine($"You have selected the following book:\n"
        + book.ToString()
        + "\n Would you like to remove it?");
        if(ChooseYesOrNo()) library.RemoveBook(book);
        


    }

    private static void AddBook()
    {
        Book book = new Book();
        AddTitle(book);
        book.Author = SelectOrAddAuthor();
        book.Cathegory = SelectOrAddCathegory();
        AddISBN(book);
        library.AddBookToCollection(book);
    }

    private static Cathegory SelectOrAddCathegory()
    {

        string inputString = "";
        int selection = 0;
        while (true)
        {
            Console.WriteLine($"Type the cathegory you want to select or use the arrows to go trough list.");
            Console.WriteLine(inputString);


            IEnumerable<Cathegory> cathegorySelection = library.Cathegorys
                .Where(cathegory => cathegory.Name
                .StartsWith(inputString, StringComparison.OrdinalIgnoreCase));


            if (selection > cathegorySelection.Count() - 1) selection = cathegorySelection.Count() - 1;
            Console.WriteLine($"{(-1 == selection ? "> " : "")}Add Cathegory");

            foreach (var (index, cathegory) in cathegorySelection.Index())
            {
                Console.WriteLine($"{(index == selection ? "> " : "")}{cathegory.Name}");

            }


            ConsoleKeyInfo inputKey = Console.ReadKey(intercept: true);
            if (inputKey.Key == ConsoleKey.Enter)
            {
                if (selection == -1) return AddCathegory();
                return cathegorySelection.ElementAt(selection);
            }
            else if (inputKey.Key == ConsoleKey.Backspace)
            {
                inputString = inputString.Remove(inputString.Length - 1);
            }
            else if (inputKey.Key == ConsoleKey.Escape)
            {
                MainMenu();
            }
            else if (inputKey.Key == ConsoleKey.DownArrow)
            {
                if (selection < cathegorySelection.Count() - 1)
                    selection++;
            }
            else if (inputKey.Key == ConsoleKey.UpArrow)
            {
                if (selection > -1)
                    selection--;
            }
            else
            {

                char inputChar = inputKey.KeyChar;
                inputString += inputChar;
            }

        }
    }

    private static void AddISBN(Book book)
    {
        bool isValidISBN = false;
        while (!isValidISBN)
        {
            Console.WriteLine("Enter the ISBN of the book");
            string input = Console.ReadLine();
            try
            {
                library.TrySetISBN(book, input);
                isValidISBN = true;
            }
            catch (ArgumentException e) { Console.WriteLine(e.Message); }
        }
    }

    private static Cathegory AddCathegory()
    {
        Console.WriteLine("Write the Cathegory you would like to add.");
        bool isvalidInput = false;
        Cathegory cathegory = new Cathegory();
        while (!isvalidInput)
        {
            cathegory.Name = Console.ReadLine();
            try
            {
                library.TryAddCathegory(cathegory);
                isvalidInput = true;

            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                SelectOrAddCathegory();

            }

        }
        return cathegory;
    }

    private static Author AddAuthor()
    {
        Author author = new Author();
        Console.WriteLine("Enter the authors first name:");
        author.FirstName = Console.ReadLine();
        Console.WriteLine("Enter the authors last name:");
        author.LastName = Console.ReadLine();
        try { library.TryAddAuthor(author); }
        catch (ArgumentException e) 
        {
            Console.WriteLine(e.Message);
            SelectOrAddAuthor();
            
        }
        return author;
    }

    private static void AddTitle(Book book)
    {

        Console.WriteLine("Enter the books Title:");
        book.Title = Console.ReadLine();

    }
}
