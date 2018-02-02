using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YNAB_Converter
{
    internal class YnabFile
    {
        /// <summary>
        /// Lines in the YNAB file
        /// </summary>
        public List<YnabLine> Lines;

        /// <summary>
        /// Path to the output file
        /// </summary>
        public string OutputPath { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="outputPath">Path to the output file</param>
        public YnabFile(string outputPath)
        {
            OutputPath = outputPath;
            Lines = new List<YnabLine>();
        }

        /// <summary>
        /// Writes output file to disc
        /// </summary>
        public void Save()
        {
            using (var fs = new FileStream(OutputPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (var file = new StreamWriter(fs))
                {
                    file.WriteLine(this.Header());
                    foreach (var ynabLine in Lines)
                    {
                        file.WriteLine(ynabLine.AsText());
                    }
                }
            }

        }

        /// <summary>
        /// Get text for header line
        /// </summary>
        /// <returns>Header line</returns>
        internal string Header()
        {
            return $@"{nameof(YnabLine.Date)},{nameof(YnabLine.Payee)},{nameof(YnabLine.Category)},{nameof(YnabLine.Memo)},{nameof(YnabLine.Outflow)},{nameof(YnabLine.Inflow)}";
        }
    }
}