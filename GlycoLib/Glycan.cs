using System;
using System.Collections.Generic;
using System.Text;

namespace COL.GlycoLib
{
    [Serializable]
    public class Glycan
    {       
        public enum Type { HexNAc = 1, Hex,DeHex, NeuAc, NeuGc,Man,Gal }
        private Type _type;
        private int _charge;
        private float _mz;

        public Glycan(Type argType, int argCharge)
        {
            _type = argType;
            _charge = argCharge;
            _mz = GlycanMass.GetGlycanMasswithCharge(argType, argCharge);
        }
        public Type GlycanType
        {
            get { return _type; }
        }
        public int Charge
        {
            get { return _charge; }
        }
        public float Mz
        {
            get { return _mz; }
        }
        public static Glycan.Type String2GlycanType(string argType)
        {
            if (argType.ToLower().Contains("glcnac") || argType.ToLower().Contains("hexnac"))
            {
                return Glycan.Type.HexNAc;
            }
            else if (argType.ToLower().Contains("fuc") || argType.ToLower().Contains("dehex"))
            {
                return Glycan.Type.DeHex;
            }
            else if (argType.ToLower().Contains("gal"))
            {
                return Glycan.Type.Hex;
            }
            else if (argType.ToLower().Contains("neuac"))
            {
                return Glycan.Type.NeuAc;
            }
            else if (argType.ToLower().Contains("neugc"))
            {
                return Glycan.Type.NeuGc;
            }
            else if (argType.ToLower().Contains("man") || argType.ToLower().Contains("hex"))
            {
                return Glycan.Type.Hex;
            }
            else
            {
                throw new Exception("IUPAC contain unrecognized glycan or string");
            }
        }

    }
}
