using System;
using UnityEngine;

namespace Fast.UI.Bindings
{
    public class SpawnPrefabTriggerBinding : BaseBinding
    {
        [SerializeField] private GameObject prefab = null;
        [SerializeField] private bool use_self_as_parent = true;
        [SerializeField] private GameObject parent = null;

        public override void OnValueRised(object value)
        {
            if(prefab == null)
            {
                return;
            }

            GameObject parent_to_set = parent;

            if(use_self_as_parent)
            {
                parent_to_set = gameObject;
            }

            if(parent_to_set == null)
            {
                return;
            }

            Instantiate(prefab, Vector3.zero, Quaternion.identity, parent_to_set.transform);
        }
    }
}
