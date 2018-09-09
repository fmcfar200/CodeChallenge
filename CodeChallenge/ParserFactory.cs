using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge
{
    class ParserFactory
    {
        public static Parser getParser(string parseType)
        {
            Parser parser = null;
            if (parseType.Equals(".xml"))
            {
                parser = new XMLParser();
            }

            return parser;
        }
    }
}
