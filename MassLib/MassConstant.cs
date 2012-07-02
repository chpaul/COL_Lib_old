using System;
using System.Collections.Generic;
using System.Text;

namespace COL.MassLib
{
    class MassConstant
    {
        
        private static float Proton = 1.0073f;
        private static float Hydrogen = 1.0078f;
        private static float Oxygen = 15.9949f;

        public enum AtomName { Proton = 0, Hydrogen, Oxygen }
        private static List<float> _massList = new List<float>() { Proton, Hydrogen, Oxygen };

        public static float GetMass(MassConstant.AtomName argType)
        {
            return _massList[Convert.ToInt32(argType)];
        }

   } 
}
