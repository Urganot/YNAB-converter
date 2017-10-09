using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YNAB_Converter
{
    class YNABLine
    {
        public DateTime Date;
        public string Payee;
        public string Category;
        public string Memo;
        public float Outflow;
        public float Inflow;

    }
}
