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
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 2)
                ShowHelp();

            var inputFilePath = args[0];

            ErrorHandling(inputFilePath);

            var bank = Banks.Get(GetConfigValue("BankIdentifier"));

            var ynab = new YnabFile(GetOutPath(args, inputFilePath));

            using (var filestream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var file = new StreamReader(filestream))
                {
                    ynab.Lines = bank.ParseLines(file);
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
            Console.WriteLine("Parameter: InputFile [BankIdentifier] [OutputFile]");
            Console.WriteLine("InputFile:\tPath to the file that should be converted.");
            Console.WriteLine("OutputFile:\tPath to the output file. If not specified a path will be determined.");
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
