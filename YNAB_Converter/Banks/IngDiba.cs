using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YNAB_Converter
{
    class IngDiba : Bank
    {
        /// <summary>
        /// Identifier for bank
        /// </summary>
        public new static string Identifier = "IngDiba";

        /// <summary>
        /// Enum to call columns by name
        /// </summary>
        public enum Columns
        {
            Buchung = 0,
            Valuta = 1,
            Auftraggeber = 2,
            Buchungstext = 3,
            Verwendungszweck = 4,
            Betrag = 7,
            WährungBetrag = 6,
            Saldo = 5,
            WährungSaldo = 8
        }

        /// <summary>
        /// Converts a StreamReader to a List of YnabLine
        /// </summary>
        /// <param name="file">Streamreader of the input file</param>
        /// <returns>List of YnabLines</returns>
        public override List<YnabLine> ParseLines(StreamReader file)
        {
            var lines = new List<YnabLine>();
            string line;

            while ((line = file.ReadLine()) != null)
            {
                var columns = line.Split(';').ToList();

                if (columns.Count != 9)
                    continue;

                if (!double.TryParse(columns[(int)Columns.Betrag], out double amount))
                    continue;

                var ynabLine = new YnabLine
                {
                    Date = Convert.ToDateTime(columns[(int)Columns.Valuta]),
                    Payee = columns[(int)Columns.Auftraggeber],
                    Memo = columns[(int)Columns.Verwendungszweck],
                };

                if (amount > 0)
                    ynabLine.Inflow = amount;
                else
                    ynabLine.Outflow = amount;

                lines.Add(ynabLine);
            }

            return lines;
        }
    }
}
