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
            
        }



        static void Main(string[] args)
        {
            string filePath = args[0];

            if (!File.Exists(filePath))
                throw new FileNotFoundException();




            var filestream = new FileStream(filePath,FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
            var file = new StreamReader(filestream);
            string lineText;


            while ((lineText = file.ReadLine()) != null)
            {


                var lines = lineText.Split(';').ToList();


                if(lines.Count<9)
                    continue;






                Debugger.Break();
            }




        }
    }
}
