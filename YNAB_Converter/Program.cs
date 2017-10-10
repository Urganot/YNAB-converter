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
            var outPath = args.Length == 2 ? args[1] : Path.GetDirectoryName(filePath) +Path.DirectorySeparatorChar+Path.GetFileNameWithoutExtension(filePath)+"_YNAB"+Path.GetExtension(filePath);

            if (!File.Exists(filePath))
                throw new FileNotFoundException();


            var ynab = new YNABFile(outPath);

            var filestream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var file = new StreamReader(filestream);
            string line;

            
            while ((line = file.ReadLine()) != null)
            {


                var columns = line.Split(';').ToList();


                if (columns.Count < 9)
                    continue;

                if (!double.TryParse(columns[(int)Columns.Betrag], out double betrag))
                    continue;

                ynab.Lines.Add(new YNABLine
                {
                    Date = Convert.ToDateTime(columns[(int)Columns.Valuta]),
                    Payee = columns[(int)Columns.Auftraggeber],
                    Memo = columns[(int)Columns.Verwendungszweck],
                    Outflow = betrag < 0 ? Math.Abs(betrag) : 0,
                    Inflow = betrag < 0 ? 0 : betrag,
                });

            }

            ynab.Save();


        }
    }
}
