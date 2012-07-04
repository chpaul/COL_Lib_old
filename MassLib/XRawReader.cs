using System;
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
        float[] _parentRawMzs;
        float[] _parentRawIntensitys;
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
            mobjPeakParameters.SignalToNoiseThreshold = _singleToNoiseRatio;
            mobjPeakParameters.PeakBackgroundRatio = _peakBackgroundRatio;          
            mobj_PeakProcessor.SetOptions(mobjPeakParameters);            
            mobj_PeakProcessor.ProfileType = GlypID.enmProfileType.CENTROIDED;
            _cidPeaks = new GlypID.Peaks.clsPeak[1];
            mobj_PeakProcessor.DiscoverPeaks(ref _cidMzs, ref _cidIntensities, ref _cidPeaks,
                                    Convert.ToSingle(mobjTransformParameters.MinMZ), Convert.ToSingle(mobjTransformParameters.MaxMZ), false);
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
            
            // for getting results
            List<MSPeak> _lstPeak = new List<MSPeak>();
            for (int chNum = 0; chNum < _cidPeaks.Length; chNum++)
            {
                _lstPeak.Add(new MSPeak(
                    _cidPeaks[chNum].mdbl_mz,
                    _cidPeaks[chNum].mdbl_intensity));            
            }
            //Setup MSScan
            scan.MSPeaks = _lstPeak;
            scan.Time = Raw.GetScanTime(scan.ScanNo);
            if (scan.MsLevel == 1)
            {
                mobjTransform = null;
                _transformResult = null;
                return scan;
            }
            mobjTransform.PerformTransform(Convert.ToSingle(mdbl_current_background_intensity), Convert.ToSingle(min_peptide_intensity), ref _cidMzs, ref _cidIntensities, ref _cidPeaks, ref _transformResult);

            // Get parent information
            scan.ParentScanNo = Raw.GetParentScan(scan.ScanNo);
            GlypID.Peaks.clsPeak[] _parentPeaks = new GlypID.Peaks.clsPeak[1];
            Raw.GetSpectrum(scan.ParentScanNo, ref _parentRawMzs, ref _parentRawIntensitys);

            GlypID.Peaks.clsPeakProcessor parentPeakProcessor = new GlypID.Peaks.clsPeakProcessor();
            parentPeakProcessor.ProfileType = GlypID.enmProfileType.PROFILE;
            parentPeakProcessor.DiscoverPeaks(ref _parentRawMzs, ref _parentRawIntensitys, ref _parentPeaks, Convert.ToSingle(mobjTransformParameters.MinMZ), Convert.ToSingle(mobjTransformParameters.MaxMZ), true);
            float _parentBackgroundIntensity = (float)parentPeakProcessor.GetBackgroundIntensity(ref _parentRawIntensitys);


            _transformResult = new GlypID.HornTransform.clsHornTransformResults[1];            
            bool found = mobjTransform.FindPrecursorTransform(Convert.ToSingle(_parentBackgroundIntensity), Convert.ToSingle(min_peptide_intensity), ref _parentRawMzs, ref _parentRawIntensitys, ref _parentPeaks, Convert.ToSingle(scan.ParentMZ), ref _transformResult);
            if (Raw.IsFTScan(scan.ParentScanNo))
            {
                // High resolution data
                found = mobjTransform.FindPrecursorTransform(Convert.ToSingle(_parentBackgroundIntensity), Convert.ToSingle(min_peptide_intensity), ref _parentRawMzs, ref _parentRawIntensitys, ref _parentPeaks, Convert.ToSingle(scan.ParentMZ), ref _transformResult);
            }
            if (!found)
            {
                // Low resolution data or bad high res spectra
                short cs = Raw.GetMonoChargeFromHeader(scan.ScanNo);
                if (cs > 0)
                {
                    short[] charges = new short[1];
                    charges[0] = cs;
                    mobjTransform.AllocateValuesToTransform(Convert.ToSingle(scan.ParentMZ), ref charges, ref _transformResult);
                }
                else
                {
                    // instrument has no charge just store 2 and 3.      
                    short[] charges = new short[2];
                    charges[0] = 2;
                    charges[1] = 3;
                    mobjTransform.AllocateValuesToTransform(Convert.ToSingle(scan.ParentMZ), ref charges, ref _transformResult);
                }
            }           
            //Setup MSScan

            scan.ParentMonoMW= (float)_transformResult[0].mdbl_mono_mw;
            scan.ParentCharge = (int)_transformResult[0].mshort_cs;          
           _transformResult = null;
            mobjTransform = null;
            return scan;
        }
    }
}
