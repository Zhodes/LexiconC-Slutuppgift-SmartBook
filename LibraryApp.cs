using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LexiconC_Slutuppgift_SmartBook
{
    static class LibraryApp
    {
        static public void MainMenu()
        {
            while (true)
            {
                Console.WriteLine("Please navigate through the menu by inputting the number \n(1, 2, 3 ,4, 0) of your choice"
                    + "\n1. Add Book"
                    + "\n2. Remove Book"
                    + "\n3. List All Books"
                    + "\n4. Search"
                    + "\n5. Set Availability Status Of Book"
                    + "\n6. Save Library To File"
                    + "\n7. Load Library From File"
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
                if (input != '0') Console.Clear();
                Console.WriteLine($"you pressed {input}");
                switch (input)
                {
                    case '1':
                        AddBook();
                        break;
                    case '2':
                        RemoveBook();
                        break;
                    case '3':
                        ListAllBooks();
                        break;
                    case '4':
                        SearchInLibrary();
                        break;
                    case '5':
                        SetAvailabilityStatusOfBook();
                        break;
                    case '6':
                        SaveLibraryToFile();
                        break;
                    case '7':
                        LoadLibraryFromFile();
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

        private static void LoadLibraryFromFile()
        {
            throw new NotImplementedException();
        }

        private static void SaveLibraryToFile()
        {
            throw new NotImplementedException();
        }

        private static void SetAvailabilityStatusOfBook()
        {
            throw new NotImplementedException();
        }

        private static void SearchInLibrary()
        {
            throw new NotImplementedException();
        }

        private static void ListAllBooks()
        {
            throw new NotImplementedException();
        }

        private static void RemoveBook()
        {

        }

        private static void AddBook()
        {
            
        }
    }
}
