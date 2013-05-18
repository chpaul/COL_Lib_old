using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace TestApp
{
    public partial class frmE3D : Form
    {
        public frmE3D()
        {
            InitializeComponent();
        }
        Dictionary<string, Dictionary<float, List<string>>> dictValue;
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                cboGlycan.Items.Clear();
                dictValue = new Dictionary<string, Dictionary<float, List<string>>>();
                StreamReader sr = new StreamReader(openFileDialog1.FileName);
                sr.ReadLine(); //title
                string[] tmp = null;
                ArrayList alstGlycans = new ArrayList();
                do
                {
                    tmp = sr.ReadLine().Split(',');
                    int Charge = Convert.ToInt32(Math.Round(Convert.ToSingle(tmp[5]) / Convert.ToSingle(tmp[3]), 0));
                    string Key = tmp[4] + "-" + Charge.ToString(); // hex-hexnac-dehax-sia
                    if (!dictValue.ContainsKey(Key))
                    {
                        dictValue.Add(Key, new Dictionary<float, List<string>>());
                        alstGlycans.Add(Key);
                    }
                    float mz = Convert.ToSingle(tmp[3].ToString());
                    mz = Convert.ToSingle(mz.ToString("F1"));
                    if (!dictValue[Key].ContainsKey(mz))
                    {
                        dictValue[Key].Add(mz, new List<string>());
                    }

                    dictValue[Key][mz].Add(tmp[0] + "-" + tmp[2]); // scan time - abuntance
                } while (!sr.EndOfStream);
                sr.Close();
                alstGlycans.Sort();
                cboGlycan.Items.AddRange(alstGlycans.ToArray());

            }
        }

        private void cboGlycan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGlycan.SelectedItem == null)
            {
                return;
            }
            string key = cboGlycan.SelectedItem.ToString();
            Dictionary<float, List<string>> keyValue = dictValue[key];

            string[] tmpLst = null;  

            double MaxIntensity = 0.0;

            List<int> GlycanMZ = new List<int>();
            COL.ElutionViewer.MSPointSet3D Eluction3DRaw = new COL.ElutionViewer.MSPointSet3D();
            foreach (float mz in keyValue.Keys)
            {
               
                foreach (string tmp in keyValue[mz])
                {
                    tmpLst = tmp.Split('-');
                    float time = Convert.ToSingle(tmpLst[0]);
                    float intensity = Convert.ToSingle(tmpLst[1]);

                    Eluction3DRaw.Add(time, mz, intensity);
                }

            }
            eluctionViewer1.SetData(COL.ElutionViewer.EViewerHandler.Create3DHandler(Eluction3DRaw));
            //eluctionViewer1.Smooth();
        }
    }
}
