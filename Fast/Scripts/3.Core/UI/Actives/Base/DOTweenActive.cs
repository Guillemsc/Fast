using System;
using UnityEngine;
using DG.Tweening;

namespace Fast.UI.Actives
{
    public class DOTweenActive : BaseActive
    {
        [SerializeField] private bool use_custom_easing = false;
        [SerializeField] private Ease easing = Ease.Linear;
        [SerializeField] private AnimationCurve custom_easing = new AnimationCurve();
        [SerializeField] private float duration = 0.0f;

        protected Sequence seq = null;

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

        protected void SetEasing(Tween tween)
        {
            if (!UseCustomEasing)
            {
                tween.SetEase(Easing);
            }
            else
            {
                tween.SetEase(CustomEasing);
            }
        }
    }
}
