using System;
using System.Collections.Generic;
using System.Text;

namespace COL.MassLib
{
    [Serializable]
    public class MSPoint : IComparable
    {
        double _mass;
        double _intensity;
        public MSPoint()
        {
        }
        public MSPoint(double mass, double intensity)
        {
            _mass = mass;
            _intensity = intensity;
        }

        public double Mass
        {
            get { return _mass; }
            set { _mass = value; }
        }
        public double Intensity
        {
            get { return _intensity; }
            set { _intensity = value; }
        }
        public int CompareTo(object obj)
        {
            if (obj is MSPoint)
            {
                MSPoint p2 = (MSPoint)obj;
                return _mass.CompareTo(p2.Mass);
            }
            else
                throw new ArgumentException("Object is not a MSPoint.");
        }
    }
}
