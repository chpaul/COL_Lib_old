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
        public static int GetClosestMassIdx(List<MSPeak> argPeaks, double argMZ)
        {
            //Convert MSPeaks mz into float[]
            argPeaks.Sort();
            double[] _cidMzs = new double[argPeaks.Count];
            for(int i =0; i<argPeaks.Count;i++)
            {
                _cidMzs[i] = argPeaks[i].MonoisotopicMZ;
            }

            //Find idx use binary search
            int min = 0;
            int max = argPeaks.Count - 1;
            int mid = -1;
            if (_cidMzs[max] < argMZ)
            {
                return max;
            }
            if (_cidMzs[min] > argMZ)
            {
                return min;
            }
            do
            {
                mid = min + (max - min) / 2;
                if (argMZ > _cidMzs[mid])
                {
                    min = mid + 1;
                }
                else
                {
                    max = mid - 1;
                }
            } while (min < max);

            mid++;
            if (mid + 1 > _cidMzs.Length - 1)
            {
                return mid;
            }


            if (Math.Abs(_cidMzs[mid] - argMZ) < Math.Abs(_cidMzs[mid + 1] - argMZ))
            {
                if (Math.Abs(_cidMzs[mid] - argMZ) < Math.Abs(_cidMzs[mid - 1] - argMZ))
                {
                    return mid;
                }
                else
                {
                    return mid - 1;
                }
            }
            else
            {
                if (Math.Abs(_cidMzs[mid + 1] - argMZ) < Math.Abs(_cidMzs[mid - 1] - argMZ))
                {
                    return mid + 1;
                }
                else
                {
                    return mid - 1;
                }
            }
        }
    }
}
