using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CodeChallenge
{
    //output generator class
    class OutputGenerator
    {
        //collection of generators
        private List<WindGenerator> windGenerators;
        private List<GasGenerator> gasGenerators;
        private List<CoalGenerator> coalGenerators;

        public OutputGenerator(List<WindGenerator> windGens, List<GasGenerator> gasGens, List<CoalGenerator> coalGens)
        {
            windGenerators = windGens;
            gasGenerators = gasGens;
            coalGenerators = coalGens;

            getDailyEmmissions(); // gets the daily emissions
          
           
        }

        //creates the xml output
        public void WriteToXML()
        {
            //filename string
            string fileName = ConfigurationSettings.AppSettings.Get("OutputDir") + "GenerationOutput.xml";

            //xml text writer 
            XmlTextWriter writer = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;

            //starts the doc
            writer.WriteStartDocument();

            writer.WriteStartElement("GenerationOutput"); // root
            writer.WriteStartElement("Totals"); // child

            //writes all the totals of the generators
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

            // max emissions node
            writer.WriteStartElement("MaxEmissionGenerators");
            List<EmissionOutput> emmissionOut = new List<EmissionOutput>(); // list of emission output objects
            emmissionOut = getHighestEmissions(); // set to the return of the gethighestEmission function

            //writes each object data
            foreach(EmissionOutput eo in emmissionOut)
            {
                writer.WriteStartElement("Day");

                writer.WriteElementString("Name", eo.name);
                writer.WriteElementString("Date", eo.date);
                writer.WriteElementString("Emission", eo.emissions.ToString());

                writer.WriteEndElement();

            }
            writer.WriteEndElement(); // max emission end

            //actual heat nodes
            writer.WriteStartElement("ActualHeatRates");
            //writes each coal element data
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
            //flush stream nd close writer and file
            writer.Flush();
            writer.Close();

            Console.WriteLine("Output Generated: " + fileName);


        }

        //sets the daily emissions
        public void getDailyEmmissions()
        {
            foreach(GasGenerator generator in gasGenerators)
            {
                foreach(Generation gen in generator.generations)
                {
                    gen.emmisions = gen.energy * generator.emissionsRating * generator.getEFactorValue();
                }
            }

            foreach (CoalGenerator generator in coalGenerators)
            {
                foreach (Generation gen in generator.generations)
                {
                    gen.emmisions = gen.energy * generator.emissionsRating * generator.getEFactorValue();

                }
            }
        }

        //returns the list of highest emmissions
        public List<EmissionOutput> getHighestEmissions()
        {
            List<EmissionOutput> emOutputs = new List<EmissionOutput>();

            //compares the emissions from matching dates and adds a new emission output object to the collection
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

    //object made for the output for highest emissions
    class EmissionOutput
    {
        //generation name , date and emissions 
        public string name;
        public string date;
        public double emissions;

        //constructor
        public EmissionOutput(string theName, string theDate, double theEmissions)
        {
            name = theName;
            date = theDate;
            emissions = theEmissions;
        }
    }
}
