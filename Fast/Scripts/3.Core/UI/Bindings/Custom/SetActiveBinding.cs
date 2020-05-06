using System;
using UnityEngine;

namespace Fast.UI.Bindings
{
    public class SetActiveBinding : MultipleGameObjectTargetsBinding
    {
        [SerializeField] private bool active = false;

        public override void OnValueRised(object value)
        {
            for (int i = 0; i < Targets.Count; ++i)
            {
                GameObject curr_target = Targets[i];

                if (curr_target == null)
                {
                    continue;
                }

                curr_target.SetActive(active);
            }
        }
    }
}
