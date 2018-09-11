using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge
{
    class ParserFactory
    {
        //gets the appropriate parser object
        public static Parser getParser(string parseType)
        {
            // returns parser (XML, JSON, DB etc..)
            Parser parser = null;
            if (parseType.Equals(".xml"))
            {
                parser = new XMLParser();
            }

            return parser;
        }
    }
}
