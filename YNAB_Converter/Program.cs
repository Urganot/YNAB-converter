using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YNAB_Converter
{
    class Program
    {
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
            Betrag = 5,
            WährungBetrag = 6,
            Saldo = 7,
            WährungSaldo = 8
        }



        static void Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 2)
                ShowHelp();

            var inputFilePath = args[0];

            ErrorHandling(inputFilePath);

            var ynab = new YnabFile(GetOutPath(args, inputFilePath));

            using (var filestream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var file = new StreamReader(filestream))
                {
                    ParseLines(ynab, file);
                }
            }

            ynab.Save();
        }

        /// <summary>
        /// Handles errors
        /// </summary>
        /// <param name="inputFilePath"></param>
        private static void ErrorHandling(string inputFilePath)
        {
            if (Path.GetExtension(inputFilePath) != ".csv")
                throw new ArgumentException("Inputfile is not a csv file.");

            if (!File.Exists(inputFilePath))
                throw new FileNotFoundException("InputFile could not be found.");
        }

        /// <summary>
        /// Shows help text 
        /// </summary>
        private static void ShowHelp()
        {
            Console.WriteLine("Parameter: InputFile [OutputFile]");
            Console.WriteLine("InputFile:\tPath to the file that should be converted.");
            Console.WriteLine("OutputFile:\tPath to the output file. If not specified a path will be determined.");
        }

        private static void ParseLines(YnabFile ynab, StreamReader file)
        {
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
                
                ynab.Lines.Add(ynabLine);
            }
        }

        /// <summary>
        /// Returns output path
        /// If specified in input args, returns this output path else determines output path
        /// </summary>
        /// <param name="args">Startup args</param>
        /// <param name="inputFilePath">Pth to the input file</param>
        /// <returns>Output path</returns>
        private static string GetOutPath(string[] args, string inputFilePath)
        {
            return args.Length == 2 ? args[1] : Path.GetDirectoryName(inputFilePath) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(inputFilePath) + "_YNAB" + Path.GetExtension(inputFilePath);
        }

        /// <summary>
        /// Get config Value
        /// </summary>
        /// <param name="configId">Specifies which config value should be returned</param>
        /// <returns>Specified config value</returns>
        public static string GetConfigValue(string configId)
        {
            return ConfigurationManager.AppSettings[configId];
        }
    }
}
