using System;

namespace SkipListAlgorithmEngineering
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Which benchmark would you want to see?");
            Console.WriteLine("PRESS 1 for Skip list itself parameter changing benchmark PRESS");
            Console.WriteLine("PRESS 2 for Code optimized skip list(comparison number reducing and go down two by two) vs conventional skip list");
            Console.WriteLine("PRESS 3 for Doubly skip list with middle destination vs conventional skip list");
            Console.WriteLine("PRESS 4 for Cache sensitive skip list vs conventional skip list");
            int operation = Convert.ToInt32(Console.ReadLine());

            switch (operation)
            {
                case 1:
                    BenchmarkFactory.ParameterBencmarkOnConventionalSkiplist();
                    break;
                case 2:
                    BenchmarkFactory.CodeOptimizedSkipListvsConvetionalSkipList();
                    break;
                case 3:
                    BenchmarkFactory.DoublyMiddleSkipListvsConvetionalSkipList();
                    break;
                case 4:
                    BenchmarkFactory.CacheSensitiveSkipListvsConventionalSkipList();
                    break;
            }


            Console.ReadKey();
        }
    }
}
