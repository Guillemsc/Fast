using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Architecture
{
    public class PresentationObjectsController : Fast.IController
    {
        private readonly Dictionary<Type, GameObject> parents
           = new Dictionary<Type, GameObject>();

        private readonly Dictionary<Type, BaseMatchPresentationObject> prefabs
           = new Dictionary<Type, BaseMatchPresentationObject>();

        private readonly Dictionary<Type, List<BaseMatchPresentationObject>> instances 
            = new Dictionary<Type, List<BaseMatchPresentationObject>>();

        public void AddParent<T>(GameObject parent) where T : BaseMatchLogicObject
        {
            Type type = typeof(T);

            parents.Add(type, parent);
        }

        private GameObject GetParent(Type type)
        {
            GameObject parent = null;
            parents.TryGetValue(type, out parent);

            if(parent == null)
            {
                Fast.FastService.MLog.LogError(this, $"Trying to get a parent that does not exist for the type: {type.Name}");
                return null;
            }

            return parent;
        }

        public void AddPrefab<T>(BaseMatchPresentationObject prefab) where T : BaseMatchLogicObject
        {
            Type type = typeof(T);

            prefabs.Add(type, prefab);
        }

        private BaseMatchPresentationObject GetPrefab(Type type)
        {
            BaseMatchPresentationObject prefab = null;
            prefabs.TryGetValue(type, out prefab);

            if(prefab == null)
            {
                Fast.FastService.MLog.LogError(this, $"Trying to get a prefab that does not exist of type: {type.Name}");
                return null;
            }

            return prefab;
        }

        public BaseMatchPresentationObject CreateInstance(BaseMatchLogicObject obj)
        {
            Type type = obj.GetType();

            BaseMatchPresentationObject prefab = GetPrefab(type);
            GameObject parent = GetParent(type);

            if(prefab == null)
            {
                Fast.FastService.MLog.LogError(this, $"Trying to create an instance but the prefab that does not exist of type: {type.Name}");
                return null;
            }

            GameObject instance_go = MonoBehaviour.Instantiate(prefab.gameObject, Vector3.zero, Quaternion.identity);
            instance_go.SetParent(parent);

            BaseMatchPresentationObject instance = instance_go.GetComponent<BaseMatchPresentationObject>();

            List<BaseMatchPresentationObject> instances_list = null;
            bool found = instances.TryGetValue(type, out instances_list);

            if(!found)
            {
                instances_list = new List<BaseMatchPresentationObject>();
                instances.Add(type, instances_list);
            }

            instances_list.Add(instance);

            instance.Init(obj);

            return instance;
        }

        public void DestroyInstance(BaseMatchPresentationObject obj)
        {
            if(obj == null)
            {
                return;
            }

            if(obj.BaseMatchLogicObject == null)
            {
                return;
            }

            DestroyInstance(obj.BaseMatchLogicObject);
        }

        public void DestroyInstance(BaseMatchLogicObject logic_obj)
        {
            if (logic_obj == null)
            {
                return;
            }

            Type type = logic_obj.GetType();

            List<BaseMatchPresentationObject> instances_list = null;
            bool found = instances.TryGetValue(type, out instances_list);

            if (!found)
            {
                return;
            }

            BaseMatchPresentationObject to_destroy = null;
            for (int i = 0; i < instances_list.Count; ++i)
            {
                BaseMatchPresentationObject curr_obj = instances_list[i];

                if (curr_obj.BaseMatchLogicObject.ObjectUID == logic_obj.ObjectUID)
                {
                    instances_list.RemoveAt(i);

                    to_destroy = curr_obj;

                    break;
                }
            }

            if (to_destroy == null)
            {
                return;
            }

            MonoBehaviour.Destroy(to_destroy);
        }

        public BaseMatchPresentationObject GetInstance(BaseMatchLogicObject logic_obj)
        {
            if (logic_obj == null)
            {
                return null;
            }

            Type type = logic_obj.GetType();

            List<BaseMatchPresentationObject> instances_list = null;
            bool found = instances.TryGetValue(type, out instances_list);

            if (!found)
            {
                return null;
            }

            for (int i = 0; i < instances_list.Count; ++i)
            {
                BaseMatchPresentationObject curr_obj = instances_list[i];

                if (curr_obj.BaseMatchLogicObject.ObjectUID == logic_obj.ObjectUID)
                {
                    instances_list.RemoveAt(i);

                    return curr_obj;
                }
            }

            Fast.FastService.MLog.LogError(this, $"Trying to get instance but it does not exist: {type.Name}");
            return null;
        }
    }
}
