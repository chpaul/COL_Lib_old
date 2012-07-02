using System;
using System.Collections.Generic;
using System.Text;

namespace COL.GlycoLib
{
    [Serializable]    
    public class GlycanTree
    {
        Glycan.Type _root;
        List<GlycanTree> _child;
        GlycanTree _parent;
        float _posX;
        float _posY;
        int _distanceToRoot = 0;
        public int DistanceToRoot
        {
            set { _distanceToRoot = value; }
            get { return _distanceToRoot; }
        }
        public GlycanTree Parent
        {
            set { _parent = value; }
            get { return _parent; }
        }
        public GlycanTree()
        {
            _child = new List<GlycanTree>();
        }
        public GlycanTree(Glycan.Type argType)
        {
            _root = argType;
            _child = new List<GlycanTree>();
        }
        public void AddChild(GlycanTree argType)
        {
            if (argType.Root == Glycan.Type.DeHex)
            {
                _child.Insert(_child.Count, argType);
            }
            else
            {
                _child.Add(argType);
            }
            _child.Sort(delegate(GlycanTree p1, GlycanTree p2)
            {
                return p1.GetChild.Count.CompareTo(p2.GetChild.Count);
            });
        }
        public Glycan.Type Root
        {
            set { _root = value; }
            get { return _root; }
        }
        public List<GlycanTree> GetChild
        {
            get { return _child; }
        }
        public float PosX
        {
            get { return _posX; }
            set { _posX = value; }
        }
        public float PosY
        {
            get { return _posY; }
            set { _posY = value; }
        }
        //public int Level
        //{
        //    get { return _level; }
        //    set { _level = value; }
        //}
        public int NumberOfFucChild
        {
            get
            {
                int tmp = 0;
                foreach (GlycanTree g in _child)
                {
                    if (g.Root == Glycan.Type.DeHex)
                    {
                        tmp = tmp + 1;
                    }
                }
                return tmp;
            }
        }
        public void UpdateDistance(int argParentDistance)
        {
            this._distanceToRoot = argParentDistance + 1;
            if (_child.Count != 0)
            {
                foreach (GlycanTree CT in _child)
                {
                    CT.UpdateDistance(this._distanceToRoot);
                }
            }
        }
        public IEnumerable<GlycanTree> TravelGlycanTreeBFS()
        {
            Queue<GlycanTree> GlycanQue = new Queue<GlycanTree>();
            List<GlycanTree> glycanOrder = new List<GlycanTree>();
            glycanOrder.Add(this);

            if (_child.Count != 0)
            {
                foreach (GlycanTree g in _child)
                {
                    GlycanQue.Enqueue(g);
                }
            }
            while (GlycanQue.Count > 0)
            {
                GlycanTree g = (GlycanTree)GlycanQue.Dequeue();
                glycanOrder.Add(g);
                foreach (GlycanTree k in g._child)
                {
                    GlycanQue.Enqueue(k);
                }
            }
            foreach (GlycanTree g in glycanOrder)
            {
                yield return g;
            }
        }
        public IEnumerable<GlycanTree> TravelGlycanTreeDFS()
        {
            Stack<GlycanTree> GlycanStk = new Stack<GlycanTree>();
            List<GlycanTree> glycanOrder = new List<GlycanTree>();
            GlycanStk.Push(this);


            while (GlycanStk.Count != 0)
            {
                GlycanTree g = (GlycanTree)GlycanStk.Peek();
                if (g._child.Count == 0)
                {
                    glycanOrder.Add((GlycanTree)GlycanStk.Pop());
                }
                else
                {
                    int NoneTravedChild = 0;
                    foreach (GlycanTree k in g._child)
                    {
                        if (!glycanOrder.Contains(k))
                        {
                            GlycanStk.Push(k);
                            NoneTravedChild++;
                        }

                    }
                    if (NoneTravedChild == 0)
                    {
                        glycanOrder.Add((GlycanTree)GlycanStk.Pop());
                    }
                }
            }

            foreach (GlycanTree g in glycanOrder)
            {
                yield return g;
            }
        }
    }
}
