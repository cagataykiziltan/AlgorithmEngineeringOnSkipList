using System;
namespace SkipListAlgorithmEngineering.StandartSkipList
{
    public class Node
    {
        public int Value { get; set; }
        public Node[] Next { get; set; }
        public Node(int value, int level)
        {
            Value = value;
            Next = new Node[level + 1];

        }
    }
    public class SkipList
    {
        public int MaxLevel { get; set; }
        public float P { get; set; }
        public int Level { get; set; }
        public Node Header;
        public int Size = 0;
        public SkipList(int maxLevel, float p)
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
                update[i].Next[i] = newNode;

            }

            //Console.WriteLine("Successfully Inserted key " + value + "\n");
            Size++;


        }
        public void DeleteElement(int value)
        {
            Node current = Header;
            Node[] update = new Node[MaxLevel + 1];

            //silinecek elemandan önceki tüm node bağlantilarini bul
            for (int i = Level; i >= 0; i--)
            {
                while (current.Next[i] != null && current.Next[i].Value < value)
                {
                    current = current.Next[i];
                }

                update[i] = current;
            }

            //artik current sileceğimiz node
            current = current.Next[0];

            if (current != null && current.Value == value)
            {
                for (int i = 0; i <= Level; i++)
                {
                    //o baglantinin ilerisi currenta eşit değilse o seviyede silme çünkü top'ı senin kadar yüksek değildir.
                    if (update[i].Next[i] != current)
                    {
                        break;
                    }

                    //diğer türlüyse sil gitsin yani baglanti koparmak yeterli. Bu noktada current sileceğğimiz node
                    update[i].Next[i] = current.Next[i];
                }

                while (Level > 0 && Header.Next[Level] == null)
                {
                    Level--;
                }

                Console.WriteLine("Successfully deleted key " + value + "");
            }

        }
        public void SearchElement(int value)
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
                Console.WriteLine("Found key: " + value + "");
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
