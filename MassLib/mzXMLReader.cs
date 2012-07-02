using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;
namespace COL.MassLib
{
    /// <summary>
    /// Need to rewrite
    /// </summary>
    public class mzXMLReader//: IRawFileReader
    {
        
        string _filepath;
        MSScan _scan;
        public mzXMLReader(string filepath)
        {
            _filepath = filepath;
        }
        /// <summary>
        /// Merge All Peaks to Scan 0
        /// </summary>
        /// <returns></returns>
        public MSScan GetAllPeaks()
        {

            XmlTextReader xmlReader = new XmlTextReader(_filepath);
            _scan = new MSScan(0);
            //List<MSPoint> _msp = new List<MSPoint>();
            
            xmlReader.XmlResolver = null;
            try
            {
                while (xmlReader.Read())
                {

                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            if (xmlReader.Name == "scan")
                            {

                                if (xmlReader.GetAttribute("msLevel") == "1")
                                {
                                    int PeakCount = Convert.ToInt32(xmlReader.GetAttribute("peaksCount"));
                                    int ScanNum = Convert.ToInt32(xmlReader.GetAttribute("num"));
                                    xmlReader.Read();
                                    xmlReader.Read();
                                    if (xmlReader.Name == "peaks")
                                    {
                                        int Precision = Convert.ToInt32(xmlReader.GetAttribute("precision"));
                                        xmlReader.Read();
                                        string peakString = xmlReader.Value;
                                        List<MSPoint> tmp = ParsePeakNode(peakString, PeakCount);
                                        foreach (MSPoint p in tmp)
                                        {
                                          //  _scan.AddPoint(p);
                                        }

                                    }
                                }
                            }
                            break;
                    }
                }
            //    _scan.ConvertHashTableToList();
                return _scan;
            }
            catch
            {
                throw new Exception("Reading Peak in mzXML format error!! Please Check input File");
            }
        }
        protected List<MSPoint> ParsePeakNode(string peakString, int peaksCount)
        {
            int offset;
            try
            {
                byte[] decoded = System.Convert.FromBase64String(peakString);

                List<MSPoint> _points = new List<MSPoint>();
                //float mz = 0.0f, intensity = 0.0f;
                for (int i = 0; i < peaksCount; i++)
                {

                    //Array.Reverse(decoded, i * 8, 4);
                    //Array.Reverse(decoded, i * 8 + 4, 4);
                    XYPair val;
                    val.x = 0;
                    val.y = 0;
                    offset = i * 8;
                    val.b0 = decoded[offset + 7];
                    val.b1 = decoded[offset + 6];
                    val.b2 = decoded[offset + 5];
                    val.b3 = decoded[offset + 4];
                    val.b4 = decoded[offset + 3];
                    val.b5 = decoded[offset + 2];
                    val.b6 = decoded[offset + 1];
                    val.b7 = decoded[offset];
                    _points.Add(new MSPoint(val.x, val.y));
                }

                return _points;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        [System.Runtime.InteropServices.StructLayout(LayoutKind.Explicit)]
        private struct XYPair
        {
            [FieldOffset(0)]
            public byte b0;
            [FieldOffset(1)]
            public byte b1;
            [FieldOffset(2)]
            public byte b2;
            [FieldOffset(3)]
            public byte b3;
            [FieldOffset(4)]
            public byte b4;
            [FieldOffset(5)]
            public byte b5;
            [FieldOffset(6)]
            public byte b6;
            [FieldOffset(7)]
            public byte b7;

            [FieldOffset(4)]
            public float x;
            [FieldOffset(0)]
            public float y;
        }
    }
}
