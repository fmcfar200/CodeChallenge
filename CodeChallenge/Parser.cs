using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace CodeChallenge
{
    //parser interface
    public interface Parser
    {
        //parses a report file
        void parseReport(string inputPath, string fileName, out List<WindGenerator> wGen, out List<GasGenerator> gGen, out List<CoalGenerator> cGen);
        //parse factor reference file
        void parseStaticData(string inputPath, string fileName, string factoryType, out double high, out double medium, out double low);
    }

    //xml parser class derived from parser interface
    public class XMLParser: Parser
    {
        
        public void parseStaticData(string inputPath, string fileName, string factorType, out double high, out double medium, out double low)
        {
            //high medium and low factors vlaues
            double h = 0, m = 0, l = 0;

            //creates xml doc object and loads data from the input path with the filename
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(inputPath + fileName);

            XmlNodeList rootNode = xmlDoc.DocumentElement.SelectNodes("/ReferenceData"); //gets the root node

            //loops through each xml node
            foreach(XmlNode n in rootNode)
            {
                XmlNodeList xmlFactors = n.ChildNodes;
                foreach (XmlNode f in xmlFactors)
                {
                    XmlNodeList fChildren = f.ChildNodes;
                    foreach (XmlNode child in fChildren)
                    {
                        if (factorType == child.Name)
                        {
                            //parses the node inner text to double
                            h = Double.Parse(child.SelectSingleNode("High").InnerText);
                            m = Double.Parse(child.SelectSingleNode("Medium").InnerText);
                            l = Double.Parse(child.SelectSingleNode("Low").InnerText);

                        }
                    }
                }
            }
            // out args set to the local values
            high = h;
            medium = m;
            low = l;

        }

        //parser for report data
        void Parser.parseReport(string inputPath, string fileName, out List<WindGenerator> wGen, out List<GasGenerator> gGen, out List<CoalGenerator> cGen)
        {
            //local collects for generators
            List<WindGenerator> windList = new List<WindGenerator>();
            List<GasGenerator> gasList = new List<GasGenerator>();
            List<CoalGenerator> coalList = new List<CoalGenerator>();

            //same code as parseStaticData
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(inputPath + fileName);

            XmlNodeList rootNode = xmlDoc.DocumentElement.SelectNodes("/GenerationReport");

            foreach (XmlNode node in rootNode)
            {
                XmlNodeList genType = node.ChildNodes;
                foreach (XmlNode type in genType)
                {
                    XmlNodeList gens = type.ChildNodes;
                    foreach (XmlNode gen in gens)
                    {
                        //grabs name from the node
                        string name = gen.SelectSingleNode("Name").InnerText;
                        List<Generation> generations = new List<Generation>(); //list of generations

                        XmlNodeList generationNodes = gen.SelectNodes("Generation/Day");
                        foreach (XmlNode dayNode in generationNodes)
                        {
                            //local data for date, enegry and price
                            string date = dayNode.SelectSingleNode("Date").InnerText;
                            double energy = double.Parse(dayNode.SelectSingleNode("Energy").InnerText);
                            double price = double.Parse(dayNode.SelectSingleNode("Price").InnerText);

                            //a new generation is instantiated
                            Generation dayGen = new Generation(date, energy, price);
                            generations.Add(dayGen);   //added to the list of generations                          
                        }

                        //extra properties for gas and coal gens
                        double emissionsRating = 0;
                        double totalHeatInput = 0;
                        double actualNetGeneration = 0;
                        //sets the data if the name is coal or gas
                        if (name.Contains("Gas") || name.Contains("Coal"))
                        {
                            emissionsRating = double.Parse(gen.SelectSingleNode("EmissionsRating").InnerText);
                            if (name.Contains("Coal"))
                            {
                                totalHeatInput = double.Parse(gen.SelectSingleNode("TotalHeatInput").InnerText);
                                actualNetGeneration = double.Parse(gen.SelectSingleNode("ActualNetGeneration").InnerText);

                            }
                        }

                      
                        // sets the value and emissions factor values based on the factor type
                        Generator.ValueFactorType vType;
                        GasGenerator.EmissionFactorType eType;

                        if (name.Contains("Offshore"))
                        {
                            vType = Generator.ValueFactorType.LOW;
                            WindGenerator g = new WindGenerator(name, generations, vType);
                            windList.Add(g);
                        }
                        else if (name.Contains("Onshore"))
                        {
                            vType = Generator.ValueFactorType.HIGH;
                            WindGenerator g = new WindGenerator(name, generations, vType);
                            windList.Add(g);

                        }
                        else if (name.Contains("Gas"))
                        {
                            vType = Generator.ValueFactorType.MEDIUM;
                            eType = GasGenerator.EmissionFactorType.MEDIUM;
                            GasGenerator g = new GasGenerator(name, generations, emissionsRating, vType, eType);
                            gasList.Add(g);
                        }
                        else if (name.Contains("Coal"))
                        {
                            vType = Generator.ValueFactorType.MEDIUM;
                            eType = GasGenerator.EmissionFactorType.HIGH;
                            CoalGenerator g = new CoalGenerator(name, generations, emissionsRating, totalHeatInput, actualNetGeneration, vType, eType);
                            coalList.Add(g);

                        }
                        else
                        {
                            vType = Generator.ValueFactorType.LOW;
                        }

                        
                       
                    }
                }

            }
            //list data is passed to 'out' variables
            wGen = windList;
            gGen = gasList;
            cGen = coalList;


        }


    }
}
