using System;
using System.Collections.Generic;
using System.Text;

namespace COL.MassLib
{
    public class MSPeak : IComparable<MSPeak>
    {
        private double _monoMass; //First peak
        private double _monoIntemsity;
        private double _chargeState;
        private double _deisotopeMz;
        private double _fitScore;
        private double _mostIntseMass;

        public MSPeak(double argMonoMass, double argMonoIntensity, double argChargeState, double argDeisotopeMz, double argFixScore, double argMostIntenseMass)
        {
            _monoMass = argMonoMass;
            _monoIntemsity = argMonoIntensity;
            _chargeState = argChargeState;
            _deisotopeMz = argDeisotopeMz;
            _fitScore = argFixScore;
            _mostIntseMass = argMostIntenseMass;
        }

        public MSPeak(double argMonoMass, double argMonoIntensity)
        {
            _monoMass = argMonoMass;
            _monoIntemsity = argMonoIntensity;
        }
        public double MonoMass
        {
            get { return _monoMass; }
        }
        public double MonoIntensity
        {
            get { return _monoIntemsity; }
        }
        public double ChargeState
        {
            get { return _chargeState; }
        }
        public double DeisotopeMz
        {
            get { return _deisotopeMz; }
        }
        public double FitScore
        {
            get { return _fitScore; }
        }
        public double MostIntenseMass
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
