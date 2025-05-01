using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiconC_Slutuppgift_SmartBook
{
    public struct Cathegory
    {
        public Cathegory()
        {
            
        }
        public Cathegory(string name)
        {
             Name = name;
        }
        public string Name { get; set; }
    }
}
