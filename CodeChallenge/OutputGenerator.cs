using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CodeChallenge
{
    class OutputGenerator
    {
        private List<WindGenerator> windGenerators;
        private List<GasGenerator> gasGenerators;
        private List<CoalGenerator> coalGenerators;

        public OutputGenerator(List<WindGenerator> windGens, List<GasGenerator> gasGens, List<CoalGenerator> coalGens)
        {
            windGenerators = windGens;
            gasGenerators = gasGens;
            coalGenerators = coalGens;

            getDailyEmmissions();
          
           
        }

        public void WriteToXML()
        {
            string fileName = ConfigurationSettings.AppSettings.Get("OutputDir") + "GenerationOutput.xml";

            XmlTextWriter writer = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;

            writer.WriteStartDocument();

            writer.WriteStartElement("GenerationOutput");
            writer.WriteStartElement("Totals");

            foreach(CoalGenerator gen in coalGenerators)
            {
                writer.WriteStartElement("Generator");
                writer.WriteElementString("Name", gen.name);
                writer.WriteElementString("Total", gen.getTotalGenerationValue().ToString());

                writer.WriteEndElement();
            }

            foreach (GasGenerator gen in gasGenerators)
            {
                writer.WriteStartElement("Generator");
                writer.WriteElementString("Name", gen.name);
                writer.WriteElementString("Total", gen.getTotalGenerationValue().ToString());

                writer.WriteEndElement();
            }

            foreach (WindGenerator cGen in windGenerators)
            {
                writer.WriteStartElement("Generator");
                writer.WriteElementString("Name", cGen.name);
                writer.WriteElementString("Total", cGen.getTotalGenerationValue().ToString());

                writer.WriteEndElement();
            }

            writer.WriteEndElement(); // totals end

            writer.WriteStartElement("MaxEmissionGenerators");
            List<EmissionOutput> emmissionOut = new List<EmissionOutput>();
            emmissionOut = getHighestEmissions();

            foreach(EmissionOutput eo in emmissionOut)
            {
                writer.WriteStartElement("Day");

                writer.WriteElementString("Name", eo.name);
                writer.WriteElementString("Date", eo.date);
                writer.WriteElementString("Emission", eo.emmissions.ToString());

                writer.WriteEndElement();

            }
            writer.WriteEndElement(); // max emission end

            writer.WriteStartElement("ActualHeatRates");
            foreach(CoalGenerator gen in coalGenerators)
            {
                writer.WriteStartElement("Generator");

                writer.WriteElementString("Name", gen.name);
                writer.WriteElementString("HeatRate", gen.getActualHeatRate().ToString());

                writer.WriteEndElement();

            }
            writer.WriteEndElement(); //heat rate end


            writer.WriteEndElement(); // output end


            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();


        }

        public void getDailyEmmissions()
        {
            foreach(GasGenerator generator in gasGenerators)
            {
                foreach(Generation gen in generator.generations)
                {
                    gen.emmisions = gen.energy * generator.emissionsRating * generator.getEFactorValue();
                    Console.WriteLine(gen.emmisions);
                }
            }

            foreach (CoalGenerator generator in coalGenerators)
            {
                foreach (Generation gen in generator.generations)
                {
                    gen.emmisions = gen.energy * generator.emissionsRating * generator.getEFactorValue();
                    Console.WriteLine(gen.emmisions);

                }
            }
        }

        public List<EmissionOutput> getHighestEmissions()
        {
            List<EmissionOutput> emOutputs = new List<EmissionOutput>();

            foreach(GasGenerator gGen in gasGenerators)
            {
                foreach(CoalGenerator cGen in coalGenerators)
                {
                    foreach(Generation gGeneration in gGen.generations)
                    {
                        foreach(Generation cGeneration in cGen.generations)
                        {
                            if (gGeneration.date == cGeneration.date)
                            {
                                if (gGeneration.emmisions > cGeneration.emmisions)
                                {
                                    EmissionOutput emO = new EmissionOutput(gGen.name, gGeneration.date, gGeneration.emmisions);
                                    emOutputs.Add(emO);
                                }
                                else if (gGeneration.emmisions < cGeneration.emmisions)
                                {
                                    EmissionOutput emO = new EmissionOutput(cGen.name, cGeneration.date, cGeneration.emmisions);
                                    emOutputs.Add(emO);
                                }
                                else
                                {
                                    EmissionOutput emO = new EmissionOutput(cGen.name, cGeneration.date, cGeneration.emmisions);
                                    emOutputs.Add(emO);
                                }
                            }
                        }
                    }
                }
            }

            return emOutputs;
        }
    }

    class EmissionOutput
    {
        public string name;
        public string date;
        public double emmissions;

        public EmissionOutput(string theName, string theDate, double theEmissions)
        {
            name = theName;
            date = theDate;
            emmissions = theEmissions;
        }
    }
}
