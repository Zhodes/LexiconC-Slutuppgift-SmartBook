using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiconC_Slutuppgift_SmartBook
{
    public class Book
    {

        
        public Book()
        {
            Available = true;
        }

        public Book(string title, Author author, Category cathegory)
        {
            Title = title;
            Author = author;
            Category = cathegory;
            Available = true;
            
        }

        public Book(string title, Author author, Category cathegory, string Isbn)
        {
            Title = title;
            Author = author;
            Category = cathegory;
            Available = true;
            ISBN = Isbn;
        }
        private string isbn = string.Empty;
        public string ISBN
        {
            get { return isbn; }
            set 
            {
                if (value.All(char.IsDigit))
                {
                    if (value.Length == 10 | value.Length == 13)
                    {
                            isbn = value;
                    }
                    else throw new ArgumentException("ISBN must be 10 or 13 digits long.");


                }
                else throw new ArgumentException("ISBN must contain only digits.");
                
                
            
            }
        }

        public string Title { get; set; }
        public Author Author { get; set; }
        public Category Category { get; set; }
        public bool Available { get; set; }

        public string AvailableAsString { get { return $"{(Available ? "Available" : "Checked Out")}"; } }

        public override string ToString()
        {
            return ($"Title: {Title}"
        + $"\nAuthor: {Author.ToString()}"
        + $"\nCathegory: {Category.Name}"
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
