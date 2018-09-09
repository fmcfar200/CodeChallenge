using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge
{
    public class Factor
    {
        public double high, medium, low;

        public Factor() { }
        public Factor(double thehigh, double themedium, double thelow)
        {
            high = thehigh;
            medium = themedium;
            low = thelow;
        }
    }

    class ValueFactor : Factor
    {
        public ValueFactor() : base() { }
        public ValueFactor(double thehigh, double themedium, double thelow) : base(thehigh,themedium,thelow)
        {

        }

    };
    class EmissionsFactor : Factor
    {
        public EmissionsFactor() : base() { }
        public EmissionsFactor(double thehigh, double themedium, double thelow) : base(thehigh,themedium,thelow)
        {

        }
    };
}
