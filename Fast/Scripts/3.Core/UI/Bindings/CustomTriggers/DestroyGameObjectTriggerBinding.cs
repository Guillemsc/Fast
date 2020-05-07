using System;
using UnityEngine;

namespace Fast.UI.Bindings
{
    public class DestroyGameObjectTriggerBinding : MultipleTargetBinding<GameObject>
    {
        protected override void SetupTarget(GameObject target, object value)
        {
            Destroy(target);
        }
    }
}
