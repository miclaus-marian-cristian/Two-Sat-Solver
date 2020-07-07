using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TwoSatSolver
{
    class TwoSat
    {
        int V;//the number of variables
        int C; // the number of clauses
        int[,] clauses;

        public void ReadFile()
        {
            List<string> lines = new List<string>();
            string line;
            using (StreamReader reader = new StreamReader(@"C:\twoSatIn.txt"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            string[] arr = lines[0].Split(" ");
            V = int.Parse(arr[0]);
            C = int.Parse(arr[1]);
            clauses = new int[C, V];
            for (int i = 0, k = 1; i < C; i++, k++)
            {
                arr = lines[k].Split(" ");
                foreach (string item in arr)
                {
                    int x = int.Parse(item);
                    int pos = Math.Abs(x) - 1;
                    clauses[i, pos] = x;
                }
            }
        }
        public int[] FindSolutionBacktracking(out bool isSat, out int count,out TimeSpan ts)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            isSat = false;
            count = 0;
            int[] permutation = new int[V];
            for (int i = 0; i < V; i++)
                permutation[i] = -1;

            int k = 0;
            while(k > -1)
            {
                bool found = true;
                if(permutation[k]<2)
                {
                    permutation[k]++;
                    if(permutation[k] == 2)
                    {
                        found = false;
                    }
                }
                if(found)
                {
                    if(k == V-1)
                    {
                        //verify satisfiability
                        count = verifySatisfiability(permutation, out isSat);
                        if(isSat)
                        {
                            stopwatch.Stop();
                            ts = stopwatch.Elapsed;
                            return permutation;
                        }
                    }
                    else
                    {
                        k++;
                        permutation[k] = -1;
                    }
                }
                else
                {
                    k--;
                }
            }
            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            return null;
        }

        public int[] SimulatedAnnealing(out bool isSat, out int count, out TimeSpan ts)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int tries = 0;
            double tMax = 0.30;
            double tMin = 0.01;
            double T = 0;
            int[] permutation;
            count = 0;
            double r = 0.8;
            isSat = false;
            int maxTries = 5;
            do
            {
                permutation = generateRandomPermutation();
                int j = 0;
                do
                {
                    count = verifySatisfiability(permutation, out isSat);
                    if (isSat)
                    {
                        stopwatch.Stop();
                        ts = stopwatch.Elapsed;
                        return permutation;
                    }
                    T = tMax * Math.Exp(-j * r);
                    Random rand = new Random();
                    for (int k = 0; k < V; k++)
                    {
                        int[] x = new int[V];
                        permutation.CopyTo(x, 0);
                        int val = x[k];
                        if (val == 0)
                            x[k] = 1;
                        else
                            x[k] = 0;
                        double delta = verifySatisfiability(x) - verifySatisfiability(permutation);
                        double expr = Math.Pow((1 + Math.Exp(-delta / T)), -1);
                        if (rand.NextDouble() < expr)
                        {
                            if (val == 0)
                            {
                                permutation[k] = 1;
                            }
                            else
                                permutation[k] = 0;
                        }
                    }
                    j++;

                } while (T >= tMin);
                tries++;
            } while (tries<=maxTries);
            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            return permutation;
        }

        private int verifySatisfiability(int[] permutation, out bool isSat)
        {
            int count = 0;
            isSat = false;
            bool clauseSat = false;
            for(int i = 0; i < C; i++)
            {
                clauseSat = false;
                for(int j = 0; j < V; j++)
                {
                    if(clauseSat == false)
                    {
                        int x = clauses[i, j];
                        if (x == 0)
                            continue;
                        else if (x < 0)
                        {
                            if (permutation[j] == 0)
                            {
                                count++;
                                clauseSat = true;
                            }

                        }
                        else
                        {
                            if (permutation[j] == 1)
                            {
                                count++;
                                clauseSat = true;
                            }

                        }
                    }
                }
            }
            if (count == C)
                isSat = true;

            return count;
        }
        private int verifySatisfiability(int[] permutation)
        {
            int count = 0;
            for (int i = 0; i < C; i++)
            {
                for (int j = 0; j < V; j++)
                {
                    int x = clauses[i, j];
                    if (x == 0)
                        continue;
                    else if (x < 0)
                    {
                        if (permutation[j] == 0)
                            count++;
                    }
                    else
                    {
                        if (permutation[j] == 1)
                            count++;
                    }
                }
            }

            return count;
        }

        private int[] generateRandomPermutation()
        {
            Random random = new Random();
            int[] perm = new int[V];
            for(int i = 0; i < V; i++)
            {
                perm[i] = random.Next(2);
            }
            return perm;
        }
    }
}
