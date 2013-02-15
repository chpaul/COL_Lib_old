using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace COL.ProtLib
{
    public class AminoAcidMass
    {
        Hashtable MWMapping;
        public AminoAcidMass()
        {
            MWMapping = new Hashtable();
            MWMapping.Add("A", 71.03711);
            MWMapping.Add("R", 156.10111);
            MWMapping.Add("N", 114.04293);
            MWMapping.Add("D", 115.02694);
            MWMapping.Add("C", 103.00919);
            MWMapping.Add("E", 129.04259);
            MWMapping.Add("Q", 128.05858);
            MWMapping.Add("G", 57.02146);
            MWMapping.Add("H", 137.05891);
            MWMapping.Add("I", 113.08406);
            MWMapping.Add("L", 113.08406);
            MWMapping.Add("K", 128.09496);
            MWMapping.Add("M", 131.04049);
            MWMapping.Add("F", 147.06841);
            MWMapping.Add("P", 97.05276);
            MWMapping.Add("S", 87.03203);
            MWMapping.Add("T", 101.04768);
            MWMapping.Add("W", 186.07931);
            MWMapping.Add("Y", 163.06333);
            MWMapping.Add("V", 99.06841);
            MWMapping.Add("U", 150.95363);
            MWMapping.Add("O", 237.14772);
            MWMapping.Add("Cys_CM", 161.01466);
            MWMapping.Add("Cys_CAM", 160.03065);
            MWMapping.Add("Cys_PE", 208.067039);
            MWMapping.Add("Cys_PAM", 174.04631);
            MWMapping.Add("MSO", 147.0354);
            MWMapping.Add("TPO", 202.0742);
            MWMapping.Add("HSL", 100.03985);
            MWMapping.Add("H2O", 18.01532);
        }
        public float GetMonoMW(string argString, bool argCYS_CAM)
        {
            float SUM = 0.0f;
            string target = argString.ToUpper();
            for (int i = 0; i < target.Length; i++)
            {
                if (argCYS_CAM)
                {
                    if (target[i] == 'C')
                    {
                        SUM = SUM + Convert.ToSingle(MWMapping["Cys_CAM"]);
                    }
                    else
                    {
                        SUM = SUM + Convert.ToSingle(MWMapping[target[i].ToString()]);
                    }
                }
                else
                {
                    SUM = SUM + Convert.ToSingle(MWMapping[target[i].ToString()]);
                }
            }
            SUM = SUM + Convert.ToSingle(MWMapping["H2O"]);
            return SUM ;
        }
    }
}
