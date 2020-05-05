using System;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.UI.Bindings
{
    public abstract class DOTweenBinding : FormBinding
    {
        [SerializeField] private bool use_starting_value = false;
        [SerializeField] private List<GameObject> targets = new List<GameObject>();
        [SerializeField] private Ease easing = Ease.Linear;
        [SerializeField] private float duration = 0.0f;

        protected bool UseStartingValue => use_starting_value;
        protected float Duration => duration;

        private Sequence seq = null;

        public override void OnValueRaised(object value)
        {
            if(seq != null)
            {
                seq.Kill();
            }

            seq = DOTween.Sequence();

            for (int i = 0; i < targets.Count; ++i)
            {
                Sequence target_sequence = DOTween.Sequence();

                GameObject curr_target = targets[i];

                if(curr_target == null)
                {
                    continue;
                }

                SetupSequence(target_sequence, curr_target);

                seq.Join(target_sequence);
            }

            seq.Play();
        }

        protected virtual void SetupSequence(Sequence seq, GameObject target)
        {

        }
    }
}
