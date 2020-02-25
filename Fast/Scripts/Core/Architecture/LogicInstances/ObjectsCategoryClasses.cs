using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Logic
{
    public class ObjectsCategoryClasses
    {
        private int category_index = 0;

        private Dictionary<int, Type> objects_classes = new Dictionary<int, Type>();

        public ObjectsCategoryClasses(int category_index)
        {
            this.category_index = category_index;
        }

        public int CategoryIndex
        {
            get { return category_index; }
        }

        public void AddObjectClass(int category_variation, Type object_type)
        {
            objects_classes.Add(category_variation, object_type);
        }

        public Type GetObjectClass(int category_variation)
        {
            Type ret = null;

            objects_classes.TryGetValue(category_variation, out ret);

            return ret;
        }
    }
}
