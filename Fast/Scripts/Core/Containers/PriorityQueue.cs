using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Containers
{
    public class PriorityQueue<T>
    {
        List<Item> list = new List<Item>();

        public class Item
        {
            public Item(T _element, float _priority)
            {
                element = _element;
                priority = _priority;
            }

            public T element;
            public float priority = 0;
        }

        public void Add(T element, float priority)
        {
            bool added = false;

            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i].priority > priority)
                {
                    list.Insert(i, new Item(element, priority));

                    added = true;

                    break;
                }
            }

            if (!added)
            {
                list.Insert(list.Count, new Item(element, priority));
            }
        }

        public T PopBack()
        {
            T ret = list[list.Count - 1].element;

            list.RemoveAt(list.Count - 1);

            return ret;
        }

        public T PopFront()
        {
            T ret = list[0].element;

            list.RemoveAt(0);

            return ret;
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public T At(int index)
        {
            return list[index].element;
        }
    };
}
