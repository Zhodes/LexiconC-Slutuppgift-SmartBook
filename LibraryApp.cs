using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LexiconC_Slutuppgift_SmartBook;

static class LibraryApp
{

    static Library library = new Library();

    static public void populateWithExampleLibrary()
    {
        Author author = new Author("jesus", "kristus");
        string cathegory = "Religion";
        Book book = new Book("Bibeln", author, cathegory, "777");
        library.authors.Add(author);
        library.cathegorys.Add(cathegory);
        //author.Books.Add(book);
        library.collection.Add(book);

        author = new Author("J.R.R.", "Tolkien");
        cathegory = "Fantasy";
        book = new Book("Bilbo - En Hobbits Äventyr", author, cathegory, "9789172631649");
        library.authors.Add(author);
        library.cathegorys.Add(cathegory);
        //author.Books.Add(book);
        library.collection.Add(book);
    }


    static private bool clearConsole = true;

    static public void MainMenu()
    {
        ClearConsole();
        while (true)
        {
            Console.WriteLine("Please navigate through the menu by pressing the number \n(1, 2, 3 ,4, 5, 6, 7, 0) of your choice"
                + "\n1. Add Book"
                + "\n2. Remove Book"
                + "\n3. List All Books"
                + "\n4. Search"
                + "\n5. Save Library To File"
                + "\n6. Load Library From File"
                + "\n0. Exit the application");
            char input = ' '; //Creates the character input to be used with the switch-case below.
                              // try
                              //{
                              //reads the first key the user presses, stores the key pressed as a char and prevents the keypress from showing on consol
            input = Console.ReadKey(intercept: true).KeyChar;
            // }
            //catch (IndexOutOfRangeException) //If the input line is empty, we ask the users for some input.
            //{
            //    Console.Clear();
            //    Console.WriteLine("Please enter some input!");
            //}
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

    private static void ClearConsole()
    {
        if (clearConsole) Console.Clear();
    }

    private static void Settings()
    {
        throw new NotImplementedException();
    }

    private static void LoadLibraryFromFile()
    {
        library = JsonSerializer.Deserialize<Library>(File.ReadAllText("library.json"));
    }

    private static void SaveLibraryToFile()
    {
        File.WriteAllText("library.json", JsonSerializer.Serialize(library));
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
                +"\n1. Update Availability"
                +"\n2. Remove Book"
                +"\n0. Return To Main Menu");
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
        Console.WriteLine($"Would you like to {(book.Available ? "check it out" : "return it")}(Y//N)");

        ChooseYesOrNo(book);

    }

    private static void ChooseYesOrNo(Book book)
    {
        bool validInput = false;
        while (!validInput)
        {
            char input = ' ';
            input = Console.ReadKey(intercept: true).KeyChar;
            switch (input)
            {
                case 'Y':
                    book.CheckBookInAndOut();
                    validInput = true;
                    break;
                case 'N':
                    validInput = true;
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
                        RemoveBook(book);
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
        else { ChooseYesOrNo(book); }
        
    }

    private static void ListAllBooks()
    {
        for (int i = 0; i < library.collection.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {library.collection[i].Title}");
        }
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

    //private static Book SelectBookByISBN()
    //{
    //    string inputString = "";
    //    while (true)
    //    {
    //        Console.Clear();

    //        Console.WriteLine("Type the ISBN of the book you would whant to remove, press enter to select the first option in list. ");
    //        Console.WriteLine(inputString);
    //        IEnumerable<Book> q1 = library.collection.Where(book => book.ISBN.StartsWith(inputString));

    //        foreach (Book book in q1)
    //        {
    //            Console.WriteLine(book.ISBN);
    //        }
    //        ConsoleKeyInfo inputKey = Console.ReadKey(intercept: true);
    //        if (inputKey.Key == ConsoleKey.Enter)
    //        {
    //            return q1.First();
    //        }
    //        if (inputKey.Key == ConsoleKey.Escape)
    //        {
    //            MainMenu();
    //        }
    //        char inputChar = inputKey.KeyChar;
    //        inputString += inputChar;

    //    }
    //}

    //private static Book SelectBookOld(Func<Book, string> selector)
    //{
    //    string inputString = "";
    //    while (true)
    //    {
    //        Console.Clear();

    //        Console.WriteLine($"Type the Title of the book you want to remove, press Enter to select the first option in list.");
    //        Console.WriteLine(inputString);

    //        // Use the selector to query the library
    //        IEnumerable<Book> q1 = library.collection
    //            .Where(book => selector(book).StartsWith(inputString, StringComparison.OrdinalIgnoreCase));

    //        foreach (Book book in q1)
    //        {
    //            Console.WriteLine(selector(book));
    //        }

    //        ConsoleKeyInfo inputKey = Console.ReadKey(intercept: true);
    //        if (inputKey.Key == ConsoleKey.Enter)
    //        {
    //            return q1.FirstOrDefault();
    //        }
    //        if (inputKey.Key == ConsoleKey.Escape)
    //        {
    //            MainMenu();
    //        }

    //        char inputChar = inputKey.KeyChar;
    //        inputString += inputChar;
    //    }
    //}

    private static Book SelectBook(Expression<Func<Book, string>> selectorExpression)
    {
        var selector = selectorExpression.Compile();
        string propertyName = GetPropertyName(selectorExpression);
        string inputString = "";
        while (true)
        {
            Console.Clear();

            Console.WriteLine($"Type the {propertyName} of the book you want to select, press Enter to select the first option in list.");
            Console.WriteLine(inputString);

            // Use the selector to query the library
            IEnumerable<Book> q1 = library.collection
                .Where(book => selector(book).StartsWith(inputString, StringComparison.OrdinalIgnoreCase));

            foreach (Book book in q1)
            {
                Console.WriteLine(selector(book));
            }

            ConsoleKeyInfo inputKey = Console.ReadKey(intercept: true);
            if (inputKey.Key == ConsoleKey.Enter)
            {
                return q1.FirstOrDefault();
            }
            if (inputKey.Key == ConsoleKey.Escape)
            {
                MainMenu();
            }

            char inputChar = inputKey.KeyChar;
            inputString += inputChar;
        }
    }

    private static string GetPropertyName(Expression<Func<Book, string>> expression)
    {
        switch (expression.Body)
        {
            case MemberExpression memberExpr:
                // Simple property access like b => b.Title
                return memberExpr.Member.Name;

            case MethodCallExpression methodCallExpr:
                // Method call like b => b.Author.ToString()
                if (methodCallExpr.Object is MemberExpression objMember)
                {
                    return objMember.Member.Name;
                }
                break;

                //case UnaryExpression unaryExpr when unaryExpr.Operand is MemberExpression member:
                //    // Handles cases where there's a conversion/cast
                //    return member.Member.Name;
        }

        throw new InvalidOperationException("Unsupported expression type for property name extraction.");
    }

    //private static Book SelectBookByTitle()
    //{
    //    string inputString = "";
    //    while (true)
    //    {
    //        Console.Clear();

    //        Console.WriteLine("Type the Title of the book you would whant to remove, press enter to select the first option in list. ");
    //        Console.WriteLine(inputString);
    //        IEnumerable<Book> q1 = library.collection.Where(book => book.Title.StartsWith(inputString));

    //        foreach (Book book in q1)
    //        {
    //            Console.WriteLine(book.Title);
    //        }
    //        ConsoleKeyInfo inputKey = Console.ReadKey(intercept: true);
    //        if (inputKey.Key == ConsoleKey.Enter)
    //        {
    //            return q1.First();
    //        }
    //        if (inputKey.Key == ConsoleKey.Escape)
    //        {
    //            MainMenu();
    //        }
    //        char inputChar = inputKey.KeyChar;
    //        inputString += inputChar;

    //    }
    //}
    private static void RemoveBookConfirmation(Book book)
    {

        ClearConsole();
        Console.WriteLine($"You have selected the following book:\n"
        + book.ToString());
        bool validInput = false;
        

    }

    private static void RemoveBook(Book book)
    {
        Console.WriteLine($"{book.Title} has been removed");
        //book.Author.Books.Remove(book);
        library.collection.Remove(book);

    }


    private static void AddBook()
    {
        string title = AddTitle();
        Author author = SetAuthor();
        string cathegory = SetCathegory();
        string ISBN = AddISBN();
        Book book = new Book(title, author, cathegory, ISBN);
        library.collection.Add(book);
        //author.Books.Add(book);
    }

    private static string AddISBN()
    {
        Console.WriteLine("Enter the ISBN of the book");
        string input = Console.ReadLine();
        return input;
    }

    private static string SetCathegory()
    {

        string message = $"Choose cathegory from list using numbers 1 to {library.cathegorys.Count}. Write 'new' if cathegory is not in list and you would like to add a cathegory\n";
        Console.WriteLine(message);
        for (int i = 0; i < library.cathegorys.Count; i++)
        {

            Console.WriteLine($"{i + 1}. {library.cathegorys[i]}");
        }
        while (true)
        {
            string input = Console.ReadLine();
            if (input == "new") return AddCathegory();
            else if (Int32.TryParse(input, out int authorSelection))
            {
                try
                {
                    return library.cathegorys[authorSelection - 1];
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Number was out of range.");
                    Console.WriteLine(message);
                }
            }
            else
            {
                Console.WriteLine(message);
            }

        }

    }

    private static string AddCathegory()
    {
        Console.WriteLine("Write the Cathegory you would like to add.");
        string input = Console.ReadLine();
        library.cathegorys.Add(input);
        return input;
    }

    private static Author SetAuthor()
    {
        string errorMessage = $"Choose author from list using numbers 1 to {library.authors.Count}. Write 'new' if author is not in list and you would like to add an author\n";
        Console.WriteLine("Choose author from list using numbers. Write 'new' if author is not in list and you would like to add an author");
        for (int i = 0; i < library.authors.Count; i++)
        {

            Console.WriteLine($"{i + 1}. {library.authors[i].ToString()}");
        }
        while (true)
        {
            string input = Console.ReadLine();
            if (input == "new") return AddAuthor();
            else if (Int32.TryParse(input, out int authorSelection))
            {
                try
                {
                    return library.authors[authorSelection - 1];
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Number was out of range.");
                    Console.WriteLine(errorMessage);
                }
            }
            else
            {
                Console.WriteLine(errorMessage);
            }

        }
    }


    private static Author AddAuthor()
    {
        Console.WriteLine("Enter the authors first name:");
        string firstName = Console.ReadLine();
        Console.WriteLine("Enter the authors last name:");
        string lastName = Console.ReadLine();
        Author author = new Author(firstName, lastName);
        library.authors.Add(author);
        return author;
    }

    private static string AddTitle()
    {
        Console.WriteLine("Enter the books Title:");
        string title = Console.ReadLine();
        return title;
    }
}
