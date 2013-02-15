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
            DT.Columns.Add(Title);
            DT.Columns.Add(Value1);
            DT.Columns.Add(Value2);
            DT.Columns.Add(Value3);
            DT.Columns.Add(Value4);
            DT.Columns.Add(Value5);
            DT.Columns.Add(Value6);        
        }
        MSScan _scan;
        DataTable DT = new DataTable();
        DataColumn Title = new DataColumn("Title", Type.GetType("System.String"));
        DataColumn Value1 = new DataColumn("Value1/ChargeState", Type.GetType("System.String"));
        DataColumn Value2 = new DataColumn("MonoisotopicMZ", Type.GetType("System.String"));
        DataColumn Value3 = new DataColumn("MonoMass", Type.GetType("System.String"));
        DataColumn Value4 = new DataColumn("MonoIntensity", Type.GetType("System.String"));
        DataColumn Value5 = new DataColumn("MostIntenseMass", Type.GetType("System.String"));
        DataColumn Value6 = new DataColumn("FitScore", Type.GetType("System.String"));
        


        private void btnReader_Click(object sender, EventArgs e)
        {

            ReadScan(Convert.ToInt32(txtScanNo.Text));
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = openFileDialog1.FileName;
            }
        }
        private void ReadScan(int argScanNo)
        {
            XRawReader raw = new XRawReader(txtFileName.Text);
            //Class1 cs = new Class1(txtFileName.Text, 1861);
            for (int i = 1; i <= raw.NumberOfScans; i++)
            {
               // cs = new Class1(txtFileName.Text, i);
                MSScan s = raw.ReadScan(i);
            }

            _scan = raw.ReadScan(argScanNo);
            dataGridView1.DataSource = DT;
            List<string> Sb = new List<string>();
            Sb.Add("ScanNo:" + _scan.ScanNo.ToString() + ",Time:" + _scan.Time.ToString() + ",MSLevel:" + _scan.MsLevel.ToString() + ",PeakCount:" + _scan.Count.ToString() +
            ",MinMz:" + _scan.MinMZ.ToString() + "MaxMz:" + _scan.MaxMZ.ToString() + ",MaxIntensity:" + _scan.MaxIntensity.ToString() + ",MinIntensity:" + _scan.MinIntensity.ToString());


            for (int i = 0; i < _scan.MSPeaks.Count; i++)
            {
                Sb.Add(i.ToString() + "," +
                              _scan.MSPeaks[i].ChargeState.ToString() + "," +
                              _scan.MSPeaks[i].MonoisotopicMZ.ToString() + "," +
                              _scan.MSPeaks[i].MonoMass.ToString() + "," +
                              _scan.MSPeaks[i].MonoIntensity.ToString() + "," +
                              _scan.MSPeaks[i].MostIntenseMass.ToString() + "," +
                              _scan.MSPeaks[i].FitScore.ToString());
            }
            DT.Rows.Clear();
            foreach (string s in Sb)
            {
                string[] tmp = s.Split(',');
                DataRow row = DT.NewRow();
                for (int i = 0; i < tmp.Length; i++)
                {
                    row[i] = tmp[i];
                }
                DT.Rows.Add(row);
            }
            
        }

        private void btnIncreaseScanNo_Click(object sender, EventArgs e)
        {
            txtScanNo.Text = (Convert.ToInt32(txtScanNo.Text) + 1).ToString();
            ReadScan(Convert.ToInt32(txtScanNo.Text));
        }

        private void btnDecreaseScanNo_Click(object sender, EventArgs e)
        {
            txtScanNo.Text = (Convert.ToInt32(txtScanNo.Text) -1).ToString();
            ReadScan(Convert.ToInt32(txtScanNo.Text));
        }
    }
}
