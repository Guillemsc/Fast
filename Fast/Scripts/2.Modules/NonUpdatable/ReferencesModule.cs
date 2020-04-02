using System;
using System.Collections.Generic;

namespace Fast.Modules
{
    public class ReferencesModule : Fast.Modules.Module
    {
        Dictionary<string, Fast.References.Reference> references = new Dictionary<string, Fast.References.Reference>();

        public void AddReference(string name, Fast.References.Reference obj)
        {
            if(obj == null)
            {
                return;
            }

            references[name] = obj;
        }

        public void RemoveReference(string name)
        {
            references.Remove(name);
        }

        public bool TryGetReference<T>(string name, out T obj) where T : Fast.References.Reference
        {
            obj = default;

            Fast.References.Reference obj_to_find = null;
            bool found = references.TryGetValue(name, out obj_to_find);

            if(!found)
            {
                return false;
            }

            if(obj_to_find == null)
            {
                return false;
            }

            obj = obj_to_find as T;

            if(obj == null)
            {
                return false;
            }

            return true;
        }
    }
}
