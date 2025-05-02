using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiconC_Slutuppgift_SmartBook
{
    public struct Category
    {
        public Category()
        {
            
        }
        public Category(string name)
        {
             Name = name;
        }
        public string Name { get; set; }
    }
}
