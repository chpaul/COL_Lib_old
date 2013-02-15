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

        private const float C = 12.000000f;
        private const float H = 1.0078246f;
        private const float O = 15.9949141f;
        private const float N = 14.0030732f;
        private const float Na = 22.989767f;
        private const float D = 2.0141021f;
        private const float Proton = 1.0073f;
        public static float ProtonMass
        {
            get { return Proton; }
        }
        public static float DeuteriumMass
        {
            get
            {
                return D;
            }
        }
        public static float CarbonMass
        {
            get
            {
                return C;
            }
        }
        public static float HydrogenMass
        {
            get
            {
                return H;
            }
        }
        public static float OxygenMass
        {
            get
            {
                return O;
            }
        }
        public static float NitrogenMass
        {
            get
            {
                return N;
            }
        }
        public static float SodiumMass
        {
            get
            {
                return Na;
            }
        }

    }
}
