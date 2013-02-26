using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
namespace COL.GlycoLib
{
    public class ReadGlycanListFromFile
    {
        public static List<GlycanCompound> ReadGlycanList(string argGlycanFile, bool argIsPermethylated,bool argIsHuman, bool argReducedReducingEnd)
        {
            return ReadGlycanList(argGlycanFile, argIsPermethylated, false, true, argIsHuman, argReducedReducingEnd);
        }
        public static List<GlycanCompound> ReadGlycanList(string argGlycanFile, bool argIsPermethylated, bool argIsDeuterium, bool argIsSodium, bool argIsHuman, bool argReducedReducingEnd)
        {

            List<GlycanCompound> _GlycanList = new List<GlycanCompound>();
            StreamReader sr;
            //Assembly assembly = Assembly.GetExecutingAssembly();
            //sr = new StreamReader(assembly.GetManifestResourceStream( "MutliNGlycanFitControls.Properties.Resources.combinations.txt"));
            int LineNumber = 0;
            sr = new StreamReader(argGlycanFile);

            string tmp; // temp line for processing
            tmp = sr.ReadLine();
            LineNumber++;
            Hashtable compindex = new Hashtable(); //Glycan Type index.



            //Read the title
            string[] spilttmp = tmp.Trim().Split(',');
            try
            {
                for (int i = 0; i < spilttmp.Length; i++)
                {
                    if (spilttmp[i].ToLower() == "neunac" || spilttmp[i].ToLower() == "neungc" || spilttmp[i].ToLower() == "sialic")
                    {
                        compindex.Add("sia", i);
                        continue;
                    }
                    if (spilttmp[i].ToLower() != "hexnac" && spilttmp[i].ToLower() != "hex" && spilttmp[i].ToLower() != "dehex" && spilttmp[i].ToLower() != "sia")
                    {
                        throw new Exception("Glycan list file title error. (Use:HexNAc,Hex,DeHex,Sia,NeuNAc,NeuNGc)");
                    }
                    compindex.Add(spilttmp[i].ToLower(), i);
                }
            }
            catch (Exception ex)
            {
                sr.Close();
                throw ex;
            }
            int processed_count = 0;

            //Read the list    
            try
            {
                do
                {
                    tmp = sr.ReadLine();
                    LineNumber++;
                    spilttmp = tmp.Trim().Split(',');
                    _GlycanList.Add(new GlycanCompound(Convert.ToInt32(spilttmp[(int)compindex["hexnac"]]),
                                             Convert.ToInt32(spilttmp[(int)compindex["hex"]]),
                                             Convert.ToInt32(spilttmp[(int)compindex["dehex"]]),
                                             Convert.ToInt32(spilttmp[(int)compindex["sia"]]),
                                             argIsPermethylated,
                                             argIsDeuterium,
                                             argReducedReducingEnd,
                                             false,
                                             true,
                                             true)
                                             );
                    processed_count++;
                } while (!sr.EndOfStream);
            }
            catch (Exception ex)
            {
                throw new Exception("Glycan list file reading error on Line:" + LineNumber + ". Please check input file. (" + ex.Message + ")");
            }
            finally
            {
                sr.Close();
            }

            if (_GlycanList.Count == 0)
            {
                throw new Exception("Glycan list file reading error. Please check input file.");
            }
           _GlycanList.Sort();
           return _GlycanList;
        }
    }
}
