using SkipListAlgorithmEngineering.CacheSensitiveSkipList;
using SkipListAlgorithmEngineering.DoublySkipListWithMiddleHeader;
using SkipListAlgorithmEngineering.SkipListWithCodeOptimizations;
using SkipListAlgorithmEngineering.StandartSkipList;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SkipListAlgorithmEngineering
{
    public class BenchmarkFactory
    {
        public static void ParameterBencmarkOnConventionalSkiplist()
        {

            for (int j = 0; j < 5; j++)
            {
                var skipList1 = new SkipList(4, (float)0.5);
                var skipList2 = new SkipList(8, (float)0.5);
                var r = new Random();

                for (int i = 0; i <= 50000; i++)
                {
                    skipList1.InsertElement(r.Next(0, 500));
                    skipList2.InsertElement(r.Next(0, 500));
                }
                skipList1.InsertElement(410);
                skipList2.InsertElement(410);

                Stopwatch w = new Stopwatch();

                w.Start();
                skipList1.SearchElement(410);
                w.Stop();

                Console.WriteLine(j + 1 + ". trial, operation takes " + w.Elapsed + " time with 5 layer");

                Stopwatch w2 = new Stopwatch();

                w2.Start();
                skipList2.SearchElement(410);
                w2.Stop();

                Console.WriteLine(j + 1 + ". trial, operation takes " + w2.Elapsed + " time with 9 layer");
            }
        }
        public static void CodeOptimizedSkipListvsConvetionalSkipList()
        {

            Console.WriteLine("Measurement with 150000 element and 250 layer wtih P=0.8");
            for (int j = 0; j < 5; j++)
            {
                CodeOptimizedSkipList skipList = new CodeOptimizedSkipList(250, (float)0.8);
                CodeOptimizedSkipList skipList2 = new CodeOptimizedSkipList(250, (float)0.8);
                Random r = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";


                for (int i = 0; i <= 150000; i++)
                {
                    var element = (new string(Enumerable.Repeat(chars, 20).Select(s => s[r.Next(s.Length)]).ToArray()));
                    Random _random = new Random();

                    int rlevel = _random.Next(0, 250);
                    skipList.InsertElement(element, rlevel);
                    skipList2.InsertElement(element, rlevel);
                }

                skipList.InsertElement("zrtyhfsdghjkmnhtreqs", 248);
                skipList2.InsertElement("zrtyhfsdghjkmnhtreqs", 248);

                Stopwatch watch1 = new Stopwatch();

                watch1.Start();
                skipList.SearchElementWithCodeOptimizedSearch("zrtyhfsdghjkmnhtreqs");
                watch1.Stop();

                Console.WriteLine(j + 1 + ". trial, operation takes " + watch1.Elapsed + " time with optimized skip list");

                Stopwatch watch2 = new Stopwatch();
                watch2.Start();
                skipList2.SearchElementWithConventionalSearch("zrtyhfsdghjkmnhtreqs");
                watch2.Stop();

                Console.WriteLine(j + 1 + ". trial, operation takes " + watch2.Elapsed + " time with normal skip list");

                Console.WriteLine();
                Console.WriteLine();
            }

        }
        public static void DoublyMiddleSkipListvsConvetionalSkipList()
        {
            Console.WriteLine("Measurement with 500000 element and 50 layer wtih P=0.6");
            for (int j = 0; j < 5; j++)
            {
                var skipList = new DoublySkipListWithMiddleDestination(50, (float)0.6);
                var r = new Random();

                for (int i = 0; i <= 500000; i++)
                {
                    var number = r.Next(0, 500);
                    skipList.InsertElement(number);
                }

                skipList.InsertElement(486);
                skipList.AddMiddleDestination();

                Stopwatch watch1 = new Stopwatch();

                watch1.Start();
                skipList.SearchElementWithConventionalSearch(486);
                watch1.Stop();

                Console.WriteLine(j + 1 + ". trial, operation takes " + watch1.Elapsed + " time wtih normal skip list");

                Stopwatch watch2 = new Stopwatch();

                watch2.Start();

                skipList.SearchStartingMiddleElementOptimized(486);

                watch2.Stop();

                Console.WriteLine(j + 1 + ". trial, operation takes " + watch2.Elapsed + " time with optimized skip list");

                Console.WriteLine();
                Console.WriteLine();
            }


        }
        public static void CacheSensitiveSkipListvsConventionalSkipList()
        {

            for (int j = 0; j < 5; j++)
            {
                SkipList skipList = new SkipList(10, (float)0.5);
                CCSL _cacheSensitive = new CCSL(10, 5);

                var r = new Random();

                for (int i = 0; i <= 500; i++)
                {
                    skipList.InsertElement(r.Next(0, 500));
                    _cacheSensitive.InsertElement(r.Next(0, 500));
                }


                skipList.InsertElement(476);
                _cacheSensitive.InsertElement(476);

                Stopwatch watch1 = new Stopwatch();

                watch1.Start();
                skipList.SearchElement(476);
                watch1.Stop();


                Console.WriteLine(j + 1 + ". trial, operation takes " + watch1.Elapsed + " time with conventional skip list");

                Stopwatch watch2 = new Stopwatch();
                watch2.Start();
                _cacheSensitive.OptimizedSearch(476);
                watch2.Stop();

                Console.WriteLine(j + 1 + ". trial, operation takes " + watch2.Elapsed + " time with cache sensitive skip list");

                Console.WriteLine();
                Console.WriteLine();
            }
        }

    }
}
