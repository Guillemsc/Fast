using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Presentation
{
    public class ObjectsCategoryPrefabs
    {
        private int category_index = 0;

        private Dictionary<int, MonoBehaviour> objects_prefabs = new Dictionary<int, MonoBehaviour>();

        private GameObject parent_to_set = null;

        public ObjectsCategoryPrefabs(int category_index, GameObject parent_to_set)
        {
            this.category_index = category_index;
            this.parent_to_set = parent_to_set;
        }

        public int CategoryIndex
        {
            get { return category_index; }
        }

        public GameObject ParentToSet
        {
            get { return parent_to_set; }
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
