using System;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.UI.Bindings
{
    public class FadeTriggerBinding : DOTweenMultipleTargetBinding<GameObject>
    {
        [SerializeField] private float start_alpha = 0.0f;
        [SerializeField] private float end_alpha = 0.0f;

        protected override void SetupTarget(Sequence seq, GameObject target, object value)
        {
            CanvasGroup cg = target.GetOrAddComponent<CanvasGroup>();

            if (UseStartingValue)
            {
                seq.Append(cg.DOFade(start_alpha, 0.0f));
            }

            seq.Append(cg.DOFade(end_alpha, Duration));
        }
    }
}
