using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiconC_Slutuppgift_SmartBook
{
    static class UserInterface
    {
        static public void FullBookLister(List<Book> bookList, bool showSelectionArrow = false, int selection = 0)
        {
            Console.Clear();
            string tableHead = Rows(nameof(Book.Title), nameof(Book.Author), nameof(Book.ISBN), nameof(Book.Category), nameof(Book.Available));
            string dividerRow = "";
            for (int i = 0; i < tableHead.Length; i++) { dividerRow += "-"; }
            Console.WriteLine(tableHead);
            Console.WriteLine(dividerRow);
            for (int i = 0; i < bookList.Count; i++)
            {
                {
                    Book book = bookList[i];
                    string title = book.Title;
                    if (showSelectionArrow)
                    {
                        if (selection == i)
                        {

                            title = "> " + book.Title;
                        }
                        else
                        {
                            title = "  " + book.Title;
                        }
                    }
                    Console.WriteLine(Rows(title, book.Author.ToString(), book.ISBN, book.Category.Name, book.AvailableAsString));
                }
            }

        }

        static string Rows(string title, string author, string ISBN, string category, string Available)
        {
            int titleLength = 40;
            int authorLength = 25;
            int categoryLength = 20;

            if (title.Length > titleLength) title = title.Substring(0, titleLength - 3) + "...";
            if (author.Length > authorLength) author = author.Substring(0, authorLength - 3) + "...";
            if (category.Length > categoryLength) category = category.Substring(0, categoryLength - 3) + "...";
            return title.PadRight(titleLength) + author.PadRight(authorLength) + ISBN.PadRight(15) + category.PadRight(categoryLength) + Available.PadRight(11);
        }

        public static void RenderSelectionMenu<T>(List<T> filteredItems, Func<T, string> displaySelector, string prompt, string inputString, int selection, bool addNewAvailable, string propertyName)
        {
            Console.Clear();
            Console.WriteLine(prompt);


            if(typeof(T) == typeof(Book))
            {
                UserInterface.FullBookLister(filteredItems.Cast<Book>().ToList(), true, selection);
            }
            else
            {
                for (int i = 0; i < filteredItems.Count; i++)
                {
                    Console.WriteLine($"{(selection == i ? "> " : "  ")}{displaySelector(filteredItems[i])}");
                }

            }

            if (addNewAvailable)
            {
                Console.WriteLine($"{(selection == filteredItems.Count ? "> " : "  ")}Add new {propertyName}");
            }
            Console.WriteLine(inputString);
        }

        public static SelectionAction HandleKeyInput(ref int selection, ref string inputString)
        {
            ConsoleKeyInfo inputKey = Console.ReadKey(intercept: true);
            switch (inputKey.Key)
            {
                case ConsoleKey.Enter:
                    return SelectionAction.Submit;

                case ConsoleKey.Escape:
                    return SelectionAction.Cancel;

                case ConsoleKey.Backspace:
                    if (inputString.Length > 0) inputString = inputString.Remove(inputString.Length - 1);
                    break;

                case ConsoleKey.DownArrow:
                    selection++;
                    break;

                case ConsoleKey.UpArrow:
                    selection--;
                    break;

                default:
                    inputString += inputKey.KeyChar;
                    break;
            }

            return SelectionAction.Continue;
        }
    }
}
