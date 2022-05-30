using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomWalkTrial
{
    class GeneratorRW
    {
        int n;
        int m;
        int stepSize;
        int smoothSteps;
        int iterNum;
        int[,] array;
        int[,] tempArray;

        //0 - wall, 1 - empty, 2 - start|end
        public GeneratorRW(int sS, int sSt, int iter, int N, int M)
        {
            n = N;
            m = M;
            stepSize = sS;
            smoothSteps = sSt;
            iterNum = iter;
            array = new int[n, m];
            tempArray = new int[n, m];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    array[j, i] = 0;
                    tempArray[j, i] = 0;
                }
            }
        }

        void paintPixels(int x, int y)
        {
            for (int i = 0; i < stepSize; i++)
            {
                for (int j = 0; j < stepSize; j++)
                {
                    array[y + j, x + i] = 1;
                    tempArray[y + j, x + i] = 1;
                }
            }
        }

        int countWhiteScale(int x, int y)  //here's where the coordinates will be transfered
        {
            int number = 0;
            number += array[y - 1, x - 1] != 0 ? 1 : 0;
            number += array[y - 1, x] != 0 ? 1 : 0;
            number += array[y - 1, x + 1] != 0 ? 1 : 0;
            number += array[y, x - 1] != 0 ? 1 : 0;
            number += array[y, x + 1] != 0 ? 1 : 0;
            number += array[y + 1, x - 1] != 0 ? 1 : 0;
            number += array[y + 1, x] != 0 ? 1 : 0;
            number += array[y + 1, x + 1] != 0 ? 1 : 0;
            return number;
        }

        int countEmptySpace(int x, int y)
        {
            int space = 0;

            space += array[y, x - 1] != 0 ? 1 : 0;
            space += array[y - 1, x - 1] != 0 ? 1 : 0;
            space += array[y - 2, x - 1] != 0 ? 1 : 0;
            space += array[y, x] != 0 ? 1 : 0;
            space += array[y - 1, x] != 0 ? 1 : 0;
            space += array[y - 2, x] != 0 ? 1 : 0;
            space += array[y, x + 1] != 0 ? 1 : 0;
            space += array[y - 1, x + 1] != 0 ? 1 : 0;
            space += array[y - 2, x + 1] != 0 ? 1 : 0;

            return space;
        }

        bool checkFloor(int x, int y)
        {
            int floor = 0;

            floor += array[y + 1, x - 1] != 0 ? 0 : 1;
            floor += array[y + 1, x] != 0 ? 0 : 1;
            floor += array[y + 1, x + 1] != 0 ? 0 : 1;

            if (floor == 3)
                return true;
            else
                return false;
        }

        public void generate()
        {
            Random rand = new Random();
            int x, y;  //init the starting point
            x = rand.Next(0, m - stepSize);
            y = rand.Next(0, n - stepSize);
            paintPixels(x, y);
            //pict.SetPixel(x, y, Color.White);

            int direction;
            //0 - up, 1 - down, 2 - left, 3 - right
            //loop the random walk
            for (int i = 0; i < iterNum; i++)
            {
                direction = rand.Next(0, 4);
                while (true)
                {
                    if (direction == 0 && y + 2 * stepSize >= n)
                        direction = rand.Next(0, 4);
                    else if (direction == 1 && y - 2 * stepSize < 0)
                        direction = rand.Next(0, 4);
                    else if (direction == 2 && x - 2 * stepSize < 0)
                        direction = rand.Next(0, 4);
                    else if (direction == 3 && x + 2 * stepSize >= m)
                        direction = rand.Next(0, 4);
                    else break;
                }
                if (direction == 0)
                    y += stepSize;
                else if (direction == 1)
                    y -= stepSize;
                else if (direction == 2)
                    x -= stepSize;
                else if (direction == 3)
                    x += stepSize;
                paintPixels(x, y);
                //pict.SetPixel(x, y, Color.White);
            }

            //Let's smooth the picture
            for (int i = 0; i < smoothSteps; i++)
            {
                for (int X = 1; X < m - 1; X++)
                {
                    for (int Y = 1; Y < n - 1; Y++)
                    {
                        int neighbours = countWhiteScale(X, Y);
                        if (neighbours < 5)
                            tempArray[Y, X] = 0;
                        else if (neighbours >= 5)
                            tempArray[Y, X] = 1;
                    }
                }
                for (int X = 0; X < m; X++)
                {
                    for (int Y = 0; Y < n; Y++)
                    {
                        array[Y, X] = tempArray[Y, X];
                    }
                }
            }

            //Now let's place the Start and End points for the player
            bool stop = false;

            //start point
            for (int X = 1; X < m - 1; X++)
            {
                for (int Y = n - 2; Y > 3; Y--)
                {
                    int count = countEmptySpace(X, Y);
                    bool floor = checkFloor(X, Y);
                    if (count == 9 && floor)
                    {
                        array[Y, X] = 2;
                        stop = true;
                        break;
                    }
                }
                if (stop)
                {
                    stop = false;
                    break;
                }
            }

            //end point
            for (int X = m - 2; X > 0; X--)
            {
                for (int Y = n - 2; Y > 3; Y--)
                {
                    int count = countEmptySpace(X, Y);
                    bool floor = checkFloor(X, Y);
                    if (count == 9 && floor)
                    {
                        array[Y, X] = 2;
                        stop = true;
                        break;
                    }
                }
                if (stop)
                {
                    stop = false;
                    break;
                }
            }
        }

        public int[,] getArray()
        {
            return array;
        }
    }
}
