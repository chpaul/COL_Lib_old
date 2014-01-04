using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
namespace TestApp
{
    public partial class frmNamePipeReader : Form
    {
        string FileName = @"d:\Fetuin_400ng_ETD_100RA_062212.raw";
        
        public frmNamePipeReader()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                COL.MassLib.RawReader raw = new COL.MassLib.RawReader(@"d:\Fetuin_400ng_ETD_100RA_062212.raw", "raw");
                List<COL.MassLib.MSScan> scans = raw.ReadAllScans();
                //textBox1.Text = sc.ScanHeader;
            }
        }

        private void btnServerNamedPipe_Click(object sender, EventArgs e)
        {

            COL.MassLib.RawReader raw = new COL.MassLib.RawReader(@"D:\Fetuin_400ng_ETD_100RA_062212.raw", "raw");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process GlypIDReader = new Process();
            GlypIDReader.StartInfo.FileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\GlypIDWrapper.exe";
            GlypIDReader.StartInfo.Arguments = "\"" + FileName + "\" " + "Raw" + " R " + "100" + "  " +
                                               "3.0" + " " +
                                                "5.0" + " " +
                                                "5.0" + " " +
                                                "10";

            GlypIDReader.Start();
            GlypIDReader.WaitForExit();
        }
    }
}
