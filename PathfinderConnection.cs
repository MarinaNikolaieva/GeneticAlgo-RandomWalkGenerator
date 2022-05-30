using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomWalkTrial
{
    class PathfinderConnection : IEquatable<PathfinderConnection>
    {
        private int destinationX;
        private int destinationY;
        private double distance;
        private char direction;  //'u' = jump only, 'd' = drop only, 'b' = both

        public PathfinderConnection(PathfinderNode destination, double distance, char direction)
        {
            this.destinationX = destination.getX();
            this.destinationY = destination.getY();
            this.distance = distance;
            this.direction = direction;
        }

        public int getDestinationX()
        {
            return destinationX;
        }

        public int getDestinationY()
        {
            return destinationY;
        }

        public double getDistance()
        {
            return distance;
        }

        public char getDirection()
        {
            return direction;
        }

        public bool Equals(PathfinderConnection con)
        {
            return this.distance == con.distance &&
                this.direction == con.direction &&
                this.destinationX == con.destinationX &&
                this.destinationY == con.destinationY;
        }
    }
}
