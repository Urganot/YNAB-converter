using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YNAB_Converter
{
    class YNABFile
    {

        public List<YNABLine> Lines;

        public YNABFile()
        {
            Lines = new List<YNABLine>();
        }

        public void Save()
        {

        }
    }
}
