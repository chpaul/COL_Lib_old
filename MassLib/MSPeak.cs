using System;
using System.Collections.Generic;
using System.Text;

namespace COL.MassLib
{
    public class MSPeak : IComparable<MSPeak>
    {
        private float _monoMass; //First peak
        private float _monoIntemsity;
        private int _chargeState=1;
        private float _fitScore;
        private float _mostIntseMass;
        private float _monoisotopicMZ = 0.0f;
        /// <summary>
        /// Full Ms Peak
        /// </summary>
        /// <param name="argMonoMass"></param>
        /// <param name="argMonoIntensity"></param>
        /// <param name="argChargeState"></param>
        /// <param name="argDeisotopeMz"></param>
        /// <param name="argFixScore"></param>
        /// <param name="argMostIntenseMass"></param>
        public MSPeak(float argMonoMass, float argMonoIntensity, int argChargeState, float argFixScore, float argMostIntenseMass)
        {
            _monoMass = argMonoMass;
            _monoIntemsity = argMonoIntensity;
            _chargeState = argChargeState;
            _fitScore = argFixScore;
            _mostIntseMass = argMostIntenseMass;
        }
        public MSPeak(float argMonoMass, float argMonoIntensity)
        {
            _monoMass = argMonoMass;
            _monoIntemsity = argMonoIntensity;
            
        }
        public float MonoMass
        {
            get { return _monoMass; }
        }
        public float MonoIntensity
        {
            get { return _monoIntemsity; }
        }
        public int ChargeState
        {
            get { return _chargeState; }
        }
        public float FitScore
        {
            get { return _fitScore; }
        }
        public float MostIntenseMass
        {
            get { return _mostIntseMass; }
        }
        /// <summary>
        /// First peak of envelope
        /// </summary>
        public float MonoisotopicMZ
        {
            get {
                if (_monoisotopicMZ == 0.0f)
                {
                    _monoisotopicMZ =  Convert.ToSingle((_monoMass + _chargeState * Atoms.ProtonMass) / _chargeState); 
                }

                return _monoisotopicMZ;
            }
        }
        public int CompareTo(MSPeak obj)
        {
            return this.MonoisotopicMZ.CompareTo(obj.MonoisotopicMZ); //low to high
        }
    }
}
