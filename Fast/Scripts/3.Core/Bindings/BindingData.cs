using System;
using System.Collections.Generic;

namespace Fast.Bindings
{
    public class BindingData
    {
        private readonly Dictionary<string, object> data = new Dictionary<string, object>();

        public IReadOnlyDictionary<string, object> Data => data;

        public void AddBindingData(string binding_key, object binding)
        {
            data.Add(binding_key, binding);
        }

        public void RemoveBindingData(string binding_key)
        {
            data.Remove(binding_key);
        }

        public T GetBindingObject<T>(string binding_key)
        {
            foreach (KeyValuePair<string, object> entry in data)
            {
                if(entry.Key == binding_key)
                {
                    return (T)entry.Value;
                }
            }

            return default;
        }
    }
}
