using System;
using System.Collections.Generic;
using System.Text;

namespace COL.ProtLib
{
    public class FastaFile
    {
        private string _fastaFullPath;
        private List<ProteinInfo> _proteinInfo;
        private int _misscleavaged;
        public FastaFile(string argFastaFullPath)
        {
            _fastaFullPath = argFastaFullPath;            
            _proteinInfo = FastaReader.ReadFasta(_fastaFullPath);
        }
        public void CreateCleavedPeptides(int argAllowMissCleavage, Protease.Type argProteaseType)
        {
            _misscleavaged = argAllowMissCleavage;
            foreach (ProteinInfo prot in _proteinInfo)
            {
                List<string> peptides = prot.CreateCleavage(_misscleavaged, argProteaseType);
            }
        }
        public void CreateCleavedOGlycoPeptides(int argAllowMissCleavage, Protease.Type argProteaseType)
        {
            _misscleavaged = argAllowMissCleavage;
            foreach (ProteinInfo prot in _proteinInfo)
            {
                List<string> glycopeptides = prot.OGlycopeptide(_misscleavaged, argProteaseType);
            }
        }
        public void CreateCleavedNGlycoPeptides(int argAllowMissCleavage, Protease.Type argProteaseType)
        {
            _misscleavaged = argAllowMissCleavage;
            foreach (ProteinInfo prot in _proteinInfo)
            {
                List<string> glycopeptides = prot.NGlycopeptide(_misscleavaged, argProteaseType);
            }
        }
    }
}
