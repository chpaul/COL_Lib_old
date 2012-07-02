using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace COL.MassLib
{
    public class MSScan :ICloneable
    {
        private List<MSPeak> _lstMsPeak;
        private int _scanNo;
        private int _parentScanNo;
        private int _msLevel;
        private double _minIntensity;
        private double _maxIntensity;
        private double _time;
        private double _minMZ;
        private double _maxMZ;

        public List<MSPeak> MSPeaks
        {
            get {return _lstMsPeak;}
            set 
            {
                _lstMsPeak = value;
                _lstMsPeak.Sort();
                foreach (MSPeak argPeak in _lstMsPeak)
                {
                    if (argPeak.MonoisotopicMZ > _maxMZ)
                    {
                        _maxMZ = argPeak.MonoisotopicMZ;
                    }
                    if (argPeak.MonoisotopicMZ <= _minMZ)
                    {
                        _minMZ = argPeak.MonoisotopicMZ;
                    }
                    if (argPeak.MostIntenseMass > _maxIntensity)
                    {
                        _maxIntensity = argPeak.MostIntenseMass;
                    }
                    if (argPeak.MostIntenseMass <= _minIntensity)
                    {
                        _minIntensity = argPeak.MostIntenseMass;
                    }          
                }
            }
        }
      //  public List<MSPoint> MSPoints
       // {

        //}
        public double Time
        {
            get {return _time;}
            set {_time =value;}
        }
        public int ParentScanNo
        {
            get { return _parentScanNo; }
            set { _parentScanNo = value; }
        }
        public int MsLevel
        {
            get { return _msLevel; }
            set { _msLevel = value; }
        }
        public double MaxIntensity
        {
            get { return _maxIntensity; }
            set { _maxIntensity = value; }
        }
        public double MinIntensity
        {
            get { return _minIntensity; }
            set { _minIntensity = value; }
        }
        public double MaxMZ
        {
            get { return _maxMZ; }
            set { _maxMZ = value; }
        }
        public double MinMZ
        {
            get { return _minMZ; }
            set { _minMZ = value; }
        }
        public int ScanNo
        {
            get { return _scanNo; }
            set { _scanNo = value; }
        }
        public int Count
        {
            get { return _lstMsPeak.Count; }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public MSScan()
        {
            _lstMsPeak = new List<MSPeak>();
            _minIntensity = 10000000000.0;
            _minMZ = 10000000000.0;
            _maxIntensity = -10000000000.0;
            _maxMZ = -10000000000.0;
        } 
        public MSScan(int argScanNo)
        {
            _lstMsPeak = new List<MSPeak>();
              _scanNo = argScanNo;

            _minIntensity = 10000000000.0;
            _minMZ = 10000000000.0;
            _maxIntensity = -10000000000.0;
            _maxMZ = -10000000000.0;
            
        }

        public void AddPeak(MSPeak argPeak)
        {

            if (argPeak.MonoisotopicMZ > _maxMZ)
                {
                    _maxMZ = argPeak.MonoisotopicMZ;
                }
            if (argPeak.MonoisotopicMZ <= _minMZ)
                {
                    _minMZ = argPeak.MonoisotopicMZ;
                }
            if (argPeak.MostIntenseMass > _maxIntensity)
                {
                    _maxIntensity = argPeak.MostIntenseMass;
                }
            if (argPeak.MostIntenseMass <= _minIntensity)
                {
                    _minIntensity = argPeak.MostIntenseMass;
                }                
                _lstMsPeak.Add(argPeak);
                _lstMsPeak.Sort();
        }



    }
}
