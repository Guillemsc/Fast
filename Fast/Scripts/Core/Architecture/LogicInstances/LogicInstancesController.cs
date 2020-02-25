using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Fast.Logic
{
    public class LogicInstancesController
    {
        private List<ObjectsCategoryClasses> classes = new List<ObjectsCategoryClasses>();

        private List<ObjectsCategoryInstances> instances = new List<ObjectsCategoryInstances>();

        public void AddClassCategory(ObjectsCategoryClasses category)
        {
            classes.Add(category);
        }

        public ObjectsCategoryClasses GetClassCategory(int category_index)
        {
            ObjectsCategoryClasses ret = null;

            for (int i = 0; i < classes.Count; ++i)
            {
                ObjectsCategoryClasses curr_category = classes[i];

                if (curr_category.CategoryIndex == category_index)
                {
                    ret = curr_category;

                    break;
                }
            }

            return ret;
        }

        public ObjectsCategoryInstances GetInstancesCategory(int category_index)
        {
            ObjectsCategoryInstances ret = null;

            for (int i = 0; i < instances.Count; ++i)
            {
                ObjectsCategoryInstances curr_category = instances[i];

                if (curr_category.CategoryIndex == category_index)
                {
                    ret = curr_category;

                    break;
                }
            }

            return ret;
        }

        public Type GetObjectClass(int category_index, int category_variation_index) 
        {
            Type ret = default(Type);

            ObjectsCategoryClasses category = GetClassCategory(category_index);

            if (category != null)
            {
                ret = category.GetObjectClass(category_variation_index);
            }

            return ret;
        }

        public T SpawnObject<T>(int category_index, int category_variation_index) where T : class
        {
            T ret = default(T);

            Type class_type = GetObjectClass(category_index, category_variation_index);

            if (class_type != null)
            {
                object instance = (T)Activator.CreateInstance(class_type);

                ret = (T)instance;

                ObjectsCategoryInstances category_to_add_at = GetInstancesCategory(category_index);

                if (category_to_add_at == null)
                {
                    category_to_add_at = new ObjectsCategoryInstances(category_index);

                    instances.Add(category_to_add_at);
                }

                category_to_add_at.AddObjectInstance(category_variation_index, ret);
            }

            return ret;
        }

        public List<T> SpawnObjects<T>(int category_index, int category_variation_index, int ammount) where T : class
        {
            List<T> ret = new List<T>();

            for (int i = 0; i < ammount; ++i)
            {
                T instance = SpawnObject<T>(category_index, category_variation_index);

                if (instance != null)
                {
                    ret.Add(instance);
                }
            }

            return ret;
        }

        public ReadOnlyCollection<T> GetSpawnedObjects<T>(int category_index, int category_variation_index) where T : class
        {
            ReadOnlyCollection<T> ret = null;

            ObjectsCategoryInstances category_to_get_from = GetInstancesCategory(category_index);

            if (category_to_get_from != null)
            {
                ret = category_to_get_from.GetObjectsInstances<T>(category_variation_index);
            }

            return ret;
        }

        public ReadOnlyCollection<T> GetSpawnedObjects<T>(int category_index) where T : class
        {
            ReadOnlyCollection<T> ret = null;

            ObjectsCategoryInstances category_to_get_from = GetInstancesCategory(category_index);

            if (category_to_get_from != null)
            {
                ret = category_to_get_from.GetAllObjectsInstances<T>();
            }

            return ret;
        }

        public void DespawnObject(int category_index, int category_variation_index, object instance)
        {
            if (instance != null)
            {
                ObjectsCategoryInstances category_to_remove_from = GetInstancesCategory(category_index);

                if (category_to_remove_from != null)
                {
                    category_to_remove_from.RemoveObjectInstance(category_variation_index, instance);
                }
            }
        }

        public void DespawnObjects(int category_index, int category_variation_index, List<object> instances)
        {
            for (int i = 0; i < instances.Count; ++i)
            {
                object curr_instance = instances[i];

                DespawnObject(category_index, category_variation_index, curr_instance);
            }
        }

        public void DespawnAllObjects(int category_index, int category_variation_index)
        {
            ObjectsCategoryInstances category_to_remove_from = GetInstancesCategory(category_index);

            if (category_to_remove_from != null)
            {
                category_to_remove_from.ClearObjectInstances(category_variation_index);
            }
        }

        public void DespawnAllObjects(int category_index)
        {
            ObjectsCategoryInstances category_to_remove_from = GetInstancesCategory(category_index);

            if (category_to_remove_from != null)
            {
                category_to_remove_from.ClearObjectInstances();
            }
        }

        public void DespawnAllObjects()
        {
            for (int i = 0; i < instances.Count; ++i)
            {
                ObjectsCategoryInstances curr_category = instances[i];

                DespawnAllObjects(curr_category.CategoryIndex);
            }
        }
    }
}
