using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiconC_Slutuppgift_SmartBook
{
    class Book
    {
        static int ISBNCount = 0;
        public Book(string title, Author author, string cathegory)
        {
            Title = title;
            Author = author;
            Cathegory = cathegory;
            Available = true;
            ISBNCount ++;
            ISBN = ISBNCount.ToString();
        }

        public Book(string title, Author author, string cathegory, string Isbn)
        {
            Title = title;
            Author = author;
            Cathegory = cathegory;
            Available = true;
            ISBN = Isbn;
        }

        public string ISBN { get; set; }
        public string Title { get; set; }
        public Author Author { get; set; }
        public string Cathegory { get; set; }
        public bool Available { get; set; }

        public string AvailableAsString { get { return $"{(Available ? "Available" : "Checked Out")}"; } }

        public override string ToString()
        {
            return ($"Title: {Title}"
        + $"\nAuthor: {Author.ToString()}"
        + $"\nCathegory: {Cathegory}"
        + $"\nISBN: {ISBN}"
        + $"\nAvailability: {AvailableAsString} ");
        }

        public void CheckBookInAndOut()
        {
            Available = !Available;
            Console.WriteLine( $"{Title} has been {(Available ? "returned" : "checked out" )}");
        }
    }
}
