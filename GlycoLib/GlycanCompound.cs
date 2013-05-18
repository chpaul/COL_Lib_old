using System;
using System.Collections.Generic;
using System.Text;
using COL.MassLib;
namespace COL.GlycoLib
{
    [Serializable]
    public class GlycanCompound : IComparable
    {
   
        //private static double Na = 22.9899;
        int _Carbon;
        int _Hydrogen;
        int _Oxygen;
        int _Nitrogen;
        int _Sodium;
        int _Deuterium;
        int _HexNAc; //Mannose , Galactose C6H12O6 
        int _Hex; //	N-Acetylglucosamine C8H15NO6
        int _DeHex; //Fucose C6H12O5        
        int _Sia; //N-Acetylneuraminic acid 	C11H19NO9 (human)   N-glycolylneuraminic acid  C11H19NO10   (Other mammals)
        int _LCorder;
        bool _Permethylated =false;
        bool _isSodium=false;
        bool _Human=true;
        bool _isReducedReducingEnd=false;
        bool _isDeuterium=false;
        double _MonoMass;
        double _AVGMass;
        public GlycanCompound(int argHexNac, int argHex, int argDeHex, int argSialic)
        {
            _HexNAc = argHexNac;
            _Hex = argHex;
            _DeHex = argDeHex;
            _Sia = argSialic;
            CalcAtom();
            CalcMass();
        }
        public GlycanCompound(int argHexNac, int argHex, int argDeHex, int argSialic, bool argIsPermethylated, bool argIsDeuterium, bool argReducedReducingEnd, bool argIsSodium, bool argIsHuman)
        {
            _HexNAc = argHexNac;
            _Hex = argHex;
            _DeHex = argDeHex;
            _Sia = argSialic;

            _Human = argIsHuman;
            _Permethylated = argIsPermethylated;
            _isSodium = argIsSodium;
            _isReducedReducingEnd = argReducedReducingEnd;
            _isDeuterium = argIsDeuterium;
            CalcAtom();
            CalcMass();
            CalcAVGMass();
        }
       public GlycanCompound(int argHexNac, int argHex, int argDeHex, int argSialic, bool argIsPermethylated, bool argIsDeuterium, bool argReducedReducingEnd, bool argIsSodium, bool argIsHuman, bool NoProfile)
        {
            _HexNAc = argHexNac;
            _Hex = argHex;
            _DeHex = argDeHex;
            _Sia = argSialic;

            _Human = argIsHuman;
            _Permethylated = argIsPermethylated;
            _isSodium = argIsSodium;
            _isReducedReducingEnd = argReducedReducingEnd;
            _isDeuterium = argIsDeuterium;
            CalcAtom();
            CalcMass();
            CalcAVGMass();
        }

        public bool isHuman
        {
            get { return _Human; }
            set { _Human = value; }
        }

        public double MonoMass
        {
            get
            {
                if (_MonoMass == 0.0)
                {
                    CalcMass();
                }
                return _MonoMass;
            }
        }
        public double AVGMass
        {
            get
            {
                if (_AVGMass == 0.0)
                {
                    CalcAVGMass();
                }
                return _AVGMass;
            }
        }
        private void CalcMass()
        {
            _MonoMass = _Carbon * Atoms.CarbonMass + _Hydrogen * Atoms.HydrogenMass + _Nitrogen * Atoms.NitrogenMass + Oxygen * Atoms.OxygenMass + Sodium * Atoms.SodiumMass + _Deuterium * Atoms.DeuteriumMass;
         }
        private void CalcAVGMass()
        {
            _AVGMass = _Carbon * Atoms.CarbonAVGMass + _Hydrogen * Atoms.HydrogenAVGMass + _Nitrogen * Atoms.NitrogenAVGMass + Oxygen * Atoms.OxygenAVGMass + Sodium * Atoms.SodiumMass + _Deuterium * Atoms.DeuteriumMass;
        }
        public bool isPermethylated
        {
            get { return _Permethylated; }
        }
        public bool isSodium
        {
            get { return _isSodium; }
        }
        public int NoOfHexNAc
        {
            get { return _HexNAc; }
        }
        public int NoOfHex
        {
            get { return _Hex; }
        }
        public int NoOfSia
        {
            get{ return _Sia; }
        }
        public int NoOfDeHex
        {
            get { return _DeHex; }
         }
        public int Sodium
        {
            get{return _Sodium;}
        }
        public int Nitrogen
        {
            get{return _Nitrogen;}
        }
        public int Carbon
        {
            get{return _Carbon;}
        }
        public int Hydrogen
        {
            get{return _Hydrogen;}
        }
        public int Oxygen
        {
            get{return _Oxygen;}
        }
        public int Deuterium
        {
            get { return _Deuterium; }            
        }
        public int GlycanLCorder
        {
            get { return _LCorder;}
            set { _LCorder = value; }
        }
        public string GlycanKey
        {
            get
            {
                return _HexNAc.ToString() + "-" +
                              _Hex.ToString() + "-" +
                              _DeHex.ToString() + "-" +
                              _Sia.ToString();
            }
        }

        /// <summary>
        ///http://www.expasy.ch/tools/glycomod/glycomod_masses.html
        ///                                                 Native                  Permethylated
        ///Hexose (Hex) Mannose                    Man      C6H10O5  	162.0528    C9H16O5     204.0998
        ///Hexose (Hex) Galactose                  Gal      C6H10O5  	162.0528    C9H16O5     204.0998
        ///HexNAc       N-Acetylglucosamine 	   GlcNAC   C8H13NO5    203.0794    C11H19NO5   245.1263
        ///Deoxyhexose  Fucose                     Fuc      C6H10O4     146.0579    C8H14O4     174.0892
        ///NeuAc        N-Acetylneuraminic acid    NeuNAc   C11H17NO8   291.0954    C16H27NO8   361.1737
        ///NeuGc        N-glycolylneuraminic acid  NeuNGc   C11H17NO9   307.0903    C17H29NO9   391.1842
        ///Permethylated -H ->CH3;
        /// </summary>
        private void CalcAtom()
        {    
            if (_Permethylated)
            {
                _Carbon = 9 * _Hex + 11 * _HexNAc + 8 * _DeHex;
                _Hydrogen = 16 * _Hex + 19 * _HexNAc + 14 * _DeHex;
                _Oxygen = 5 * _Hex + 5 * _HexNAc + 4 * _DeHex;
                _Nitrogen = 1 * _HexNAc + 1 * _Sia;

                if (_isDeuterium) //Replace some hydrogen to Deutrtium
                {
                    _Hydrogen = 10 * _Hex + 13 * _HexNAc + 10 * _DeHex;
                    _Deuterium = 6 * _Hex + 6 * _HexNAc + 4 * _DeHex;
                }

                if (_Human)
                {
                    _Carbon = _Carbon + 16 * _Sia;
                    _Hydrogen = _Hydrogen + 27 * _Sia;
                    _Oxygen = _Oxygen + 8 * _Sia;
                }
                else
                {
                    _Carbon = _Carbon + 17 * _Sia;
                    _Hydrogen = _Hydrogen + 29 * _Sia;
                    _Oxygen = _Oxygen + 9 * _Sia;
                }
                //Nonreducing end  -CH3
                _Carbon = _Carbon + 1;
                _Hydrogen = _Hydrogen + 3;
                if (_isReducedReducingEnd) //C2OH7
                {
                    _Carbon = _Carbon + 2;
                    _Oxygen = _Oxygen + 1;
                    _Hydrogen = _Hydrogen + 7;
                }
                else //COH3
                {
                    _Carbon = _Carbon + 1;
                    _Oxygen = _Oxygen + 1;
                    _Hydrogen = _Hydrogen + 3;
                }
            }
            else
            {
                _Carbon = 6 * _Hex + 8 * _HexNAc + 6 * _DeHex;
                _Hydrogen = 10 * _Hex + 13 * _HexNAc + 10 * _DeHex;
                _Oxygen = 5 * _Hex + 5 * _HexNAc + 4 * _DeHex;
                _Nitrogen = 1 * _HexNAc + 1 * _Sia;

                if (_Human)
                {
                    _Carbon = _Carbon + 11 * _Sia;
                    _Hydrogen = _Hydrogen + 17 * _Sia;
                    _Oxygen = _Oxygen + 8 * _Sia;
                }
                else
                {
                    _Carbon = _Carbon + 11 * _Sia;
                    _Hydrogen = _Hydrogen + 17 * _Sia;
                    _Oxygen = _Oxygen + 9 * _Sia;
                }
                //Nonreducing end -H
                _Hydrogen = _Hydrogen + 1;
                if (_isReducedReducingEnd) //OH3
                {
                    _Oxygen = _Oxygen + 1;
                    _Hydrogen = _Hydrogen + 3;
                }
                else //OH
                {
                    _Oxygen = _Oxygen + 1;
                    _Hydrogen = _Hydrogen + 1;
                }
            }
            if (_isSodium)
            {
                _Sodium = 1;
            }            
        }
        public int CompareTo(object obj)
        {
            if (obj is GlycanCompound)
            {
                GlycanCompound p2 = (GlycanCompound)obj;
                return _MonoMass.CompareTo(p2.MonoMass);
            }
            else
                throw new ArgumentException("Object is not a Compound.");
        }

    }
    
}
