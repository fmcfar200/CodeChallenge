using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Xml;

namespace CodeChallenge
{
    class Program
    {
       
        public static List<WindGenerator> windGenerators = new List<WindGenerator>();
        public static List<GasGenerator> gasGenerators = new List<GasGenerator>();
        public static List<CoalGenerator> coalGenerators = new List<CoalGenerator>();

        static void Main(string[] args)
        {

            GetFactorData();

            //run 
            string inputPath = ConfigurationSettings.AppSettings.Get("InputDir");
            do
            {
                Console.WriteLine("Directory Empty: No Input File Found");
            }
            while (IsDirectoryEmpty(inputPath));


            if (!IsDirectoryEmpty(inputPath))
            {
                DirectoryInfo dInfo = new DirectoryInfo(inputPath);
                foreach(FileInfo fileInfo in dInfo.GetFiles())
                {
                    Parser parser = ParserFactory.getParser(fileInfo.Extension);
                    parser.parseReport(inputPath, fileInfo.Name, out windGenerators, out gasGenerators, out coalGenerators);
                }
            }

          
            OutputGenerator og = new OutputGenerator(windGenerators, gasGenerators, coalGenerators);
            og.WriteToXML();
        }


        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private static void GetFactorData()
        {
            string dataPath = ConfigurationSettings.AppSettings.Get("ReferenceDataPath");
            DirectoryInfo refDataInfo = new DirectoryInfo(dataPath);

            foreach (FileInfo dataFileInfo in refDataInfo.GetFiles())
            {
                Console.WriteLine(dataFileInfo.Name);


                Parser dataParser = ParserFactory.getParser(dataFileInfo.Extension);
                dataParser.parseStaticData(dataPath, dataFileInfo.Name, "ValueFactor", out ValueFactor.high, out ValueFactor.medium, out ValueFactor.low);
                dataParser.parseStaticData(dataPath, dataFileInfo.Name, "EmissionsFactor", out EmissionsFactor.high, out EmissionsFactor.medium, out EmissionsFactor.low);

            }

        }


    }
}
