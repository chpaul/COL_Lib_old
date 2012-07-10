using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using COL.MassLib;
using COL.GlycoLib;
namespace COL.GlycoLib
{
    public class GlycanImage
    {
        public static Image GetAnnotatedImage(string argTitle, List<MSPoint> argPeaks, GlycanStructure argStructure)
        {
            ZedGraph.GraphPane Pane = new ZedGraph.GraphPane(new RectangleF(0.0f, 0.0f, 2000.0f, 1500.0f), argTitle, "m/z", "Intensity");
            //ZedGraph.MasterPane Pane = new ZedGraph.MasterPane(argTitle,new RectangleF(0.0f, 0.0f, 2400.0f, 1800.0f) );
            Pane.XAxis.MajorTic.IsInside = false;
            Pane.XAxis.MinorTic.IsInside = false;
            Pane.Legend.IsVisible = false;
            ZedGraph.PointPairList Peaks = new ZedGraph.PointPairList();


            double MaxIntensity = 0.0;
            /////////////////
            //Peaks
            ////////////////
            foreach (MSPoint p in argPeaks)
            {
                if (p.Intensity > MaxIntensity)
                {
                    MaxIntensity = p.Intensity;
                }
            }
            foreach (MSPoint p in argPeaks)
            {
                Peaks.Add(p.Mass, (p.Intensity / MaxIntensity) * 100.0);
            }
            Pane.AddStick("Peaks", Peaks, Color.Red);

            //////////////////
            //Y1 text object
            //////////////////
            /* ZedGraph.TextObj txtY1 = new ZedGraph.TextObj("Y1", argStructure.Y1.MZ, 102);
             txtY1.FontSpec.Size = txtY1.FontSpe.cSize*0.5f;
             txtY1.FontSpec.Border.IsVisible = false;
             Pane.GraphObjList.Insert(0, txtY1);*/

            /////////////////
            //Structure
            ////////////////
            GlycansDrawer GS;
            double previousBoundary = 0;

            foreach (GlycanTreeNode FGS in argStructure.TheoreticalFragment)
            {
                GS = new GlycansDrawer(FGS.GetIUPACString(), false, 1);
                double glycopeptideMZ = GlycanMass.GetGlycanMasswithCharge(FGS.GlycanType,FGS.Charge) + argStructure.Y1.Mass - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, FGS.Charge);
                Image imgStructure = GlycanImage.RotateImage(GS.GetImage(), 270);

                ZedGraph.TextObj txtGlycanMz = new ZedGraph.TextObj(glycopeptideMZ.ToString("0.000"), 100, 131);


                double PositionX = glycopeptideMZ;

                if (previousBoundary >= PositionX)
                {
                    PositionX = previousBoundary + 20;
                }

                if (imgStructure.Width > txtGlycanMz.Location.Width)
                {
                    previousBoundary = imgStructure.Width + PositionX;
                }
                else
                {
                    previousBoundary = (float)txtGlycanMz.Location.Width + PositionX;
                }
                ZedGraph.ImageObj glycan = new ZedGraph.ImageObj(imgStructure, PositionX, 130, imgStructure.Width + 20, imgStructure.Height);
                
                glycan.IsScaled = false;
                glycan.Location.AlignV = ZedGraph.AlignV.Bottom;

                txtGlycanMz.Location.X = glycan.Location.X + (float)glycan.Image.Width / 2 - (float)txtGlycanMz.Location.Width / 2;
                txtGlycanMz.FontSpec.Size = txtGlycanMz.FontSpec.Size * 0.3f;
                txtGlycanMz.FontSpec.Border.IsVisible = false;

                Pane.GraphObjList.Add(txtGlycanMz);
                Pane.GraphObjList.Add(glycan);

                double interval = 100000;
                int idx = 0;
                for (int i = 0; i < Peaks.Count; i++)
                {
                    if (Math.Abs(Peaks[i].X - glycopeptideMZ) < interval)
                    {
                        interval = Math.Abs((float)Peaks[i].X - glycopeptideMZ);
                        idx = i;
                    }
                }
                string mzLabelwPPM = Peaks[idx].X.ToString("0.000") + "\n(" + MassUtility.GetMassPPM(glycopeptideMZ, (float)Peaks[idx].X).ToString("0") + "PPM)";
                ZedGraph.TextObj PeakLabel = new ZedGraph.TextObj(mzLabelwPPM, Peaks[idx].X, Peaks[idx].Y + 3.0);
                PeakLabel.FontSpec.Size = PeakLabel.FontSpec.Size * 0.3f;
                PeakLabel.FontSpec.Border.IsVisible = false;
                PeakLabel.FontSpec.Fill.IsVisible = false;
                Pane.GraphObjList.Add(PeakLabel);
            }
            Pane.AxisChange();

            Pane.YAxis.Scale.Max = 145;
            Pane.XAxis.Scale.Min = Convert.ToInt32(argStructure.Y1.Mass - 100);
            Pane.XAxis.Scale.Max = Peaks[Peaks.Count - 1].X + 100;
            ////////////
            //Glycan Structure
            ////////////
            GS = new GlycansDrawer(argStructure.IUPACString, false);
            Image imgStruc = GS.GetImage();
            ZedGraph.ImageObj fullStructure = new ZedGraph.ImageObj(imgStruc, Pane.XAxis.Scale.Min + 20, 140, imgStruc.Width + 20, imgStruc.Height);
            fullStructure.IsScaled = false;
            Pane.GraphObjList.Add(fullStructure);
            ///////////////
            //Glycan M/Z
            //////////////
            double glycopeptidemz = GlycanMass.GetGlycanMasswithCharge(argStructure.Root.GlycanType,argStructure.Charge) + argStructure.Y1.Mass - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, argStructure.Charge);
            ZedGraph.TextObj txtGlycanMZ = new ZedGraph.TextObj("\nGlycoPeptide m/z:" + glycopeptidemz.ToString("0.000") +
                                                                                                                "\n           Glycan m/z:" + argStructure.GlycanMZ.ToString("0.000") +
                                                                                                                "\n                   Y1 m/z:" + argStructure.Y1.Mass.ToString("0.000")
                    , Pane.GraphObjList[Pane.GraphObjList.Count - 1].Location.X2, 140);
            txtGlycanMZ.FontSpec.Size = txtGlycanMZ.FontSpec.Size * 0.3f;
            txtGlycanMZ.FontSpec.Border.IsVisible = false;
            txtGlycanMZ.FontSpec.Fill.IsVisible = false;
            Pane.GraphObjList.Add(txtGlycanMZ);
            Image tmp = (Image)Pane.GetImage();
         
            return tmp;
        }
        private static Image RotateImage(Image inputImg, double degreeAngle)
        {
            //Corners of the image
            PointF[] rotationPoints = { new PointF(0, 0),
                                        new PointF(inputImg.Width, 0),
                                        new PointF(0, inputImg.Height),
                                        new PointF(inputImg.Width, inputImg.Height)};

            //Rotate the corners
            PointMath.RotatePoints(rotationPoints, new PointF(inputImg.Width / 2.0f, inputImg.Height / 2.0f), degreeAngle);

            //Get the new bounds given from the rotation of the corners
            //(avoid clipping of the image)
            Rectangle bounds = PointMath.GetBounds(rotationPoints);

            //An empy bitmap to draw the rotated image
            Bitmap rotatedBitmap = new Bitmap(bounds.Width, bounds.Height);

            using (Graphics g = Graphics.FromImage(rotatedBitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                //Transformation matrix
                System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
                m.RotateAt((float)degreeAngle, new PointF(inputImg.Width / 2.0f, inputImg.Height / 2.0f));
                m.Translate(-bounds.Left, -bounds.Top, System.Drawing.Drawing2D.MatrixOrder.Append); //shift to compensate for the rotation

                g.Transform = m;
                g.DrawImage(inputImg, 0, 0);
            }
            return (Image)rotatedBitmap;
        }
    }
}
