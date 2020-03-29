using System;
using System.Collections.Generic;

namespace Fast.Bindings
{
    [System.Serializable]
    public class BindingLink
    {
        private List<BindingLinkData> links = new List<BindingLinkData>();

        public IReadOnlyList<BindingLinkData> Links => links;

        public bool AddBindingLink(string binding_key, string binding_type)
        {
            BindingLinkData checking = GetBindingLinkData(binding_key);

            if(checking != null)
            {
                return false;
            }

            BindingLinkData data = new BindingLinkData();

            data.Key = binding_key;
            data.Type = binding_type;

            links.Add(data);

            return true;
        }

        public void RemoveBindingLink(string binding_key)
        {
            for (int i = 0; i < links.Count; ++i)
            {
                BindingLinkData curr_data = links[i];

                if (curr_data.Key == binding_key)
                {
                    links.RemoveAt(i);
                }
            }
        }

        public BindingLinkData GetBindingLinkData(string key)
        {
            for(int i = 0; i < links.Count; ++i)
            {
                BindingLinkData curr_data = links[i];

                if(curr_data.Key == key)
                {
                    return curr_data;
                }
            }

            return null;
        }

        private bool BindingLinkExists(string binding_key, object binding)
        {
            if(binding == null)
            {
                return false;
            }

            Type binding_type = binding.GetType();

            string binding_type_string = binding_type.ToString();

            BindingLinkData data = GetBindingLinkData(binding_key);

            if(data == null)
            {
                return false;
            }

            if(binding_type_string != data.Type)
            {
                return false;
            }

            return true;
        }

        public bool BindingDataIsValid(BindingData data)
        {
            if (data == null)
            {
                return false;
            }

            foreach (KeyValuePair<string, object> entry in data.Data)
            {
                bool exists = BindingLinkExists(entry.Key, entry.Value);

                if(!exists)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
