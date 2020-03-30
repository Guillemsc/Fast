using System;
using UnityEngine;

namespace Fast.Bindings
{
    [System.Serializable]
    public class BindingLinkData
    {
        [SerializeField] private string key = "";
        [SerializeField] private string type = null;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
