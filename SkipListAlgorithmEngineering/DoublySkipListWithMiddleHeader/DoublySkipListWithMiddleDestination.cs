using System;
using System.Collections.Generic;
using System.Text;

namespace SkipListAlgorithmEngineering.DoublySkipListWithMiddleHeader
{
    public class Node
    {
        public int Value { get; set; }
        public Node[] Next { get; set; }
        public Node[] Back { get; set; }

        public Node(int value, int level)
        {
            Value = value;
            Next = new Node[level + 1];
            Back = new Node[level + 1];
        }
    }
    public class DoublySkipListWithMiddleDestination
    {
        public int MaxLevel { get; set; }
        public float P { get; set; }
        public int Level { get; set; }

        public Node Header;
        public Node Middle;

        public int Size = 0;

        public DoublySkipListWithMiddleDestination(int maxLevel, float p)
        {
            MaxLevel = maxLevel;
            P = p;
            Header = new Node(-1, maxLevel);
        }

        public int RandomLevel()
        {

            Random _random = new Random();
            float r = (float)_random.NextDouble();
            int lvl = 0;

            while (r < P && lvl < MaxLevel)
            {
                lvl++;
                r = (float)_random.NextDouble();
            }

            return lvl;
        }
        public void InsertElement(int value)
        {
            Node current = Header;
            Node[] update = new Node[MaxLevel + 1];

            for (int i = Level; i >= 0; i--)
            {
                while (current.Next[i] != null && current.Next[i].Value < value)
                {
                    current = current.Next[i];
                }

                update[i] = current;
            }


            int rlevel = RandomLevel();

            if (rlevel > Level)
            {
                for (int i = Level + 1; i <= rlevel; i++)
                {
                    update[i] = Header;
                }

                Level = rlevel;
            }

            var newNode = new Node(value, rlevel);

            for (int i = 0; i <= rlevel; i++)
            {
                newNode.Next[i] = update[i].Next[i];

                if (update[i].Next[i] != null)
                {
                    update[i].Next[i].Back[i] = newNode;
                }


                update[i].Next[i] = newNode;
                newNode.Back[i] = update[i];
            }

            Size++;

        }
        public void AddMiddleDestination()
        {
            Node currentMiddle = Header;
            var middlePoint = Size / 2;
            int inner = 0;
            for (int i = Level; i >= 0; i--)
            {
                while (currentMiddle.Next[0] != null && inner < middlePoint)
                {
                    currentMiddle = currentMiddle.Next[0];
                    inner++;
                }
            }

            int mockValue = currentMiddle.Value + 1;

            Node current = Header;
            Node[] update = new Node[MaxLevel + 1];
            for (int i = Level; i >= 0; i--)
            {
                while (current.Next[i] != null && current.Next[i].Value < mockValue)
                {
                    current = current.Next[i];
                }

                update[i] = current;
            }

            var newNode = new Node(-1, Level);

            for (int i = 0; i <= Level; i++)
            {
                newNode.Next[i] = update[i].Next[i];
                update[i].Next[i] = newNode;
                newNode.Back[i] = update[i];
            }

            Middle = newNode;
            Size++;
        }
        public void SearchElementWithConventionalSearch(int value)
        {
            Node current = Header;

            for (int i = Level; i >= 0; i--)
            {

                while (current.Next[i] != null && current.Next[i].Value < value)
                {
                    current = current.Next[i];
                }

            }

            current = current.Next[0];

            if (current != null && current.Value == value)
            {
                //Console.WriteLine("Found key: " + value + "");
            }

        }
        public void SearchStartingMiddleElementOptimized(int value)
        {
            Node current = Middle;
            if (value >= current.Next[0].Value)
            {
                for (int i = Level; i >= 0; i--)
                {

                    while (current.Next[i] != null && current.Next[i].Value < value)
                    {
                        current = current.Next[i];
                    }
                }
                current = current.Next[0];
            }
            if (value <= current.Back[0].Value)
            {
                for (int i = Level; i >= 0; i--)
                {

                    while (current.Back[i] != null && current.Back[i].Value > value)
                    {
                        current = current.Back[i];
                    }

                }
                current = current.Back[0];
            }

            if (current != null && current.Value == value)
            {
                //Console.WriteLine("Found key: " + value + "");
            }
        }
        public void DisplaySkipList()
        {
            Console.WriteLine("*** Skip List***");

            for (int i = 0; i <= Level; i++)
            {
                var node = Header.Next[i];
                Console.Write("Level " + i + " -- ");

                while (node != null)
                {
                    Console.Write(" " + node.Value + " ");
                    node = node.Next[i];

                }
                Console.WriteLine();
            }
        }

    }
}
