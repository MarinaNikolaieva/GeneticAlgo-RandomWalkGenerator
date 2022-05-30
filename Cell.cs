using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomWalkTrial
{
    class Cell : IEquatable<Cell>
    {
        private bool reachable;
        private int x;
        private int y;
        private int g;
        private int h;
        private int f;
        private Cell parent;

        public Cell(int X, int Y, bool reach)
        {
            reachable = reach;
            x = X;
            y = Y;
            g = 0;
            h = 0;
            f = 0;
            parent = null;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public int getF()
        {
            return f;
        }

        public int getG()
        {
            return g;
        }

        public int getH()
        {
            return h;
        }

        public Cell getParent()
        {
            return parent;
        }

        public bool getReach()
        {
            return reachable;
        }

        public void setF(int a)
        {
            f = a;
        }

        public void setG(int a)
        {
            g = a;
        }

        public void setH(int a)
        {
            h = a;
        }

        public void setParent(Cell a)
        {
            parent = a;
        }

        public bool Equals(Cell c)
        {
            return c.getX() == this.x && c.getY() == this.y && c.getF() == this.f && c.getG() == this.g && 
                c.getH() == this.h && c.getReach() == this.reachable;
        }
    }
}
