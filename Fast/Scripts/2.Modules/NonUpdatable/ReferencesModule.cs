using System;
using System.Collections.Generic;

namespace Fast.Modules
{
    public class ReferencesModule : Fast.Modules.Module
    {
        Dictionary<string, Fast.MonoBehaviourReference> references = new Dictionary<string, Fast.MonoBehaviourReference>();

        public void AddReference(string name, Fast.MonoBehaviourReference obj)
        {
            if (obj == null)
            {
                return;
            }

            references[name] = obj;
        }

        public void RemoveReference(string name)
        {
            references.Remove(name);
        }

        public bool TryGetReference<T>(string name, out T obj) where T : Fast.MonoBehaviourReference
        {
            obj = default;

            Fast.MonoBehaviourReference obj_to_find = null;
            bool found = references.TryGetValue(name, out obj_to_find);

            if (!found)
            {
                return false;
            }

            if (obj_to_find == null)
            {
                return false;
            }

            obj = obj_to_find as T;

            if (obj == null)
            {
                return false;
            }

            return true;
        }
    }
}
