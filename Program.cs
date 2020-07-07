using System;
using System.IO;

namespace TwoSatSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            TwoSat twoSat = new TwoSat();
            twoSat.ReadFile();
            string opt;
            do
            {
                Console.WriteLine("Press 1 for backtracking.");
                Console.WriteLine("Press 2 for Simulated Annealing.");
                opt = Console.ReadLine();
                if (opt.Equals("1"))
                {
                    bruteForce(twoSat);
                }
                else if (opt.Equals("2"))
                {
                    SA(twoSat);
                }
                Console.WriteLine("Do you wish to try with another algorithm?(y/n)");
                opt = Console.ReadLine();
            } while (opt.Equals("y"));
        }
            

        static void bruteForce(TwoSat twoSat)
        {
            bool isSat;
            int count;
            TimeSpan timeSpan;
            int[] solution = twoSat.FindSolutionBacktracking(out isSat, out count, out timeSpan);
            outputData(isSat, solution, timeSpan);
        }
        static void SA(TwoSat twoSat)
        {
            bool isSat;
            int count;
            TimeSpan timeSpan;
            int[] solution = twoSat.SimulatedAnnealing(out isSat, out count, out timeSpan);
            outputData(isSat, solution, timeSpan);
        }
        static void outputData(bool isSat, int[] solution, TimeSpan ts)
        {
            using (StreamWriter stream = new StreamWriter(@"C:\Tema_ACC\twoSatOut.txt"))
            {
                if (!isSat)
                {
                    stream.WriteLine("Unsatisfiable");
                    Console.WriteLine("Unsatisfiable!");
                }
                else
                {
                    Console.WriteLine("Satisfiable");
                    stream.WriteLine("Satisfiable");
                    for (int i = 0; i < solution.Length; i++)
                    {
                        int x = 0;
                        if (solution[i] == 0)
                            x = (i + 1) * -1;
                        else
                            x = i + 1;
                        stream.Write(x + ", ");
                        Console.Write(x + ", ");
                    }
                    Console.WriteLine("\nTime: "+ ts);
                }
            }
        }
    }
}
