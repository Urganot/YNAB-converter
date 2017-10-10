using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YNAB_Converter
{
    class YNABFile
    {
        public List<YNABLine> Lines;

        public string OutputPath { get; }

        public YNABFile(string outputPath)
        {
            OutputPath = outputPath;
            Lines = new List<YNABLine>();
        }

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

        internal string Header()
        {
            return $@"{nameof(YNABLine.Date)},{nameof(YNABLine.Payee)},{nameof(YNABLine.Category)},{nameof(YNABLine.Memo)},{nameof(YNABLine.Outflow)},{nameof(YNABLine.Inflow)}";

        }
    }
}