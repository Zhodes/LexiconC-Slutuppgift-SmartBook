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

    static public void loadStartingLibrary()
    {
        library.LoadLibraryFromFile();
    }
    static public void MainMenu()
    {
        Console.Clear();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Please navigate through the menu by pressing the number of your choice"
                + "\n1. Add Book"
                + "\n2. List All Books"
                + "\n3. Search"
                + "\n4. Save Library To File"
                + "\n5. Load Library From File"
                + "\n0. Exit the application");
            char input = ' ';
            input = Console.ReadKey(intercept: true).KeyChar;
            switch (input)
            {
                case '1':
                    AddBook();
                    break;
                case '2':
                    ListAllBooks();
                    break;
                case '3':
                    SearchInLibrary();
                    break;
                case '4':
                    SaveLibraryToFile();
                    break;
                case '5':
                    LoadLibraryFromFile();
                    break;
                case '0':
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Please enter some valid input (0, 1, 2, 3, 4, 5, 0)");
                    break;
            }
        }
    }

    private static void SaveLibraryToFile()
    {
        Console.Clear();
        Console.WriteLine("Save Library to File?");
        if (ChooseYesOrNo(true))
        {
            library.SaveLibraryToFile();
            Console.WriteLine("Library Saved");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
    static public void LoadLibraryFromFile()
    {
        Console.Clear();
        Console.WriteLine("Load Library From File?");
        if (ChooseYesOrNo(true))
        {
            library.LoadLibraryFromFile();
            Console.WriteLine("Library Loaded");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
    private static void ListAllBooks()
    {
        UserInterface.FullBookLister(library.ListAllBooks());
        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
    }

    private static void SearchInLibrary()
    {
        Console.Clear();
        Console.WriteLine("Navigate by pressing digits"
            + "\n1. Search by Title"
            + "\n2. Search by Author"
            + "\n3. Search by ISBN"
            + "\n0. Return to main menu"
            );
        bool validSelection = false;

        while (!validSelection)
        {
            char input = Console.ReadKey(intercept: true).KeyChar;
            switch (input)
            {
                case '1':
                    SearchByTitle();
                    validSelection = true;
                    break;
                case '2':
                    SearchByAuthor();
                    validSelection = true;
                    break;
                case '3':
                    SearchByISBN();
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

    private static void SearchByISBN()
    {
        Book book = SelectBook(b => b.ISBN);
        Console.WriteLine(book.ToString());
        BookMenu(book);
    }

    private static void SearchByAuthor()
    {
        Book book = SelectBook(b => b.Author.ToString());
        Console.WriteLine(book.ToString());
        BookMenu(book);
    }

    private static void SearchByTitle()
    {

        Book book = SelectBook(b => b.Title);
        Console.WriteLine(book.ToString());
        BookMenu(book);
    }

    private static void BookMenu(Book book)
    {
        Console.Clear();
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
        if (ChooseYesOrNo()) book.CheckBookInAndOut();
    }

    private static bool ChooseYesOrNo(bool secure = false)
    {
        if (secure)
        {
            Console.WriteLine($"Yes/No:");
            while (true)
            {
                string input = Console.ReadLine();
                switch (input)
                {
                    case "Yes":
                        return true;
                    case "No":
                        return false;
                    default:
                        Console.WriteLine("write Yes or No to make decission.");
                        break;

                }
            }
        }
        else
        {
            Console.WriteLine($"(Y)es/(N)o:");
            while (true)
            {
                string input = Console.ReadKey(intercept: true).KeyChar.ToString().ToUpper();
                switch (input)
                {
                    case "Y":
                        return true;
                    case "N":
                        return false;
                    default:
                        Console.WriteLine("press Y for 'Yes' or N for 'No' to make decission.");
                        break;
                }
            }
        }

    }

    private static Book SelectBook(Expression<Func<Book, string>> selectorExpression) => SelectItem(library.Collection, selectorExpression, "Type the title of the book to select:");

    private static Author SelectOrAddAuthor() => SelectItem(library.Authors, a => a.ToString(), "Type the name of the author:", AddAuthor);

    private static Category SelectOrAddCategory() => SelectItem(library.Categorys, c => c.Name, "Type the category:", AddCategory);


    private static T SelectItem<T>(IEnumerable<T> source, Expression<Func<T, string>> selectorExpression, string prompt, Func<T> onAddNew = null)
    {
        var Selector = selectorExpression.Compile();
        string inputString = "";
        int selection = 0;
        bool addNewAvailable = onAddNew != null;
        string propertyName = GetPropertyName(selectorExpression);

        while (true)
        {

            List<T> filtered = FilterItemInSourceBySelector(source, Selector, inputString);
            int selectionHighestValue = addNewAvailable ? filtered.Count : filtered.Count - 1;
            if (selection > selectionHighestValue) selection = 0;
            if (selection < 0) selection = selectionHighestValue;


            UserInterface.RenderSelectionMenu(filtered, Selector, prompt, inputString, selection, addNewAvailable, propertyName);


            SelectionAction action = UserInterface.HandleKeyInput(ref selection, ref inputString);

            switch (action)
            {
                case SelectionAction.Submit:
                    return selection == filtered.Count ? onAddNew() : filtered[selection];
                case SelectionAction.Cancel:
                    MainMenu();
                    break;
            }
        }
    }

    private static List<T> FilterItemInSourceBySelector<T>(IEnumerable<T> source, Func<T, string> Selector, string inputString)
    {
        return source
                        .Where(item => Selector(item)
                        .StartsWith(inputString, StringComparison.OrdinalIgnoreCase))
                        .OrderBy(Selector)
                        .ToList();
    }

    private static string GetPropertyName<T>(Expression<Func<T, string>> expression)
    {
        switch (expression.Body)
        {
            case MemberExpression memberExpr:
                if (typeof(T).Name == "Category")
                    return typeof(T).Name;
                else
                    return memberExpr.Member.Name;
            case MethodCallExpression methodCallExpr:
                if (methodCallExpr.Object is MemberExpression objMember)
                    return objMember.Member.Name;

                else if (methodCallExpr.Method.ReturnType.Name == "String")
                    return typeof(T).Name;
                break;
        }

        throw new InvalidOperationException("Unsupported expression type for property name extraction.");
    }

    private static void RemoveBookConfirmation(Book book)
    {
        Console.Clear();
        Console.WriteLine($"You have selected the following book:\n"
        + book.ToString()
        + "\n Would you like to remove it?");
        if (ChooseYesOrNo(true)) library.RemoveBook(book);

    }

    private static void AddBook()
    {
        Book book = new Book();
        Console.Clear();
        AddTitle(book);
        book.Author = SelectOrAddAuthor();
        book.Category = SelectOrAddCategory();
        AddISBN(book);
        library.AddBookToCollection(book);
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
                library.SetISBNWithValidation(book, input);
                isValidISBN = true;
            }
            catch (ArgumentException e) { Console.WriteLine(e.Message); }
        }
    }

    private static Category AddCategory()
    {
        Console.WriteLine("Write the Category you would like to add.");
        bool isvalidInput = false;
        Category category = new Category();
        while (!isvalidInput)
        {
            category.Name = Console.ReadLine();
            try
            {
                library.TryAddCategory(category);
                isvalidInput = true;

            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                SelectOrAddCategory();

            }

        }
        return category;
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
