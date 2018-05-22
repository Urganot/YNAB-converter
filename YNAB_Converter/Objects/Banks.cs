using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YNAB_Converter
{
    class Banks
    {
        /// <summary>
        /// Returns an instance of a bank class depending on the identifier
        /// </summary>
        /// <param name="identifier">A string to identify the bank</param>
        /// <returns>An instance of a specific Bank class</returns>
        public static Bank Get(string identifier)
        {
            var listOfInheritedTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                   from assemblyType in domainAssembly.GetTypes()
                                   where typeof(Bank).IsAssignableFrom(assemblyType)
                                   select assemblyType).ToList();

            var bank = listOfInheritedTypes.Single(x => x.GetField(nameof(Bank.Identifier)).GetValue(x)?.ToString() == identifier);

            return (Bank)Activator.CreateInstance(bank);
        }
    }
}
