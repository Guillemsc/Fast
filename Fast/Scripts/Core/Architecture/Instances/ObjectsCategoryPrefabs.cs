using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Instances
{
    public class ObjectsCategoryPrefabs
    {
        private int category_index = 0;

        private Dictionary<int, MonoBehaviour> objects_prefabs = new Dictionary<int, MonoBehaviour>();

        public ObjectsCategoryPrefabs(int category_index)
        {
            this.category_index = category_index;
        }

        public int CategoryIndex
        {
            get { return category_index; }
        }

        public void AddObjectPrefab(int category_variation, MonoBehaviour object_prefab)
        {
            if (object_prefab != null)
            {
                objects_prefabs.Add(category_variation, object_prefab);
            }
        }

        public MonoBehaviour GetObjectPrefab(int category_variation)
        {
            MonoBehaviour ret = null;

            objects_prefabs.TryGetValue(category_variation, out ret);

            return ret;
        }
    }
}
