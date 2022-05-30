using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RandomWalkTrial
{
    class PathfinderNode : IEquatable<PathfinderNode>
    {
        private int xCoordinate;
        private int yCoordinate;
        private bool reachable;
        private int g;
        private int h;
        private int f;
        private PathfinderNode parent;
        private List<PathfinderConnection> connections;

        public PathfinderNode(int x, int y, bool reach)  //default
        {
            this.xCoordinate = x;
            this.yCoordinate = y;
            reachable = reach;
            g = 0;
            h = 0;
            f = 0;
            parent = null;
            this.connections = new List<PathfinderConnection>();
        }

        public PathfinderNode(int x, int y, List<PathfinderConnection> cons)  //mostly for cloning
        {
            this.xCoordinate = x;
            this.yCoordinate = y;
            this.connections = new List<PathfinderConnection>();
            this.connections.AddRange(cons);
        }

        public int getX()
        {
            return xCoordinate;
        }

        public int getY()
        {
            return yCoordinate;
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

        public PathfinderNode getParent()
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

        public void setParent(PathfinderNode a)
        {
            parent = a;
        }

        public List<PathfinderConnection> getCons()
        {
            return connections;
        }

        public void addConnection(PathfinderConnection con)
        {
            if (!connections.Contains(con))
                connections.Add(con);
        }

        public PathfinderConnection getConnectionByCoords(int x, int y)
        {
            for (int i = 0; i < connections.Count; i++)
                if (connections.ElementAt(i).getDestinationX() == x && connections.ElementAt(i).getDestinationY() == y)
                    return connections.ElementAt(i);
            return null;
        }

        public bool Equals(PathfinderNode other)
        {
            bool equal = other.getX() == this.xCoordinate && other.getY() == this.yCoordinate &&
                other.getF() == this.f && other.getG() == this.g &&
                other.getH() == this.h && other.getReach() == this.reachable;
            try
            {
                CollectionAssert.AreEquivalent(this.connections, other.connections);
            }
            catch (AssertFailedException)
            {
                equal = false;
            }
            return equal;
        }
    }
}
