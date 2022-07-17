using System;
using System.Collections.Generic;
using System.Text;

namespace IBKR
{
    class Symbol
    {
        public Symbol()
        {
            

            string[] symbols = { "EUR", "GBP", "CAD", "CHF", "AUD" };
            List<String> symbolList = new List<string>();

            foreach (string a in symbols)
                Console.WriteLine(a);
        }

        

}
}
