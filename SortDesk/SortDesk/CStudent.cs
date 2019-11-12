using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SortDesk
{
    class CStudent
    {
         public CStudent()
        {
        }

         public CStudent(string name)
         {
             this.Name = name;
         }        
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
