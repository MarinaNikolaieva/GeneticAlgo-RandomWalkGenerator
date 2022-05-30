using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RandomWalkTrial
{
    class Combination_Chromosome : IEquatable<Combination_Chromosome>
    {
        List<Platform_Gene> platforms;
        int[,] keepArray;
        List<List<Cell>> Cells;
        double fitnessRate;
        double distance;

        public Combination_Chromosome()  //Default constructor
        {
            platforms = new List<Platform_Gene>();
            fitnessRate = 0.0;
            distance = 0.0;
            keepArray = new int[10, 10];
            Cells = new List<List<Cell>>();
        }

        public Combination_Chromosome(List<Platform_Gene> list, ref int[,] array)  //We may need to pass the platforms immediately
        {
            platforms = new List<Platform_Gene>();
            platforms.AddRange(list);
            fitnessRate = 0.0;
            distance = 0.0;
            //keepArray = new int[array.GetLength(0), array.GetLength(1)];
            keepArray = array.Clone() as int[,];
            Cells = new List<List<Cell>>();
        }

        public void addPlatform(Platform_Gene platf)
        {
            if (!platforms.Contains(platf))
                platforms.Add(platf);
        }

        public List<Platform_Gene> getPlatforms()
        {
            return platforms;
        }

        public Platform_Gene getPlatfByIndex(int i)
        {
            if (i >= platforms.Count())
                return null;
            else
                return platforms.ElementAt(i);
        }

        public Platform_Gene getPlatfByIndexBeforeMid(int i)
        {
            if (i >= platforms.Count() && platforms.ElementAt(i).getAfterMid())
                return null;
            else
                return platforms.ElementAt(i);
        }

        public Platform_Gene getPlatfByIndexAfterMid(int i)
        {
            if (i >= platforms.Count() && !platforms.ElementAt(i).getAfterMid())
                return null;
            else
                return platforms.ElementAt(i);
        }

        public void deletePlatform(Platform_Gene platf)
        {
            if (platforms.Contains(platf))
                platforms.Remove(platf);
        }

        private double getPath(int xBeg, int yBeg, int xEnd, int yEnd)
        {

            int maxJumpHeight = 4;
            int jumpWidthStand = 0;
            int jumpWidthWalk = 4;
            int jumpWidthRun = 8;
            int walkWidth = 1;
            int runWidth = 3;

            //I have to adapt the algorithm using these parameters
            //MAYBE make these parameters configurable?

            double path = new PathfinderDifferent(ref keepArray, keepArray.GetLength(0), keepArray.GetLength(1), xBeg, yBeg, xEnd, yEnd).run();
            
            return path;
        }

        public double getDistance(int xBeg, int yBeg, int xEnd, int yEnd) 
            //this will be a hard method. This one has to find a way from Beg to End...
            //IF the level is uncompletable, return -1!
        {
            for (int i = 0; i < platforms.Count(); i++)
            {
                for (int j = platforms[i].getYSt(); j < platforms[i].getYE(); j++)
                {
                    for (int k = platforms[i].getXSt(); k < platforms[i].getXE(); k++)
                    {
                        keepArray[j, k] = 0;
                    }
                }
            }

            for (int i = 0; i < keepArray.GetLength(0); i++)
            {
                Cells.Add(new List<Cell>());
                for (int j = 0; j < keepArray.GetLength(1); j++)
                {
                    Cells.ElementAt(i).Add(new Cell(j, i, keepArray[i,j] == 0 ? true : false));
                }
            }

            double dist = 0.0;

            double path = getPath(xBeg, yBeg, xEnd, yEnd);
            if (path == -1.0)
                dist = -1.0;
            else
                dist = path;
            //distance = dist;
            return dist;
        }

        public double getFitness(int xBeg, int yBeg, int xEnd, int yEnd)
        {
            if (fitnessRate == 0.0)
            {
                distance = getDistance(xBeg, yBeg, xEnd, yEnd);
                fitnessRate = 1.0 / distance;
            }
            return fitnessRate;
        }

        public bool Equals(Combination_Chromosome other)
        {
            bool res = true;
            if (this.distance != other.distance)
                res = false;
            else if (this.fitnessRate != other.fitnessRate)
                res = false;
            else if (this.platforms.Count != other.platforms.Count)
                res = false;
            else
            {
                try
                {
                    CollectionAssert.AreEquivalent(this.platforms, other.platforms);
                }
                catch(AssertFailedException)
                {
                    res = false;
                }
            }
            return res;
        }
    }
}
