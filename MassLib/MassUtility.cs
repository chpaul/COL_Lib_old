using System;
using System.Collections.Generic;
using System.Text;

namespace COL.MassLib
{
    public class MassUtility
    {
        public static double GetMassPPM(double argExactMass, double argMeasureMass)
        {
            return Math.Abs(Convert.ToDouble(((argMeasureMass - argExactMass) / argExactMass) * Math.Pow(10.0, 6.0)));
        }
        public static double GetMassPPM(float argExactMass, float argMeasureMass)
        {
            return Math.Abs(Convert.ToSingle(((argMeasureMass - argExactMass) / argExactMass) * Math.Pow(10.0, 6.0)));
        }
        public static int GetClosestMassIdx(List<MSPoint> argPoints, float argMZ)
        {
            argPoints.Sort();
            List<float> lstCIDMz = new List<float>();
            for (int i = 0; i < argPoints.Count; i++)
            {
                lstCIDMz.Add(argPoints[i].Mass);
            }
            return GetClosestMassIdx(lstCIDMz, argMZ);
        }
        public static int GetClosestMassIdx(List<MSPeak> argPeaks, float argMZ)
        {
            //Convert MSPeaks mz into float[]
            argPeaks.Sort();
            List<float> lstCIDMz = new List<float>();
            for(int i =0; i<argPeaks.Count;i++)
            {
                  lstCIDMz.Add(argPeaks[i].MonoMass);
            }
            return GetClosestMassIdx(lstCIDMz, argMZ);
           
        }
        public static int GetClosestMassIdx(List<float> argPeaks, float argMZ)
        {
            int KeyIdx = argPeaks.BinarySearch(argMZ);
            if (KeyIdx < 0)
            {
                KeyIdx = ~KeyIdx;
            }

            int ClosetIdx = 0;
            double ClosestValue = 10000.0;
            for (int i = KeyIdx - 2; i <= KeyIdx + 2; i++)
            {
                if (i >= 0 && i < argPeaks.Count)
                {
                    if (Math.Abs(argPeaks[i] - argMZ) <= ClosestValue)
                    {
                        ClosestValue = Math.Abs(argPeaks[i] - argMZ);
                        ClosetIdx = i;
                    }
                }
            }
            return ClosetIdx;
        }  
    }
}
