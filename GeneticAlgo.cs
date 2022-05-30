using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomWalkTrial
{
    class GeneticAlgo
    {
        int numOfVertParts;
        int sizeOfOneVert;
        //int sizeOfOneVertExtra;
        int numOfHorizSectors;
        int sizeOfOneHoriz;
        //int sizeOfOneHorizExtra;
        //List<List<int>> sectorPlatformCapacity;
        int[,] sectorPlatfCapacity;

        int xS;
        int yS;
        int xE;
        int yE;

        List<Combination_Chromosome> individuals;

        int n;
        int m;

        int iters;
        int generationSize;
        double eliteRate;
        int eliteNum;
        double mutationRate;
        public GeneticAlgo(ref int[,] array, int nvp, int nhs, int iter, int genS, double elR, double mutR)
        {
            //sectorPlatformCapacity = new List<List<int>>();
            individuals = new List<Combination_Chromosome>();
            n = array.GetLength(0);
            m = array.GetLength(1);
            numOfVertParts = nvp;
            sizeOfOneVert = m / nvp;
            //if (m % nvp != 0)
            //    sizeOfOneVertExtra = m % nvp;
            //else
            //    sizeOfOneVertExtra = 0;
            numOfHorizSectors = nhs;
            sizeOfOneHoriz = n / nhs;

            sectorPlatfCapacity = new int[numOfVertParts, numOfHorizSectors];
            //if (n % nhs != 0)
            //    sizeOfOneHorizExtra = n % nhs;
            //else
            //    sizeOfOneHorizExtra = 0;

            //Maybe I'll uncomment this if I'll make the option. Let's assume the sides are dividable

            iters = iter;
            generationSize = genS;
            eliteRate = elR;
            eliteNum = (int)(generationSize * elR);
            mutationRate = mutR;

            bool got = false;
            for (int i = 1; i < m - 1; i++)
            {
                for (int j = n - 2; j > 3; j--)
                {
                    if (array[j, i] == 2)
                    {
                        xS = i;
                        yS = j;
                        got = true;
                        break;
                    }
                }
                if (got)
                    break;
            }
            got = false;
            for (int i = m - 2; i > 0; i--)
            {
                for (int j = n - 2; j > 3; j--)
                {
                    if (array[j, i] == 2)
                    {
                        xE = i;
                        yE = j;
                        got = true;
                        break;
                    }
                }
                if (got)
                    break;
            }
        }

        //NEEDED:
        //calculate the capacity of sectors  DONE
        //make a function for making an individual  PROBABLY DONE
        //keep the places for platforms (for EACH individual separately) (that's a chromosome)  DONE
        //make a class for genes (places for platforms) and/or chromosomes (combinations)  PROBABLY DONE
        //make a function for selection  PROBABLY DONE
        //make a Crossover (divide the array in half, one half from one, one from another)  PROBABLY DONE
        //make a Mutation (randomly delete and replace a platform/platforms)  PROBABLY DONE
        //make a function to optimize (the longest way to run through a level)  PROBABLY DONE
        //make a function for sorting (I'll need to sort my individuals by their run-through length from the longest)  DONE
        //throw away the uncompletable levels (maybe inside the optimization part?)  PROBABLY DONE
        //after finishing, make a function for drawing the result into the main Array  PROBABLY DONE
        //TEST EVERYTHING AND MAKE A COMPLETE DEBUG!
        //MAYBE
        //continue the run until the fitness barrier is reached, not the iters number?
        //No. It will take way too long
        private void preparation(ref int[,] array)
        {
            for (int i = 0; i < numOfVertParts; i++)
            {
                for (int j = 0; j < numOfHorizSectors; j++)
                {
                    int[] histogram = new int[m];
                    for (int k = 0; k < m; k++)
                        histogram[i] = 0;
                    int maxWidth = 0;
                    int maxHeight = 0;

                    int largestArea = 0;

                    for (int k = j * sizeOfOneHoriz; k < j * sizeOfOneHoriz + sizeOfOneHoriz; k++)
                    {
                        Stack<int> stack = new Stack<int>();
                        for (int l = i * sizeOfOneVert; l < i * sizeOfOneVert + sizeOfOneVert; l++)
                        {
                            if (array[k, l] == 1)
                                histogram[l] += 1;
                            else
                                histogram[l] = 0;

                            while(stack.Count > 0 && histogram[stack.Peek()] >= histogram[l])
                            {
                                int tempHeight = stack.Peek();
                                int tempArea = histogram[stack.Pop()] * (stack.Count == 0 ? l : (l - stack.Peek() - 1));
                                if (largestArea < tempArea)
                                {
                                    maxHeight = tempHeight;
                                    maxWidth = stack.Count == 0 ? l : (l - stack.Peek() - 1);
                                }
                                largestArea = Math.Max(largestArea, tempArea);
                            }
                            stack.Push(l);
                        }
                    }

                    if (maxWidth < 5 || maxHeight < 3)
                        sectorPlatfCapacity[i, j] = 0;
                    else
                        sectorPlatfCapacity[i, j] = maxHeight / 3;
                }
            }
        }

        private Combination_Chromosome makeIndividual(ref int[,] array)
        {
            List<Platform_Gene> platfs = new List<Platform_Gene>();
            for (int i = 0; i < numOfVertParts; i++)
            {
                for (int j = 0; j < numOfHorizSectors; j++)
                {
                    if (sectorPlatfCapacity[i, j] != 0)
                    {
                        Random rand = new Random();

                        int xs, xe, ys, ye;  //randomly choose a place for the platform
                        //while (true)  //MAYBE give the algo some trials to place the platform?
                        for (int q = 0; q < 5; q++)  //Maybe 5 trials
                        {
                            //NEEDED make these 5 and 3 editable as variables
                            xs = rand.Next(i * sizeOfOneVert, (i + 1) * sizeOfOneVert);
                            xe = (xs + 5 >= (i + 1) * sizeOfOneVert ? (i + 1) * sizeOfOneVert : xs + 5);
                            ys = rand.Next(j * sizeOfOneHoriz, (j + 1) * sizeOfOneHoriz);
                            ye = (ys + 3 >= (j + 1) * sizeOfOneHoriz ? (j + 1) * sizeOfOneHoriz : ys + 3);
                            bool fits = true;
                            for (int k = ys; k <= ye; k++) {  //if there's no obstacles in place...
                                bool cut = false;
                                for (int l = xs; l <= xe; l++)
                                {
                                    if (array[k, l] == 0)
                                    {
                                        fits = false;
                                        cut = true;
                                        break;
                                    }
                                }
                                if (cut)
                                    break;
                            }
                            if (fits)  //...then add the platform
                            {
                                platfs.Add(new Platform_Gene(xs, ys, xe, ye, m / 2));
                                break;
                            }
                        }
                    }
                }
            }
            Combination_Chromosome combo = new Combination_Chromosome(platfs, ref array);
            return combo;
        }

        private void formPopulation(ref int[,] array)
        {
            for (int i = 0; i < generationSize; i++)
            {
                while (true) {
                    Combination_Chromosome individ = makeIndividual(ref array);
                    if (individ.getFitness(xS, yS, xE, yE) != -1.0)
                    {
                        individuals.Add(individ);
                        break;
                    }
                }
            }
        }

        private List<Combination_Chromosome> selection(List<Combination_Chromosome> forSelection)
        {
            List<Combination_Chromosome> result = new List<Combination_Chromosome>();
            List<double> cumulativeSums = new List<double>();
            double totalSum = 0.0;

            for (int i = 0; i < forSelection.Count; i++)  //calculating weights for further selection
            {
                double tempSum = 0.0;
                for (int j = 0; j <= i; j++)
                {
                    tempSum += forSelection.ElementAt(j).getFitness(xS, yS, xE, yE);
                }
                cumulativeSums.Add(tempSum);
                totalSum += tempSum;
            }
            List<double> weights = new List<double>();
            for (int i = 0; i < forSelection.Count; i++)
            {
                weights.Add(100.0 * cumulativeSums[i] / totalSum);
            }

            for (int i = 0; i < eliteNum; i++)  //keeping the best results
            {
                result.Add(forSelection[i]);
            }

            Random rand = new Random();  //selecting random individuals
            for (int i = 0; i < forSelection.Count - eliteNum; i++)
            {
                double picker = 100.0 * rand.NextDouble();
                for (int j = 0; j < forSelection.Count; j++)
                {
                    if (picker <= weights[j] && !result.Contains(forSelection[j]))
                    {
                        result.Add(forSelection[j]);
                        break;
                    }
                }
            }

            return result;
        }

        private List<Combination_Chromosome> crossover(List<Combination_Chromosome> matingPool, ref int[,] array)
        {
            List<Combination_Chromosome> children = new List<Combination_Chromosome>();
            List<Platform_Gene> platfs = new List<Platform_Gene>();

            for (int i = 0; i < eliteNum; i++)  //keep the best results
            {
                children.Add(matingPool.ElementAt(i));
            }

            for (int k = 0; k < matingPool.Count - eliteNum; k++)  //make new ones
            {
                Combination_Chromosome fst = matingPool[k];
                Combination_Chromosome scnd = matingPool[matingPool.Count - 1 - k];
                for (int i = 0; i < fst.getPlatforms().Count; i++)
                {
                    Platform_Gene plat = fst.getPlatfByIndexBeforeMid(i);
                    if (plat != null)
                        platfs.Add(plat);
                }
                for (int i = 0; i < scnd.getPlatforms().Count; i++)
                {
                    Platform_Gene plat = scnd.getPlatfByIndexAfterMid(i);
                    if (plat != null)
                        platfs.Add(plat);
                }
                children.Add(new Combination_Chromosome(platfs, ref array));
            }
            return children;
        }

        private Combination_Chromosome mutateOne(Combination_Chromosome candidate, ref int[,] array)
        {
            Random rand = new Random();
            Combination_Chromosome mutant = new Combination_Chromosome(candidate.getPlatforms(), ref array);
            bool hasChanged = false;
            for (int i = 0; i < candidate.getPlatforms().Count; i++)
            {
                if (rand.NextDouble() < mutationRate)
                {
                    hasChanged = true;

                    int platfToRemove = rand.Next(mutant.getPlatforms().Count);  //randomly select a platform to remove
                    mutant.deletePlatform(mutant.getPlatfByIndex(platfToRemove));

                    int vertPartToPlace = 0;
                    int horizSectorToPlace = 0;
                    while (true)  //randomly choose a sector to place the new platform in
                    {
                        vertPartToPlace = rand.Next(numOfVertParts);
                        horizSectorToPlace = rand.Next(numOfHorizSectors);
                        //the sector we got must have a place for the platform!
                        if (sectorPlatfCapacity[vertPartToPlace, horizSectorToPlace] != 0)
                            break;
                    }

                    int xs, xe, ys, ye;  //randomly choose a place for the platform
                    while (true)
                    {
                        //NEEDED make these 5 and 3 editable as variables
                        xs = rand.Next(vertPartToPlace * sizeOfOneVert, (vertPartToPlace + 1) * sizeOfOneVert);
                        xe = xs + (xs + 5 >= (vertPartToPlace + 1) * sizeOfOneVert ? (vertPartToPlace + 1) * sizeOfOneVert - 1 : xs + 5);
                        if (xe >= m)
                            xe = m - 1;
                        ys = rand.Next(horizSectorToPlace * sizeOfOneHoriz, (horizSectorToPlace + 1) * sizeOfOneHoriz);
                        ye = ys + (ys + 3 >= (horizSectorToPlace + 1) * sizeOfOneHoriz ? (horizSectorToPlace + 1) * sizeOfOneHoriz - 1 : ys + 3);
                        if (ye >= n)
                            ye = n - 1;
                        bool fits = true;
                        for (int k = ys; k <= ye; k++)  //if there's no obstacles in place...
                            for (int l = xs; l <= xe; l++)
                                if (array[k, l] == 0)
                                {
                                    fits = false;
                                    break;
                                }
                        if (fits)  //...then add the platform
                        {
                            mutant.addPlatform(new Platform_Gene(xs, ys, xe, ye, m / 2));
                        }
                    }
                }
            }
            if (hasChanged && mutant.getFitness(xS, yS, xE, yE) != -1.0)
                return mutant;
            else  //If the level became uncompletable or the mutation didn't work, return the original one
                return candidate;
        }

        private List<Combination_Chromosome> mutateAll(List<Combination_Chromosome> population, ref int[,] array)
        {
            List<Combination_Chromosome> mutated = new List<Combination_Chromosome>();
            for (int i = 0; i < population.Count; i++)
            {
                Combination_Chromosome mutant = mutateOne(population.ElementAt(i), ref array);
                mutated.Add(mutant);
            }
            return mutated;
        }

        private void nextGen(ref int[,] array)
        {
            individuals = individuals.OrderBy(x => x.getFitness(xS, yS, xE, yE)).ToList();
            List<Combination_Chromosome> matingPool = selection(individuals);
            List<Combination_Chromosome> children = crossover(matingPool, ref array);
            List<Combination_Chromosome> mutated = mutateAll(children, ref array);
            individuals.Clear();
            individuals.AddRange(mutated);
        }

        private void drawOnBase(ref int[,] array)
        {
            Combination_Chromosome best = individuals[0];
            for (int i = 0; i < best.getPlatforms().Count; i++)
            {
                for (int k = best.getPlatfByIndex(i).getYSt(); k < best.getPlatfByIndex(i).getYE(); k++)
                {
                    for (int l = best.getPlatfByIndex(i).getXSt(); l < best.getPlatfByIndex(i).getXE(); l++)
                    {
                        array[k, l] = 0;
                    }
                }
            }
        }

        public void run(ref int[,] array)
        {
            preparation(ref array);
            formPopulation(ref array);
            for (int i = 0; i < iters; i++)
            {
                nextGen(ref array);
            }
            individuals.OrderBy(x => x.getFitness(xS, yS, xE, yE)).ToList();
            drawOnBase(ref array);
        }
    }
}
