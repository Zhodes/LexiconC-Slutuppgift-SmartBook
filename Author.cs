using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiconC_Slutuppgift_SmartBook
{
    class Author
    {
        public Author(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            //Books = new List<Book>();
        }
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string LastName { get; set; }

        //public List<Book> Books { get; set; }

        public override string ToString()
        {
            return $"{LastName}, {FirstName} {MiddleNames}";
        }
    }
}
