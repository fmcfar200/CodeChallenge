using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge
{
    
    public class Generator
    {
        public enum ValueFactorType
        {
           HIGH,MEDIUM,LOW
        }

        public string name;
        public List<Generation> generations = new List<Generation>();
        public ValueFactorType vFactorType;

        public Generator(string theName, List<Generation> theGenerations, ValueFactorType vfType)
        {
            name = theName;
            generations = theGenerations;
            vFactorType = vfType;
           
        }

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

    public class WindGenerator : Generator
    {
        public WindGenerator(string theName, List<Generation> theGenerations, ValueFactorType vfType) : 
            base(theName,theGenerations,vfType)
        {
           
        } 
    }

    public class GasGenerator: Generator
    {
        public double emissionsRating;
        public enum EmissionFactorType
        {
            HIGH, MEDIUM, LOW
        }

        public EmissionFactorType eFactorType;

        public GasGenerator(string theName, List<Generation> theGenerations, double theEmissionsRating, ValueFactorType vfType, EmissionFactorType efType): base(theName, theGenerations, vfType)
        {
            name = theName;
            generations = theGenerations;
            emissionsRating = theEmissionsRating;
            vFactorType = vfType;
            eFactorType = efType;
        }

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
    public class CoalGenerator: GasGenerator
    {
        public double totalHeatInput;
        public double actualNetGeneration;
        public CoalGenerator(string theName, List<Generation> theGenerations, double theEmissions, double theTotalHeat, double theActualNetGen, ValueFactorType vfType, EmissionFactorType efType): base(theName, theGenerations, theEmissions, vfType, efType)
        {
            totalHeatInput = theTotalHeat;
            actualNetGeneration = theActualNetGen;
        }

        public double getActualHeatRate()
        {
            return totalHeatInput / actualNetGeneration;
        }

    }

    public class Generation
    {
        public string date;
        public double energy;
        public double price;
        public double emmisions;

        public Generation(string thedate, double theEnergy, double thePrice)
        {
            date = thedate;
            energy = theEnergy;
            price = thePrice;
        }
    }




}
