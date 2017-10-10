using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YNAB_Converter
{
    class Program
    {

        public enum Columns
        {
            Buchung = 0,
            Valuta = 1,
            Auftraggeber = 2,
            Buchungstext = 3,
            Verwendungszweck = 4,
            Betrag = 5,
            WährungBetrag = 6,
            Saldo = 7,
            WährungSaldo = 8
        }



        static void Main(string[] args)
        {
            var filePath = args[0];
            var outPath = args.Length == 2 ? args[1] : Path.GetDirectoryName(filePath) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(filePath) + "_YNAB" + Path.GetExtension(filePath);

            if (!File.Exists(filePath))
                throw new FileNotFoundException();


            var ynab = new YnabFile(outPath);

            var filestream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var file = new StreamReader(filestream);
            string line;


            while ((line = file.ReadLine()) != null)
            {


                var columns = line.Split(';').ToList();


                if (columns.Count < 9)
                    continue;

                if (!double.TryParse(columns[(int)Columns.Betrag], out double amount))
                    continue;


                var inflow = amount > 0;

                var ynabLine = new YnabLine
                {
                    Date = Convert.ToDateTime(columns[(int) Columns.Valuta]),
                    Payee = columns[(int) Columns.Auftraggeber],
                    Memo = columns[(int) Columns.Verwendungszweck],
                };


                if (inflow)
                {
                    ynabLine.Category = "Inflow: To be Budgeted";
                    ynabLine.Inflow = amount;
                }
                else
                {

                    ynabLine.Outflow = amount;

                    if (ynabLine.Payee.Contains("SEQR"))
                    {
                        ynabLine.Category = "SEQR";
                    }else if (ynabLine.Payee.Contains("Kickstarter"))
                    {
                        ynabLine.Category = "Just for Fun: Kickstarter";
                    }
                }

               

                ynab.Lines.Add(ynabLine);

            }

            ynab.Save();


        }
    }
}
