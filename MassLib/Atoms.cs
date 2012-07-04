using System;
using System.Collections.Generic;
using System.Text;

namespace COL.MassLib
{
    public class Atoms
    {
        //Monoisotopic mass
        //H	Hydrogen	1.00783
        //C	Carbon	12.000000
        //N	Nitrogen	14.003072
        //O	Oxygen	15.9949141
        //Na	Oxygen	22.989767

        private const double C = 12.000000;
        private const double H = 1.0078246;
        private const double O = 15.9949141;
        private const double N = 14.0030732;
        private const double Na = 22.989767;
        private const double D = 2.0141021;
        private const double Proton = 1.0073;
        public static double ProtonMass
        {
            get { return Proton; }
        }
        public static double DeuteriumMass
        {
            get
            {
                return D;
            }
        }
        public static double CarbonMass
        {
            get
            {
                return C;
            }
        }
        public static double HydrogenMass
        {
            get
            {
                return H;
            }
        }
        public static double OxygenMass
        {
            get
            {
                return O;
            }
        }
        public static double NitrogenMass
        {
            get
            {
                return N;
            }
        }
        public static double SodiumMass
        {
            get
            {
                return Na;
            }
        }

    }
}
