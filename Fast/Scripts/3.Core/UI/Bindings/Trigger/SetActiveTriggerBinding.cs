using System;
using UnityEngine;

namespace Fast.UI.Bindings
{
    public class SetActiveTriggerBinding : MultipleTargetBinding<GameObject>
    {
        [SerializeField] private bool active = false;

        protected override void SetupTarget(GameObject target, object value)
        {
            target.SetActive(active);
        }
    }
}
