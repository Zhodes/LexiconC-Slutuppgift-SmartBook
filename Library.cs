using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiconC_Slutuppgift_SmartBook
{
    class Library
    {
        public List<Book> collection = new List<Book>();
        public List<Author> authors = new List<Author>();
        private List<string> cathegorys = new List<string>();

        public void TrySetISBN(Book book, string ISBN)
        {
            if (collection.Any(b => b.ISBN == ISBN))
            {
                throw new ArgumentException("There already exist a book with that ISBN");
            }
            else
            {
                book.ISBN = ISBN; 
            }


        }

        public void TryAddCathegory(string cathegory)
        {
            if (cathegorys.Any(b => b.Equals(cathegory)))
            {
                throw new ArgumentException("That title already exists");
            }
            else cathegorys.Add(cathegory);
        }

        public List<String> Cathegorys
        {
            get
            {
                return cathegorys;
            }

            set
            {
                //if (cathegorys.Any(b => b.Equals(value)))
                //{
                //    throw new ArgumentException("That title already exists");
                //}
                //else cathegorys.Add(value);
                cathegorys = value;
            }
        }

    }
}
