using System;
using System.Collections.Generic;
using System.Text;

namespace SkipListAlgorithmEngineering.CacheSensitiveSkipList
{
    public class _CSSL_DataNode
    {
        public int key { get; set; }
        public _CSSL_DataNode Next { get; set; }

        public _CSSL_DataNode(int value, _CSSL_DataNode node)
        {
            key = value;
            Next = node;

        }
    }
    public class _CSSL_ProxyNode
    {
        public int[] keys;
        public _CSSL_DataNode[] pointers;
        public bool updated;

        public _CSSL_ProxyNode()
        {
            keys = new int[5];
            pointers = new _CSSL_DataNode[5];
        }
    }
    class CCSL
    {
        public int max_level;
        int skip;
        int num_elements;
        public int[] items_per_level;
        public int[] flane_items;
        public int[] starts_of_flanes;
        public int[] flanes;
        public _CSSL_ProxyNode[] flane_pointers;
        _CSSL_DataNode head, tail;

        public CCSL(int maxLevel, int _skip)
        {
            max_level = maxLevel;
            num_elements = 0;
            head = new _CSSL_DataNode(0, null);
            tail = head;
            skip = _skip > 1 ? _skip : 2;
            items_per_level = new int[maxLevel];
            starts_of_flanes = new int[maxLevel];
            flane_items = new int[maxLevel];

            for (int level = 0; level < maxLevel; level++)
                flane_items[level] = 0;

            BuildFastLanes();
        }
        public _CSSL_ProxyNode NewProxyNode(_CSSL_DataNode node)
        {
            _CSSL_ProxyNode proxy = new _CSSL_ProxyNode();
            proxy.keys[0] = node.key;
            proxy.updated = false;

            for (int i = 1; i < skip; i++)
                proxy.keys[i] = int.MaxValue;
            proxy.pointers[0] = node;

            return proxy;
        }
        public void FindAndInsertIntoProxyNode(_CSSL_DataNode node)
        {
            _CSSL_ProxyNode proxy = flane_pointers[flane_items[0] - 1];

            for (int i = 1; i < skip; i++)
            {
                if (proxy.keys[i] == int.MaxValue)
                {
                    proxy.keys[i] = node.key;
                    proxy.pointers[i] = node;
                    return;
                }
            }
        }
        public void InsertElement(int key)
        {
            _CSSL_DataNode new_node = new _CSSL_DataNode(key, null);
            bool nodeInserted = true;
            bool flaneInserted = false;

            //EMIN DEGILIM
            // add new node at the end of the data list
            tail.Next = new_node;
            tail = new_node;

            // add key to fast lanes
            for (int level = 0; level < max_level; level++)
            {
                if (num_elements % Math.Pow(skip, (level + 1)) == 0 && nodeInserted)
                {  //EMIN DEGILIM
                    nodeInserted = InsertItemIntoFastLane(level, new_node) != 0;
                }
                else
                {
                    break;
                }

                flaneInserted = true;
            }

            if (!flaneInserted)
                FindAndInsertIntoProxyNode(new_node);

            num_elements++;

            if (num_elements % (16 * Math.Pow(skip, max_level)) == 0)
                ResizeFastLanes();
        }
        public int InsertItemIntoFastLane(int level, _CSSL_DataNode newNode)
        {
            int curPos = starts_of_flanes[level] + flane_items[level];
            int levelLimit = curPos + items_per_level[level];

            if (curPos > levelLimit)
            {
                curPos = levelLimit;
            }

            while (newNode.key > flanes[curPos] && curPos < levelLimit)
                curPos++;

            if (flanes[curPos] == int.MaxValue)
            {
                flanes[curPos] = newNode.key;
                if (level == 0)
                    flane_pointers[curPos - starts_of_flanes[0]] = NewProxyNode(newNode);
                flane_items[level]++;
            }
            else
            {
                return int.MaxValue;
            }

            return curPos;
        }
        public void BuildFastLanes()
        {
            int flane_size = 16;
            items_per_level[max_level - 1] = flane_size;
            starts_of_flanes[max_level - 1] = 0;

            // calculate level sizes level by level
            for (int level = max_level - 2; level >= 0; level--)
            {
                items_per_level[level] = items_per_level[level + 1] * skip;
                starts_of_flanes[level] = starts_of_flanes[level + 1] + items_per_level[level + 1];
                flane_size += items_per_level[level];
            }

            flanes = new int[flane_size];
            for (int i = 0; i < flane_size; i++)
            {
                flanes[i] = int.MaxValue;
            }
            flane_pointers = new _CSSL_ProxyNode[items_per_level[0]];
            for (int i = 0; i < items_per_level[0]; i++)
            {
                flane_pointers[i] = null;
            }



        }
        public void ResizeFastLanes()
        {
            int new_size = items_per_level[max_level - 1] + 16;
            int[] level_items = new int[max_level];
            int[] level_starts = new int[max_level];

            level_items[max_level - 1] = new_size;
            level_starts[max_level - 1] = 0;

            for (int level = max_level - 2; level >= 0; level--)
            {
                level_items[level] = level_items[level + 1] * skip;
                level_starts[level] = level_starts[level + 1] + level_items[level + 1];
                new_size += level_items[level];
            }

            int[] new_flanes = new int[new_size];
            var new_fpointers = new _CSSL_ProxyNode[level_items[0]];

            for (int i = flane_items[max_level - 1]; i < new_size; i++)
            {
                new_flanes[i] = int.MaxValue;
            }

            //EMIN DEGILIM
            for (int level = max_level - 1; level >= 0; level--)
            {
                new_flanes[level_starts[level]] = flanes[starts_of_flanes[level]];
            }


            flanes = new_flanes;
            flane_pointers = new_fpointers;
            items_per_level = level_items;
            starts_of_flanes = level_starts;
        }
        public int OptimizedSearch(int key)
        {
            int curPos = 0;
            int first = 0;
            int last = items_per_level[max_level - 1] - 1;
            int middle = 0;

            // scan highest fast lane with binary search
            while (first < last)
            {
                middle = (first + last) / 2;

                if (flanes[middle] < key)
                {
                    first = middle + 1;
                }
                else if (flanes[middle] == key)
                {
                    curPos = middle;
                    break;
                }
                else
                {
                    last = middle;
                }

            }

            if (first > last)
                curPos = last;

            int level;
            // traverse over fast lanes
            for (level = max_level - 1; level >= 0; level--)
            {
                int rPos = curPos - starts_of_flanes[level];

                while (rPos < items_per_level[level] && key >= flanes[++curPos])
                {
                    rPos++;
                }
                if (level == 0)
                    break;
                curPos = starts_of_flanes[level - 1] + rPos * skip;

            }

            if (key == flanes[--curPos])
            {
                return key;
            }

            var proxy = flane_pointers[curPos - starts_of_flanes[0]];
            for (int i = 1; i < skip; i++)
            {
                if (proxy.keys[i] == key)
                {

                    return key;

                }

            }

            return int.MaxValue;
        }

    }
}
