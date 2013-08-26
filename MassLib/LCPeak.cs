using System;
using System.Collections.Generic;
using System.Text;

namespace COL.MassLib
{
    public class LCPeak
    {
        private float _startLCTime = 0.0f;
        private float _endLCTime=0.0f;
        private List<MSPoint> _lstMSPoints = new List<MSPoint>();
        private MSPoint _apex;
        private double _sumofIntensity = 0;
        public LCPeak(float argStartTime, float argEndTime, List<MSPoint> argMSPs)
        {
            _startLCTime = argStartTime;
            _endLCTime = argEndTime;

            _lstMSPoints = argMSPs;

            //Find Apex in raw data
            if (_lstMSPoints.Count > 0)
            {
                _apex = new MSPoint(0.0f,0.0f);
                for (int i = 0; i < _lstMSPoints.Count; i++)
                {
                    _sumofIntensity = _sumofIntensity + _lstMSPoints[i].Intensity;
                    if (_lstMSPoints[i].Intensity >= _apex.Intensity)
                    {
                        _apex = _lstMSPoints[i];
                    }
                }
            }
        }
        public float StartTime
        {
            get { return _startLCTime; }
        }
        public float EndTime
        {
            get { return _endLCTime; }
        }
        public List<MSPoint> RawPoint
        {
            get { return _lstMSPoints; }
        }
        public double PeakArea
        {
            get { return IntegralPeakArea.IntegralArea(this); }
        }
        public MSPoint Apex
        {
            get { return _apex; }
        }
        public double SumOfIntensity
        {
            get { return _sumofIntensity; }
        }
        public float MZ
        {
            get { return _lstMSPoints[0].Mass; }
        }
    }
    
}
