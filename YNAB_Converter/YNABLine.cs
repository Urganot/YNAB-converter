using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YNAB_Converter
{
    internal class YnabLine
    {

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

        public double Outflow
        {
            get => Math.Abs(_outflow);
            set => _outflow = value;
        }

        public double Inflow
        {
            get => Math.Abs(_inflow);
            set => _inflow = value;
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private double _outflow;
        private double _inflow;
        private string _payee;
        private string _memo;
        private string _category;
        private DateTime _date;

        public YnabLine()
        {
            _payee = _memo = _category = "";
            _outflow = _inflow = 0;
        }

        public string AsText()
        {
            var nfi = new NumberFormatInfo { NumberDecimalSeparator = "." };

            return $@"{Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)},{Payee},{Category},{Memo},{Outflow.ToString(nfi)},{Inflow.ToString(nfi)}";
        }

        private static string Escape(string text)
        {
            return text.Replace(",", ";");
        }

    }
}
