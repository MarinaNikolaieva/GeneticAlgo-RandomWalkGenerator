using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RandomWalkTrial
{
    class Program
    {
        static Bitmap pict;
        static int[,] array;
        

        static void Main(string[] args)
        {
            //init picture dimentions
            Console.WriteLine("Please enter how many pixels do you want");
            Console.WriteLine("Height: ");
            int n = int.Parse(Console.ReadLine());
            Console.WriteLine("Width: ");
            int m = int.Parse(Console.ReadLine());

            pict = new Bitmap(m, n);

            //black - solid, white - open
            Console.WriteLine("How many steps do you want? ");
            int steps = int.Parse(Console.ReadLine());

            Console.WriteLine("What will be your step size? (recommended: 5 for 300x600) ");
            int stepSize = int.Parse(Console.ReadLine());

            Console.WriteLine("How many smoothing iterations do you want? (recommended: 10) ");
            int smoothSteps = int.Parse(Console.ReadLine());

            try
            {
                //generate the basis and smooth it
                GeneratorRW generator = new GeneratorRW(stepSize, smoothSteps, steps, n, m);
                generator.generate();
                array = generator.getArray().Clone() as int[,];

                //copy to Bitmap for export
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        int k = array[i, j];
                        Color color;
                        if (k == 0)
                            color = Color.Black;
                        else if (k == 1)
                            color = Color.White;
                        else if (k == 2)
                            color = Color.Red;
                        else
                            color = Color.Black;
                        pict.SetPixel(j, i, color);
                    }
                }

                //export
                pict.Save("C:\\Data\\Diploma\\imageBefore.jpeg", ImageFormat.Jpeg);

                //run the genetic algorithm
                int verticalStripsNum = m / 10 > 1 ? m / 10 : m / 5;
                int horizontalSectorsNum = n / 10 > 1 ? n / 10 : n / 3;
                int iterations = 5;
                int generationsNum = 10;
                double eliteRate = 0.2;
                double mutationRate = 0.05;
                GeneticAlgo genetic = new GeneticAlgo(ref array, verticalStripsNum, horizontalSectorsNum, iterations, generationsNum, eliteRate, mutationRate);
                genetic.run(ref array);

                //copy to Bitmap for export
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        int k = array[i, j];
                        Color color;
                        if (k == 0)
                            color = Color.Black;
                        else if (k == 1)
                            color = Color.White;
                        else if (k == 2)
                            color = Color.Red;
                        else
                            color = Color.Black;
                        pict.SetPixel(j, i, color);
                    }
                }

                //export
                pict.Save("C:\\Data\\Diploma\\imageAfter.jpeg", ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.TargetSite);
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
