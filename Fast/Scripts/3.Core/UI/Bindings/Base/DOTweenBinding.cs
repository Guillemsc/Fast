using System;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.UI.Bindings
{
    public abstract class DOTweenBinding : MultipleGameObjectTargetsBinding
    {
        [SerializeField] [HideInInspector] private bool use_starting_value = false;
        [SerializeField] [HideInInspector] private bool use_custom_easing = false;
        [SerializeField] [HideInInspector] private Ease easing = Ease.Linear;
        [SerializeField] [HideInInspector] private AnimationCurve custom_easing = new AnimationCurve();
        [SerializeField] [HideInInspector] private float duration = 0.0f;

        private Sequence seq = null;

        public bool UseStartingValue
        {
            get { return use_starting_value; }
            set { use_starting_value = value; }
        }

        public bool UseCustomEasing
        {
            get { return use_custom_easing; }
            set { use_custom_easing = value; }
        }

        public Ease Easing
        {
            get { return easing; }
            set { easing = value; }
        }

        public AnimationCurve CustomEasing
        {
            get { return custom_easing; }
            set { custom_easing = value; }
        }

        public float Duration
        {
            get { return duration; }
            set
            {
                duration = value;

                if(duration < 0)
                {
                    duration = 0.0f;
                }
            }
        }

        public override void OnValueRised(object value)
        {
            if(seq != null)
            {
                seq.Kill();
            }

            seq = DOTween.Sequence();

            for (int i = 0; i < Targets.Count; ++i)
            {
                Sequence target_sequence = DOTween.Sequence();

                GameObject curr_target = Targets[i];

                if(curr_target == null)
                {
                    continue;
                }

                SetupSequence(target_sequence, curr_target);

                if (!UseCustomEasing)
                {
                    target_sequence.SetEase(Easing);
                }
                else
                {
                    target_sequence.SetEase(CustomEasing);
                }

                seq.Join(target_sequence);
            }

            seq.Play();
        }

        protected virtual void SetupSequence(Sequence seq, GameObject target)
        {

        }
    }
}
