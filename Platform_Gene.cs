using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomWalkTrial
{
    class Platform_Gene : IEquatable <Platform_Gene>
    {
        private int xStart { get; set; }
        private int yStart { get; set; }
        private int xEnd { get; set; }
        private int yEnd { get; set; }
        private int length { get; set; }
        private bool afterMid { get; set; }

        public Platform_Gene(int xs, int ys, int xe, int ye, int middle)
        {
            if (xs > xe)
            {
                xStart = xe;
                xEnd = xs;
                yStart = ye;
                yEnd = ys;
            }
            else
            {
                xStart = xs;
                xEnd = xe;
                yStart = ys;
                yEnd = ye;
            }
            calcLength();
            if (xEnd >= middle)
                afterMid = true;
            else
                afterMid = false;
        }

        public void calcLength()
        {
            length = xEnd - xStart;
        }

        public bool getAfterMid()
        {
            return afterMid;
        }

        public int getXSt()
        {
            return xStart;
        }

        public int getYSt()
        {
            return yStart;
        }

        public int getXE()
        {
            return xEnd;
        }

        public int getYE()
        {
            return yEnd;
        }

        public bool Equals(Platform_Gene platf)
        {
            return this.xStart == platf.xStart &&
                this.xEnd == platf.xEnd &&
                this.yStart == platf.yStart &&
                this.yEnd == platf.yEnd;
        }
    }
}
