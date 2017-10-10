using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YNAB_Converter
{
    class YNABLine
    {
        public DateTime Date;

        public string Payee
        {
            get => Escape(_payee);
            set => _payee = value;
        }

        public string Category
        {
            get => Escape(_category);
            set => _category = value;
        }

        public string Memo
        {
            get => Escape(_memo);
            set => _memo = value;
        }

        public double Outflow;
        public double Inflow;
        private string _payee;
        private string _memo;
        private string _category;

        public YNABLine()
        {
            _payee = _memo = _category = "";
        }

        public string AsText()
        {
            var nfi = new NumberFormatInfo { NumberDecimalSeparator = "." };

            return $@"{Date},{Payee},{Category},{Memo},{Outflow.ToString(nfi)},{Inflow.ToString(nfi)}";
        }

        private static string Escape(string text)
        {
            return text.Replace(",", ";");
        }

    }
}
