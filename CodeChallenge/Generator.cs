using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge
{
    //generator base class
    public class Generator
    {
        //all generators will have a value factor
        public enum ValueFactorType
        {
           HIGH,MEDIUM,LOW
        }

        //name, list of generations and valuefactor type
        public string name;
        public List<Generation> generations = new List<Generation>();
        public ValueFactorType vFactorType;

        //constructor
        public Generator(string theName, List<Generation> theGenerations, ValueFactorType vfType)
        {
            name = theName;
            generations = theGenerations;
            vFactorType = vfType;
           
        }

        //gets the correct value factor value from the ValueFactor static class 
        public double getVFactorValue()
        {
            double factor = 0;
            switch(vFactorType)
            {
                case ValueFactorType.HIGH:
                    factor = ValueFactor.high;
                    break;
                case ValueFactorType.MEDIUM:
                    factor = ValueFactor.medium;
                    break;
                case ValueFactorType.LOW:
                    factor =  ValueFactor.low;
                    break;
               
            }

            return factor;
        }

        //gets the total generation value of the generator
        public double getTotalGenerationValue()
        {
            double total = 0;

            foreach(Generation day in generations)
            {
                total += day.energy * day.price * getVFactorValue();
            }


            return total;
        }
    }

    //wind generator derived from generator
    public class WindGenerator : Generator
    {
        public WindGenerator(string theName, List<Generation> theGenerations, ValueFactorType vfType) : 
            base(theName,theGenerations,vfType)
        {
           
        } 
    }

    //gas generator derived from generator
    public class GasGenerator: Generator
    {
        //include emmisions rating and emmision factor
        public enum EmissionFactorType
        {
            HIGH, MEDIUM, LOW
        }
        public EmissionFactorType eFactorType;
        public double emissionsRating;

        //constructor
        public GasGenerator(string theName, List<Generation> theGenerations, double theEmissionsRating, ValueFactorType vfType, EmissionFactorType efType): base(theName, theGenerations, vfType)
        {
            name = theName;
            generations = theGenerations;
            emissionsRating = theEmissionsRating;
            vFactorType = vfType;
            eFactorType = efType;
        }

        //gets the emissions factor value
        public double getEFactorValue()
        {
            double factor = 0;
            switch (eFactorType)
            {
                case EmissionFactorType.HIGH:
                    factor = EmissionsFactor.high;
                    break;
                case EmissionFactorType.MEDIUM:
                    factor = EmissionsFactor.medium;
                    break;
                case EmissionFactorType.LOW:
                    factor = EmissionsFactor.low;
                    break;

            }

            return factor;
        }

    }

    //coal generator derived from gas generator
    public class CoalGenerator: GasGenerator
    {
        //includes a total heat and actual net generation values
        public double totalHeatInput;
        public double actualNetGeneration;

        //constructor
        public CoalGenerator(string theName, List<Generation> theGenerations, double theEmissions, double theTotalHeat, double theActualNetGen, ValueFactorType vfType, EmissionFactorType efType): base(theName, theGenerations, theEmissions, vfType, efType)
        {
            totalHeatInput = theTotalHeat;
            actualNetGeneration = theActualNetGen;
        }

        //gets the actual heat rate value
        public double getActualHeatRate()
        {
            return totalHeatInput / actualNetGeneration;
        }

    }

    //a generation class
    public class Generation
    {
        //date, energy, price and emission
        public string date;
        public double energy;
        public double price;
        public double emmisions;

        //constructor
        public Generation(string thedate, double theEnergy, double thePrice)
        {
            date = thedate;
            energy = theEnergy;
            price = thePrice;
        }
    }




}
