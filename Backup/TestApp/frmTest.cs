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
            //Class1 cs = new Class1(txtFileName.Text, 1861);
            for (int i = 1; i <= raw.NumberOfScans; i++)
            {
               // cs = new Class1(txtFileName.Text, i);
                MSScan s = raw.ReadScan(i);
            }
        }
    }
}
