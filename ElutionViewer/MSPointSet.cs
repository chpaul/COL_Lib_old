using System;
using System.Collections.Generic;
using System.Text;

namespace COL.ElutionViewer
{
    public class MSPointSet
    {
        List<float> _x;
        List<float> _y;
        int _maxMZidx = 0;
        int _minMZidx = 0;
        int _maxIntensityidx = 0;
        int _minIntensityidx = 0;
        public List<float> XLst
        {
            get { return _x; }
        }
        public List<float> YLst
        {
            get { return _y; }
        }
        public float X(int argIdx)
        {
            return _x[argIdx];
        }
        public float Y(int argIdx)
        {
            return _y[argIdx];
        }
        public int Count
        {
            get { return _x.Count; }
        }
        public int MaxIntensityIdx
        {
            get { return _maxIntensityidx; }
        }
        public int MinIntensityIdx
        {
            get { return _minIntensityidx; }
        }
        public MSPointSet()
        {
            _x = new List<float>();
            _y = new List<float>();
        }
        public void Add(float argX, float argY)
        {
            _x.Add(argX);
            _y.Add(argY);
            UpdateMaxMin();
        }
        public void AddMSPoints(List<float> argMZ, List<float> argIntensity)
        {
            _x.AddRange(argMZ);           
            _y.AddRange(argIntensity);
            UpdateMaxMin();
        }
        private void UpdateMaxMin()
        {
            for (int i = 0; i < _x.Count; i++)
            {
                if (_x[i] > _x[_maxMZidx])
                {
                    _maxMZidx = i;
                }
                if (_x[i] < _x[_minMZidx])
                {
                    _minMZidx = i;
                }
            }
            for (int i = 0; i < _y.Count; i++)
            {
                if (_y[i] > _y[_maxIntensityidx])
                {
                    _maxIntensityidx = i;
                }
                if (_y[i] < _y[_minIntensityidx])
                {
                    _minIntensityidx = i;
                }
            }
        }
    }
}
