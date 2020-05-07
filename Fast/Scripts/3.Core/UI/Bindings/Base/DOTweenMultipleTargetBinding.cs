using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Fast.UI.Bindings
{
    public abstract class DOTweenMultipleTargetBinding<T> : DOTweenBinding, IMultipleTarget<T> where T : UnityEngine.Object
    {
        [SerializeField] private List<T> targets = new List<T>();

        public IReadOnlyList<T> Targets => targets;

        public override void OnValueRised(object value)
        {
            if (seq != null)
            {
                seq.Kill();
            }

            seq = DOTween.Sequence();

            for (int i = 0; i < targets.Count; ++i)
            {
                Sequence target_sequence = DOTween.Sequence();

                T curr_target = targets[i];

                if (curr_target == null)
                {
                    continue;
                }

                SetupTarget(target_sequence, curr_target, value);

                seq.Join(target_sequence);
            }


            if (!UseCustomEasing)
            {
                seq.SetEase(Easing);
            }
            else
            {
                seq.SetEase(CustomEasing);
            }

            seq.Play();
        }

        protected virtual void SetupTarget(Sequence seq, T target, object value)
        {

        }
    }
}
