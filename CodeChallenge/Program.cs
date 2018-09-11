using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Configuration;

namespace CodeChallenge
{
    class Program
    {
        //collections of different generators
        public static List<WindGenerator> windGenerators = new List<WindGenerator>();
        public static List<GasGenerator> gasGenerators = new List<GasGenerator>();
        public static List<CoalGenerator> coalGenerators = new List<CoalGenerator>();

        static void Main(string[] args)
        {

            Factor.GetFactorData();

            string inputPath = ConfigurationSettings.AppSettings.Get("InputDir"); // input path for xml file detection
            
            //loops while the directory is empty          
            while (IsDirectoryEmpty(inputPath))
            {
                Console.WriteLine("Directory Empty: No Input File Found");
            }

            //if directory is not empty
            if (!IsDirectoryEmpty(inputPath))
            {
                //gets the directory info
                DirectoryInfo dInfo = new DirectoryInfo(inputPath);
               
                    foreach (FileInfo fileInfo in dInfo.GetFiles())
                    {
                        //if the current files name contains report then parse
                        if (fileInfo.Name.Contains("Report"))
                        {
                           
                           Console.WriteLine("Input Data Found: " + fileInfo.Name);

                           //gets a parser from the factory that matched the extenion type (XML, DB or JSON etc.)
                           Parser parser = ParserFactory.getParser(fileInfo.Extension);
                           parser.parseReport(inputPath, fileInfo.Name, out windGenerators, out gasGenerators, out coalGenerators); // parse a report
                            
                        }

                    }
                //generates an output file with all the collected data
                OutputGenerator og = new OutputGenerator(windGenerators, gasGenerators, coalGenerators);
                og.WriteToXML();
            }

                   
        }

        //returns bool for directory being empty
        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

      


    }
}
