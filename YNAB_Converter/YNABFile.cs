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
        public List<YnabLine> Lines;

        public string OutputPath { get; }

        public YnabFile(string outputPath)
        {
            OutputPath = outputPath;
            Lines = new List<YnabLine>();
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
            return $@"{nameof(YnabLine.Date)},{nameof(YnabLine.Payee)},{nameof(YnabLine.Category)},{nameof(YnabLine.Memo)},{nameof(YnabLine.Outflow)},{nameof(YnabLine.Inflow)}";
        }
    }
}