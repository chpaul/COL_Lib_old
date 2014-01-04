using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
namespace COL.MassLib
{
    public class RawReader : IRawFileReader
    {
        private string _fullFilePath;
        private string _fileType;
        private List<MSScan> _scans;
        private double _peptideMinBackgroundRatio = 5.0;
        private double _peakBackgroundRatio = 5.0;
        private double _singleToNoiseRatio = 3.0;
        private short _maxCharge = 10;
        private int _scanNo = 1;
        private int _numOfScans = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argFullPath"></param>
        /// <param name="argFileType">raw or mzxml</param>
        public RawReader(string argFullPath, string argFileType)
        {
            _fullFilePath = argFullPath;
            _fileType = argFileType;
        }
        public RawReader(string argFullPath, string argFileType, double argSingleToNoise, double argPeakBackground, double argPeptideBackground, short argMaxCharge)
        {
            _fullFilePath = argFullPath;
            _fileType = argFullPath;
            _singleToNoiseRatio=argSingleToNoise;
            _peakBackgroundRatio=argPeakBackground;
            _peptideMinBackgroundRatio=argPeptideBackground;
            _maxCharge = argMaxCharge;
        }
        public int NumberOfScans
        {
            get 
            {
                if (_numOfScans == 0)
                {
                    Process GlypIDReader = new Process();
                    GlypIDReader.StartInfo.FileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\GlypIDWrapper.exe";
                    GlypIDReader.StartInfo.Arguments = "\"" + _fullFilePath + "\" " + _fileType + " N ";
                    GlypIDReader.StartInfo.RedirectStandardOutput = true;
                    GlypIDReader.StartInfo.UseShellExecute = false;
                    GlypIDReader.StartInfo.CreateNoWindow = true;
                    GlypIDReader.Start();
                    GlypIDReader.WaitForExit();
                    _numOfScans = GlypIDReader.ExitCode;
                }


                return _numOfScans;
            }
        }
        public int GetMsLevel(int argScan)
        {
            MSScan scan = ReadScan(argScan);
            return scan.MsLevel;
        }
        public string RawFilePath
        {
            get { return _fullFilePath; }
        }


        PipeServer pipeSrv;
        AutoResetEvent aResetEvt;
        byte[] RecivedScan;
        public MSScan ReadScan(int argScan)
        {
            aResetEvt = new AutoResetEvent(false);
            _scanNo = argScan;
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            Thread PipeServerThread = new Thread(StartNamePipeServer);
            PipeServerThread.Start();
            PipeServerThread.Join();
 


            Thread WrapperThread = new Thread(StartWrapper);
            WrapperThread.Start();
            WrapperThread.Join();

            
            System.Runtime.Serialization.IFormatter f = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            aResetEvt.WaitOne();
            System.IO.MemoryStream memStream = new System.IO.MemoryStream(RecivedScan);
            MSScan scan = (MSScan)f.Deserialize(memStream);
            return scan;
        }
        private void StartWrapper()
        {
            Process GlypIDReader = new Process();
            GlypIDReader.StartInfo.FileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\GlypIDWrapper.exe";
            GlypIDReader.StartInfo.Arguments = "\"" + _fullFilePath + "\" " + _fileType + " R " + _scanNo.ToString() + "  " +
                                                _singleToNoiseRatio.ToString() + " " +
                                                _peakBackgroundRatio.ToString() + " " +
                                                _peptideMinBackgroundRatio.ToString() + " " +
                                                _maxCharge.ToString();
            GlypIDReader.StartInfo.RedirectStandardOutput = true;
            GlypIDReader.StartInfo.UseShellExecute = false;
            GlypIDReader.StartInfo.CreateNoWindow = true;

            GlypIDReader.Start();
            while (!GlypIDReader.StandardOutput.EndOfStream)
            {
                Console.WriteLine(GlypIDReader.StandardOutput.ReadLine());
            }


            GlypIDReader.WaitForExit();
        }
        private void StartNamePipeServer()
        {
            pipeSrv = new PipeServer();
            pipeSrv.MessageReceived += new PipeServer.MessageReceivedHandler(MsgRevied);
            pipeSrv.Start("\\\\.\\pipe\\GlypIDPipe");
        }
        private void MsgRevied(byte[] message)
        {
            aResetEvt.Set();
            RecivedScan = message;
        }

  
        public List<MSScan> ReadScans(int argStart, int argEnd)
        {
            List<MSScan> scans = new List<MSScan>();
            if (argStart <= 0)
            {
                argStart = 1;
            }
            if (argEnd > this.NumberOfScans)
            {
                argEnd = this.NumberOfScans;
            }
            for (int i = argStart; i <= argEnd; i++)
            {
                scans.Add(ReadScan(i));
            }
            return scans;
        }
        public List<MSScan> ReadAllScans()
        {
            int EndScan  =  this.NumberOfScans;
            return ReadScans( 1, EndScan);
        }
        public List<MSScan> ReadScanWMSLevel(int argStart, int argEnd, int argMSLevel)
        {
            List<MSScan> scans = new List<MSScan>();
            if (argStart <= 0)
            {
                argStart = 1;
            }
            if (argEnd > this.NumberOfScans)
            {
                argEnd = this.NumberOfScans;
            }
            for(int i =argStart;i<=argEnd;i++)
            {
                MSScan scan = ReadScan(i);
                if (scan.MsLevel == argMSLevel)
                {
                    scans.Add(scan);
                }
            }
            return scans;
        }
           
    }
}
