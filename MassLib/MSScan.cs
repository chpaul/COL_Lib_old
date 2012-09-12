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
        private int _parentCharge = 0;
        private float _parentMz = 0;
        private float _parentMonoMW = 0;
        private int _msLevel;
        private float _minIntensity;
        private float _maxIntensity;
        private double _time;
        private float _minMZ;
        private float _maxMZ;

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
                    if (argPeak.MonoIntensity > _maxIntensity)
                    {
                        _maxIntensity = argPeak.MonoIntensity;
                    }
                    if (argPeak.MonoIntensity <= _minIntensity)
                    {
                        _minIntensity = argPeak.MonoIntensity;
                    }          
                }
            }
        }
        public int ParentCharge
        {
            get { return _parentCharge; }
            set { _parentCharge = value; }
        }
        public float ParentMZ
        {
            get { return _parentMz; }
            set { _parentMz = value; }
        }
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
        public float ParentMonoMW
        {
            get { return _parentMonoMW; }
            set { _parentMonoMW = value; }
        }
        public int MsLevel
        {
            get { return _msLevel; }
            set { _msLevel = value; }
        }
        public float MaxIntensity
        {
            get { return _maxIntensity; }
            set { _maxIntensity = value; }
        }
        public float MinIntensity
        {
            get { return _minIntensity; }
            set { _minIntensity = value; }
        }
        public float MaxMZ
        {
            get { return _maxMZ; }
            set { _maxMZ = value; }
        }
        public float MinMZ
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
            _minIntensity = 10000000000.0f;
            _minMZ = 10000000000.0f;
            _maxIntensity = -10000000000.0f;
            _maxMZ = -10000000000.0f;
        } 
        public void  Clear()
        {
            _lstMsPeak.Clear(); 

        }
        public MSScan(int argScanNo)
        {
            _lstMsPeak = new List<MSPeak>();
              _scanNo = argScanNo;

            _minIntensity = 10000000000.0f;
            _minMZ = 10000000000.0f;
            _maxIntensity = -10000000000.0f;
            _maxMZ = -10000000000.0f;            
        }
        public MSScan(float[] argMz, float[] argIntensity, float argParentMZ,float argParentMonoMW, int argParentCharge)
        {
            _lstMsPeak = new List<MSPeak>();
            for (int i = 0; i < argMz.Length; i++)
            {
                AddPeak(new MSPeak(argMz[i], argIntensity[i]));
            }
            _parentMonoMW = argParentMonoMW;

            _minIntensity = 10000000000.0f;
            _minMZ = 10000000000.0f;
            _maxIntensity = -10000000000.0f;
            _maxMZ = -10000000000.0f;
            _parentCharge = argParentCharge;
            _parentMz = argParentMZ;
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
            if (argPeak.MonoIntensity > _maxIntensity)
                {
                    _maxIntensity = argPeak.MonoIntensity;
                }
            if (argPeak.MonoIntensity <= _minIntensity)
                {
                    _minIntensity = argPeak.MonoIntensity;
                }                
                _lstMsPeak.Add(argPeak);
                _lstMsPeak.Sort();
        }



    }
}
