﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace COL.GlycoLib
{
    public class GlycansDrawer
    {
        GlycanTree GTree;
        
        private string _iupac;
        int Interval_X = 40;
        int Interval_Y = 30;
        int MaxXAxis = 0;
        int MaxYAxis = 0;
        bool _isBW = false;
        int _ScaleFactor = 1;
        public GlycansDrawer(string argIUPAC)
        {
            _iupac = argIUPAC;
            ConstructTree();            
            
            Placement(GTree);            
        }
        public GlycansDrawer(string argIUPAC, bool argIsBW)
        {
            _isBW = argIsBW;
            _iupac = argIUPAC;
            ConstructTree();
           
            Placement(GTree);      
        }
        public GlycansDrawer(string argIUPAC, bool argIsBW, int argScaleFactor)
        {
            _isBW = argIsBW;
            _iupac = argIUPAC;
               
            _ScaleFactor = argScaleFactor;
            Interval_X = Interval_X * _ScaleFactor;
            Interval_Y = Interval_Y * _ScaleFactor;
            ConstructTree();
            Placement(GTree);            
        }
        public GlycansDrawer(GlycanTree argTree)
        {
            GTree = argTree;
            Placement(GTree);
        }
        public Image GetImage()
        {
            int SizeX = MaxXAxis + 15 * _ScaleFactor;
            int SizeY = MaxYAxis + 15 * _ScaleFactor;
            Bitmap bmp = new Bitmap(SizeX, SizeY );
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255)), 0, 0, SizeX, SizeY); //white background
            Pen Line = new Pen(Color.Black, 1.0f * _ScaleFactor); //Branch line
            AssignColor();
            foreach (GlycanTree T in GTree.TravelGlycanTreeBFS())
            {
                if (T.GetChild.Count != 0)
                {
                    foreach (GlycanTree GChild in T.GetChild) //Draw Line
                    {
                        g.DrawLine(Line, T.PosX + 5 * _ScaleFactor, T.PosY + 5 * _ScaleFactor, GChild.PosX + 5 * _ScaleFactor, GChild.PosY + 5 * _ScaleFactor);
                    }
                    g.DrawImage(GlycanCartoon(T.Root), T.PosX, T.PosY);
                }
                else
                {
                    g.DrawImage(GlycanCartoon(T.Root), T.PosX, T.PosY);
                }
            }
            
            return bmp;
        }
        private void AssignColor()
        {
            foreach (GlycanTree T in GTree.TravelGlycanTreeBFS())
            {
                if(T.DistanceToRoot <=3 && T.Root == Glycan.Type.Hex)
                {
                     T.Root = Glycan.Type.Man;
                }
                if (T.DistanceToRoot > 3 && T.Root == Glycan.Type.Hex)
                {
                    T.Root = Glycan.Type.Man;
                    GlycanTree Parent = T.Parent;
                    bool _HexNacFound = false;
                    for (int i = T.DistanceToRoot - 1; i >= 3; i--)
                    {
                        if (Parent.Root == Glycan.Type.HexNAc)
                        {
                            _HexNacFound = true;
                        }
                        Parent = Parent.Parent;
                    }
                    if (_HexNacFound)
                    {
                        T.Root = Glycan.Type.Gal;
                    }
                }
            }
        }       
        private Image GlycanCartoon(Glycan.Type argType)
        {

            Bitmap glycan = new Bitmap(11 * _ScaleFactor,11 * _ScaleFactor);
            Graphics g = Graphics.FromImage(glycan);

            Brush SolidBrush;
            Pen Linear = new Pen(Color.Black, 1.0f * _ScaleFactor);
            Point[] loc;
            switch (argType)
            {
                case Glycan.Type.DeHex:
                    SolidBrush = new SolidBrush(Color.FromArgb(255,0,0) );
                    if (_isBW)
                    {
                        SolidBrush = new SolidBrush(Color.White);
                    }
                    loc = new Point[] { new Point(0, 10 * _ScaleFactor), new Point(5 * _ScaleFactor, 0), new Point(10 * _ScaleFactor, 10 * _ScaleFactor) };
                    g.FillPolygon(SolidBrush, loc);
                    g.DrawPolygon(Linear, loc);
                    break;
                case Glycan.Type.Gal:
                    SolidBrush = new SolidBrush(Color.FromArgb(255, 255, 0));
                    if (_isBW)
                    {
                        SolidBrush = new SolidBrush(Color.White);
                    }
                    g.FillEllipse(SolidBrush, 0, 0, 10 * _ScaleFactor, 10 * _ScaleFactor);
                    g.DrawEllipse(Linear, 0, 0, 10 * _ScaleFactor, 10 * _ScaleFactor);
                    break;
                case Glycan.Type.HexNAc:
                    SolidBrush = new SolidBrush(Color.FromArgb(0, 0, 250));
                    if (_isBW)
                    {
                        SolidBrush = new SolidBrush(Color.White);
                    }
                    g.FillRectangle(SolidBrush, 0, 0, 10 * _ScaleFactor, 10 * _ScaleFactor);
                    g.DrawRectangle(Linear, 0, 0, 10 * _ScaleFactor, 10 * _ScaleFactor);
                    break;
                case Glycan.Type.Man:
                    SolidBrush = new SolidBrush(Color.FromArgb(0, 200, 50));
                    if (_isBW)
                    {
                        SolidBrush = new SolidBrush(Color.White);
                    }
                    g.FillEllipse(SolidBrush, 0, 0, 10 * _ScaleFactor, 10 * _ScaleFactor);
                    g.DrawEllipse(Linear, 0, 0, 10 * _ScaleFactor, 10 * _ScaleFactor);
                    break;
                case Glycan.Type.NeuAc:
                    SolidBrush = new SolidBrush(Color.FromArgb(200, 0, 200));
                    if (_isBW)
                    {
                        SolidBrush = new SolidBrush(Color.White);
                    }
                    loc = new Point[] { new Point(0, 5 * _ScaleFactor), new Point(5 * _ScaleFactor, 10 * _ScaleFactor), new Point(10 * _ScaleFactor, 5 * _ScaleFactor), new Point(5 * _ScaleFactor, 0) };
                    g.FillPolygon(SolidBrush, loc);
                    g.DrawPolygon(Linear, loc);
                    break;
                case Glycan.Type.NeuGc:
                    SolidBrush = new SolidBrush(Color.FromArgb(233, 255, 255));
                    if (_isBW)
                    {
                        SolidBrush = new SolidBrush(Color.White);
                    }
                    loc = new Point[] { new Point(0, 5 * _ScaleFactor), new Point(5 * _ScaleFactor, 10 * _ScaleFactor), new Point(10 * _ScaleFactor, 5 * _ScaleFactor), new Point(5 * _ScaleFactor, 0) };
                    g.FillPolygon(SolidBrush, loc);
                    g.DrawPolygon(Linear, loc);
                    break;
            }
            return glycan;
        }
        
        private void ConstructTree()
        {
            string[] glycans = _iupac.Replace(" ", "").Replace("??", "-").Split('-');
            Stack TreeStake = new Stack();
            for (int i = 0; i < glycans.Length; i++)
            {
                if (glycans[i].Contains(")("))
                {
                    TreeStake.Push(")");
                    TreeStake.Push("(");
                    TreeStake.Push(new GlycanTree(String2GlycanType(glycans[i])));
                }
                else if (glycans[i].Contains("("))
                {
                    List<GlycanTree> GI = new List<GlycanTree>();
                    while (TreeStake.Count != 0 && TreeStake.Peek().ToString() != ")" && TreeStake.Peek().ToString() != "(")
                    {
                        GI.Add((GlycanTree)TreeStake.Pop());
                    }
                    TreeStake.Push(ConnectTree(GI));


                    TreeStake.Push("(");
                    TreeStake.Push(new GlycanTree(String2GlycanType(glycans[i])));
                }
                else if (glycans[i].Contains(")"))
                {
                    List<GlycanTree> GILst = new List<GlycanTree>();
                    while (TreeStake.Count != 0 && TreeStake.Peek().ToString() != ")" && TreeStake.Peek().ToString() != "(")
                    {
                        GILst.Add((GlycanTree)TreeStake.Pop());
                    }
                    TreeStake.Push(ConnectTree(GILst));

                    TreeStake.Push(new GlycanTree(String2GlycanType(glycans[i])));
                    GlycanTree GI = (GlycanTree)TreeStake.Pop();
                    GlycanTree child = (GlycanTree)TreeStake.Pop();
                    child.Parent = GI;
                    GI.AddChild(child);
                    TreeStake.Pop(); //(
                    if (TreeStake.Peek().ToString() == ")") //One More Link
                    {
                        TreeStake.Pop();//)
                        child = (GlycanTree)TreeStake.Pop();
                        child.Parent = GI;
                        GI.AddChild(child);
                        TreeStake.Pop();//(
                        if (TreeStake.Peek().ToString() == ")") //One More Link
                        {
                            TreeStake.Pop();//)
                            child = (GlycanTree)TreeStake.Pop();
                            child.Parent = GI;
                            GI.AddChild(child);
                            TreeStake.Pop();//(
                        }
                        child = (GlycanTree)TreeStake.Pop();
                        child.Parent = GI;
                        GI.AddChild(child);
                    }
                    else
                    {
                        child = (GlycanTree)TreeStake.Pop();
                        child.Parent = GI;
                        GI.AddChild(child);                        
                    }
                    TreeStake.Push(GI);
                }
                else
                {
                    if (TreeStake.Count != 0 && TreeStake.Peek().ToString() != ")" && TreeStake.Peek().ToString() != "(")
                    {
                        GlycanTree GI = new GlycanTree(String2GlycanType(glycans[i]));
                        GlycanTree child = (GlycanTree)TreeStake.Pop();
                        child.Parent = GI;
                        GI.AddChild(child);
                        TreeStake.Push(GI);
                    }
                    else
                    {
                        TreeStake.Push(new GlycanTree(String2GlycanType(glycans[i])));
                    }
                }
            }
            GTree = (GlycanTree)TreeStake.Pop();
            if (TreeStake.Count != 0)
            {
                throw new Exception("Steak is not zero,Parsing Error");
            }
            GTree.UpdateDistance(-1);
             if(!_isBW)
            {
                AssignColor();
            }
        }
        private void Placement(GlycanTree argTree)
        {
            //PostOrderTravel
            List<GlycanTree> DFSOrder = new List<GlycanTree>();

            foreach (GlycanTree t in argTree.TravelGlycanTreeDFS())
            {
                DFSOrder.Add(t);     
            }

            //Assign X Level
            for (int i = 0; i < DFSOrder.Count; i++)
            {
                GlycanTree t = DFSOrder[i];
                if (t.Root == Glycan.Type.DeHex)
                {
                    t.PosX = t.DistanceToRoot - 1;
                }
                else
                {
                    t.PosX = t.DistanceToRoot;
                }
            }

            //Assign Y Level
            for (int i = 0; i < DFSOrder.Count; i++)
            {
                GlycanTree t = DFSOrder[i];

                if (i == 0) //leaf
                {
                    t.PosY =0.0f;
                }
                else
                {
                    if (t.GetChild.Count == 1 && DFSOrder[i - 1].Parent == t) // Only one chlid
                    {
                        t.PosY = DFSOrder[i - 1].PosY;

                        if (t.GetChild[0].Root == Glycan.Type.DeHex)
                        {
                            t.GetChild[0].PosY = t.PosY + 0.5f;
                        }
                    }
                    else if(DFSOrder[i-1].Parent!= t) //branch
                    {
                        t.PosY = DFSOrder[i - 1].PosY + 1.0f;
                    }
                    else if(t.GetChild.Count==2)
                    {
                        if (t.NumberOfFucChild == 0)
                        {
                            t.PosY = (t.GetChild[0].PosY + t.GetChild[1].PosY) / 2;
                        }
                        else
                        {
                            if (t.GetChild[0].Root == Glycan.Type.DeHex && t.GetChild[1].Root == Glycan.Type.DeHex)
                            {
                                t.PosY = (t.GetChild[1].PosY);
                                t.GetChild[0].PosY = t.PosY + 0.5f;
                                t.GetChild[1].PosY = t.PosY + 0.5f;
                            }
                            else if (t.GetChild[0].Root == Glycan.Type.DeHex && t.GetChild[1].Root != Glycan.Type.DeHex)
                            {
                                t.PosY = (t.GetChild[1].PosY);
                                t.GetChild[0].PosY = t.PosY + 0.5f;
                            }
                            else
                            {
                                t.PosY = (t.GetChild[0].PosY);
                                t.GetChild[1].PosY = t.PosY + 0.5f;
                            }
                        }
                    }
                    else if (t.GetChild.Count == 3)
                    {


                        List<GlycanTree> Fuc = new List<GlycanTree>();
                        List<GlycanTree> NonFuc = new List<GlycanTree>();
                        foreach (GlycanTree Gt in t.GetChild)
                        {
                            if (Gt.Root == Glycan.Type.DeHex)
                            {
                                Fuc.Add(Gt);
                            }
                            else
                            {
                                NonFuc.Add(Gt);
                            }
                        }
                        List<float> matrix = new List<float>();
                        foreach (GlycanTree Ct in NonFuc)
                        {
                            matrix.Add(Ct.PosY);
                        }
                        matrix.Sort();

                        if (Fuc.Count == 0)
                        {
                            t.PosY = matrix[1];
                        }
                        else if (Fuc.Count == 1)
                        {
                            t.PosY = (matrix[0]+matrix[1])/2;
                            Fuc[0].PosY = t.PosY + 0.5f;
                        }
                        else if (Fuc.Count == 2)
                        {
                            t.PosY = NonFuc[0].PosY;
                            Fuc[0].PosY = t.PosY + 0.5f;
                            Fuc[1].PosY = t.PosY - 0.5f;
                        }
                        else
                        {
                            foreach (GlycanTree Ct in Fuc)
                            {
                                matrix.Add(Ct.PosY);
                            }
                            matrix.Sort();
                            t.PosY = matrix[1];
                            Fuc[0].PosY = t.PosY + 0.5f;
                            Fuc[1].PosY = t.PosY - 0.5f;
                            Fuc[2].PosY = t.PosY + 0.5f;
                            Fuc[2].PosX = t.PosX + 0.5f;
                        }     

                    }
                    else if (t.GetChild.Count == 4)
                    {
                        if (t.NumberOfFucChild == 0)
                        {
                            List<float> matrix = new List<float>();
                            matrix.Add(t.GetChild[0].PosY);
                            matrix.Add(t.GetChild[1].PosY);
                            matrix.Add(t.GetChild[2].PosY);
                            matrix.Add(t.GetChild[3].PosY);
                            matrix.Sort();
                            t.PosY =( matrix[1]+ matrix[2])/2;
                        }
                        else
                        {
                            List<GlycanTree> Fuc = new List<GlycanTree>();
                            List<GlycanTree> NonFuc = new List<GlycanTree>();
                            foreach (GlycanTree Gt in t.GetChild)
                            {
                                if(Gt.Root == Glycan.Type.DeHex)
                                {
                                    Fuc.Add(Gt);
                                }
                                else
                                {
                                    NonFuc.Add(Gt);
                                }
                            }
                            List<float> matrix = new List<float>();
                            foreach(GlycanTree Ct in NonFuc)
                            {
                                matrix.Add(Ct.PosY);
                            }
                            matrix.Sort();
                            if(Fuc.Count==1)
                            {
                                t.PosY=matrix[1];
                                Fuc[0].PosY = t.PosY + 0.5f;
                            }
                            else if (Fuc.Count==2)
                            {
                                t.PosY = (matrix[0]+matrix[1])/2;
                                Fuc[0].PosY = t.PosY + 0.5f;
                                Fuc[1].PosY = t.PosY - 0.5f;
                            }
                            else if (Fuc.Count==3) 
                            {
                                
                                t.PosY = matrix[0];
                                Fuc[0].PosY = t.PosY + 0.5f;
                                Fuc[1].PosY = t.PosY - 0.5f;
                                Fuc[2].PosY = t.PosY + 0.5f;
                                Fuc[2].PosX = Fuc[2].PosX + 0.5f;

                            }
                            else if (Fuc.Count==4)
                            {
                                foreach (GlycanTree Ct in Fuc)
                                {
                                    matrix.Add(Ct.PosY);
                                }
                                matrix.Sort();
                                t.PosY = (matrix[1]+matrix[2])/2;
                                Fuc[0].PosY = t.PosY + 0.5f;
                                Fuc[1].PosY = t.PosY - 0.5f;
                                Fuc[2].PosY = t.PosY + 0.5f;
                                Fuc[2].PosX = Fuc[2].PosX + 0.5f;
                                Fuc[3].PosY = t.PosY - 0.5f;
                                Fuc[3].PosX = Fuc[2].PosX - 0.5f;

                            }

                        }


                    }

                  
                }
            }
 
            //Convert Level to Real Position

            
            for (int i = 0; i < DFSOrder.Count; i++)
            {
                GlycanTree t = DFSOrder[i];
                t.PosX = t.PosX * Interval_X+2.0f;
                t.PosY = t.PosY * Interval_Y+2.0f;

                if (t.PosX > MaxXAxis)
                {
                    MaxXAxis = Convert.ToInt32(t.PosX);
                }
                if (t.PosY > MaxYAxis)
                {
                    MaxYAxis = Convert.ToInt32(t.PosY);
                }
            }


        }
        public Image GetImage1()
        {

            int Xinterval = 30;
            int Y45Interval = Xinterval;
            int Y60Invetval = (int)Math.Round(Xinterval * 1.414f, 0);
            int FucYInterval = 150;
            int SizeX = 500;
            int SizeY = 500;
            int BoundX = 0;
            int BoundY = 0;
            //3-branch 60 degree (X-130  Y-225)
            //2-Branch 45 degree (X-130  Y-130)
            Bitmap bmp = new Bitmap(SizeX, SizeY);
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255)), 0, 0, SizeX, SizeY); //white background
            int StartPosX = SizeX - 100, StartPosY = SizeY / 2;

            Queue glycans = new Queue();
            Queue posXY = new Queue();
            glycans.Enqueue(GTree);
            posXY.Enqueue(new Point(StartPosX, StartPosY));

            GlycanTree GI;
            Pen Line = new Pen(Color.Black, 1.0f); //Branch line
            int MaxXDeapt = SizeX;
            int MinYTop = SizeY;
            int MaxYDown = SizeY / 2;
            do
            {
                GI = (GlycanTree)glycans.Dequeue();
                Point pnt = (Point)posXY.Dequeue();

                #region DrawBranch
                if (GI.GetChild.Count == 1)
                {
                    if (GI.GetChild[0].Root == Glycan.Type.DeHex && GI.GetChild[0].GetChild.Count == 0)
                    {
                        g.DrawLine(Line, pnt.X, pnt.Y, pnt.X, pnt.Y - FucYInterval);
                    }
                    else
                    {
                        g.DrawLine(Line, pnt.X, pnt.Y, pnt.X - Xinterval, pnt.Y);
                    }
                }
                else if (GI.GetChild.Count == 2)
                {
                    if (GI.GetChild[0].Root == Glycan.Type.DeHex || GI.GetChild[1].Root == Glycan.Type.DeHex)
                    {
                        g.DrawLine(Line, pnt.X, pnt.Y, pnt.X, pnt.Y - FucYInterval);
                        g.DrawLine(Line, pnt.X, pnt.Y, pnt.X - Xinterval, pnt.Y);
                    }
                    else
                    {
                        g.DrawLine(Line, pnt.X, pnt.Y, pnt.X - Xinterval, pnt.Y - Y45Interval);
                        g.DrawLine(Line, pnt.X, pnt.Y, pnt.X - Xinterval, pnt.Y + Y45Interval);
                    }
                }
                else if (GI.GetChild.Count == 3)
                {
                    if (GI.GetChild[0].Root == Glycan.Type.DeHex || GI.GetChild[1].Root == Glycan.Type.DeHex)
                    {
                        g.DrawLine(Line, pnt.X, pnt.Y, pnt.X - Xinterval, pnt.Y - Y45Interval);
                        g.DrawLine(Line, pnt.X, pnt.Y, pnt.X, pnt.Y - FucYInterval);
                        g.DrawLine(Line, pnt.X, pnt.Y, pnt.X - Xinterval, pnt.Y + Y45Interval);
                    }
                    else
                    {
                        g.DrawLine(Line, pnt.X, pnt.Y, pnt.X - Xinterval, pnt.Y - Y60Invetval);
                        g.DrawLine(Line, pnt.X, pnt.Y, pnt.X - Xinterval, pnt.Y);
                        g.DrawLine(Line, pnt.X, pnt.Y, pnt.X - Xinterval, pnt.Y + Y60Invetval);
                    }
                }
                #endregion


                g.DrawImage(GlycanCartoon(GI.Root), pnt.X - 5, pnt.Y - 5); //Draw Glycan
                if (pnt.X - 50 < MaxXDeapt)
                {
                    MaxXDeapt = pnt.X - 50;
                }
                if (pnt.Y - 50 > MaxYDown)
                {
                    MaxYDown = pnt.Y - 50;
                }
                if (pnt.Y - 50 < MinYTop)
                {
                    MinYTop = pnt.Y - 50;
                }

                if (StartPosY - (pnt.Y - Y60Invetval) > BoundY)
                {
                    BoundY = StartPosY - (pnt.Y - Y60Invetval);
                }
                if (StartPosX - (pnt.X - Xinterval) > BoundX)
                {
                    BoundX = StartPosX - (pnt.X - Xinterval);
                }


                #region add child glycan to queue
                if (GI.GetChild.Count == 1)
                {
                    if (GI.GetChild[0].Root == Glycan.Type.DeHex && GI.GetChild[0].GetChild.Count == 0)
                    {
                        posXY.Enqueue(new Point(pnt.X, pnt.Y - FucYInterval));
                    }
                    else
                    {
                        posXY.Enqueue(new Point(pnt.X - Xinterval, pnt.Y));
                    }
                    glycans.Enqueue(GI.GetChild[0]);

                }
                else if (GI.GetChild.Count == 2)
                {
                    if (GI.GetChild[0].Root == Glycan.Type.DeHex)
                    {
                        posXY.Enqueue(new Point(pnt.X, pnt.Y - FucYInterval));
                        posXY.Enqueue(new Point(pnt.X - Xinterval, pnt.Y));
                        glycans.Enqueue(GI.GetChild[0]);
                        glycans.Enqueue(GI.GetChild[1]);
                    }
                    else if (GI.GetChild[1].Root == Glycan.Type.DeHex)
                    {
                        posXY.Enqueue(new Point(pnt.X, pnt.Y - FucYInterval));
                        posXY.Enqueue(new Point(pnt.X - Xinterval, pnt.Y));
                        glycans.Enqueue(GI.GetChild[1]);
                        glycans.Enqueue(GI.GetChild[0]);

                    }
                    else
                    {
                        posXY.Enqueue(new Point(pnt.X - Xinterval, pnt.Y - Y45Interval));
                        posXY.Enqueue(new Point(pnt.X - Xinterval, pnt.Y + Y45Interval));
                        glycans.Enqueue(GI.GetChild[0]);
                        glycans.Enqueue(GI.GetChild[1]);

                    }
                }
                else if (GI.GetChild.Count == 3)
                {
                    if (GI.GetChild[0].Root == Glycan.Type.DeHex)
                    {
                        posXY.Enqueue(new Point(pnt.X, pnt.Y - FucYInterval));
                        posXY.Enqueue(new Point(pnt.X - Xinterval, pnt.Y - Y45Interval));
                        posXY.Enqueue(new Point(pnt.X - Xinterval, pnt.Y + Y45Interval));
                        glycans.Enqueue(GI.GetChild[0]);
                        glycans.Enqueue(GI.GetChild[1]);
                        glycans.Enqueue(GI.GetChild[2]);

                    }
                    else if (GI.GetChild[1].Root == Glycan.Type.DeHex)
                    {
                        posXY.Enqueue(new Point(pnt.X, pnt.Y - FucYInterval));
                        posXY.Enqueue(new Point(pnt.X - Xinterval, pnt.Y - Y45Interval));
                        posXY.Enqueue(new Point(pnt.X - Xinterval, pnt.Y + Y45Interval));
                        glycans.Enqueue(GI.GetChild[1]);
                        glycans.Enqueue(GI.GetChild[0]);
                        glycans.Enqueue(GI.GetChild[2]);

                    }
                    else if (GI.GetChild[2].Root == Glycan.Type.DeHex)
                    {
                        posXY.Enqueue(new Point(pnt.X, pnt.Y - FucYInterval));
                        posXY.Enqueue(new Point(pnt.X - Xinterval, pnt.Y - Y45Interval));
                        posXY.Enqueue(new Point(pnt.X - Xinterval, pnt.Y + Y45Interval));
                        glycans.Enqueue(GI.GetChild[2]);
                        glycans.Enqueue(GI.GetChild[0]);
                        glycans.Enqueue(GI.GetChild[1]);

                    }
                    else
                    {
                        posXY.Enqueue(new Point(pnt.X - Xinterval, pnt.Y - Y60Invetval));
                        posXY.Enqueue(new Point(pnt.X - Xinterval, pnt.Y));
                        posXY.Enqueue(new Point(pnt.X - Xinterval, pnt.Y + Y60Invetval));

                        glycans.Enqueue(GI.GetChild[0]);
                        glycans.Enqueue(GI.GetChild[2]);
                        glycans.Enqueue(GI.GetChild[1]);


                    }
                }
                #endregion


            } while (glycans.Count != 0);

            int PicWidth = SizeX - MaxXDeapt + 250;
            int PicHight = MaxYDown - MinYTop + 200;
            Bitmap reSize = new Bitmap(PicWidth, PicHight);

            Rectangle Desc = new Rectangle(0, 0, reSize.Width, reSize.Height);
            Rectangle Src = new Rectangle(MaxXDeapt - 50, MinYTop - 50, reSize.Width, reSize.Height);
            Graphics.FromImage(reSize).DrawImage(bmp, Desc, Src, GraphicsUnit.Pixel);

            return reSize;
        }
        //private void oldplacement()
        //{
        //    //Assign Level
        //    Queue Qglycans = new Queue();
        //    Qglycans.Enqueue(GTree);
        //    int MaxLevel = 0;
        //    int MaxWide = 0;
        //    Hashtable LevelWide = new Hashtable();
        //    LevelWide.Add(1, 1);
        //    do
        //    {
        //        GlycanTree GT = (GlycanTree)Qglycans.Dequeue();
        //        if (GT.Level == 0)
        //        {
        //            GT.Level = 1;
        //        }
        //        if (GT.GetChild.Count != 0)
        //        {
        //            foreach (GlycanTree childT in GT.GetChild)
        //            {
        //                if (childT.Root == GlycanTree.GlycanType.Fuc)
        //                {
        //                    childT.Level = GT.Level;
        //                }
        //                else
        //                {
        //                    childT.Level = GT.Level + 1;
        //                    if (childT.Level > MaxLevel)
        //                    {
        //                        MaxLevel = childT.Level;
        //                    }
        //                }
        //                if (LevelWide.ContainsKey(childT.Level))
        //                {
        //                    LevelWide[childT.Level] = (int)(LevelWide[childT.Level]) + 1;
        //                }
        //                else
        //                {
        //                    LevelWide.Add(childT.Level, 1);
        //                }
        //                Qglycans.Enqueue(childT);
        //            }
        //        }
        //    } while (Qglycans.Count != 0);


        //    //Assign Pos X and Pos Y
        //    Qglycans.Enqueue(GTree);
        //    do
        //    {
        //        GlycanTree GT = (GlycanTree)Qglycans.Dequeue();
        //        GT.PosX = 10 + Interval_X * (MaxLevel - GT.Level);
        //        GT.PosY = 0;
        //        if (GT.GetChild.Count == 1)
        //        {
        //            GlycanTree childT = GT.GetChild[0];
        //            if (childT.Root == GlycanTree.GlycanType.Fuc)
        //            {
        //                childT.PosX = GT.PosX;
        //                childT.PosY = GT.PosY - Interval_Fuc;

        //            }
        //            else
        //            {
        //                childT.PosX = GT.PosX - Interval_X;
        //                childT.PosY = GT.PosY;
        //            }
        //            Qglycans.Enqueue(childT);
        //        }
        //        else if (GT.GetChild.Count == 2)
        //        {
        //            GlycanTree childT1 = GT.GetChild[0];
        //            GlycanTree childT2 = GT.GetChild[1];
        //            if (childT1.Root == GlycanTree.GlycanType.Fuc && childT2.Root == GlycanTree.GlycanType.Fuc)
        //            {
        //                childT1.PosX = GT.PosX;
        //                childT1.PosY = GT.PosY - Interval_Fuc;
        //                childT2.PosX = GT.PosX;
        //                childT2.PosY = GT.PosY + Interval_Fuc;

        //            }
        //            else if (childT1.Root == GlycanTree.GlycanType.Fuc)
        //            {
        //                childT1.PosX = GT.PosX;
        //                childT1.PosY = GT.PosY - Interval_Fuc;
        //                childT2.PosX = GT.PosX - Interval_X;
        //                childT2.PosY = GT.PosY;
        //            }
        //            else if (childT2.Root == GlycanTree.GlycanType.Fuc)
        //            {
        //                childT1.PosX = GT.PosX - Interval_X;
        //                childT1.PosY = GT.PosY;
        //                childT2.PosX = GT.PosX;
        //                childT2.PosY = GT.PosY - Interval_Fuc;
        //            }
        //            else
        //            {
        //                AsignPosY2Nodes(GT);
        //            }
        //            Qglycans.Enqueue(childT1);
        //            Qglycans.Enqueue(childT2);
        //        }
        //        else if (GT.GetChild.Count == 3)
        //        {
        //            GlycanTree childT1 = GT.GetChild[0];
        //            GlycanTree childT2 = GT.GetChild[1];
        //            GlycanTree childT3 = GT.GetChild[2];

        //            if (childT1.Root == GlycanTree.GlycanType.Fuc && childT2.Root == GlycanTree.GlycanType.Fuc && childT3.Root == GlycanTree.GlycanType.Fuc)
        //            {
        //                childT1.PosX = GT.PosX;
        //                childT2.PosX = GT.PosX;
        //                childT3.PosX = GT.PosX - Interval_Fuc;
        //                childT1.PosY = GT.PosY - Interval_Fuc;
        //                childT2.PosY = GT.PosY + Interval_Fuc;
        //                childT3.PosY = GT.PosY;

        //            }
        //            else if (childT1.Root == GlycanTree.GlycanType.Fuc)
        //            {
        //                childT1.PosX = GT.PosX;
        //                childT1.PosY = GT.PosY - Interval_Fuc;
        //                if (childT2.Root == GlycanTree.GlycanType.Fuc)
        //                {
        //                    childT2.PosX = GT.PosX;
        //                    childT2.PosY = GT.PosY + Interval_Fuc;
        //                    childT3.PosX = GT.PosX - Interval_X;
        //                    childT3.PosY = GT.PosY;
        //                }
        //                else if (childT3.Root == GlycanTree.GlycanType.Fuc)
        //                {
        //                    childT2.PosX = GT.PosX - Interval_X;
        //                    childT2.PosY = GT.PosY;
        //                    childT3.PosX = GT.PosX;
        //                    childT3.PosY = GT.PosY + Interval_Fuc;
        //                }
        //                else
        //                {
        //                    AsignPosY2Nodes(GT); //child 2,3
        //                }
        //            }
        //            else if (childT2.Root == GlycanTree.GlycanType.Fuc)
        //            {
        //                childT2.PosX = GT.PosX;
        //                childT2.PosY = GT.PosY - Interval_Fuc;
        //                if (childT3.Root == GlycanTree.GlycanType.Fuc)
        //                {
        //                    childT3.PosX = GT.PosX;
        //                    childT3.PosY = GT.PosY + Interval_Fuc;
        //                    childT1.PosX = GT.PosX - Interval_X;
        //                    childT1.PosY = GT.PosY;
        //                }
        //                else
        //                {
        //                    AsignPosY2Nodes(GT); // child 1, 3 
        //                }
        //            }
        //            else
        //            {
        //                AsignPosY3Nodes(GT); //child 1,2,3
        //            }
        //            Qglycans.Enqueue(childT1);
        //            Qglycans.Enqueue(childT2);
        //            Qglycans.Enqueue(childT3);
        //        }
        //        else if (GT.GetChild.Count == 4)
        //        {
        //            GlycanTree childT1 = GT.GetChild[0];
        //            GlycanTree childT2 = GT.GetChild[1];
        //            GlycanTree childT3 = GT.GetChild[2];
        //            GlycanTree childT4 = GT.GetChild[3];
        //            if (childT1.Root == GlycanTree.GlycanType.Fuc)
        //            {
        //                if (childT2.Root == GlycanTree.GlycanType.Fuc)
        //                {
        //                    if (childT3.Root == GlycanTree.GlycanType.Fuc)
        //                    {
        //                        if (childT4.Root == GlycanTree.GlycanType.Fuc)
        //                        {
        //                            childT1.PosX = GT.PosX;
        //                            childT1.PosY = GT.PosY - Interval_Fuc;
        //                            childT2.PosX = GT.PosX;
        //                            childT2.PosY = GT.PosY + Interval_Fuc;
        //                            childT3.PosX = GT.PosX - Interval_X;
        //                            childT3.PosY = GT.PosY - Interval_Y;
        //                            childT4.PosX = GT.PosX - Interval_X;
        //                            childT4.PosY = GT.PosY + Interval_Y;
        //                        }
        //                        else
        //                        {
        //                            childT1.PosX = GT.PosX;
        //                            childT1.PosY = GT.PosY - Interval_Fuc;
        //                            childT2.PosX = GT.PosX;
        //                            childT2.PosY = GT.PosY + Interval_Fuc;
        //                            childT3.PosX = GT.PosX - Interval_X;
        //                            childT3.PosY = GT.PosY - Interval_Y;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (childT4.Root == GlycanTree.GlycanType.Fuc)
        //                        {
        //                        }
        //                        else
        //                        {
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (childT3.Root == GlycanTree.GlycanType.Fuc)
        //                    {
        //                        if (childT4.Root == GlycanTree.GlycanType.Fuc)
        //                        {
        //                        }
        //                        else
        //                        {
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (childT4.Root == GlycanTree.GlycanType.Fuc)
        //                        {
        //                        }
        //                        else
        //                        {
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (childT2.Root == GlycanTree.GlycanType.Fuc)
        //                {
        //                    if (childT3.Root == GlycanTree.GlycanType.Fuc)
        //                    {
        //                        if (childT4.Root == GlycanTree.GlycanType.Fuc)
        //                        {

        //                        }
        //                        else
        //                        {

        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (childT4.Root == GlycanTree.GlycanType.Fuc)
        //                        {

        //                        }
        //                        else
        //                        {

        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (childT3.Root == GlycanTree.GlycanType.Fuc)
        //                    {
        //                        if (childT4.Root == GlycanTree.GlycanType.Fuc)
        //                        {

        //                        }
        //                        else
        //                        {

        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (childT4.Root == GlycanTree.GlycanType.Fuc)
        //                        {

        //                        }
        //                        else
        //                        {

        //                        }
        //                    }
        //                }
        //            }

        //        }
        //        /*Qglycans.Enqueue(childT1);
        //        Qglycans.Enqueue(childT2);
        //        Qglycans.Enqueue(childT3);
        //        Qglycans.Enqueue(childT4);*/

        //    } while (Qglycans.Count != 0);
        //}
        private GlycanTree ConnectTree(List<GlycanTree> argList)
        {
            GlycanTree Tree = argList[0];
            if (argList.Count == 1)
            {
                return argList[0];
            }
            else
            {                
                for (int i = 1; i < argList.Count; i++)
                {
                    if(Tree.GetChild.Count==0)
                    {
                        Tree.AddChild(argList[i]);
                    }
                    else
                    {
                        GlycanTree GI = Tree.GetChild[0];
                        while (GI.GetChild.Count != 0)
                        {
                            GI = GI.GetChild[0];
                        } 
                        GI.AddChild(argList[i]);
                    }
                }
                return Tree;
            }
        }    
        private Glycan.Type String2GlycanType(string argType)
        {
            if (argType.ToLower().Contains("glcnac") || argType.ToLower().Contains("hexnac"))
            {
                return Glycan.Type.HexNAc;
            }
            else if (argType.ToLower().Contains("fuc")|| argType.ToLower().Contains("dehex"))
            {
                return Glycan.Type.DeHex;
            }
            else if (argType.ToLower().Contains("gal"))
            {
                return Glycan.Type.Gal;
            }
            else if (argType.ToLower().Contains("neuac"))
            {
                return Glycan.Type.NeuAc;
            }
            else if (argType.ToLower().Contains("neugc"))
            {
                return Glycan.Type.NeuGc;
            }
            else if (argType.ToLower().Contains("man"))
            {
                return Glycan.Type.Man;
            }
            else if (argType.ToLower().Contains("hex"))
            {
                return Glycan.Type.Hex;
            }
            else
            {
                throw new Exception("IUPAC contain unrecognized glycan or string");
            }
        }
        
        private void AsignPosY2Nodes(GlycanTree GT)
        {
            GlycanTree childT1 = GT.GetChild[0];
            GlycanTree childT2 = GT.GetChild[1];
            if (GT.GetChild.Count == 3)
            {
                if (GT.GetChild[0].Root == Glycan.Type.DeHex)
                {
                    childT1 = GT.GetChild[1];
                    childT2 = GT.GetChild[2];
                }
                if (GT.GetChild[1].Root == Glycan.Type.DeHex)
                {
                    childT1 = GT.GetChild[0];
                    childT2 = GT.GetChild[2];
                }
                if (GT.GetChild[2].Root == Glycan.Type.DeHex)
                {
                    childT1 = GT.GetChild[0];
                    childT2 = GT.GetChild[1];
                }
            }
            
            int GradChildT1 = childT1.GetChild.Count - childT1.NumberOfFucChild;
            int GradChildT2 = childT2.GetChild.Count - childT2.NumberOfFucChild;
            
            childT1.PosX = GT.PosX - Interval_X;
            childT2.PosX = GT.PosX - Interval_X;
            if (GradChildT1 + GradChildT2 <= 2)
            {
                childT1.PosY = GT.PosY - Interval_Y;
                childT2.PosY = GT.PosY + Interval_Y;
            }
            else
            {
                switch (GradChildT1)
                {
                    case 1:
                        switch (GradChildT2)
                        {
                            case 2:
                                childT1.PosY = GT.PosY - Interval_Y * 1.5f;
                                childT2.PosY = GT.PosY + Interval_Y * 1.5f;
                                break;
                            case 3:
                                childT1.PosY = GT.PosY - Interval_Y * 2.0f;
                                childT2.PosY = GT.PosY + Interval_Y * 2.0f;
                                break;
                            case 4:
                                childT1.PosY = GT.PosY - Interval_Y * 2.5f;
                                childT2.PosY = GT.PosY + Interval_Y * 2.5f;
                                break;
                        }
                        break;
                    case 2:
                        switch (GradChildT2)
                        {
                            case 1:
                                childT1.PosY = GT.PosY - Interval_Y * 1.5f;
                                childT2.PosY = GT.PosY + Interval_Y * 1.5f;
                                break;
                            case 2:
                                childT1.PosY = GT.PosY - Interval_Y * 2.0f;
                                childT2.PosY = GT.PosY + Interval_Y * 2.0f;
                                break;
                            case 3:
                                childT1.PosY = GT.PosY - Interval_Y * 2.5f;
                                childT2.PosY = GT.PosY + Interval_Y * 2.5f;
                                break;
                            case 4:
                                childT1.PosY = GT.PosY - Interval_Y * 3.0f;
                                childT2.PosY = GT.PosY + Interval_Y * 3.0f;
                                break;
                        }
                        break;
                    case 3:
                        switch (GradChildT2)
                        {
                            case 1:
                                childT1.PosY = GT.PosY - Interval_Y * 2.0f;
                                childT2.PosY = GT.PosY + Interval_Y * 2.0f;
                                break;
                            case 2:
                                childT1.PosY = GT.PosY - Interval_Y * 2.5f;
                                childT2.PosY = GT.PosY + Interval_Y * 2.5f;
                                break;
                            case 3:
                                childT1.PosY = GT.PosY - Interval_Y * 3.0f;
                                childT2.PosY = GT.PosY + Interval_Y * 3.0f;
                                break;
                            case 4:
                                childT1.PosY = GT.PosY - Interval_Y * 3.5f;
                                childT2.PosY = GT.PosY + Interval_Y * 3.5f;
                                break;
                        }
                        break;
                    case 4:
                        switch (GradChildT2)
                        {
                            case 1:
                                childT1.PosY = GT.PosY - Interval_Y * 2.5f;
                                childT2.PosY = GT.PosY + Interval_Y * 2.5f;
                                break;
                            case 2:
                                childT1.PosY = GT.PosY - Interval_Y * 3.0f;
                                childT2.PosY = GT.PosY + Interval_Y * 3.0f;
                                break;
                            case 3:
                                childT1.PosY = GT.PosY - Interval_Y * 3.5f;
                                childT2.PosY = GT.PosY + Interval_Y * 3.5f;
                                break;
                            case 4:
                                childT1.PosY = GT.PosY - Interval_Y * 4.0f;
                                childT2.PosY = GT.PosY + Interval_Y * 4.0f;
                                break;
                        }
                        break;
                }
            }
        }
        private void AsignPosY3Nodes(GlycanTree GT)
        {
            GlycanTree childT1 = GT.GetChild[0];
            GlycanTree childT2 = GT.GetChild[1];
            GlycanTree childT3 = GT.GetChild[2];
            if (GT.GetChild.Count == 4)
            {
                if (GT.GetChild[0].Root == Glycan.Type.DeHex)
                {
                    childT1 = GT.GetChild[1];
                    childT2 = GT.GetChild[2];
                    childT3 = GT.GetChild[3];
                }
                else if (GT.GetChild[1].Root == Glycan.Type.DeHex)
                {
                    childT1 = GT.GetChild[0];
                    childT2 = GT.GetChild[2];
                    childT3 = GT.GetChild[3];
                }
                else if (GT.GetChild[2].Root == Glycan.Type.DeHex)
                {
                    childT1 = GT.GetChild[0];
                    childT2 = GT.GetChild[1];
                    childT3 = GT.GetChild[3];
                }
                else
                {
                    childT1 = GT.GetChild[0];
                    childT2 = GT.GetChild[1];
                    childT3 = GT.GetChild[2];
                }
            }
           
            

            int GradChildT1 = childT1.GetChild.Count - childT1.NumberOfFucChild;
            int GradChildT2 = childT2.GetChild.Count - childT2.NumberOfFucChild;
            int GradChildT3 = childT3.GetChild.Count - childT3.NumberOfFucChild;
            childT1.PosX = GT.PosX - Interval_X;
            childT2.PosX = GT.PosX - Interval_X;
            childT3.PosX = GT.PosX - Interval_X;
            childT2.PosY = GT.PosY;
            if (GradChildT1 + GradChildT2 <= 3)
            {
                childT1.PosY = GT.PosY - Interval_Y * 2.0f;
                childT2.PosY = GT.PosY;
                childT3.PosY = GT.PosY + Interval_Y * 2.0f;
            }
            else
            {
                switch (GradChildT1)
                {
                    case 1:
                        switch (GradChildT2)
                        {
                            case 2:                                
                                    childT1.PosY = GT.PosY - Interval_Y * 3.0f;
                                    childT3.PosY = GT.PosY + Interval_Y * (  2.5f + GradChildT3 * 0.5f );                 
                                break;
                            case 3:
                                    childT1.PosY = GT.PosY - Interval_Y * 4.0f;
                                    childT3.PosY = GT.PosY + Interval_Y * (  3.5f + GradChildT3 * 0.5f );
                                break;
                            case 4:                                
                                    childT1.PosY = GT.PosY - Interval_Y * 4.5f;
                                    childT3.PosY = GT.PosY + Interval_Y * (  4.0f + GradChildT3 * 0.5f );
                                break;
                        }
                        break;
                    case 2:
                        switch (GradChildT2)
                        {
                            case 1:
                                childT1.PosY = GT.PosY - Interval_Y * 3.0f;
                                childT3.PosY = GT.PosY + Interval_Y * (1.5f + GradChildT3 *0.5f);
                                break;
                            case 2:
                                childT1.PosY = GT.PosY - Interval_Y * 4.0f;
                                childT3.PosY = GT.PosY + Interval_Y * (2.5f + GradChildT3 * 0.5f);
                                break;
                            case 3:
                                childT1.PosY = GT.PosY - Interval_Y * 4.5f;
                                childT3.PosY = GT.PosY + Interval_Y * (3.5f + GradChildT3 * 0.5f);
                                break;
                            case 4:
                                childT1.PosY = GT.PosY - Interval_Y * 4.5f;
                                childT3.PosY = GT.PosY + Interval_Y * (4.0f + GradChildT3 * 0.5f);
                                break;
                        }
                        break;
                    case 3:
                        switch (GradChildT2)
                        {
                            case 1:
                                childT1.PosY = GT.PosY - Interval_Y * 4.0f;
                                childT3.PosY = GT.PosY + Interval_Y * (1.5f + GradChildT3 * 0.5f);
                                break;
                            case 2:
                                childT1.PosY = GT.PosY - Interval_Y * 4.5f;
                                childT3.PosY = GT.PosY + Interval_Y * (2.5f + GradChildT3 * 0.5f);
                                break;
                            case 3:
                                childT1.PosY = GT.PosY - Interval_Y * 5.0f;
                                childT3.PosY = GT.PosY + Interval_Y * (3.5f + GradChildT3 * 0.5f);
                                break;
                            case 4:
                                childT1.PosY = GT.PosY - Interval_Y * 5.5f;
                                childT3.PosY = GT.PosY + Interval_Y * (4.0f + GradChildT3 * 0.5f);
                                break;
                        }
                        break;
                    case 4:
                        switch (GradChildT2)
                        {
                            case 1:
                                childT1.PosY = GT.PosY - Interval_Y * 4.5f;
                                childT3.PosY = GT.PosY + Interval_Y * (1.5f + GradChildT3 * 0.5f);
                                break;
                            case 2:
                                childT1.PosY = GT.PosY - Interval_Y * 5.0f;
                                childT3.PosY = GT.PosY + Interval_Y * (2.5f + GradChildT3 * 0.5f);
                                break;
                            case 3:
                                childT1.PosY = GT.PosY - Interval_Y * 5.5f;
                                childT3.PosY = GT.PosY + Interval_Y * (3.5f + GradChildT3 * 0.5f);
                                break;
                            case 4:
                                childT1.PosY = GT.PosY - Interval_Y * 6.0f;
                                childT3.PosY = GT.PosY + Interval_Y * (4.0f + GradChildT3 * 0.5f);
                                break;
                        }
                        break;
                }
            }
        }
    }
    
}
