using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomWalkTrial
{
    class PathfinderDifferent
    {
        private PathfinderNode[,] nodes;
        private int xBeg;
        private int yBeg;
        private int xEnd;
        private int yEnd;
        private int N;
        private int M;

        public PathfinderDifferent(ref int[,] array, int n, int m, int xS, int yS, int xE, int yE)
        {
            N = n;
            M = m;
            nodes = new PathfinderNode[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    bool reach = array[i, j] != 0 ? true : false;
                    nodes[i, j] = new PathfinderNode(j, i, reach);
                }
            }
            xBeg = xS;
            yBeg = yS;
            xEnd = xE;
            yEnd = yE;
        }

        private int getHeuristic(PathfinderNode node)
        {
            int h = Math.Abs(node.getX() - xEnd) + Math.Abs(node.getY() - yEnd);
            return h;
        }

        private void updateAdjCell(PathfinderNode nodeToUp, PathfinderNode par)
        {
            nodeToUp.setG(par.getG() + 1);
            nodeToUp.setH(getHeuristic(nodeToUp));
            nodeToUp.setParent(par);
            nodeToUp.setF(nodeToUp.getH() + nodeToUp.getG());
        }

        private List<PathfinderNode> getAdjusted(PathfinderNode node)
        {
            List<PathfinderNode> adjs = new List<PathfinderNode>();

            List<PathfinderConnection> cons = node.getCons();
            for (int i = 0; i < cons.Count; i++)
                adjs.Add(nodes[cons[i].getDestinationY(), cons[i].getDestinationX()]);

            return adjs;
        }

        private void prepare(int maxJumpWid, int maxJumpHeig, int walkWid)
        {
            //I won't make the drop deadly. IF I WILL, I'll need to limit the distance for falling
            for (int i = 1; i < nodes.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < nodes.GetLength(1) - 1; j++)
                {
                    if (nodes[i, j].getReach() && !nodes[i + 1, j].getReach())  //if node is above the ground...
                    {
                        //...it can be examined
                        //Walking and droppind directions first
                        if (nodes[i, j - 1].getReach())  // to the left side of the node
                        {
                            if (!nodes[i + 1, j - 1].getReach())
                            {
                                nodes[i, j].addConnection(new PathfinderConnection(nodes[i, j - 1], 1.0, 'b'));
                                nodes[i, j - 1].addConnection(new PathfinderConnection(nodes[i, j], 1.0, 'b'));
                            }
                            else
                            {
                                PathfinderNode keep = nodes[i + 1, j - 1];
                                for (int k = i + 1; k < nodes.GetLength(0) - 1; k++)
                                    if (!nodes[k, j - 1].getReach())
                                    {
                                        keep = nodes[k, j - 1];
                                        break;
                                    }
                                double distance = Math.Pow(Math.Abs(nodes[i, j].getX() - keep.getX()), 2);
                                distance += Math.Pow(Math.Abs(nodes[i, j].getY() - keep.getY()), 2);
                                distance = Math.Sqrt(distance);  //Pythagor's theorem
                                if (Math.Abs(i + 1 - keep.getY()) > maxJumpHeig - 1)  //if we can't jump back, we just drop
                                    nodes[i, j].addConnection(new PathfinderConnection(keep, distance, 'd'));
                                else
                                {  //if we can, we jump and drop -> both ways
                                    nodes[i, j].addConnection(new PathfinderConnection(keep, distance, 'b'));
                                    nodes[keep.getY(), keep.getX()].addConnection(new PathfinderConnection(nodes[i, j], distance, 'b'));
                                }
                            }
                        }
                        if (nodes[i, j + 1].getReach())  //to the right side of the node
                        {
                            if (!nodes[i + 1, j + 1].getReach())
                            {
                                nodes[i, j].addConnection(new PathfinderConnection(nodes[i, j + 1], 1.0, 'b'));
                                nodes[i, j + 1].addConnection(new PathfinderConnection(nodes[i, j], 1.0, 'b'));
                            }
                            else
                            {
                                PathfinderNode keep = nodes[i + 1, j + 1];
                                for (int k = i + 1; k < nodes.GetLength(0) - 1; k++)
                                    if (!nodes[k, j + 1].getReach())
                                    {
                                        keep = nodes[k, j + 1];
                                        break;
                                    }
                                double distance = Math.Pow(Math.Abs(nodes[i, j].getX() - keep.getX()), 2);
                                distance += Math.Pow(Math.Abs(nodes[i, j].getY() - keep.getY()), 2);
                                distance = Math.Sqrt(distance);  //Pythagor's theorem
                                if (Math.Abs(i + 1 - keep.getY()) > maxJumpHeig - 1)  //if we can't jump back, we just drop
                                    nodes[i, j].addConnection(new PathfinderConnection(keep, distance, 'd'));
                                else
                                {  //if we can, we jump and drop -> both ways
                                    nodes[i, j].addConnection(new PathfinderConnection(keep, distance, 'b'));
                                    nodes[keep.getY(), keep.getX()].addConnection(new PathfinderConnection(nodes[i, j], distance, 'b'));
                                }
                            }
                        }

                        //Now the jumping directions (oh dear...)
                        //Let's imagine the jumping trajectory as half of the circle. That means we can reach everything within this circle
                        
                        //first - the closest ones
                        for (int k = i + 1; k < i + maxJumpHeig; k++)
                        {
                            if (k >= nodes.GetLength(0) - 1)
                                break;
                            for (int l = j - 1; l >= j - maxJumpHeig; l--)
                            {
                                if (l < 0)
                                    break;
                                if (nodes[k + 1, l].getReach() && !nodes[k, l].getReach())
                                {
                                    PathfinderNode keep = nodes[k + 1, l];
                                    double distance = Math.Pow(Math.Abs(nodes[i, j].getX() - keep.getX()), 2);
                                    distance += Math.Pow(Math.Abs(nodes[i, j].getY() - keep.getY()), 2);
                                    distance = Math.Sqrt(distance);
                                    nodes[i, j].addConnection(new PathfinderConnection(keep, distance, 'b'));
                                    nodes[keep.getY(), keep.getX()].addConnection(new PathfinderConnection(nodes[i, j], distance, 'b'));
                                }
                            }

                            for (int l = j + 1; l <= j + maxJumpHeig; l++)
                            {
                                if (l >= nodes.GetLength(1) - 1)
                                    break;
                                if (nodes[k + 1, l].getReach() && !nodes[k, l].getReach())
                                {
                                    PathfinderNode keep = nodes[k + 1, l];
                                    double distance = Math.Pow(Math.Abs(nodes[i, j].getX() - keep.getX()), 2);
                                    distance += Math.Pow(Math.Abs(nodes[i, j].getY() - keep.getY()), 2);
                                    distance = Math.Sqrt(distance);
                                    nodes[i, j].addConnection(new PathfinderConnection(keep, distance, 'b'));
                                    nodes[keep.getY(), keep.getX()].addConnection(new PathfinderConnection(nodes[i, j], distance, 'b'));
                                }
                            }
                        }

                        //Then the further ones
                        //To the left side of the node
                        double centerX = j - maxJumpHeig - 0.5;
                        double centerY = i + 0.5;
                        for (double k = (centerY - maxJumpHeig >= 0 ? centerY - maxJumpHeig : 0); k < (centerY + maxJumpHeig < N ? centerY + maxJumpHeig : N); k++)
                        {
                            if (k >= nodes.GetLength(0) - 1)
                                break;
                            for (double l = (centerX - maxJumpHeig >= 0 ? centerX - maxJumpHeig : 0); l < (centerX + maxJumpHeig < M ? centerX + maxJumpHeig : M); l++)
                            {
                                if (l < 0)
                                    break;
                                if ((Math.Pow(l - centerX, 2.0) + Math.Pow(k - centerY, 2.0)) <= Math.Pow(maxJumpHeig, 2.0))
                                {
                                    if (nodes[(int)(k - 0.5), (int)(l + 0.5)].getReach() && nodes[i + 1, (int)(l + 0.5)].getReach())
                                    {
                                        PathfinderNode keep = nodes[(int)(k - 0.5), (int)(l + 0.5)];
                                        double distance = Math.Pow(Math.Abs(nodes[i, j].getX() - keep.getX()), 2);
                                        distance += Math.Pow(Math.Abs(nodes[i, j].getY() - keep.getY()), 2);
                                        distance = Math.Sqrt(distance);
                                        nodes[i, j].addConnection(new PathfinderConnection(keep, distance, 'b'));
                                        nodes[keep.getY(), keep.getX()].addConnection(new PathfinderConnection(nodes[i, j], distance, 'b'));
                                    }
                                }
                            }
                        }
                        //To the right side of the node
                        centerX = j + maxJumpHeig + 0.5;
                        for (double k = (centerY - maxJumpHeig >= 0 ? centerY - maxJumpHeig : 0); k < (centerY + maxJumpHeig < N ? centerY + maxJumpHeig : N); k++)
                        {
                            if (k >= nodes.GetLength(0) - 1)
                                break;
                            for (double l = (centerX - maxJumpHeig >= 0 ? centerX - maxJumpHeig : 0); l < (centerX + maxJumpHeig < M ? centerX + maxJumpHeig : M); l++)
                            {
                                if (l >= nodes.GetLength(1) - 1)
                                    break;
                                if ((Math.Pow(l - centerX, 2.0) + Math.Pow(k - centerY, 2.0)) <= Math.Pow(maxJumpHeig, 2.0))
                                {
                                    if (nodes[(int)(k - 0.5), (int)(l - 0.5)].getReach() && nodes[i + 1, (int)(l - 0.5)].getReach())
                                    {
                                        PathfinderNode keep = nodes[(int)(k - 0.5), (int)(l - 0.5)];
                                        double distance = Math.Pow(Math.Abs(nodes[i, j].getX() - keep.getX()), 2);
                                        distance += Math.Pow(Math.Abs(nodes[i, j].getY() - keep.getY()), 2);
                                        distance = Math.Sqrt(distance);
                                        nodes[i, j].addConnection(new PathfinderConnection(keep, distance, 'b'));
                                        nodes[keep.getY(), keep.getX()].addConnection(new PathfinderConnection(nodes[i, j], distance, 'b'));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public double run()
        {
            int maxJumpHeight = 4;
            //int jumpWidthStand = 0;
            //int jumpWidthWalk = 4;
            int jumpWidthRun = 2 * maxJumpHeight; //it was 8, let it be like that
            int walkWidth = 1;
            //int runWidth = 3;

            prepare(jumpWidthRun, maxJumpHeight, walkWidth);

            List<PathfinderNode> finalized = new List<PathfinderNode>();
            List<PathfinderNode> toLook = new List<PathfinderNode>();
            List<PathfinderNode> heap = new List<PathfinderNode>();

            PathfinderNode start = new PathfinderNode(xBeg, yBeg, true);
            start.setG(-1);
            updateAdjCell(nodes[yBeg, xBeg], start);
            heap.Add(nodes[yBeg, xBeg]);  //add the starting point

            while (heap.Count != 0)
            {
                PathfinderNode curNode = heap.ElementAt(0);  //pop the cell from queue
                heap.RemoveAt(0);
                finalized.Add(nodes[curNode.getY(), curNode.getX()]);  //we don't need to process the cell twice
                if (curNode.getX() == xEnd && curNode.getY() == yEnd)
                    break;  //if the end's reached, break

                //if not, get adjusted cells
                toLook.Clear();
                toLook = getAdjusted(curNode);
                for (int i = 0; i < toLook.Count; i++)
                {
                    if (toLook.ElementAt(i).getReach() && !finalized.Contains(toLook.ElementAt(i)))
                    {
                        if (heap.Contains(toLook.ElementAt(i)))
                        {
                            double tempDist = curNode.getConnectionByCoords(toLook.ElementAt(i).getX(), toLook.ElementAt(i).getY()).getDistance();
                            if (toLook.ElementAt(i).getG() > curNode.getG() + tempDist)
                                updateAdjCell(toLook.ElementAt(i), curNode);
                        }
                        else
                        {
                            updateAdjCell(toLook.ElementAt(i), curNode);
                            heap.Add(toLook.ElementAt(i));
                        }
                    }
                }
                //This is important! Keep the cell with the lowest F on top
                heap = heap.OrderBy(x => x.getF()).ToList();
            }

            List<PathfinderNode> result = new List<PathfinderNode>();
            PathfinderNode end = nodes[yEnd, xEnd];
            result.Add(end);
            double dist = 0.0;
            while (true)
            {
                PathfinderNode par = end.getParent();
                if (par == null)  //if we couldn't reach the end cell
                    break;
                if (par != null && !(par.getX() == xBeg && par.getY() == yBeg))
                {
                    result.Add(par);
                    dist += Math.Sqrt(Math.Pow(end.getX() - par.getX(), 2.0) + Math.Pow(end.getY() - par.getY(), 2.0));
                    end = par;
                }
                else
                    break;
            }

            if (result.Count == 1)
                return -1.0;

            result.Add(nodes[yBeg, xBeg]);
            result.Reverse();
            dist += Math.Sqrt(Math.Pow(result.ElementAt(0).getX() - result.ElementAt(1).getX(), 2.0) + Math.Pow(result.ElementAt(0).getY() - result.ElementAt(1).getY(), 2.0));

            return dist;
        }
    }
}
