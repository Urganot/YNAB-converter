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
        /// <summary>
        /// Payment receiver/sender
        /// </summary>
        public string Payee
        {
            get => Escape(_payee);
            set => _payee = value;
        }

        /// <summary>
        /// Transaction category
        /// </summary>
        public string Category
        {
            get => Escape(_category);
            set => _category = value;
        }

        /// <summary>
        /// Note for transaction
        /// </summary>
        public string Memo
        {
            get => Escape(_memo);
            set => _memo = value;
        }

        /// <summary>
        /// Outgoing amount
        /// </summary>
        public double Outflow
        {
            get => Math.Abs(_outflow);
            set => _outflow = value;
        }

        /// <summary>
        /// Incoming amount
        /// </summary>
        public double Inflow
        {
            get => Math.Abs(_inflow);
            set => _inflow = value;
        }

        /// <summary>
        /// Transaction date
        /// </summary>
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

        /// <summary>
        /// Constructor
        /// </summary>
        public YnabLine()
        {
            _payee = _memo = _category = "";
            _outflow = _inflow = 0;
        }

        /// <summary>
        /// Converts a line to a string
        /// </summary>
        /// <param name="numberDecimalSeparator">Optional: Number decimal seperator</param>
        /// <returns>YNAB line</returns>
        public string AsText(string numberDecimalSeparator = ".")
        {
            var nfi = new NumberFormatInfo { NumberDecimalSeparator = numberDecimalSeparator };

            return $@"{Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)},{Payee},{Category},{Memo},{Outflow.ToString(nfi)},{Inflow.ToString(nfi)}";
        }

        /// <summary>
        /// Replaces commas with semicolon to not break csv format
        /// </summary>
        /// <param name="text">Text to escape</param>
        /// <returns>Escaped text</returns>
        private static string Escape(string text)
        {
            return text.Replace(",", ";");
        }

    }
}
