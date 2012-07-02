using System;
using System.Collections.Generic;
using System.Text;

namespace COL.Prot_Lib
{
    class FastaFile
    {
        private string _fastaFullPath;
        private List<ProteinInfo> _proteinInfo;
        private int _misscleavaged;
        public FastaFile(string argFastaFullPath)
        {
            _fastaFullPath = argFastaFullPath;            
            _proteinInfo = FastaReader.ReadFasta(_fastaFullPath);
        }
        public void CreateCleavedPeptides(int argAllowMissCleavage)
        {
            _misscleavaged = argAllowMissCleavage;
            foreach (ProteinInfo prot in _proteinInfo)
            {
                List<string> peptides = prot.Glycopeptide(_misscleavaged);
            }
        }
        public void CreateCleavedGlycoPeptides(int argAllowMissCleavage)
        {
            _misscleavaged = argAllowMissCleavage;
            foreach (ProteinInfo prot in _proteinInfo)
            {
                List<string> glycopeptides = prot.Glycopeptide(_misscleavaged);
            }
        }
    }
}
