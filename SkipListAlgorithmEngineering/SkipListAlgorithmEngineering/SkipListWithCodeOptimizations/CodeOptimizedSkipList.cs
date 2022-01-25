using System;
using System.Collections.Generic;
using System.Text;

namespace SkipListAlgorithmEngineering.SkipListWithCodeOptimizations
{
    public class Node
    {
        public string Value { get; set; }
        public Node[] Next { get; set; }
        public int[] fDistance { get; set; }

        public Node(string value, int level)
        {
            Value = value;
            Next = new Node[level + 1];
        }
    }
    public class CodeOptimizedSkipList
    {
        public int MaxLevel { get; set; }
        public float P { get; set; }
        public int Level { get; set; }
        public int Size { get; set; } = 0;
        public Node Header;

        public CodeOptimizedSkipList(int maxLevel, float p)
        {
            MaxLevel = maxLevel;
            P = p;
            Header = new Node("*", maxLevel);
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

        public void InsertElement(string value, int level)
        {
            Node current = Header;
            Node[] update = new Node[MaxLevel + 1];

            for (int i = Level; i >= 0; i--)
            {
                while (current.Next[i] != null && string.Compare(current.Next[i].Value, value) == -1)
                {
                    current = current.Next[i];
                }

                update[i] = current;
            }

            int rlevel = level;

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
                update[i].Next[i] = newNode;
            }


        }

        public void SearchElementWithCodeOptimizedSearch(string value)
        {
            Node current = Header;
            Node alreadyChecked = null;

            for (int i = Level; i >= 0;)
            {

                while (current.Next[i] != null && current.Next[i] != alreadyChecked && string.Compare(current.Next[i].Value, value) == -1)
                {
                    current = current.Next[i];
                }

                alreadyChecked = current.Next[i];
                i -= ((i - 1 >= 0) && current.Next[i].Value.Equals(current.Next[i - 1].Value)) ? 2 : 1;
            }

            if (current.Next[0].Value == value)
            {
                //Console.WriteLine("Found key: " + value + "");
            }

        }

        public void SearchElementWithConventionalSearch(string value)
        {
            Node current = Header;

            for (int i = Level; i >= 0; i--)
            {

                while (current.Next[i] != null && string.Compare(current.Next[i].Value, value) == -1)
                {
                    current = current.Next[i];
                }
            }

            if (current.Next[0] != null && current.Next[0].Value == value)
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
