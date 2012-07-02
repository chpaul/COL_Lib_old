using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using COL.GlycoLib;
namespace TestApp
{
    public partial class frmDrawer : Form
    {
        public frmDrawer()
        {
            InitializeComponent();
            comboBox1.Items.Add("Hex-Hex-(Hex-)(Hex-HexNAc-)(Hex-HexNAc-Hex-)Hex-HexNAc-(DeHex-)HexNAc");
            comboBox1.Items.Add("NeuGc a2-3/6 Gal b1-4 GlcNAc b1-2( NeuGc a2-3/6 Gal b1-4 GlcNAc b1-4) Man a1-3( Gal b1-4( Fuc a1-3) GlcNAc b1-2 Man a1-6) Man b1-4 GlcNAc b1-4( Fuc a1-6) GlcNAc ");
            //http://www.functionalglycomics.org/glycomics/CarbohydrateServlet?pageType=view&view=view&operationType=view&carbId=carbNlink_33014_A&sideMenu=no
            comboBox1.Items.Add("Man??(Man??)Man??GlcNAc??(Fuc??)GlcNAc");
            comboBox1.Items.Add("NeuAc??(GalNAc??)Gal??GlcNAc??(Gal??GlcNAc??)Man?? (Gal??Gal??GlcNAc??(Gal??GlcNAc??Gal??GlcNAc??Gal??GlcNAc??)Man??)Man??GlcNAc??(Fuc??)GlcNAc");
            //http://www.functionalglycomics.org/glycomics/CarbohydrateServlet?pageType=view&view=view&operationType=view&carbId=carbNlink_35397_A&sideMenu=no
            comboBox1.Items.Add("GlcNAc ??( NeuAc ?? Gal ??( Fuc ??) GlcNAc ??( NeuAc ?? Gal ?? GlcNAc ??) Man ??)  ( GlcNAc ?? Man ??) Man ?? GlcNAc ??( Fuc ??) GlcNAc");
            //http://www.functionalglycomics.org/glycomics/CarbohydrateServlet?pageType=view&view=view&operationType=view&carbId=carbNlink_35416_A&sideMenu=no
            comboBox1.Items.Add("DeHex-(DeHex-)HexNAc-Hex-(DeHex-(DeHex-)HexNAc-Hex-)Hex-HexNAc-HexNAc");
            comboBox1.Items.Add("");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            //GlycansDraw GS = new GlycansDraw();
            COL.GlycoLib.GlycansDrawer GS = new COL.GlycoLib.GlycansDrawer(comboBox1.Text.ToString(), false,1);
           
            
             Image a = GS.GetImage();
             a.Save(@"D:\!SVN\GlycanSequencing\Code\bin\x86\Debug\aaa.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
             pictureBox1.Image = a;
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (!Directory.Exists(folderBrowserDialog1.SelectedPath))
                    {
                        Directory.CreateDirectory(folderBrowserDialog1.SelectedPath);
                    }
                    if (!Directory.Exists(folderBrowserDialog1.SelectedPath + "\\Pics"))
                    {
                        Directory.CreateDirectory(folderBrowserDialog1.SelectedPath + "\\Pics");
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                    sb.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                    sb.AppendLine("<head>");
                    sb.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                    sb.AppendLine("<title>GlycoSeq</title>\n</head>\n<body>");
                    GlycansDrawer Draw;
                    StreamReader SR = new StreamReader(openFileDialog1.FileName);
                    sb.AppendLine("<table  border=\"1\">");
                    sb.AppendLine("<tr><td>ID</td><td>Orignal Glycan</td><td>Add Glycan</td><td>Result</td></tr>");
                    int idx = 1;
                    do
                    {
                        string[] tmp = SR.ReadLine().Split(',');
                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td>");
                        sb.AppendLine(idx.ToString());
                        sb.AppendLine("</td>");

                        Draw = new GlycansDrawer(tmp[0], false);
                        Image tmpImg = Draw.GetImage();
                        tmpImg.Save(folderBrowserDialog1.SelectedPath + "\\Pics\\PreStructure_" + idx.ToString("000") + ".png");
                        tmpImg.Dispose();
                        tmpImg = null;
                        sb.AppendLine("<td><img src=\".\\Pics\\PreStructure_" + idx.ToString("000") + ".png\"/><br>" + tmp[0]+ "</td>");

                        Draw = new GlycansDrawer(tmp[1], false);
                        tmpImg = Draw.GetImage();
                        tmpImg.Save(folderBrowserDialog1.SelectedPath + "\\Pics\\AddStructure_" + idx.ToString("000") + ".png");
                        tmpImg.Dispose();
                        tmpImg = null;
                        sb.AppendLine("<td><img src=\".\\Pics\\AddStructure_" + idx.ToString("000") + ".png\"/><br>"+tmp[1] + "</td>");

                        sb.AppendLine("<td>");

                        for (int i = 2; i < tmp.Length; i++)
                        {
                            Draw = new GlycansDrawer(tmp[i], false);
                            tmpImg = Draw.GetImage();
                            tmpImg.Save(folderBrowserDialog1.SelectedPath + "\\Pics\\AddedStructure_" + idx.ToString("000")+"-"+(i-1).ToString("000") + ".png");
                            tmpImg.Dispose();
                            tmpImg = null;
                            sb.AppendLine("<img src=\".\\Pics\\AddedStructure_"  + idx.ToString("000")+"-"+(i-1).ToString("000")+ ".png\"/>" );

                        }
                        sb.AppendLine("</td>");
                        sb.AppendLine("</tr>");
                        idx++;
                    } while (!SR.EndOfStream);
                    sb.AppendLine("</table><br>-----------------------------------------------------------<br>");
                    sb.AppendLine("</body>\n</html>");
                    StreamWriter Sw = new StreamWriter(folderBrowserDialog1.SelectedPath + "\\Result.htm");
                    Sw.Write(sb.ToString());
                    Sw.Flush();
                    Sw.Close();
                    MessageBox.Show("Done");
                }
            }

        }
    }
}
