using System;
using System.Collections.Generic;
using System.Text;

namespace COL.MassLib
{
    public class MSPeak : IComparable<MSPeak>
    {
        private float _monoMass; //First peak
        private float _monoIntemsity;
        private float _chargeState;
        private float _deisotopeMz;
        private float _fitScore;
        private float _mostIntseMass;

        public MSPeak(float argMonoMass, float argMonoIntensity, float argChargeState, float argDeisotopeMz, float argFixScore, float argMostIntenseMass)
        {
            _monoMass = argMonoMass;
            _monoIntemsity = argMonoIntensity;
            _chargeState = argChargeState;
            _deisotopeMz = argDeisotopeMz;
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
        public float ChargeState
        {
            get { return _chargeState; }
        }
        public float DeisotopeMz
        {
            get { return _deisotopeMz; }
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
            get { return Convert.ToSingle((_monoMass + _chargeState * 1.0073) / _chargeState); }
        }
        public int CompareTo(MSPeak obj)
        {
            return this.MonoisotopicMZ.CompareTo(obj.MonoisotopicMZ); //low to high
        }
    }
}
