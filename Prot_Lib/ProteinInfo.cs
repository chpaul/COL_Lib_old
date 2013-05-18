﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace COL.ProtLib
{
    public class ProteinInfo
    {
        private string _title;
        private string _sequence;
        public ProteinInfo(string argTitle, string argSequence)
        {
            _title = argTitle;
            _sequence = argSequence.ToUpper();
        }
        public string Title
        {
            get { return _title; }
        }
        public string Sequence
        {
            get { return _sequence; }
        }
        public List<string> NGlycopeptide(int argAllowMissCleavage, Protease.Type argProteaseType)
        {
            List<string> _cleavages = CreateCleavage(argAllowMissCleavage, argProteaseType);

            List<string> _glycopep = new List<string>();
            Regex sequon = new Regex("N[ARNDCEQGHILKMFSTWYV][S|T]", RegexOptions.IgnoreCase);  //NXS NXT  X!=P
            Regex sequonEnd = new Regex("N[ARNDCEQGHILKMFSTWYV]$", RegexOptions.IgnoreCase);  //NX NX  X!=P in the end
            foreach (string pep in _cleavages)
            {
                Match Sequon = sequon.Match(pep);
                if (Sequon.Length != 0)
                {
                    _glycopep.Add(pep);
                }

                Match SequonEnd = sequonEnd.Match(pep); //Go to full sequence to check if the sequence contain S/T
                if (SequonEnd.Length != 0)
                {
                    int idx = _sequence.IndexOf(pep) + pep.Length;
                    if (idx < _sequence.Length)
                    {
                        if (_sequence[idx] == 'S' || _sequence[idx] == 'T')
                        {
                            _glycopep.Add(pep);
                        }
                    }
                }
            }
            return _glycopep;
        }
        public List<string> OGlycopeptide(int argAllowMissCleavage, Protease.Type argProteaseType)
        {
            List<string> _cleavages = CreateCleavage(argAllowMissCleavage, argProteaseType);

            List<string> _glycopep = new List<string>();
             foreach (string pep in _cleavages)
            {
                if (pep.ToUpper().Contains("S") || pep.ToUpper().Contains("T"))
                {
                    _glycopep.Add(pep);
                }
             }
            return _glycopep;
        }
        public List<string> CreateCleavage(int argAllowMissCleavage, Protease.Type argProteaseType)
        {
            List<string> _cleavedPeptides;
            if (argAllowMissCleavage == -1)
            {
                _cleavedPeptides = new List<string>();
                _cleavedPeptides.Add(_sequence);
                return _cleavedPeptides;
            }
            if (argProteaseType == Protease.Type.None)
            {
                _cleavedPeptides = new List<string>();
                _cleavedPeptides.Add(_sequence);
            }
            else if (argProteaseType == Protease.Type.Trypsin)
            {
                _cleavedPeptides = TrypsinDigest();
            }
            else if (argProteaseType == Protease.Type.GlucED)
            {
                _cleavedPeptides = GluCDigest(true);
            }
            else 
            {
                _cleavedPeptides = GluCDigest(false);
            }
            string Combind = "";
            List<string> MissCleavage = new List<string>();
            for (int i = 1; i <= argAllowMissCleavage; i++)
            {
                for (int j = 0; j < _cleavedPeptides.Count; j++)
                {
                    Combind = _cleavedPeptides[j];
                    for (int k = 1; k <= i; k++)
                    {
                        if (j + i < _cleavedPeptides.Count)
                        {
                            Combind = Combind + _cleavedPeptides[j + k];
                        }
                    }
                    if (Combind != _cleavedPeptides[j])
                    {
                        MissCleavage.Add(Combind);
                    }
                }
            }
            foreach (string cleavage in MissCleavage)
            {
                _cleavedPeptides.Add(cleavage);
            }
            return _cleavedPeptides;
        }
        /// <summary>
        /// http://web.mit.edu/toxms/www/filters/python/Biotin_peptide_ID_program/trypsin.py
        /// </summary>
        private List<string> TrypsinDigest()
        {
            string Seq = _sequence;
            List<string> _cleavages = new List<string>();
            int p = 0;
            for (int i = 0; i < _sequence.Length - 1; i++)
            {
                if (Seq[i] == 'R' && Seq[i + 1] != 'P')
                {
                    _cleavages.Add(Seq.Substring(p, Seq.IndexOf("R", p) + 1 - p));
                    p = i + 1;
                }
                if (Seq[i] == 'K' && Seq[i + 1] != 'P')
                {
                    _cleavages.Add(Seq.Substring(p, Seq.IndexOf("K", p) + 1 - p));
                    p = i + 1;
                }
            }
            _cleavages.Add(Seq.Substring(p));
            return _cleavages;
        }
        /// <summary>
        /// non-phosphate buffers, Glu-C cleaves on the C-terminal side of glutamic acid (E)
        /// in phosphate buffer  Glu-C cleaves on the C-terminal side of both glutamic acid(E) and aspartic acid (D)
        /// </summary>
        private List<string> GluCDigest(bool argIsPhosphate)
        {
            string Seq = _sequence;
            List<string> _cleavages = new List<string>();
            int p = 0;
            for (int i = 0; i < _sequence.Length - 1; i++)
            {
                if (Seq[i] == 'E' )
                {
                    _cleavages.Add(Seq.Substring(p, Seq.IndexOf("E", p) + 1 - p));
                    p = i + 1;
                }
                if (argIsPhosphate)
                {
                    if (Seq[i] == 'D')
                    {
                        _cleavages.Add(Seq.Substring(p, Seq.IndexOf("D", p) + 1 - p));
                        p = i + 1;
                    }
                }
            }
            _cleavages.Add(Seq.Substring(p));
            return _cleavages;
        }
    }
}
