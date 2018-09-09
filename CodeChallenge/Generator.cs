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
            HIGH, MEDIUM, LOW
        }

        public string name;
        public List<Day> generations = new List<Day>();
        public ValueFactorType vFactorType;

        public Generator(string theName, List<Day> theGenerations, ValueFactorType vfType )
        {
            name = theName;
            generations = theGenerations;
            vFactorType = vfType;
        }
    }

    public class WindGenerator : Generator
    {
        public WindGenerator(string theName, List<Day> theGenerations, ValueFactorType vfType) : 
            base(theName,theGenerations,vfType) { } 
    }

    public class GasGenerator: Generator
    {
        public enum EmissionFactorType
        {
            HIGH, MEDIUM, LOW
        }

        public EmissionFactorType eFactorType;

        public GasGenerator(string theName, List<Day> theGenerations, ValueFactorType vfType, EmissionFactorType efType): base(theName, theGenerations, vfType)
        {
            name = theName;
            generations = theGenerations;
            vFactorType = vfType;
            eFactorType = efType;
        }
    }
    public class CoalGenerator: GasGenerator
    {
        public CoalGenerator(string theName, List<Day> theGenerations, ValueFactorType vfType, EmissionFactorType efType): base(theName, theGenerations, vfType, efType){ }

    }

    public class Day
    {
        public string date;
        public double energy;
        public double price;

        public Day(string thedate, double theEnergy, double thePrice)
        {
            date = thedate;
            energy = theEnergy;
            price = thePrice;
        }
    }



}
