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
        public static EmissionsFactor eFactor = new EmissionsFactor();
        public static ValueFactor vFactor = new ValueFactor();

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

          
            foreach(WindGenerator gen in windGenerators)
            {
                Console.WriteLine(gen.name);
                foreach(Day day in gen.generations)
                {
                    Console.Write(day.date + "\n" + day.energy + "\n" + day.price + "\n"); 
                }
                Console.WriteLine("-------------------------------");
                Console.WriteLine("-------------------------------");

            }

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
                dataParser.parseStaticData(dataPath, dataFileInfo.Name, eFactor.GetType().Name, out eFactor.high, out eFactor.medium, out eFactor.low);
                dataParser.parseStaticData(dataPath, dataFileInfo.Name, vFactor.GetType().Name, out vFactor.high, out vFactor.medium, out vFactor.low);

            }

        }


    }
}
