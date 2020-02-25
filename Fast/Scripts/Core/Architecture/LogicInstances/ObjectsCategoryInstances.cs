﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Logic
{
    public class ObjectsCategoryInstances
    {
        private int category_index = 0;

        private Dictionary<int, List<object>> objects_instances = new Dictionary<int, List<object>>();

        public ObjectsCategoryInstances(int category_index)
        {
            this.category_index = category_index;
        }

        public int CategoryIndex
        {
            get { return category_index; }
        }

        public void AddObjectInstance(int category_variation, object object_instance)
        {
            if (object_instance != null)
            {
                List<object> instances_type = null;

                bool found = objects_instances.TryGetValue(category_variation, out instances_type);

                if (!found)
                {
                    instances_type = new List<object>();

                    objects_instances.Add(category_variation, instances_type);
                }

                instances_type.Add(object_instance);
            }
        }

        public ReadOnlyCollection<T> GetObjectsInstances<T>(int category_variation) where T : class
        {
            ReadOnlyCollection<T> ret = null;

            List<object> instances_list = new List<object>();

            bool found = objects_instances.TryGetValue(category_variation, out instances_list);

            List<T> instances_list_cast = instances_list.Cast<T>().ToList();

            ret = instances_list_cast.AsReadOnly();

            return ret;
        }

        public ReadOnlyCollection<T> GetAllObjectsInstances<T>() where T : class
        {
            ReadOnlyCollection<T> ret = null;

            List<T> instances_list = new List<T>();

            foreach (KeyValuePair<int, List<object>> entry in objects_instances)
            {
                List<T> instances_list_cast = entry.Value.Cast<T>().ToList();

                instances_list.AddRange(instances_list_cast);
            }

            ret = instances_list.AsReadOnly();

            return ret;
        }

        public bool RemoveObjectInstance(int category_variation, object object_instance)
        {
            bool ret = false;

            if (object_instance != null)
            {
                List<object> instances_type = null;

                bool found = objects_instances.TryGetValue(category_variation, out instances_type);

                if (!found)
                {
                    for (int i = 0; i < instances_type.Count; ++i)
                    {
                        object curr_instance = instances_type[i];

                        if (curr_instance == object_instance)
                        {
                            instances_type.RemoveAt(i);

                            ret = true;

                            break;
                        }
                    }
                }
            }

            return ret;
        }

        public void ClearObjectInstances(int category_variation)
        {
            List<object> category_instances = null;

            objects_instances.TryGetValue(category_variation, out category_instances);

            if (category_instances != null)
            {
                category_instances.Clear();
            }
        }

        public void ClearObjectInstances()
        {
            objects_instances.Clear();
        }
    }
}
