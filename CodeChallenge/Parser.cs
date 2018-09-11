using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CodeChallenge
{
    public interface Parser
    {
        void parseReport(string inputPath, string fileName, out List<WindGenerator> wGen, out List<GasGenerator> gGen, out List<CoalGenerator> cGen);
        void parseStaticData(string inputPath, string fileName, string factoryType, out double high, out double medium, out double low);
    }


    public class XMLParser: Parser
    {
        public void parseStaticData(string inputPath, string fileName, string factorType, out double high, out double medium, out double low)
        {
            double h = 0, m = 0, l = 0;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(inputPath + fileName);

            XmlNodeList rootNode = xmlDoc.DocumentElement.SelectNodes("/ReferenceData");

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
                            h = Double.Parse(child.SelectSingleNode("High").InnerText);
                            m = Double.Parse(child.SelectSingleNode("Medium").InnerText);
                            l = Double.Parse(child.SelectSingleNode("Low").InnerText);

                        }



                    }
                }
            }

            high = h;
            medium = m;
            low = l;

        }

        void Parser.parseReport(string inputPath, string fileName, out List<WindGenerator> wGen, out List<GasGenerator> gGen, out List<CoalGenerator> cGen)
        {
            List<WindGenerator> windList = new List<WindGenerator>();
            List<GasGenerator> gasList = new List<GasGenerator>();
            List<CoalGenerator> coalList = new List<CoalGenerator>();


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
                        string name = gen.SelectSingleNode("Name").InnerText;
                        List<Generation> generations = new List<Generation>();

                        XmlNodeList generationNodes = gen.SelectNodes("Generation/Day");
                        foreach (XmlNode dayNode in generationNodes)
                        {
                            string date = dayNode.SelectSingleNode("Date").InnerText;
                            double energy = double.Parse(dayNode.SelectSingleNode("Energy").InnerText);
                            double price = double.Parse(dayNode.SelectSingleNode("Price").InnerText);

                            Generation dayGen = new Generation(date, energy, price);
                            generations.Add(dayGen);                            
                        }

                        double emissionsRating = 0;
                        double totalHeatInput = 0;
                        double actualNetGeneration = 0;
                        if (name.Contains("Gas") || name.Contains("Coal"))
                        {
                            emissionsRating = double.Parse(gen.SelectSingleNode("EmissionsRating").InnerText);
                            if (name.Contains("Coal"))
                            {
                                totalHeatInput = double.Parse(gen.SelectSingleNode("TotalHeatInput").InnerText);
                                actualNetGeneration = double.Parse(gen.SelectSingleNode("ActualNetGeneration").InnerText);

                            }
                        }

                      

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
            wGen = windList;
            gGen = gasList;
            cGen = coalList;


        }


    }
}
