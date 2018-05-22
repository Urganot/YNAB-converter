using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace YNAB_Converter
{
    internal abstract class Bank
    {
        /// <summary>
        /// Identifier for bank
        /// </summary>
        public static string Identifier;

        /// <summary>
        /// Converts a StreamReader to a List of YnabLine
        /// </summary>
        /// <param name="file">Streamreader of the input file</param>
        /// <returns>List of YnabLines</returns>
        public abstract List<YnabLine> ParseLines(StreamReader file);
    }
}
