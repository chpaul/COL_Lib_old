using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using COL.MassLib;
namespace TestApp
{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
        }

        private void btnReader_Click(object sender, EventArgs e)
        {
            XRawReader raw = new XRawReader(txtFileName.Text);
            MSScan Scans = raw.ReadScan(1861);
        }
    }
}
