using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Fast.Presentation
{
    public class PresentationInstancesController
    {
        private List<ObjectsCategoryPrefabs> prefabs = new List<ObjectsCategoryPrefabs>();

        private List<ObjectsCategoryInstances> instances = new List<ObjectsCategoryInstances>();

        public void AddPrefabCategory(ObjectsCategoryPrefabs category)
        {
            prefabs.Add(category);
        }

        public ObjectsCategoryPrefabs GetPrefabsCategory(int category_index)
        {
            ObjectsCategoryPrefabs ret = null;

            for(int i = 0; i < prefabs.Count; ++i)
            {
                ObjectsCategoryPrefabs curr_category = prefabs[i];

                if(curr_category.CategoryIndex == category_index)
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

        public T GetObjectPrefab<T>(int category_index, int category_variation_index, out GameObject parent_to_set) where T : MonoBehaviour
        {
            T ret = default(T);

            parent_to_set = null;

            ObjectsCategoryPrefabs category = GetPrefabsCategory(category_index);

            if(category != null)
            {
                MonoBehaviour prefab = category.GetObjectPrefab(category_variation_index);

                if(prefab != null)
                {
                    parent_to_set = category.ParentToSet;

                    ret = prefab as T;
                }
            }

            return ret;
        }

        public T SpawnObject<T>(int category_index, int category_variation_index) where T : MonoBehaviour
        {
            T ret = default(T);

            GameObject parent_to_set = null;

            T prefab = GetObjectPrefab<T>(category_index, category_variation_index, out parent_to_set);

            if(prefab != null)
            {
                Transform parent_transform_to_set = null;

                if(parent_to_set != null)
                {
                    parent_transform_to_set = parent_to_set.transform;
                }

                GameObject instance = MonoBehaviour.Instantiate(prefab.gameObject, Vector3.zero, Quaternion.identity, parent_transform_to_set);

                ret = instance.GetComponent<T>();

                ObjectsCategoryInstances category_to_add_at = GetInstancesCategory(category_index);

                if(category_to_add_at == null)
                {
                    category_to_add_at = new ObjectsCategoryInstances(category_index);

                    instances.Add(category_to_add_at);
                }

                category_to_add_at.AddObjectInstance(category_variation_index, ret);
            }

            return ret;
        }

        public List<T> SpawnObjects<T>(int category_index, int category_variation_index, int ammount) where T : MonoBehaviour
        {
            List<T> ret = new List<T>();

            for(int i = 0; i< ammount; ++i)
            {
                T instance = SpawnObject<T>(category_index, category_variation_index);

                if(instance != null)
                {
                    ret.Add(instance);
                }
            }

            return ret;
        }

        public ReadOnlyCollection<T> GetSpawnedObjects<T>(int category_index, int category_variation_index) where T : MonoBehaviour
        {
            ReadOnlyCollection<T> ret = null;

            ObjectsCategoryInstances category_to_get_from = GetInstancesCategory(category_index);

            if(category_to_get_from != null)
            {
                ret = category_to_get_from.GetObjectsInstances<T>(category_variation_index);
            }

            return ret;
        }

        public ReadOnlyCollection<T> GetSpawnedObjects<T>(int category_index) where T : MonoBehaviour
        {
            ReadOnlyCollection<T> ret = null;

            ObjectsCategoryInstances category_to_get_from = GetInstancesCategory(category_index);

            if (category_to_get_from != null)
            {
                ret = category_to_get_from.GetAllObjectsInstances<T>();
            }

            return ret;
        }

        public void DespawnObject(int category_index, int category_variation_index, MonoBehaviour instance)
        {
            if (instance != null)
            {
                ObjectsCategoryInstances category_to_remove_from = GetInstancesCategory(category_index);

                if (category_to_remove_from != null)
                {
                    bool removed = category_to_remove_from.RemoveObjectInstance(category_variation_index, instance);

                    if (removed)
                    { 
                        MonoBehaviour.Destroy(instance.gameObject);
                    }
                }
            }
        }

        public void DespawnObjects(int category_index, int category_variation_index, List<MonoBehaviour> instances)
        {
            for(int i = 0; i < instances.Count; ++i)
            {
                MonoBehaviour curr_instance = instances[i];

                DespawnObject(category_index, category_variation_index, curr_instance);
            }
        }

        public void DespawnAllObjects(int category_index, int category_variation_index)
        {
            ObjectsCategoryInstances category_to_remove_from = GetInstancesCategory(category_index);

            if (category_to_remove_from != null)
            {
                ReadOnlyCollection<MonoBehaviour> instances = category_to_remove_from.GetObjectsInstances<MonoBehaviour>(category_variation_index);

                category_to_remove_from.ClearObjectInstances(category_variation_index);

                for (int i = 0; i < instances.Count; ++i)
                {
                    MonoBehaviour curr_instance = instances[i];

                    MonoBehaviour.Destroy(curr_instance.gameObject);
                }
            }
        }

        public void DespawnAllObjects(int category_index)
        {
            ObjectsCategoryInstances category_to_remove_from = GetInstancesCategory(category_index);

            if (category_to_remove_from != null)
            {
                ReadOnlyCollection<MonoBehaviour> instances = category_to_remove_from.GetAllObjectsInstances<MonoBehaviour>();

                category_to_remove_from.ClearObjectInstances();

                for (int i = 0; i < instances.Count; ++i)
                {
                    MonoBehaviour curr_instance = instances[i];

                    MonoBehaviour.Destroy(curr_instance.gameObject);
                }
            }
        }

        public void DespawnAllObjects()
        {
            for(int i = 0; i < instances.Count; ++i)
            {
                ObjectsCategoryInstances curr_category = instances[i];

                DespawnAllObjects(curr_category.CategoryIndex);
            }
        }
    }
}
