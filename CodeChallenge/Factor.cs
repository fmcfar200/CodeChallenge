using System;
using System.Configuration;
using System.IO;

namespace CodeChallenge
{
    //generic factor class for collecting the reference data
    public static class Factor
    {
        public static void GetFactorData() //gets the factor data for the static values
        {
            
            string dataPath = ConfigurationSettings.AppSettings.Get("ReferenceDataPath"); //reference data path
            DirectoryInfo refDataInfo = new DirectoryInfo(dataPath); //dir info for ref data

            
            foreach (FileInfo dataFileInfo in refDataInfo.GetFiles())
            {
                Console.WriteLine("Reference Data Found: " + dataFileInfo.Name);
                //gets the correct parser from the factory
                Parser dataParser = ParserFactory.getParser(dataFileInfo.Extension);
                //parses static data
                dataParser.parseStaticData(dataPath, dataFileInfo.Name, "ValueFactor", out ValueFactor.high, out ValueFactor.medium, out ValueFactor.low);
                dataParser.parseStaticData(dataPath, dataFileInfo.Name, "EmissionsFactor", out EmissionsFactor.high, out EmissionsFactor.medium, out EmissionsFactor.low);

            }

        }
    }

    //value factor class
    public static class ValueFactor
    {
        //high medium and low values
        public static double high, medium, low;

    
    }
    //emissions factor class
    public static class EmissionsFactor
    {
        public static double high, medium, low;


    }



}
