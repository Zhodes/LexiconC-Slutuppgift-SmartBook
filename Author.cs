using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiconC_Slutuppgift_SmartBook
{
    public class Author
    {
        public Author()
        {
            
        }
        public Author(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public override string ToString()
        {
            return $"{LastName}, {FirstName}";
        }
    }
}
