using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiconC_Slutuppgift_SmartBook
{
    class Book
    {

        
        public Book()
        {
            
        }

        public Book(string title, Author author, string cathegory)
        {
            Title = title;
            Author = author;
            Cathegory = cathegory;
            Available = true;
            
        }

        public Book(string title, Author author, string cathegory, string Isbn)
        {
            Title = title;
            Author = author;
            Cathegory = cathegory;
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
                        //if ()
                        //{

                        //}
                            isbn = value;
                    }
                    else throw new ArgumentException("ISBN must be 10 or 13 digits long.");


                }
                else throw new ArgumentException("ISBN must contain only digits.");
                
                
            
            }
        }


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
