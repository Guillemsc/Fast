using System;
using DG.Tweening;
using UnityEngine;

namespace Fast.UI.Bindings
{
    public abstract class DOTweenBinding : BaseBinding
    {
        [SerializeField] [HideInInspector] private bool use_starting_value = false;
        [SerializeField] [HideInInspector] private bool use_custom_easing = false;
        [SerializeField] [HideInInspector] private Ease easing = Ease.Linear;
        [SerializeField] [HideInInspector] private AnimationCurve custom_easing = new AnimationCurve();
        [SerializeField] [HideInInspector] private float duration = 0.0f;

        protected Sequence seq = null;

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

                if (duration < 0)
                {
                    duration = 0.0f;
                }
            }
        }
    }
}
