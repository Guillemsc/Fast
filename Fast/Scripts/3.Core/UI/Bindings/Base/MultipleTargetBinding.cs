using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.UI.Bindings
{
    public abstract class MultipleTargetBinding<T> : BaseBinding, IMultipleTarget<T> where T : UnityEngine.Object
    {
        [SerializeField] private List<T> targets = new List<T>();

        public IReadOnlyList<T> Targets => targets;

        public override void OnValueRised(object value)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                T curr_target = targets[i];

                if (curr_target == null)
                {
                    continue;
                }

                SetupTarget(curr_target, value);
            }
        }

        protected virtual void SetupTarget(T target, object value)
        {

        }
    }
}
