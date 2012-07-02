﻿using System;
using System.Collections.Generic;
using System.Text;

namespace COL.MassLib
{
    /// <summary>
    /// Use GlypIDEngine dll to read Xcalibur RAW (.raw)
    /// </summary>
    public class XRawReader: IRawFileReader
    {
        private string _fullFilePath;
        private List<MSScan> _scans;
        GlypID.Readers.clsRawData Raw;

        private double _peptideMinBackgroundRatio = 5.0;
        private double _peakBackgroundRatio = 5.0;
        private double _singleToNoiseRatio = 3.0;
        private short _maxCharge = 10;

        public int NumberOfScans
        {
            get { return Raw.GetNumScans(); }
        }
        public GlypID.Peaks.clsPeakProcessorParameters PeakProcessorParameter
        {
            set
            {
                _singleToNoiseRatio = value.SignalToNoiseThreshold;
                _peakBackgroundRatio = value.PeakBackgroundRatio;
            }
        }
        public GlypID.HornTransform.clsHornTransformParameters TransformParameter
        {
            set
            {
                _peptideMinBackgroundRatio = value.PeptideMinBackgroundRatio;
                _maxCharge = value.MaxCharge;
            }
        }
        public int GetMsLevel(int argScan)
        {
            return Raw.GetMSLevel(argScan);
        }
        public XRawReader(string argFullFilePath)
        {
            _fullFilePath = argFullFilePath;
            Raw = new GlypID.Readers.clsRawData(_fullFilePath, GlypID.Readers.FileType.FINNIGAN);
        }
        public List<MSScan> ReadScans(int argStart, int argEnd)
        {
            _scans = new List<MSScan>();
            for (int i = argStart; i <= argEnd; i++)
            {
                _scans.Add(GetScanFromFile(i));
            }
            return _scans;
        }
        public MSScan ReadScan(int argScan)
        {
            return GetScanFromFile(argScan);
        }
        public List<MSScan> ReadAllScans()
        {
            int argStart = 1;
            int argEnd = Raw.GetNumScans();
            return ReadScans(argStart, argEnd);
        }
        public List<MSScan> ReadScanWMSLevel(int argStart, int argEnd, int argMSLevel)
        {
            _scans = new List<MSScan>();
            List<MSScan> tmpScans = ReadScans(argStart, argEnd);
            foreach (MSScan scan in tmpScans)
            {
                if (scan.MsLevel == argMSLevel)
                {
                    _scans.Add(scan);
                }
            }
            return _scans;
        }
        float[] _cidMzs ;
        float[] _cidIntensities ;
        private MSScan GetScanFromFile(int argScanNo)
        {
            MSScan scan = new MSScan(argScanNo);

            GlypID.Peaks.clsPeak[] _cidPeaks;
            GlypID.HornTransform.clsHornTransformResults[] _transformResult;


            Raw.GetSpectrum(argScanNo,ref _cidMzs,ref  _cidIntensities);
            scan.MsLevel = Raw.GetMSLevel(argScanNo);


            GlypID.HornTransform.clsHornTransformParameters mobjTransformParameters;
            GlypID.Peaks.clsPeakProcessorParameters mobjPeakParameters;
            double mdbl_current_background_intensity;
            GlypID.Peaks.clsPeakProcessor mobj_PeakProcessor;
            GlypID.HornTransform.clsHornTransform mobjTransform;
            // Inits
            mobjTransform = new GlypID.HornTransform.clsHornTransform();
            mobjTransformParameters = new GlypID.HornTransform.clsHornTransformParameters();
            mobj_PeakProcessor = new GlypID.Peaks.clsPeakProcessor();
            mobjPeakParameters = new GlypID.Peaks.clsPeakProcessorParameters();

            // Now find peaks
            mobjPeakParameters.ThresholdedData = true; //for Thermo data
            mobjPeakParameters.SignalToNoiseThreshold = _singleToNoiseRatio;
            mobjPeakParameters.PeakBackgroundRatio = _peakBackgroundRatio;          
            mobj_PeakProcessor.SetOptions(mobjPeakParameters);
            _cidPeaks = new GlypID.Peaks.clsPeak[1];
            mobj_PeakProcessor.ProfileType = GlypID.enmProfileType.PROFILE;
            mobj_PeakProcessor.DiscoverPeaks(ref _cidMzs, ref _cidIntensities, ref _cidPeaks,
                                    Convert.ToSingle(mobjTransformParameters.MinMZ), Convert.ToSingle(mobjTransformParameters.MaxMZ), true);
            mdbl_current_background_intensity = mobj_PeakProcessor.GetBackgroundIntensity(ref _cidIntensities);

            //  Now perform deisotoping
            _transformResult = new GlypID.HornTransform.clsHornTransformResults[1];
            mobjTransformParameters.PeptideMinBackgroundRatio = _peptideMinBackgroundRatio;
            mobjTransformParameters.MaxCharge = _maxCharge;
            mobjTransform.TransformParameters = mobjTransformParameters;

            double min_peptide_intensity = mdbl_current_background_intensity * mobjTransformParameters.PeptideMinBackgroundRatio;
            if (mobjTransformParameters.UseAbsolutePeptideIntensity)
            {
                if (min_peptide_intensity < mobjTransformParameters.AbsolutePeptideIntensity)
                    min_peptide_intensity = mobjTransformParameters.AbsolutePeptideIntensity;
            }
            mobjTransform.PerformTransform(Convert.ToSingle(mdbl_current_background_intensity), Convert.ToSingle(min_peptide_intensity), ref _cidMzs, ref _cidIntensities, ref _cidPeaks, ref _transformResult);

            // for getting results

            List<MSPeak> _lstPeak = new List<MSPeak>();
            for (int chNum = 0; chNum < _transformResult.Length; chNum++)
            {
                _lstPeak.Add(new MSPeak(
                    _transformResult[chNum].mdbl_mono_mw,
                    _transformResult[chNum].mint_abundance,
                    _transformResult[chNum].mshort_cs,
                    _transformResult[chNum].mdbl_mz,
                    _transformResult[chNum].mdbl_fit,
                    _transformResult[chNum].mdbl_most_intense_mw));
            }

            _transformResult = null;
            mobjTransform = null;
            
            //Setup MSScan
            scan.MSPeaks = _lstPeak;
            if (scan.MsLevel != 1)
            {
                scan.ParentScanNo = Raw.GetParentScan(scan.ScanNo);
            }
            scan.Time = Raw.GetScanTime(scan.ScanNo);

           _transformResult = null;
            mobjTransform = null;
            return scan;
        }
    }
}